using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace MduiBlazor
{
    public partial class MduiNavMenuItem : MduiComponentBase, IDisposable
    {
        protected string Classname =>
            new ClassBuilder("mdui-list-item")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass("mdui-ripple", !DisableRipple)
            .AddClass(Class)
            .Build();

        [CascadingParameter]
        private MduiNavMenuCollapseItem? CollapseList { get; set; }

        [Parameter]
        public bool DisableRipple { get; set; }

        [Parameter]
        public string? Href { get; set; }

        [Parameter]
        public NavLinkMatch Match { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public string? IconColor { get; set; }

        [Parameter]
        public bool CustomIcon { get; set; }

        protected override void OnInitialized()
        {
            CollapseList?.AddItem();
            base.OnInitialized();
        }

        void IDisposable.Dispose()
        {
            CollapseList?.RemoveItem();
            GC.SuppressFinalize(this);
        }
    }
}
