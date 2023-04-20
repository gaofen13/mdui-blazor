using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiListItemAvatar : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-list-item-avatar")
            .AddClass(Class)
            .Build();

        [Parameter]
        public string? ImgSrc { get; set; }

        [Parameter]
        public string? IconName { get; set; }

        [Parameter]
        public string? IconColor { get; set; }

        [Parameter]
        public bool CustomIcon { get; set; }
    }
}
