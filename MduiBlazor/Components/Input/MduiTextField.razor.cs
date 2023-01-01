using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MduiBlazor
{
    public partial class MduiTextField
    {
        private bool _invalid;
        private int _wordNumber;

        protected string Classname =>
          new ClassBuilder("mdui-textfield")
            .AddClass("mdui-textfield-floating-label", string.IsNullOrWhiteSpace(Placeholder) && FloatingLabel)
            .AddClass("mdui-textfield-has-bottom", !string.IsNullOrWhiteSpace(HelperText))
            .AddClass("mdui-textfield-invalid", _invalid)
            .AddClass(FieldClass)
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? Label { get; set; }

        /// <summary>
        /// only shows when placeholder is empty
        /// </summary>
        [Parameter]
        public bool FloatingLabel { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public string? ErrorText { get; set; }

        [Parameter]
        public string? HelperText { get; set; }

        [Parameter]
        public bool AutoFocus { get; set; }

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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender && AutoFocus)
            {
                await Element.FocusAsync();
            }
            await base.OnAfterRenderAsync(firstRender);
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
            if (MaxLength > 0 && MaxLength <= int.MaxValue)
            {
                _wordNumber = args.Value?.ToString()?.Length ?? 0;
            }
        }
    }
}
