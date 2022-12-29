using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiButton
    {
        protected string Classname =>
            new ClassBuilder("mdui-btn")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass("mdui-ripple", Ripple)
            .AddClass("mdui-btn-raised", !Text)
            .AddClass("mdui-btn-dense", Dense)
            .AddClass("mdui-btn-block", Block)
            .AddClass("mdui-btn-active", Actived)
            .AddClass($"mdui-shadow-{Shadow}", Shadow >= 0 && Shadow <= 24)
            .AddClass($"mdui-color-{Color}", !string.IsNullOrWhiteSpace(Color) && !Text)
            .AddClass($"mdui-text-color-{Color}", Text && !string.IsNullOrWhiteSpace(Color))
            .AddClass(Class)
            .Build();

        /// <summary>
        /// If true, the button will take up 100% of available width.
        /// </summary>
        [Parameter]
        public bool Block { get; set; }

        [Parameter]
        public int? Shadow { get; set; }

        [Parameter]
        public bool Actived { get; set; }
    }
}
