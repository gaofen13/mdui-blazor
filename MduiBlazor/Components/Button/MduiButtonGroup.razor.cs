using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiButtonGroup : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-btn-group")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter]
        public EventCallback<SwipeDirection> OnSwipe { get; set; }
    }
}
