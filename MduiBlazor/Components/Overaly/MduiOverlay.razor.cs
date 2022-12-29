using MduiBlazor.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MduiBlazor
{
    public partial class MduiOverlay
    {
        protected string Classname =>
            new ClassBuilder("mdui-overlay mdui-overlay-show")
            .AddClass("mdui-typo", UseMduiTypo)
            .Build();
    }
}
