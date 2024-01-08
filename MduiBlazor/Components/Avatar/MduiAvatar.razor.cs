using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiAvatar : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-avatar")
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? ImgSrc { get; set; }
    }
}