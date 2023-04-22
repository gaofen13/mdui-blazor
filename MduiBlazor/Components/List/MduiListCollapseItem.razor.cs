using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiListCollapseItem : MduiComponentBase, IDisposable
    {
        protected string Classname =>
            new ClassBuilder("mdui-collapse-item")
            .AddClass("mdui-collapse-item-open", Open)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        private string HeaderClassname =>
            new ClassBuilder("mdui-collapse-item-header mdui-list-item")
            .AddClass("mdui-ripple", !DisableRipple)
            .Build();

        private string BodyClassname =>
            new ClassBuilder("mdui-collapse-item-body mdui-list")
            .AddClass("mdui-list-dense", Dense)
            .Build();

        private string BodyStylelist =>
            new StyleBuilder()
            .AddStyle("max-height", "100vh", Open)
            .AddStyle("overflow-y", "auto", Open)
            .Build();

        [CascadingParameter]
        private MduiList? MduiList { get; set; }

        [Parameter]
        public bool Open { get; set; }

        [Parameter]
        public bool Dense { get; set; }

        [Parameter]
        public bool DisableRipple { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Icon { get; set; }

        [Parameter]
        public string? IconColor { get; set; }

        [Parameter]
        public bool CustomIcon { get; set; }

        protected override void OnInitialized()
        {
            MduiList?.AddSubitem(this);
            base.OnInitialized();
        }

        private void OnTitleClicked()
        {
            if (!Open && MduiList?.Accordion == true)
            {
                MduiList.CloseAllSubitems();
            }
            Open = !Open;
        }

        public void Close()
        {
            Open = false;
        }

        void IDisposable.Dispose()
        {
            MduiList?.RemoveSubitem(this);
            GC.SuppressFinalize(this);
        }
    }
}
