using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiBottomNav : MduiComponentBase
    {
        private readonly List<MduiBottomNavItem> _items = new();
        private MduiBottomNavItem? _activeItem;

        protected string Classname =>
            new ClassBuilder("mdui-bottom-nav")
            .AddClass($"mdui-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass("mdui-bottom-nav-fixed", Fixed)
            .AddClass("mdui-bottom-nav-text-auto", TextAuto)
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? Color { get; set; }

        [Parameter]
        public bool Fixed { get; set; }

        [Parameter]
        public bool TextAuto { get; set; }

        internal void AddItem(MduiBottomNavItem item)
        {

            if (!_items.Contains(item))
            {
                _items.Add(item);
                if (item.Default)
                {
                    OnActiveItemChanged(item);
                }
                StateHasChanged();
            }
        }

        internal void RemoveItem(MduiBottomNavItem item)
        {
            if (_items.Contains(item))
            {
                _items.Remove(item);
                if (_activeItem == item)
                {
                    var activeTab = _items.FirstOrDefault();
                    if (activeTab != null)
                    {
                        OnActiveItemChanged(activeTab);
                    }
                }
                StateHasChanged();
            }
        }

        internal void OnActiveItemChanged(MduiBottomNavItem item)
        {
            if (_activeItem != item)
            {
                _activeItem?.Disactive();
                _activeItem = item;
                _activeItem.Active();
            }
        }
    }
}