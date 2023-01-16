using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace MduiBlazor
{
    public partial class MduiNavMenuItem : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-list-item mdui-ripple")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? Href { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public NavLinkMatch Match { get; set; } = NavLinkMatch.Prefix;
    }
}
