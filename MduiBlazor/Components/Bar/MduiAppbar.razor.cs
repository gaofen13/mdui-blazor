using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiAppbar : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-appbar")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass("mdui-appbar-fixed", Fixed)
            .AddClass($"mdui-shadow-{Shadow}", Shadow >= 0 && Shadow <= 24)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Fixed { get; set; }

        [Parameter]
        public int? Shadow { get; set; }
    }
}
