using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace MduiBlazor
{
    public abstract class MduiInputBase<TValue> : MduiComponentBase, IDisposable
    {
        private readonly EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;
        private bool _previousParsingAttemptFailed;
        private ValidationMessageStore? _parsingValidationMessages;
        private Type? _nullableUnderlyingType;

        [CascadingParameter]
        private EditContext? CascadedEditContext { get; set; }

        [CascadingParameter(Name = "Field")]
        protected MduiField? Field { get; set; }

        [Parameter]
        public string? Name { get; set; }

        /// <summary>
        /// When true, the control will be immutable by user interaction. <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/readonly">readonly</see> HTML attribute for more information.
        /// </summary>
        [Parameter]
        public bool Readonly { get; set; }

        /// <summary>
        /// Disables the form control, ensuring it doesn't participate in form submission.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Whether the element needs to have a value
        /// </summary>
        [Parameter]
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets the value of the input. This should be used with two-way binding.
        /// </summary>
        /// <example>
        /// @bind-Value="model.PropertyName"
        /// </example>
        [Parameter]
        public TValue? Value { get; set; }

        /// <summary>
        /// Gets or sets a callback that updates the bound value.
        /// </summary>
        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        /// <summary>
        /// Gets or sets an expression that identifies the bound value.
        /// </summary>
        [Parameter]
        public Expression<Func<TValue>>? ValueExpression { get; set; }

        /// <summary>
        /// Gets or sets the display name for this field.
        /// <para>This value is used when generating error messages when the input value fails to parse correctly.</para>
        /// </summary>
        [Parameter]
        public string? DisplayName { get; set; }

        /// <summary>
        /// Gets the associated <see cref="AspNetCore.Components.Forms.EditContext"/>.
        /// This property is uninitialized if the input does not have a parent <see cref="EditForm"/>.
        /// </summary>
        protected EditContext EditContext { get; set; } = default!;

        /// <summary>
        /// Gets the <see cref="FieldIdentifier"/> for the bound value.
        /// </summary>
        protected internal FieldIdentifier FieldIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the current value of the input.
        /// </summary>
        protected TValue? CurrentValue
        {
            get => Value;
            set
            {
                var hasChanged = !EqualityComparer<TValue>.Default.Equals(value, Value);
                if (hasChanged)
                {
                    Value = value;
                    _ = ValueChanged.InvokeAsync(Value);
                    EditContext?.NotifyFieldChanged(FieldIdentifier);
                }
            }
        }

        /// <summary>
        /// Gets or sets the current value of the input, represented as a string.
        /// </summary>
        protected string? CurrentValueAsString
        {
            get => FormatValueAsString(CurrentValue);
            set
            {
                _parsingValidationMessages?.Clear();

                bool parsingFailed;

                if (_nullableUnderlyingType != null && string.IsNullOrEmpty(value))
                {
                    // Assume if it's a nullable type, null/empty inputs should correspond to default(T)
                    // Then all subclasses get nullable support almost automatically (they just have to
                    // not reject Nullable<T> based on the type itself).
                    parsingFailed = false;
                    CurrentValue = default!;
                }
                else if (TryParseValueFromString(value, out var parsedValue, out var validationErrorMessage))
                {
                    parsingFailed = false;
                    CurrentValue = parsedValue!;
                }
                else
                {
                    parsingFailed = true;

                    // EditContext may be null if the input is not a child component of EditForm.
                    if (EditContext is not null)
                    {
                        _parsingValidationMessages ??= new ValidationMessageStore(EditContext);
                        _parsingValidationMessages.Add(FieldIdentifier, validationErrorMessage);

                        // Since we're not writing to CurrentValue, we'll need to notify about modification from here
                        EditContext.NotifyFieldChanged(FieldIdentifier);
                    }
                }

                // We can skip the validation notification if we were previously valid and still are
                if (parsingFailed || _previousParsingAttemptFailed)
                {
                    EditContext?.NotifyValidationStateChanged();
                    _previousParsingAttemptFailed = parsingFailed;
                }
            }
        }

        protected IEnumerable<string> InvalidMessageList { get; set; } = Enumerable.Empty<string>();

        /// <summary>
        /// Constructs an instance of <see cref="InputBase{TValue}"/>.
        /// </summary>
        protected MduiInputBase()
        {
            _validationStateChangedHandler = OnValidateStateChanged;
        }

        /// <summary>
        /// Formats the value as a string. Derived classes can override this to determine the formating used for <see cref="CurrentValueAsString"/>.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A string representation of the value.</returns>
        protected virtual string? FormatValueAsString(TValue? value)
            => value?.ToString();

        /// <summary>
        /// Parses a string to create an instance of <typeparamref name="TValue"/>. Derived classes can override this to change how
        /// <see cref="CurrentValueAsString"/> interprets incoming values.
        /// </summary>
        /// <param name="value">The string value to be parsed.</param>
        /// <param name="result">An instance of <typeparamref name="TValue"/>.</param>
        /// <param name="validationErrorMessage">If the value could not be parsed, provides a validation error message.</param>
        /// <returns>True if the value could be parsed; otherwise false.</returns>
        protected abstract bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage);

        /// <inheritdoc />
        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (EditContext != null || CascadedEditContext != null)
            {
                // This is the first run
                // Could put this logic in OnInit, but its nice to avoid forcing people who override OnInit to call base.OnInit()

                if (ValueExpression == null)
                {
                    return Task.CompletedTask;
                    //throw new InvalidOperationException($"{GetType()} requires a value for the 'ValueExpression' " +
                    //  $"parameter. Normally this is provided automatically when using 'bind-Value'.");
                }

                FieldIdentifier = FieldIdentifier.Create(ValueExpression);

                if (CascadedEditContext != null)
                {
                    EditContext = CascadedEditContext;
                    EditContext.OnValidationStateChanged += _validationStateChangedHandler;
                }

                _nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(TValue));
            }
            else if (CascadedEditContext != EditContext)
            {
                // Not the first run

                // We don't support changing EditContext because it's messy to be clearing up state and event
                // handlers for the previous one, and there's no strong use case. If a strong use case
                // emerges, we can consider changing this.
                throw new InvalidOperationException($"{GetType()} does not support changing the " +
                    $"{nameof(Microsoft.AspNetCore.Components.Forms.EditContext)} dynamically.");
            }

            // For derived components, retain the usual lifecycle with OnInit/OnParametersSet/etc.
            return base.SetParametersAsync(ParameterView.Empty);
        }

        /// <summary>
        /// Exposes the elements FocusAsync() method.
        /// </summary>
        public async ValueTask FocusAsync()
        {
            await Element!.FocusAsync();
        }

        private void OnValidateStateChanged(object? sender, ValidationStateChangedEventArgs eventArgs)
        {
            UpdateAdditionalValidationAttributes();

            StateHasChanged();
        }

        private void UpdateAdditionalValidationAttributes()
        {
            if (EditContext is null)
            {
                return;
            }

            InvalidMessageList = EditContext.GetValidationMessages(FieldIdentifier);
            if (InvalidMessageList.Any())
            {
                Field?.SetInvalid(InvalidMessageList.FirstOrDefault());
            }
            else
            {
                Field?.RemoveInvalid();
            }
        }

        void IDisposable.Dispose()
        {
            // When initialization in the SetParametersAsync method fails, the EditContext property can remain equal to null
            if (EditContext is not null)
            {
                EditContext.OnValidationStateChanged -= _validationStateChangedHandler;
            }
            GC.SuppressFinalize(this);
        }
    }
}
