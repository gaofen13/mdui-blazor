using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiImage : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder()
            .AddClass("mdui-img-fluid", Fluid)
            .AddClass("mdui-img-rounded", Rounded)
            .AddClass("mdui-img-circle", Circle)
            .AddClass(Class)
            .Build();

        [Parameter, EditorRequired]
        public string? Src { get; set; }

        [Parameter]
        public bool Fluid { get; set; }

        [Parameter]
        public bool Rounded { get; set; }

        [Parameter]
        public bool Circle { get; set; }
    }
}