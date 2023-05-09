using MduiBlazor.Utilities;

namespace MduiBlazor
{
    public partial class MduiLayoutContent : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-layout-content")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();
    }
}
