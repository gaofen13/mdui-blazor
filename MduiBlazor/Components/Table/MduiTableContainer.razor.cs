using MduiBlazor.Utilities;

namespace MduiBlazor
{
    public partial class MduiTableContainer : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-table-fluid")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

    }
}
