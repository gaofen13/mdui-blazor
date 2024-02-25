using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace MduiBlazor
{
    public partial class MduiInputNumber<TValue> : MduiInputBase<TValue>
    {
        protected string Classname =>
        new ClassBuilder("mdui-input")
            .AddClass(Class)
            .Build();

        [Parameter]
        public TValue? Min { get; set; } = default;

        [Parameter]
        public TValue Max { get; set; } = (TValue)(object)100;

        [Parameter]
        public TValue? Step { get; set; }

        [Parameter]
        public bool AutoFocus { get; set; }

        [Parameter]
        public string? Placeholder { get; set; }

        [Parameter]
        public string ParsingErrorMessage { get; set; } = "The {0} field must be a number.";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && AutoFocus)
            {
                await Element.FocusAsync();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
        {
            if (BindConverter.TryConvertTo<TValue>(value, CultureInfo.InvariantCulture, out result))
            {
                validationErrorMessage = null;
                return true;
            }
            else
            {
                validationErrorMessage = string.Format(CultureInfo.InvariantCulture, ParsingErrorMessage, DisplayName ?? FieldIdentifier.FieldName);
                return false;
            }
        }

        /// <summary>
        /// Formats the value as a string. Derived classes can override this to determine the formatting used for <c>CurrentValueAsString</c>.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A string representation of the value.</returns>
        protected override string? FormatValueAsString(TValue? value)
        {
            // Avoiding a cast to IFormattable to avoid boxing.
            return value switch
            {
                null => null,
                int @int => BindConverter.FormatValue(@int, CultureInfo.InvariantCulture),
                long @long => BindConverter.FormatValue(@long, CultureInfo.InvariantCulture),
                short @short => BindConverter.FormatValue(@short, CultureInfo.InvariantCulture),
                float @float => BindConverter.FormatValue(@float, CultureInfo.InvariantCulture),
                double @double => BindConverter.FormatValue(@double, CultureInfo.InvariantCulture),
                decimal @decimal => BindConverter.FormatValue(@decimal, CultureInfo.InvariantCulture),
                _ => throw new InvalidOperationException($"Unsupported type {value.GetType()}"),
            };
        }

        private void OnTextInput(ChangeEventArgs args)
        {
            var value = args.Value?.ToString();
            if (value?.Length > 0)
            {
                Field?.SetNotEmpty();
            }
            else
            {
                Field?.RemoveNotEmpty();
            }
        }

        private void OnInputFocus(FocusEventArgs args)
        {
            Field?.SetFocus();
            if (OnFocus.HasDelegate)
            {
                OnFocus.InvokeAsync(args);
            }
        }

        private void OnInputBlur(FocusEventArgs args)
        {
            Field?.SetBlur();
            if (OnBlur.HasDelegate)
            {
                OnBlur.InvokeAsync(args);
            }
        }
    }
}
