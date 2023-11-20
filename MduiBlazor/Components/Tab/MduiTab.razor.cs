using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTab : MduiComponentBase
    {
        private readonly List<MduiTabItem> _items = [];
        private MduiTabItem? _activedItem;

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

        public void AddItem(MduiTabItem item)
        {
            if (!_items.Contains(item))
            {
                _items.Add(item);
                if (item.Default)
                {
                    OnActivedItemChanged(item);
                }
                StateHasChanged();
            }
        }

        public void RemoveItem(MduiTabItem item)
        {
            if (_items.Contains(item))
            {
                _items.Remove(item);
                if (_activedItem == item)
                {
                    var activedTab = _items.FirstOrDefault();
                    if (activedTab != null)
                    {
                        OnActivedItemChanged(activedTab);
                    }
                }
                StateHasChanged();
            }
        }

        public void OnActivedItemChanged(MduiTabItem item)
        {
            if (!item.Disabled && _activedItem?.Id != item.Id)
            {
                _activedItem?.DisactiveItem();
                _activedItem = item;
                _activedItem.ActiveItem();
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
