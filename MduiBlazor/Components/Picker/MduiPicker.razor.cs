using System.Linq.Expressions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiPicker<TValue> : MduiComponentBase
    {
        private bool _isOpen;
        private bool _readonly;
        private MduiPickerItem<TValue>? _currentItem;
        private readonly HashSet<MduiPickerItem<TValue>> _items = [];

        private string Classname =>
            new ClassBuilder("mdui-picker")
            .AddClass("mdui-picker-open", _isOpen)
            .AddClass($"mdui-picker-{(DirectionTop ? "top" : "bottom")}")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        private static string ContentClassname =>
            new ClassBuilder("mdui-picker-content")
            .Build();

        private string ContentStylelist =>
            new StyleBuilder()
            .AddStyle("max-height", MaxHeight!, !string.IsNullOrWhiteSpace(MaxHeight))
            .Build();

        internal string? DisplayValue { get; set; }

        [Parameter]
        public bool Searchable { get; set; }

        [Parameter]
        public string? Label { get; set; }

        [Parameter]
        public TValue? Value { get; set; }

        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

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
                _currentItem = item;
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
                _currentItem = item;
                DisplayValue = item.Label;
                StateHasChanged();
            }
        }

        internal void RemoveItem(MduiPickerItem<TValue> item)
        {
            _items.Remove(item);
        }

        private void OnInput(ChangeEventArgs args)
        {
            if (Searchable)
            {
                var inputValue = args.Value?.ToString();
                if (inputValue?.Equals(DisplayValue) != true)
                {
                    DisplayValue = inputValue;
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
            if (Searchable)
            {
                if (DisplayValue != _currentItem?.Label)
                {
                    DisplayValue = _currentItem?.Label;
                }
            }
        }
    }
}