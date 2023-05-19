using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiContainer : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder()
            .AddClass($"mdui-container{(Fluid ? "-fluid" : "")}")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Fluid { get; set; }
    }
}