using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiMenuItem : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-menu-item")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Disabled { get; set; }
    }
}
