using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiNavMenuCollapse : MduiComponentBase, IDisposable
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
            .AddClass("mdui-typo", UseMduiTypo)
            .Build();

        private string BodyClassname =>
            new ClassBuilder("mdui-collapse-item-body mdui-list")
            .AddClass("mdui-list-dense", Dense)
            .Build();

        private string BodyStylelist =>
            new StyleBuilder()
            .AddStyle("max-height", $"{ChildItemCount * (Dense ? 40 : 48)}px", Open)
            .Build();

        private int ChildItemCount { get; set; }

        [CascadingParameter]
        private MduiNavMenu? NavMenu { get; set; }

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
        public RenderFragment? IconContent { get; set; }

        protected override void OnInitialized()
        {
            NavMenu?.AddSubitem(this);
            base.OnInitialized();
        }

        private void OnTitleClicked()
        {
            if (!Open && NavMenu?.Accordion == true)
            {
                NavMenu.CloseAllSubitems();
            }
            Open = !Open;
        }

        public void Close()
        {
            Open = false;
        }

        public void AddItem()
        {
            ChildItemCount++;
        }

        public void RemoveItem()
        {
            ChildItemCount--;
        }

        void IDisposable.Dispose()
        {
            NavMenu?.RemoveSubitem(this);
            GC.SuppressFinalize(this);
        }
    }
}
