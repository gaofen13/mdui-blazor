using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTab : MduiComponentBase
    {
        private readonly List<MduiTabItem> _items = new();
        private MduiTabItem? _activeTabItem;

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

        public void AddTab(MduiTabItem item)
        {
            if (!_items.Contains(item))
            {
                _items.Add(item);
                if (item.Default)
                {
                    OnActiveTabChanged(item);
                }
                StateHasChanged();
            }
        }

        public void RemoveTab(MduiTabItem item)
        {
            if (_items.Contains(item))
            {
                _items.Remove(item);
                if (_activeTabItem == item)
                {
                    var activeTab = _items.FirstOrDefault();
                    if (activeTab != null)
                    {
                        OnActiveTabChanged(activeTab);
                    }
                }
                StateHasChanged();
            }
        }

        private void OnActiveTabChanged(MduiTabItem item)
        {
            if (!item.Disabled && _activeTabItem != item)
            {
                _activeTabItem?.DisactiveTab();
                _activeTabItem = item;
                _activeTabItem.ActiveTab();
            }
        }
    }
}
