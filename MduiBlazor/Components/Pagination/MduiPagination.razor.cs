using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiPagination : MduiComponentBase
    {
        private int _index = 1;

        private bool PrevDisabled => PageIndex <= 1 || Total <= 0;

        private bool NexDisabled => PageIndex == Total || Total <= 0;

        [Parameter]
        public int Total { get; set; }

        [Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
        public int PageIndex
#pragma warning restore BL0007 // Component parameters should be auto properties
        {
            get => _index;
            set
            {
                if (value > 0 && value <= Total && _index != value)
                {
                    _index = value;
                    PageIndexChanged.InvokeAsync(value);
                }
            }
        }

        [Parameter]
        public EventCallback<int> PageIndexChanged { get; set; }

        private void OnClickPrev()
        {
            if (PageIndex > 1)
            {
                PageIndex--;
            }
        }

        private void OnClickNex()
        {
            if (PageIndex < Total)
            {
                PageIndex++;
            }
        }

        private void OnClickPager(int pager)
        {
            PageIndex = pager;
        }

        public void GotoPager(int pager)
        {
            if (pager >= 0 && pager <= Total)
            {
                PageIndex = pager;
            }
        }
    }
}
