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
    public partial class MduiIconButton
    {
        protected string Classname =>
            new ClassBuilder("mdui-btn-icon")
            .AddClass(Class)
            .Build();

        [Parameter, EditorRequired]
        public string? IconName { get; set; }

        [Parameter]
        public Color? IconColor { get; set; }
    }
}
