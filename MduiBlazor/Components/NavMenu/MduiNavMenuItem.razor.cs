using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace MduiBlazor
{
    public partial class MduiNavMenuItem : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-list-item")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass("mdui-ripple", !DisableRipple)
            .AddClass(Class)
            .Build();

        private string Stylelist =>
            new StyleBuilder()
            .AddStyle("padding-left", $"{Level * 36}px", Parent is not null)
            .AddStyle(Style)
            .Build();

        internal int Level { get; set; }

        private bool HasIcon => Icon is not null || IconContent is not null;

        private int PaddingLeft => HasIcon ? Level * 36 : (Level + 1) * 36;

        [CascadingParameter]
        private MduiNavMenuCollapse? Parent { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public bool DisableRipple { get; set; }

        [Parameter]
        public string? Href { get; set; }

        [Parameter]
        public NavLinkMatch Match { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public RenderFragment? IconContent { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Parent is not null)
            {
                Level = Parent.Level + 1;
            }
        }
    }
}
