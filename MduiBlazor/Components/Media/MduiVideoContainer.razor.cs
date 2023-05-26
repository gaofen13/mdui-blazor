using MduiBlazor.Utilities;

namespace MduiBlazor
{
    public partial class MduiVideoContainer : MduiComponentBase
    {
        protected string Classname =>
            new ClassBuilder("mdui-video-container")
            .AddClass(Class)
            .Build();
    }
}