namespace MduiBlazor
{
    public class DialogOptions
    {
        public bool ShowConfirm { get; set; }
        public string ConfirmText { get; set; } = "确定";
        public bool ShowCancel { get; set; }
        public string CancelText { get; set; } = "取消";
        public bool Modal { get; set; }
        public DialogType Type { get; set; }
    }
}