namespace MduiBlazor
{
    public class PagerState
    {
        public PagerState(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}
