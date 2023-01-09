using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace MduiBlazor
{
    public partial class MduiNumberField<TValue>
    {
        private bool _isFocus;
        private bool _notEmpty;

        private string FieldClassname =>
            new ClassBuilder()
            .AddClass("mdui-textfield-focus", _isFocus)
            .AddClass("mdui-textfield-disabled", Disabled)
            .AddClass("mdui-textfield-not-empty", _notEmpty)
            .AddClass("mdui-textfield-has-bottom", !string.IsNullOrWhiteSpace(ErrorText) || !string.IsNullOrWhiteSpace(HelperText))
            .AddClass("mdui-textfield-floating-label", FloatingLabel)
            .AddClass("mdui-typo", UseMduiTypo)
            .Build();

        protected string Classname =>
        new ClassBuilder("mdui-textfield-input")
            .AddClass(FieldClass)
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public string? ErrorText { get; set; }

        [Parameter]
        public string? HelperText { get; set; }

        /// <summary>
        /// only shows when placeholder is empty
        /// </summary>
        [Parameter]
        public bool FloatingLabel { get; set; }

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

        private void OnFocus()
        {
            _isFocus = true;
        }

        private void OnBlur()
        {
            _isFocus = false;
        }

        private void OnInput(ChangeEventArgs args)
        {
            _notEmpty = args.Value?.ToString()?.Length > 0;
        }
    }
}
