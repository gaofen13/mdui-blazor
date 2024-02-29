using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiListItem : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-list-item")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass("mdui-ripple", !DisableRipple)
            .AddClass("mdui-list-item-active", Active)
            .AddClass(Class)
            .Build();

        private string Stylelist =>
            new StyleBuilder()
            .AddStyle("padding-left", $"{Level * 36}px", Parent is not null)
            .AddStyle(Style)
            .Build();

        internal int Level { get; set; }

        [CascadingParameter]
        private MduiListCollapse? Parent { get; set; }

        [Parameter]
        public bool DisableRipple { get; set; }

        [Parameter]
        public bool Active { get; set; }

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
