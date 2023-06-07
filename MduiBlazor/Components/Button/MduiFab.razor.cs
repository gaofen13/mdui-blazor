using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MduiBlazor
{
    public partial class MduiFab : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-fab mdui-ripple")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass("mdui-ripple", !DisableRipple)
            .AddClass("mdui-fab-mini", Mini)
            .AddClass("mdui-fab-hide", Hide)
            .AddClass($"mdui-text-color-{TextColor}", !string.IsNullOrWhiteSpace(TextColor))
            .AddClass($"mdui-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool DisableRipple { get; set; }

        [Parameter]
        public string? IconName { get; set; }

        [Parameter]
        public bool Mini { get; set; }

        [Parameter]
        public bool Hide { get; set; }

        [Parameter]
        public string? Color { get; set; }

        [Parameter]
        public string? TextColor { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }
    }
}
