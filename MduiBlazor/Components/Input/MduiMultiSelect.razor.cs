using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace MduiBlazor
{
    public partial class MduiMultiSelect<TValue> : MduiInputBase<TValue>
    {
        protected string Classname =>
          new ClassBuilder("mdui-select")
            .AddClass(Class)
            .Build();

        [Parameter]
        public int Height { get; set; }

        protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
            => this.TryParseSelectableValueFromString(value, out result, out validationErrorMessage);

        private void SetCurrentValueAsStringArray(string?[]? value)
        {
            CurrentValue = BindConverter.TryConvertTo<TValue>(value, CultureInfo.CurrentCulture, out var result)
                ? result
                : default;
        }
    }
}
