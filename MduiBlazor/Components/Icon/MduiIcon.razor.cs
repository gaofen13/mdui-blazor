using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiIcon : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-icon")
            .AddClass($"mdui-text-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass("material-icons", !Custom)
            .AddClass(Name, Custom)
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? Name { get; set; }

        [Parameter]
        public bool Custom { get; set; }

        [Parameter]
        public string? Color { get; set; }
    }
}
