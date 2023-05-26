using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiText : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder()
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass($"mdui-typo-{Typo?.ToDescriptionString()}{(Opacity ? "-opacity" : "")}", Typo != null)
            .AddClass($"mdui-text-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? Color { get; set; }

        [Parameter]
        public Typo? Typo { get; set; }

        [Parameter]
        public bool Opacity { get; set; }
    }
}
