using MduiBlazor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MduiBlazor
{
    public partial class MduiTableContainer
    {
        protected string Classname =>
            new ClassBuilder("mdui-table-fluid")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

    }
}
