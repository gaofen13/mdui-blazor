using Microsoft.AspNetCore.Components;

namespace MduiBlazor.Components.Snackbar
{
    internal class SnackbarInstance
    {
        public SnackbarInstance(SnackbarOptions settings)
        {
            Options = settings;
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public SnackbarOptions? Options { get; set; }

        public RenderFragment? MessageContent { get; set; }
    }
}
