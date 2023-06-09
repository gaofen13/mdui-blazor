using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiField : MduiComponentBase
    {
        private string? _errorText;
        private bool _notEmpty;
        private bool _isFocus;
        private bool _invalid;

        protected string Classname =>
          new ClassBuilder("mdui-textfield")
            .AddClass("mdui-textfield-has-bottom", _invalid || !string.IsNullOrWhiteSpace(HelperText))
            .AddClass("mdui-textfield-not-empty", _notEmpty)
            .AddClass("mdui-textfield-disabled", Disabled)
            .AddClass("mdui-textfield-required", Required)
            .AddClass("mdui-textfield-invalid", _invalid)
            .AddClass("mdui-textfield-focus", _isFocus)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public bool Required { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public RenderFragment? IconContent { get; set; }

        [Parameter]
        public string? ErrorText { get; set; }

        [Parameter]
        public string? HelperText { get; set; }

        protected override void OnInitialized()
        {
            if (!string.IsNullOrWhiteSpace(ErrorText))
            {
                _errorText = ErrorText;
            }
        }

        public void SetNotEmpty()
        {
            if (!_notEmpty)
            {
                _notEmpty = true;
                StateHasChanged();
            }
        }

        public void RemoveNotEmpty()
        {
            if (_notEmpty)
            {
                _notEmpty = false;
                StateHasChanged();
            }
        }

        public void SetInvalid(string? error)
        {
            if (!_invalid)
            {
                _invalid = true;
                if (string.IsNullOrWhiteSpace(ErrorText))
                {
                    _errorText = error;
                }
                StateHasChanged();
            }
        }

        public void RemoveInvalid()
        {
            if (_invalid)
            {
                _invalid = false;
                if (string.IsNullOrWhiteSpace(ErrorText))
                {
                    _errorText = string.Empty;
                }
                StateHasChanged();
            }
        }

        public void SetFocus()
        {
            if (!_isFocus)
            {
                _isFocus = true;
                StateHasChanged();
            }
        }

        public void SetBlur()
        {
            if (_isFocus)
            {
                _isFocus = false;
                StateHasChanged();
            }
        }
    }
}
