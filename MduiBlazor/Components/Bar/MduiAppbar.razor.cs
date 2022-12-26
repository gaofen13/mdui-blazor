using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiAppbar
    {
        protected string Classname =>
            new ClassBuilder("mdui-appbar")
            .AddClass("mdui-appbar-fixed", Fixed)
            .AddClass("mdui-appbar-scroll-hide", ScrollHide)
            .AddClass("mdui-appbar-scroll-toolbar-hide", ScrollToolbarHide)
            .AddClass($"mdui-shadow-{Shadow}", Shadow >= 0 && Shadow <= 24)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Fixed { get; set; }

        [Parameter]
        public bool ScrollHide { get; set; }

        [Parameter]
        public bool ScrollToolbarHide { get; set; }

        [Parameter]
        public int? Shadow { get; set; }
    }
}
