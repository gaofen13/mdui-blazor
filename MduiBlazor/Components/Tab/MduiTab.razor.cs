using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTab : MduiComponentBase
    {
        private MduiTabItem? _activedItem;

        public List<MduiTabItem> Items {get;} = [];
        
        protected string Classname =>
            new ClassBuilder("mdui-tab")
            .AddClass($"mdui-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass("mdui-tab-scrollable", Scrollable)
            .AddClass("mdui-tab-full-width", FullWidth)
            .AddClass("mdui-tab-centered", Centered)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Scrollable { get; set; }

        [Parameter]
        public bool Centered { get; set; }

        [Parameter]
        public bool FullWidth { get; set; }

        [Parameter]
        public string? Color { get; set; }

        internal void AddItem(MduiTabItem item)
        {
            if (!Items.Any(i => i.Id == item.Id))
            {
                Items.Add(item);
                if (item.Default)
                {
                    OnActivedItemChanged(item);
                }
                StateHasChanged();
            }
        }

        internal void RemoveItem(MduiTabItem item)
        {
            if (Items.Any(i => i.Id == item.Id))
            {
                Items.Remove(item);
                if (_activedItem == item)
                {
                    var activedTab = Items.FirstOrDefault();
                    if (activedTab != null)
                    {
                        OnActivedItemChanged(activedTab);
                    }
                }
                StateHasChanged();
            }
        }

        internal void OnActivedItemChanged(MduiTabItem item)
        {
            if (!item.Disabled && _activedItem?.Id != item.Id)
            {
                _activedItem?.DisactiveItem();
                _activedItem = item;
                _activedItem.ActiveItem();
            }
        }

        public void ActiveItemByIndex(int index)
        {
            if (index >= 0 && index < Items.Count)
            {
                OnActivedItemChanged(Items[index]);
            }
        }
    }
}
