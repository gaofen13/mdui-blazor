using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiToolbar : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-toolbar")
            .AddClass($"mdui-text-color-{TextColor}", !string.IsNullOrWhiteSpace(TextColor))
            .AddClass($"mdui-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? Color { get; set; }

        [Parameter]
        public string? TextColor { get; set; }
    }
}
