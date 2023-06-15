using MduiBlazor.Components.Snackbar;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace MduiBlazor
{
    public partial class MduiSnackbarProvider : MduiComponentBase
    {
        [Inject]
        private SnackbarService SnackbarService { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Parameter]
        public bool RemoveSnackbarOnNavigate { get; set; }

        [Parameter]
        public int MaxItemsShown { get; set; } = 10;

        [Parameter]
        public SnackbarOptions Options { get; set; } = new();

        private List<SnackbarInstance> SnackbarList { get; set; } = new();

        private Queue<SnackbarInstance> SnackbarWaitingQueue { get; set; } = new();

        protected override void OnInitialized()
        {
            SnackbarService.OnShow += ShowSnackbar;
            SnackbarService.OnShowComponent += ShowSnackbar;
            SnackbarService.OnClearAll += ClearAll;

            if (RemoveSnackbarOnNavigate)
            {
                NavigationManager.LocationChanged += ClearSnackbars;
            }
        }

        public void CloseSnackbar(Guid snackbarId)
        {
            InvokeAsync(() =>
            {
                var snackbarInstance = SnackbarList.SingleOrDefault(x => x.Id == snackbarId);

                if (snackbarInstance is not null)
                {
                    SnackbarList.Remove(snackbarInstance);
                    StateHasChanged();
                }

                if (SnackbarWaitingQueue.Any())
                {
                    ShowEnqueuedSnackbar();
                }
            });
        }

        private void ClearSnackbars(object? sender, LocationChangedEventArgs args)
        {
            InvokeAsync(() =>
            {
                SnackbarList.Clear();
                StateHasChanged();

                if (SnackbarWaitingQueue.Any())
                {
                    ShowEnqueuedSnackbar();
                }
            });
        }

        private void ShowSnackbar(RenderFragment message, SnackbarOptions? options)
        {
            InvokeAsync(() =>
            {
                var snackbar = new SnackbarInstance(options ?? Options) { MessageContent = message };

                if (SnackbarList.Count < MaxItemsShown)
                {
                    SnackbarList.Add(snackbar);
                    StateHasChanged();
                }
                else
                {
                    SnackbarWaitingQueue.Enqueue(snackbar);
                }
            });

        }

        private void ShowSnackbar(Type contentComponent, ComponentParameters? parameters, SnackbarOptions? options)
        {
            InvokeAsync(() =>
            {
                var childContent = new RenderFragment(builder =>
                {
                    var i = 0;
                    builder.OpenComponent(i++, contentComponent);
                    if (parameters is not null)
                    {
                        foreach (var parameter in parameters.Parameters)
                        {
                            builder.AddAttribute(i++, parameter.Key, parameter.Value);
                        }
                    }
                    builder.CloseComponent();
                });

                var snackbar = new SnackbarInstance(options ?? Options) { MessageContent = childContent };

                if (SnackbarList.Count < MaxItemsShown)
                {
                    SnackbarList.Add(snackbar);
                    StateHasChanged();
                }
                else
                {
                    SnackbarWaitingQueue.Enqueue(snackbar);
                }
            });
        }

        private void ShowEnqueuedSnackbar()
        {
            InvokeAsync(() =>
            {
                var snackbar = SnackbarWaitingQueue.Dequeue();

                SnackbarList.Add(snackbar);

                StateHasChanged();
            });
        }

        private void ClearAll()
        {
            InvokeAsync(() =>
            {
                SnackbarList.Clear();
                StateHasChanged();
            });
        }
    }
}
