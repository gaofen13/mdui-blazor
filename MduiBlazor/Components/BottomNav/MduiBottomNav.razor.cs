using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiBottomNav : MduiComponentBase
    {
        private readonly List<MduiBottomNavItem> _items = [];
        private MduiBottomNavItem? _activedItem;

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

            if (!_items.Any(i=>i.Id == item.Id))
            {
                _items.Add(item);
                if (item.Default)
                {
                    OnActivedItemChanged(item);
                }
                StateHasChanged();
            }
        }

        internal void RemoveItem(MduiBottomNavItem item)
        {
            if (_items.Any(i=>i.Id == item.Id))
            {
                _items.Remove(item);
                if (_activedItem == item)
                {
                    var activeTab = _items.FirstOrDefault();
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

        private void OnSwipe(SwipeDirection direction)
        {
            if (_items.Count > 0)
            {
                if (_activedItem == null)
                {
                    OnActivedItemChanged(_items.First());
                }
                else
                {
                    var index = _items.FindIndex(i => i.Id == _activedItem?.Id);
                    if (direction == SwipeDirection.LeftToRight)
                    {
                        if (index > 0)
                        {
                            OnActivedItemChanged(_items[index - 1]);
                        }
                    }
                    else if (direction == SwipeDirection.RightToLeft)
                    {
                        if (index + 1 != _items.Count)
                        {
                            OnActivedItemChanged(_items[index + 1]);
                        }
                    }
                }
            }
        }
    }
}