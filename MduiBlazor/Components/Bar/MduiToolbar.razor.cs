using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiToolbar
    {
        protected string Classname =>
            new ClassBuilder("mdui-toolbar")
            .AddClass($"mdui-color-{Color?.ToDescriptionString()}", Color != null)
            .AddClass(Class)
            .Build();

        [Parameter]
        public Color? Color { get; set; }
    }
}
