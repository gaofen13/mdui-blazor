namespace MduiBlazor
{
    public class SnackbarOptions
    {
        public int Timeout { get; set; } = 4000;

        public HorizontalPosition HorizontalPosition { get; set; }

        public VerticalPosition VerticalPosition { get; set; }

        public bool ShowCloseButton { get; set; }

        public string? CloseBtnColor { get; set; }
    }
}
