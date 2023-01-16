using MduiBlazor.Utilities;

namespace MduiBlazor
{
    public partial class MduiOverlay : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-overlay mdui-overlay-show")
            .AddClass("mdui-typo", UseMduiTypo)
            .Build();
    }
}
