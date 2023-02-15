using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiDialog : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-dialog-content")
            .AddClass(Class)
            .Build();

        [Parameter] public RenderFragment? ActionsContent { get; set; }
    }
}
