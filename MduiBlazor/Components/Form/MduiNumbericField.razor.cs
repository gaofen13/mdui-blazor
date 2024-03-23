using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace MduiBlazor
{
    public partial class MduiNumbericField<TValue> : MduiComponentBase
    {
        private MduiInputNumber<TValue> _input = default!;

        private string Classname =>
            new ClassBuilder()
            .AddClass("mdui-field-disabled", Disabled)
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? Name { get; set; }

        [Parameter]
        public TValue Value { get; set; } = default!;

        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        [Parameter]
        public Expression<Func<TValue?>>? ValueExpression { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public string? Label { get; set; }

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
        public bool Required { get; set; }

        [Parameter]
        public bool Readonly { get; set; }

        [Parameter]
        public TValue? Min { get; set; } = default;

        [Parameter]
        public TValue Max { get; set; } = (TValue)(object)100;

        [Parameter]
        public TValue? Step { get; set; }

        private void OnValueChanged(TValue value)
        {
            if (Value?.Equals(value) != true)
            {
                Value = value;
                if (ValueChanged.HasDelegate)
                {
                    ValueChanged.InvokeAsync(value);
                }
            }
        }

        public ValueTask FocusAsync()
        {
            return _input.FocusAsync();
        }
    }
}
