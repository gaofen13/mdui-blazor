using MduiBlazor.Utilities;

namespace MduiBlazor
{
    public partial class MduiToolbarSpace : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-toolbar-spacer")
            .AddClass(Class)
            .Build();
    }
}
