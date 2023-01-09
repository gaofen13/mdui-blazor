using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace MduiBlazor
{
    public partial class MduiRadioGroup<TValue>
    {
        private MduiRadio<TValue>? _checkedRadio;

        private readonly string _defaultGroupName = Guid.NewGuid().ToString("N");

        public string RadioName => Name ?? _defaultGroupName;

        private string RadioGroupClass =>
          new ClassBuilder("mdui-radio-group")
            .AddClass($"mdui-group-vertical", Vertical)
            .AddClass(FieldClass)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Vertical { get; set; }

        [Parameter]
        public string? Name { get; set; }

        protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
            => this.TryParseSelectableValueFromString(value, out result, out validationErrorMessage);

        public void OnCheckedRadioChanged(MduiRadio<TValue> radio)
        {
            _checkedRadio = radio;
            if (!EqualityComparer<TValue>.Default.Equals(radio.Value, CurrentValue))
            {
                CurrentValue = radio.Value;
            }
        }
    }
}
