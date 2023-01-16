using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTable : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-table")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass("mdui-table-hoverable", Hoverable)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Hoverable { get; set; }
    }
}
