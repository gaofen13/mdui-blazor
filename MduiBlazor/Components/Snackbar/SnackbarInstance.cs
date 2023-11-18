using Microsoft.AspNetCore.Components;

namespace MduiBlazor.Components.Snackbar
{
    internal class SnackbarInstance
    {
        public SnackbarInstance(SnackbarOptions settings, string? color = null)
        {
            Options = settings;
            Color = color;
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public string? Color { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public SnackbarOptions? Options { get; set; }

        public RenderFragment? MessageContent { get; set; }
    }
}
