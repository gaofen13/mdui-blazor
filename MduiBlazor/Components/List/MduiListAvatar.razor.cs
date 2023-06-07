using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiListAvatar : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-list-item-avatar")
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? ImgSrc { get; set; }
    }
}
