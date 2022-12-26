using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MduiBlazor
{
    public partial class MduiFabButton
    {
        protected string Classname =>
            new ClassBuilder("mdui-fab mdui-ripple")
            .AddClass($"mdui-color-{Color?.ToDescriptionString()}", Color != null)
            .AddClass("mdui-fab-mini", Mini)
            .AddClass("mdui-fab-hide", Hide)
            .AddClass("mdui-fab-fixed", Fixed)
            .AddClass(Class)
            .Build(); 
        
        [Parameter]
        public Color? Color { get; set; }

        [Parameter, EditorRequired]
        public string? IconName { get; set; }

        [Parameter]
        public Color? IconColor { get; set; }

        [Parameter]
        public bool Mini { get; set; }

        [Parameter]
        public bool Hide { get; set; }

        [Parameter]
        public bool Fixed { get; set; }
    }
}
