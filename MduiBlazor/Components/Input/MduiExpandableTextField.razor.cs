﻿using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace MduiBlazor
{
    public partial class MduiExpandableTextField : MduiInputBase<string?>
    {
        private bool _expanded;
        private bool _isFocus;

        private string Classname =>
            new ClassBuilder("mdui-textfield mdui-textfield-expandable")
            .AddClass("mdui-textfield-expanded", _expanded)
            .AddClass("mdui-textfield-focus", _isFocus)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter]
        public string Icon { get; set; } = "search";

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

        private void OnFocus()
        {
            _isFocus = true;
        }

        private void OnBlur()
        {
            _isFocus = false;
        }

        private async Task OpenAsync()
        {
            _expanded = true;
            await Element!.FocusAsync();
            StateHasChanged();
        }

        private void Close()
        {
            _expanded = false;
            _isFocus = false;
            StateHasChanged();
        }
    }
}
