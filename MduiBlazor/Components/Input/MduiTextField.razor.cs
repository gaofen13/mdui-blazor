using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace MduiBlazor
{
    public partial class MduiTextField : MduiComponentBase
    {
        private MduiField? _field;
        private string? _value;

        private string Classname =>
            new ClassBuilder()
            .AddClass("mdui-textfield-has-bottom", MaxLength > 0)
            .AddClass("mdui-textfield-floating-label", FloatingLabel)
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? Name { get; set; }

        [Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
        public string? Value
#pragma warning restore BL0007 // Component parameters should be auto properties
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    if (value?.Length > 0)
                    {
                        _field?.SetNotEmpty();
                    }
                    else
                    {
                        _field?.RemoveNotEmpty();
                    }
                    if (ValueChanged.HasDelegate)
                    {
                        ValueChanged.InvokeAsync(value);
                    }
                }
            }
        }

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        [Parameter]
        public Expression<Func<string>>? ValueExpression { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public bool LabelRequired { get; set; }

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
        public bool Required { get; set; }

        [Parameter]
        public bool Readonly { get; set; }

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

        [Parameter]
        public EventCallback<string> OnInput { get; set; }

        private void OnValueChanged(string? value)
        {
            Value = value;
        }
    }
}
