using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;

namespace MduiBlazor
{
    public partial class MduiInputText : MduiInputBase<string?>
    {
        private int _wordNumber;

        protected string Classname =>
        new ClassBuilder("mdui-input")
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool AutoFocus { get; set; }

        [Parameter]
        public string? Placeholder { get; set; }

        [Parameter]
        public int? MaxLength { get; set; }

        [Parameter]
        public int Rows { get; set; } = 1;

        [Parameter]
        public string Type { get; set; } = "text";

        [Parameter]
        public string? Pattern { get; set; }

        [Parameter]
        public bool Trim { get; set; }

        [Parameter]
        public EventCallback<string> OnInput { get; set; }

        protected override void OnInitialized()
        {
            if (Value?.Length > 0)
            {
                Field?.SetNotEmpty();
                Field?.ChangedState();
            }
            if (MaxLength > 0)
            {
                _wordNumber = Value?.Length ?? 0;
            }
            base.OnInitialized();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (!firstRender)
            {
                if (MaxLength > 0)
                {
                    if (_wordNumber == Value?.Length)
                    {
                        return;
                    }
                    _wordNumber = Value?.Length ?? 0;
                    StateHasChanged();
                }
            }
        }

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

        private void OnTextInput(ChangeEventArgs args)
        {
            var value = args.Value?.ToString();
            if (MaxLength > 0)
            {
                _wordNumber = value?.Length ?? 0;
            }
            if (OnInput.HasDelegate)
            {
                OnInput.InvokeAsync(value);
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
