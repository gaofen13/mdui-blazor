using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MduiBlazor
{
    public partial class MduiDivider
    {
        protected string Classname =>
            new ClassBuilder("mdui-divider")
            .AddClass("mdui-divider-inset", Inset)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Inset { get; set; }
    }
}
