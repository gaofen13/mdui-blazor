using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiDialog : MduiComponentBase
    {
        [Parameter] public RenderFragment? ActionsContent { get; set; }
    }
}
