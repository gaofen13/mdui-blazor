using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiMenuItem : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-menu-item")
            .AddClass("mdui-menu-item-active", Actived)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Actived { get; set; }

        [Parameter]
        public string? HelperText { get; set; }

        [Parameter]
        public bool Disabled { get; set; }
    }
}
