using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiBottomNav : MduiComponentBase
    {
        private MduiBottomNavItem? _activedItem;

        public List<MduiBottomNavItem> Items {get;} = [];

        protected string Classname =>
            new ClassBuilder("mdui-bottom-nav")
            .AddClass("mdui-bottom-nav-fixed", Fixed)
            .AddClass("mdui-bottom-nav-text-auto", TitleAuto)
            .AddClass($"mdui-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Fixed { get; set; }

        [Parameter]
        public bool TitleAuto { get; set; }

        [Parameter]
        public string? Color { get; set; }

        internal void AddItem(MduiBottomNavItem item)
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

        internal void RemoveItem(MduiBottomNavItem item)
        {
            if (Items.Any(i => i.Id == item.Id))
            {
                Items.Remove(item);
                if (_activedItem == item)
                {
                    var activeTab = Items.FirstOrDefault();
                    if (activeTab != null)
                    {
                        OnActivedItemChanged(activeTab);
                    }
                }
                StateHasChanged();
            }
        }

        internal void OnActivedItemChanged(MduiBottomNavItem item)
        {
            if (_activedItem?.Id != item.Id)
            {
                _activedItem?.Disactive();
                _activedItem = item;
                _activedItem.Active();
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