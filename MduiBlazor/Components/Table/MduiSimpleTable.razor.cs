using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiSimpleTable : MduiComponentBase
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
