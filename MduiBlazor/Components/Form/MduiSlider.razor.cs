using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace MduiBlazor
{
    public partial class MduiSlider<TValue> : MduiInputBase<TValue>
    {
        private bool Actived { get; set; }

        private double PercentWidth => (Convert.ToDouble(Value) - Convert.ToDouble(Min)) / (Convert.ToDouble(Max) - Convert.ToDouble(Min)) * 100;

        private string FieldClassname =>
            new ClassBuilder("mdui-slider")
            .AddClass("mdui-slider-actived", Actived)
            .AddClass("mdui-slider-disabled", Disabled)
            .AddClass("mdui-slider-zero", Convert.ToDouble(Value) == 0)
            .Build();

        private string FillStyle =>
            new StyleBuilder()
            .AddStyle("width", $"{PercentWidth}%")
            .Build();

        private string ThumbStyle =>
            new StyleBuilder()
            .AddStyle("left", $"{PercentWidth}%")
            .Build();

        [Parameter]
        public TValue? Step { get; set; }

        [Parameter]
        [EditorRequired]
        public TValue? Min { get; set; }

        [Parameter]
        [EditorRequired]
        public TValue? Max { get; set; }

        [Parameter]
        public bool NoLabel { get; set; }

        [Parameter]
        public string ParsingErrorMessage { get; set; } = "The {0} field must be a number.";

        protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
        {
            if (BindConverter.TryConvertTo<TValue>(value, CultureInfo.InvariantCulture, out result))
            {
                validationErrorMessage = null;
                InvokeAsync(StateHasChanged);
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

        private void OnInputBlur(FocusEventArgs args)
        {
            Field?.SetBlur();
            if (OnBlur.HasDelegate)
            {
                OnBlur.InvokeAsync(args);
            }
        }

        private void OnMouseDown()
        {
            Actived = true;
            Field?.SetFocus();
        }

        private void Active()
        {
            Actived = true;
        }

        private void Disactive()
        {
            Actived = false;
        }
    }
}
