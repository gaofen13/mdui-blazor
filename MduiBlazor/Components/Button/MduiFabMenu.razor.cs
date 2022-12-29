using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MduiBlazor
{
    public partial class MduiFabMenu
    {
        [Parameter]
        public string? Color { get; set; }

        [Parameter, EditorRequired]
        public string? IconName { get; set; }

        [Parameter]
        public string? OpenedIconName { get; set; }

        [Parameter]
        public bool Hover { get; set; }
    }
}
