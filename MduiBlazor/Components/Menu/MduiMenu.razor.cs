using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MduiBlazor
{
    public partial class MduiMenu
    {
        protected string Classname =>
            new ClassBuilder("mdui-menu")
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Position { get; set; }

        [Parameter]
        public RenderFragment? ActivatorContent { get; set; }

        private readonly string id;

        public MduiMenu()
        {
            id = Guid.NewGuid().ToString("n");
        }
    }
}
