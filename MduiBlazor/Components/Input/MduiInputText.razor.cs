using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace MduiBlazor
{
    public partial class MduiInputText : MduiInputBase<string?>
    {
        private int _wordNumber;

        protected string Classname =>
        new ClassBuilder("mdui-textfield-input")
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

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
            if (MaxLength > 0)
            {
                _wordNumber = Value?.Length ?? 0;
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
            if (MaxLength > 0)
            {
                _wordNumber = args.Value?.ToString()?.Length ?? 0;
            }
        }

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
