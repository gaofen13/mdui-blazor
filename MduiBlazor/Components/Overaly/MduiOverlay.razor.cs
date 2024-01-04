using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MduiBlazor
{
    public partial class MduiOverlay : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-overlay")
            .AddClass("mdui-overlay-transparent", Transparent)
            .AddClass("mdui-overlay-show", Visible)
            .AddClass("mdui-typo", UseMduiTypo)
            .Build();

        [Parameter]
        public bool Visible { get; set; }

        [Parameter]
        public bool Transparent { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnBackgroundClick { get; set; }
    }
}
