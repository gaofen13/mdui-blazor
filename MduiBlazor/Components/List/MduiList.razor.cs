using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiList : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-list")
            .AddClass("mdui-list-dense", Dense)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Dense { get; set; }
    }
}
