using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiProgress : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder()
            .AddClass(Indeterminate ? "mdui-progress-indeterminate" : "mdui-progress-determinate")
            .AddClass(Class)
            .Build();

        private string Stylename =>
            new StyleBuilder()
            .AddStyle("width", $"{Value}%", !Indeterminate)
            .AddStyle(Style)
            .Build();

        [Parameter]
        public bool Indeterminate { get; set; }

        [Parameter]
        public int Value { get; set; }
    }
}