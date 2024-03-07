using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiField : MduiComponentBase
    {
        private string? _errorText;
        private bool _disabled;
        private bool _required;
        private bool _notEmpty;
        private bool _isFocus;
        private bool _invalid;

        protected string Classname =>
          new ClassBuilder("mdui-field")
            .AddClass("mdui-field-has-bottom", _invalid || !string.IsNullOrWhiteSpace(HelperText))
            .AddClass("mdui-field-not-empty", _notEmpty)
            .AddClass("mdui-field-disabled", _disabled)
            .AddClass("mdui-field-required", _required)
            .AddClass("mdui-field-invalid", _invalid)
            .AddClass("mdui-field-focus", _isFocus)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? Label { get; set; }

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

        internal void ChangedState()
        {
            StateHasChanged();
        }

        internal void SetNotEmpty()
        {
            if (!_notEmpty)
            {
                _notEmpty = true;
            }
        }

        internal void RemoveNotEmpty()
        {
            if (_notEmpty)
            {
                _notEmpty = false;
            }
        }

        internal void SetInvalid(string? error)
        {
            if (string.IsNullOrWhiteSpace(ErrorText))
            {
                _errorText = error;
            }
            if (!_invalid)
            {
                _invalid = true;
            }
        }

        internal void RemoveInvalid()
        {
            if (_invalid)
            {
                _invalid = false;
                if (string.IsNullOrWhiteSpace(ErrorText))
                {
                    _errorText = null;
                }
            }
        }

        internal void SetFocus()
        {
            if (!_isFocus)
            {
                _isFocus = true;
                StateHasChanged();
            }
        }

        internal void SetBlur()
        {
            if (_isFocus)
            {
                _isFocus = false;
                StateHasChanged();
            }
        }

        internal void SetDisabled(bool disabled)
        {
            if (_disabled != disabled)
            {
                _disabled = disabled;
                StateHasChanged();
            }
        }

        internal void SetRequired(bool required)
        {
            if (_required != required)
            {
                _required = required;
                StateHasChanged();
            }
        }
    }
}
