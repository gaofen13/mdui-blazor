using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiToolbar : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-toolbar")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass($"mdui-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? Color { get; set; }
    }
}
