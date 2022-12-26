using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MduiBlazor
{
    public partial class MduiText
    {
        protected string Classname =>
            new ClassBuilder("mdui-icon")
            .AddClass($"mdui-typo-{Typo?.ToDescriptionString()}{(Opacity ? "-opacity" : "")}", Typo != null)
            .AddClass($"mdui-text-color-{Color?.ToDescriptionString()}", Color != null)
            .AddClass(Class)
            .Build();

        [Parameter]
        public Color? Color { get; set; }

        [Parameter]
        public Typo? Typo { get; set; }

        [Parameter]
        public bool Opacity { get; set; }
    }
}
