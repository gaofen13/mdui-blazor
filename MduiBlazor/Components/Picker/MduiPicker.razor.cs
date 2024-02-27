using System.Linq.Expressions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiPicker<TValue> : MduiComponentBase
    {
        private bool _isOpen;
        private bool _readonly;

        private readonly HashSet<MduiPickerItem<TValue>> _items = [];

        private string Classname =>
            new ClassBuilder("mdui-picker")
            .AddClass("mdui-picker-open", _isOpen)
            .AddClass($"mdui-picker-{(DirectionTop ? "top" : "bottom")}")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        private string ContentClassname =>
            new ClassBuilder("mdui-picker-content")
            .AddClass("mdui-overlay-parent", Overlay is not null)
            .Build();

        private string ContentStylelist =>
            new StyleBuilder()
            .AddStyle("max-height", MaxHeight!, !string.IsNullOrWhiteSpace(MaxHeight))
            .Build();

        internal string? DisplayValue { get; set; }

        [CascadingParameter]
        private MduiOverlay? Overlay { get; set; }

        [Parameter]
        public string? Name { get; set; }

        [Parameter]
        public string? DisplayName { get; set; }

        [Parameter]
        public bool Searchable { get; set; }

        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public TValue? Value { get; set; }

        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        [Parameter]
        public Expression<Func<TValue>>? ValueExpression { get; set; }

        [Parameter]
        public string? Placeholder { get; set; }

        [Parameter]
        public string? MaxHeight { get; set; }

        [Parameter]
        public bool Readonly { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool AutoFocus { get; set; }

        [Parameter]
        public bool Required { get; set; }

        [Parameter]
        public bool DirectionTop { get; set; }

        [Parameter]
        public bool DisableRipple { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (!Searchable)
            {
                _readonly = true;
            }
            else
            {
                _readonly = Readonly;
            }
        }

        internal void SetValue(MduiPickerItem<TValue> item)
        {
            if (item.Value?.Equals(Value) != true)
            {
                Value = item.Value;
                ValueChanged.InvokeAsync(item.Value);
                DisplayValue = item.Label;
            }
            _isOpen = false;
            StateHasChanged();
        }

        internal void AddItem(MduiPickerItem<TValue> item)
        {
            _items.Add(item);
            if (Value?.Equals(item.Value) == true)
            {
                DisplayValue = item.Label;
                StateHasChanged();
            }
        }

        internal void RemoveItem(MduiPickerItem<TValue> item)
        {
            _items.Remove(item);
        }

        private void OnInput(string input)
        {
            if (Searchable)
            {
                if (!input.Equals(DisplayValue))
                {
                    DisplayValue = input;
                }
            }
        }

        private void OnPickerFocus()
        {
            if (!Readonly)
            {
                _isOpen = true;
            }
        }

        private void OnClickBackground()
        {
            _isOpen = false;
        }
    }
}