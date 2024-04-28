using System.Diagnostics.CodeAnalysis;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTreePicker<TValue, TItem> : MduiInputBase<TValue?>, ITreePicker<TValue>
    {
        private bool _isOpen;
        private MduiTreePickerItem<TValue>? _currentItem;
        private List<TValue?> _selectedValues = [];
        private readonly HashSet<MduiTreePickerItem<TValue>> _items = [];

        private string Classname =>
            new ClassBuilder("mdui-picker mdui-tree-picker")
            .AddClass("mdui-picker-dense", Dense)
            .AddClass("mdui-picker-open", _isOpen)
            .AddClass($"mdui-picker-{(DirectionTop ? "top" : "bottom")}")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        private string ContentClassname =>
            new ClassBuilder("mdui-picker-content")
            .AddClass("mdui-picker-appbar", Appbar is not null)
            .AddClass("mdui-picker-drawer", Drawer is not null)
            .AddClass("mdui-picker-snackbar", SnackbarProvider is not null)
            .AddClass("mdui-picker-overlay", Overlay is not null)
            .AddClass(ContentClass)
            .Build();

        private string ContentStylelist =>
            new StyleBuilder()
            .AddStyle("max-height", MaxHeight!, !string.IsNullOrWhiteSpace(MaxHeight))
            .AddStyle(ContentStyle)
            .Build();

        public string? DisplayValue { get; set; }

        [CascadingParameter(Name = "Appbar")]
        private MduiAppbar? Appbar { get; set; }

        [CascadingParameter(Name = "Drawer")]
        private MduiDrawer? Drawer { get; set; }

        [CascadingParameter(Name = "Overlay")]
        private MduiOverlay? Overlay { get; set; }

        [CascadingParameter(Name = "SnackbarProvider")]
        private MduiSnackbarProvider? SnackbarProvider { get; set; }

        [Parameter]
        public string Indent { get; set; } = "16px";

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
        public Func<TItem, IEnumerable<TItem>?>? ChildrenFunc { get; set; }

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

        public void SetValue(MduiTreePickerItem<TValue> item)
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

        public void AddItem(MduiTreePickerItem<TValue> item)
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

        public void RemoveItem(MduiTreePickerItem<TValue> item)
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
                DisplayValue = string.Join(", ", _items.Where(i => _selectedValues.Contains(i.Value)).Select(i => i.DisplayString));
                StateHasChanged();
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