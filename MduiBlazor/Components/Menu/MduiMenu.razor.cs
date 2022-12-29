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
            new ClassBuilder()
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public RenderFragment? ActivatorContent { get; set; }

        [Parameter]
        public PositionX Align { get; set; } = PositionX.End;

        [Parameter]
        public PositionY Position { get; set; } = PositionY.Bottom;

        [Parameter]
        public int Gutter { get; set; } = 16;

        [Parameter]
        public bool Covered { get; set; }

        [Parameter]
        public bool Fixed { get; set; }

        [Parameter]
        public bool Hover { get; set; }

        private readonly string id;

        public MduiMenu()
        {
            id = Guid.NewGuid().ToString("n");
        }

        private string Options => BuildOptions();

        private string BuildOptions()
        {
            var builder = new StringBuilder();
            builder.Append('{');
            builder.Append($"target:'#{id}',");
            builder.Append($"position:'{BuildPositonY(Position)}',");
            builder.Append($"align:'{BuildPositonX(Align)}',");
            builder.Append($"gutter:{Gutter},");
            builder.Append($"fixed:{Fixed.ToString().ToLower()},");
            builder.Append($"covered:{Covered.ToString().ToLower()},");
            builder.Append($"subMenuTrigger:'{(Hover ? "hover" : "click")}'");
            builder.Append('}');
            return builder.ToString();
        }

        private static string BuildPositonX(PositionX position)
        {
            return position switch
            {
                PositionX.Start => "left",
                PositionX.End => "right",
                PositionX.Center => "center",
                _ => "auto",
            };
        }

        private static string BuildPositonY(PositionY position)
        {
            return position switch
            {
                PositionY.Top => "top",
                PositionY.Bottom => "bottom",
                PositionY.Middle => "center",
                _ => "auto",
            };
        }
    }
}
