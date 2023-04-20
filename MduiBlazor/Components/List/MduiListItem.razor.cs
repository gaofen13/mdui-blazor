using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace MduiBlazor
{
    public partial class MduiListItem : MduiComponentBase, IDisposable
    {
        protected string Classname =>
            new ClassBuilder("mdui-list-item")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass("mdui-ripple", !DisableRipple)
            .AddClass(Class)
            .Build();

        [CascadingParameter]
        private MduiCollapseItem? CollapseList { get; set; }

        [CascadingParameter]
        private MduiList? MduiList { get; set; }

        [Parameter]
        public bool DisableRipple { get; set; }

        [Parameter]
        public string? Href { get; set; }

        [Parameter]
        public NavLinkMatch Match { get; set; }

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
