using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiButton : MduiButtonBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-btn")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass("mdui-ripple", !DisableRipple)
            .AddClass("mdui-btn-raised", !DisableRaised && !Text)
            .AddClass("mdui-btn-dense", Dense)
            .AddClass("mdui-btn-block", Block)
            .AddClass("mdui-btn-active", Actived)
            .AddClass($"mdui-text-color-{TextColor}", !string.IsNullOrWhiteSpace(TextColor))
            .AddClass($"mdui-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass($"mdui-shadow-{Shadow}", Shadow >= 0 && Shadow <= 24)
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

        [Parameter]
        public bool Text { get; set; }

        [Parameter]
        public bool DisableRaised { get; set; }
    }
}
