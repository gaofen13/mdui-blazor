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
            .AddClass($"mdui-text-color-{TextColor}", !string.IsNullOrWhiteSpace(TextColor))
            .AddClass($"mdui-typo-{Typo?.ToDescriptionString()}{(Opacity ? "-opacity" : "")}", Typo != null)
            .AddClass(Class)
            .Build();

        [Parameter]
        public Typo? Typo { get; set; }

        [Parameter]
        public bool Opacity { get; set; }

        [Parameter]
        public string? TextColor { get; set; }
    }
}
