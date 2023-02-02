using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiPanel : MduiComponentBase
    {
        private MduiPanelItem? _currentOpenedItem;

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
    }
}