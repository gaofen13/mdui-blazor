
using MduiBlazor.Components.Snackbar;
using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MduiBlazor
{
    public partial class MduiSnackbar : MduiComponentBase
    {
        private CountdownTimer? _countdownTimer;

        protected string Classname =>
          new ClassBuilder("mdui-snackbar")
            .AddClass($"mdui-snackbar-{Options.PositionX.ToDescriptionString()}-{Options.PositionY.ToDescriptionString()}")
            .AddClass(Class)
            .Build();

        [CascadingParameter]
        private MduiSnackbarContainer? SnackbarContainer { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public SnackbarOptions Options { get; set; } = new();

        [Parameter]
        public Guid SnackbarId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _countdownTimer = new CountdownTimer(Options.TimeOut)
                .OnElapsed(Close);

            await _countdownTimer.StartAsync();
        }

        public void Close() => SnackbarContainer?.RemoveSnackbar(SnackbarId);

        private void CloseBtnClicked()
        {
            Close();
            if (Options.OnCloseSnackbar is not null)
            {
                Options.OnCloseSnackbar.Invoke();
            }
        }

        void IDisposable.Dispose()
        {
            _countdownTimer?.Dispose();
            _countdownTimer = null;
            GC.SuppressFinalize(this);
        }
    }
}