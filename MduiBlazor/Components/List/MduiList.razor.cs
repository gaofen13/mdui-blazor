using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiList : MduiComponentBase
    {
        private readonly List<MduiListCollapse> _subItems = new();

        protected string Classname =>
            new ClassBuilder("mdui-list")
            .AddClass("mdui-list-dense", Dense)
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass(Class)
            .Build();

        [Parameter]
        public bool Dense { get; set; }

        [Parameter]
        public bool Accordion { get; set; }

        public void AddSubitem(MduiListCollapse item)
        {
            if (!_subItems.Contains(item))
            {
                _subItems.Add(item);
            }
        }

        public void RemoveSubitem(MduiListCollapse item)
        {
            if (_subItems.Contains(item))
            {
                _subItems.Remove(item);
            }
        }

        public void CloseAllSubitems()
        {
            var openedItems = _subItems.Where(i => i.Open);
            if (openedItems.Any())
            {
                foreach (var item in openedItems)
                {
                    item.Close();
                }
                StateHasChanged();
            }
        }
    }
}
