using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTreeView<TValue> : MduiComponentBase
    {
        private MduiTreeViewItem<TValue>? _activedItem;

        protected string Classname =>
            new ClassBuilder("mdui-treeview")
            .AddClass("mdui-treeview-dense", Dense)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        private string Stylelist =>
            new StyleBuilder()
            .AddStyle("max-height", MaxHeight!, !string.IsNullOrWhiteSpace(MaxHeight))
            .AddStyle(Style)
            .Build();

        [Parameter]
        public bool Dense { get; set; }

        [Parameter]
        public string? MaxHeight { get; set; }

        [Parameter]
        public bool DisableRipple { get; set; }

        [Parameter]
        public bool MultiSelection { get; set; }

        [Parameter]
        public string Indent { get; set; } = "16px";

        [Parameter]
        public TValue? SelectedValue { get; set; }

        [Parameter]
        public EventCallback<TValue?> SelectedValueChanged { get; set; }

        [Parameter]
        public IEnumerable<TValue?> SelectedValues { get; set; } = [];

        [Parameter]
        public EventCallback<IEnumerable<TValue?>> SelectedValuesChanged { get; set; }

        internal async Task OnCheckedItemsChangedAsync(MduiTreeViewItem<TValue> item)
        {
            var selectedValues = SelectedValues.ToList();
            if (item.Checked)
            {
                if (selectedValues.Contains(item.Value))
                {
                    return;
                }
                selectedValues.Add(item.Value);
            }
            else
            {
                selectedValues.Remove(item.Value);
            }
            SelectedValues = selectedValues;
            await SelectedValuesChanged.InvokeAsync(SelectedValues);
        }

        internal async Task SetActivedItemAsync(MduiTreeViewItem<TValue> item)
        {
            if (item.Actived)
            {
                _activedItem?.SetActive(false);
                _activedItem = item;
            }
            else
            {
                _activedItem = null;
            }
            SelectedValue = _activedItem is null ? default : _activedItem.Value;
            await SelectedValueChanged.InvokeAsync(SelectedValue);
        }
    }
}