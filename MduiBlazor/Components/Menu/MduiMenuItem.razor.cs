using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MduiBlazor
{
    public partial class MduiMenuItem
    {
        protected string Classname =>
            new ClassBuilder("mdui-menu-item")
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Disabled { get; set; }
    }
}
