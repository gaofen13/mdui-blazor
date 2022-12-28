using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MduiBlazor
{
    public partial class MduiTable
    {
        protected string Classname =>
            new ClassBuilder("mdui-table")
            .AddClass("mdui-table-hoverable", Hoverable)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Hoverable { get; set; }
    }
}
