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
            .AddClass($"mdui-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass("mdui-fab-mini", Mini)
            .AddClass("mdui-fab-hide", Hide)
            .AddClass(Class)
            .Build();

        /// <summary>
        /// The color of the component. It supports the theme colors.
        /// </summary>
        [Parameter]
        public string? Color { get; set; }

        [Parameter]
        public bool DisableRipple { get; set; }

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
        public EventCallback<MouseEventArgs> OnClick { get; set; }
    }
}
