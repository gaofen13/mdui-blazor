using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;

namespace MduiBlazor
{
    public partial class MduiExpandableTextField : MduiInputBase<string?>
    {
        private bool _expanded;
        private bool _isFocus;

        private string Classname =>
            new ClassBuilder("mdui-field mdui-field-expandable")
            .AddClass("mdui-field-expanded", _expanded)
            .AddClass("mdui-field-disabled", Disabled)
            .AddClass("mdui-field-focus", _isFocus)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public RenderFragment? IconContent { get; set; }

        [Parameter]
        public string? Placeholder { get; set; }

        [Parameter]
        public int? MaxLength { get; set; }

        [Parameter]
        public string Type { get; set; } = "text";

        [Parameter]
        public string? Pattern { get; set; }

        [Parameter]
        public bool Trim { get; set; }

        protected override bool TryParseValueFromString(string? value, out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
        {
            if (Trim)
            {
                result = value?.Trim();
            }
            else
            {
                result = value;
            }
            validationErrorMessage = null;
            return true;

        }
        private void OnInputFocus(FocusEventArgs args)
        {
            _isFocus = true;
            if (OnFocus.HasDelegate)
            {
                OnFocus.InvokeAsync(args);
            }
        }

        private void OnInputBlur(FocusEventArgs args)
        {
            _isFocus = false;
            if (OnBlur.HasDelegate)
            {
                OnBlur.InvokeAsync(args);
            }
        }

        private async Task ToggleAsync()
        {
            if (_expanded)
            {
                _expanded = false;
            }
            else
            {
                _expanded = true;
                await Element!.FocusAsync();
            }
            await InvokeAsync(StateHasChanged);
        }
    }
}
