using MduiBlazor.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MduiBlazor
{
    public partial class MduiToolbarSpace
    {
        protected string Classname =>
            new ClassBuilder("mdui-toolbar-spacer")
            .AddClass(Class)
            .Build();
    }
}
