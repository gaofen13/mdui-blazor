﻿using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;

namespace MduiBlazor
{
    public partial class MduiSelect<TValue> : MduiInputBase<TValue>
    {
        protected string Classname =>
          new ClassBuilder("mdui-input mdui-select")
            .AddClass(Class)
            .Build();

        protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
            => this.TryParseSelectableValueFromString(value, out result, out validationErrorMessage);

        protected override string? FormatValueAsString(TValue? value)
        {
            // We special-case bool values because BindConverter reserves bool conversion for conditional attributes.
            if (typeof(TValue) == typeof(bool))
            {
                return (bool)(object)value! ? "true" : "false";
            }
            else if (typeof(TValue) == typeof(bool?))
            {
                return value is not null && (bool)(object)value ? "true" : "false";
            }

            return base.FormatValueAsString(value);
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
