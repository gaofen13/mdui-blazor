using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MduiBlazor
{
    public partial class MduiTextField
    {
        private int _wordNumber;

        private string FieldClassname =>
            new ClassBuilder()
            .AddClass("mdui-textfield-floating-label", string.IsNullOrWhiteSpace(Placeholder) && FloatingLabel)
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
        public int? MaxLength { get; set; }

        [Parameter]
        public int Rows { get; set; } = 1;

        [Parameter]
        public string Type { get; set; } = "text";

        [Parameter]
        public string? Pattern { get; set; }

        [Parameter]
        public bool Trim { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && AutoFocus)
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
