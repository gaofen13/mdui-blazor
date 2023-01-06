using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiFab
    {
        protected string Classname =>
            new ClassBuilder("mdui-fab mdui-ripple")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass($"mdui-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass("mdui-fab-mini", Mini)
            .AddClass("mdui-fab-hide", Hide)
            .AddClass("mdui-fab-fixed", Fixed)
            .AddClass(Class)
            .Build();

        [Parameter, EditorRequired]
        public string? Icon { get; set; }

        [Parameter]
        public string? IconClass { get; set; }

        [Parameter]
        public string? IconColor { get; set; }

        [Parameter]
        public bool Mini { get; set; }

        [Parameter]
        public bool Hide { get; set; }

        [Parameter]
        public bool Fixed { get; set; }
    }
}
