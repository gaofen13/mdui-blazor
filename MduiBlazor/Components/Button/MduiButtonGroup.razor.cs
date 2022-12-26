using MduiBlazor.Utilities;

namespace MduiBlazor
{
    public partial class MduiButtonGroup
    {
        protected string Classname =>
            new ClassBuilder("mdui-btn-group")
            .AddClass(Class)
            .Build();
    }
}
