using Microsoft.AspNetCore.Components;

namespace MduiBlazor.Components.Snackbar
{
    internal class SnackbarInstance(SnackbarOptions settings, string? color = null)
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? Color { get; set; } = color;

        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public SnackbarOptions? Options { get; set; } = settings;

        public RenderFragment? MessageContent { get; set; }
    }
}
