using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTh : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder()
            .AddClass("mdui-table-col-numeric", AlignRight)
            .Build();

        [Parameter]
        public bool AlignRight { get; set; }
    }
}