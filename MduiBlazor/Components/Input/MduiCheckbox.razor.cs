using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;

namespace MduiBlazor
{
    public partial class MduiCheckbox : MduiInputBase<bool>
    {
        protected string Classname =>
            new ClassBuilder($"mdui-{TypeName}")
            .AddClass(Class)
            .Build();

        private string TypeName => Switch ? "switch" : "checkbox";

        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public bool Switch { get; set; }

        protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage) => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");

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
