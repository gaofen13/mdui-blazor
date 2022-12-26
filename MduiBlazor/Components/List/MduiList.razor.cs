using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MduiBlazor
{
    public partial class MduiList
    {
        protected string Classname =>
            new ClassBuilder("mdui-list")
            .AddClass("mdui-list-dense", Dense)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Dense { get; set; }
    }
}
