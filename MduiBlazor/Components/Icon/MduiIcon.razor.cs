using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiIcon : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-icon")
            .AddClass(Name, Custom)
            .AddClass("material-icons", !Custom)
            .AddClass($"mdui-text-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .AddClass(Class)
            .Build();

        [Parameter, EditorRequired]
        public string? Name { get; set; }

        [Parameter]
        public string? Color { get; set; }

        [Parameter]
        public bool Custom { get; set; }
    }
}
