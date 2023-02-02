using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiCardHeader : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-card-header")
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? Avatar { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? SubTitle { get; set; }
    }
}
