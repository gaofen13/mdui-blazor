using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiPanel : MduiComponentBase
    {
        private readonly List<MduiPanelItem> _items = [];

        protected string Classname =>
            new ClassBuilder("mdui-panel")
            .AddClass("mdui-panel-gapless", Gapless)
            .AddClass("mdui-panel-popout", Popout)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Accordion { get; set; }

        [Parameter]
        public bool Popout { get; set; }

        [Parameter]
        public bool Gapless { get; set; }

        internal void AddItem(MduiPanelItem item)
        {
            if (!_items.Contains(item))
            {
                _items.Add(item);
            }
        }

        internal void RemoveItem(MduiPanelItem item)
        {
            _items.Remove(item);
        }

        internal void CloseAllItems()
        {
            var openedItems = _items.Where(i => i.Open == true);
            foreach (var openedItem in openedItems)
            {
                openedItem.ClosePanel();
                openedItem.ReRender();
            }
        }
    }
}