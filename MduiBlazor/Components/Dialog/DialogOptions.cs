namespace MduiBlazor
{
    public class DialogOptions
    {
        public bool Modal { get; set; }
        public bool ShowCancelButton { get; set; }
        public string CancelText { get; set; } = "取消";
        public bool ShowConfirmButton { get; set; }
        public string ConfirmText { get; set; } = "确认";
        public bool ActionsStack { get; set; }
        public string? MaxWidth { get; set; }
    }
}