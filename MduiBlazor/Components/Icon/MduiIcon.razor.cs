using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiIcon
    {
        protected string Classname =>
            new ClassBuilder("mdui-icon")
            .AddClass(Name, Custom)
            .AddClass("material-icons", !Custom)
            .AddClass($"mdui-text-color-{Color?.ToDescriptionString()}", Color != null)
            .AddClass(Class)
            .Build();

        [Parameter, EditorRequired]
        public string? Name { get; set; }

        [Parameter]
        public Color? Color { get; set; }

        [Parameter]
        public bool Custom { get; set; }
    }
}
