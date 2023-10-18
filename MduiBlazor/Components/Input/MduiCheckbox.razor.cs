using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace MduiBlazor
{
    public partial class MduiCheckbox : MduiInputBase<bool>
    {
        private string TypeName => Switch ? "switch" : "checkbox";
        
        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public bool Switch { get; set; }

        protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage) => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");

        private void OnFocus()
        {
            Field?.SetFocus();
        }

        private void OnBlur()
        {
            Field?.SetBlur();
        }
    }
}
