using System.Diagnostics.CodeAnalysis;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiPicker<TValue, TItem> : MduiInputBase<TValue?>, IPicker<TValue>
    {
        private bool _isOpen;
        private bool _readonly;
        private MduiPickerItem<TValue>? _currentItem;
        private List<TValue?> _selectedValues = [];
        private readonly HashSet<MduiPickerItem<TValue>> _items = [];

        private string Classname =>
            new ClassBuilder("mdui-picker")
            .AddClass("mdui-picker-dense", Dense)
            .AddClass("mdui-picker-open", _isOpen)
            .AddClass($"mdui-picker-{(DirectionTop ? "top" : "bottom")}")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        private string InputClassname =>
            new ClassBuilder("mdui-input")
            .AddClass("mdui-input-searchable", _isOpen && Searchable)
            .Build();

        private string ContentClassname =>
            new ClassBuilder("mdui-picker-content")
            .AddClass(ContentClass)
            .Build();

        private string ContentStylelist =>
            new StyleBuilder()
            .AddStyle("max-height", MaxHeight!, !string.IsNullOrWhiteSpace(MaxHeight))
            .AddStyle(ContentStyle)
            .Build();

        public string? DisplayValue { get; set; }

        [Parameter]
        public bool MultiSelection { get; set; }

        [Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
        public IEnumerable<TValue?>? SelectedValues
#pragma warning restore BL0007 // Component parameters should be auto properties
        {
            get => _selectedValues;
            set
            {
                if (_selectedValues.Count != value?.Count())
                {
                    _selectedValues = value?.ToList() ?? [];
                    SelectedValuesChanged.InvokeAsync(value);
                }
            }
        }

        [Parameter]
        public EventCallback<IEnumerable<TValue?>?> SelectedValuesChanged { get; set; }

        [Parameter]
        public bool Searchable { get; set; }

        [Parameter]
        public string? Placeholder { get; set; }

        [Parameter]
        public string? MaxHeight { get; set; }

        [Parameter]
        public bool AutoFocus { get; set; }

        [Parameter]
        public bool DirectionTop { get; set; }

        [Parameter]
        public bool DisableRipple { get; set; }

        [Parameter]
        public string? ContentClass { get; set; }

        [Parameter]
        public string? ContentStyle { get; set; }

        [Parameter]
        public bool Dense { get; set; }

        [Parameter]
        public IEnumerable<TItem>? Items { get; set; }

        [Parameter]
        public Func<TItem, TValue>? ValueFunc { get; set; }

        [Parameter]
        public Func<TItem, string>? DisplayStringFunc { get; set; }

        [Parameter]
        public RenderFragment<TItem>? OptionContent { get; set; }

        protected override bool TryParseValueFromString(string? value, out TValue? result, [NotNullWhen(false)] out string? validationErrorMessage)
        {
            if (_currentItem is not null)
            {
                result = _currentItem.Value;
            }
            else
            {
                result = default;
            }

            validationErrorMessage = null;
            return true;
        }

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

        public void SetValue(MduiPickerItem<TValue> item)
        {
            if (item.Value?.Equals(Value) != true)
            {
                _currentItem = item;
                Value = item.Value;
                ValueChanged.InvokeAsync(item.Value);
                DisplayValue = item.DisplayString;
            }
            _isOpen = false;
            StateHasChanged();
        }

        public void AddItem(MduiPickerItem<TValue> item)
        {
            _items.Add(item);
            if (MultiSelection)
            {
                if (SelectedValues?.Contains(item.Value) == true)
                {
                    DisplayValue = string.Join(", ", _items.Where(i => _selectedValues.Contains(i.Value)).Select(i => i.DisplayString));
                    StateHasChanged();
                }
            }
            else
            {
                if (Value?.Equals(item.Value) == true)
                {
                    _currentItem = item;
                    DisplayValue = item.DisplayString;
                    StateHasChanged();
                }
            }
        }

        public void RemoveItem(MduiPickerItem<TValue> item)
        {
            _items.Remove(item);
            RemoveSelectedValue(item.Value);
        }

        public void AddSelectedValue(TValue? value)
        {
            if (!_selectedValues.Contains(value))
            {
                _selectedValues.Add(value);
                SelectedValuesChanged.InvokeAsync(_selectedValues);
            }
            if (!Searchable)
            {
                DisplayValue = string.Join(", ", _items.Where(i => _selectedValues.Contains(i.Value)).Select(i => i.DisplayString));
                StateHasChanged();
            }
        }

        public void RemoveSelectedValue(TValue? value)
        {
            if (_selectedValues.Contains(value))
            {
                _selectedValues.Remove(value);
                SelectedValuesChanged.InvokeAsync(_selectedValues);
            }
            if (!Searchable)
            {
                DisplayValue = string.Join(", ", _items.Where(i => _selectedValues.Contains(i.Value)).Select(i => i.DisplayString));
                StateHasChanged();
            }
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
                if (MultiSelection)
                {
                    DisplayValue = string.Join(", ", _items.Where(i => _selectedValues.Contains(i.Value)).Select(i => i.DisplayString));
                }
                else
                {
                    DisplayValue = _currentItem?.DisplayString;
                }
            }
        }

    }
}