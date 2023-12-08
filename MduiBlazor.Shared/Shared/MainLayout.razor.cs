using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace MduiBlazor.Shared.Shared
{
    public partial class MainLayout
    {
        private bool _open;
        private bool _showThemeDialog;
        private bool _hideScrollToTop = true;
        private PrimaryColor _primaryColor = PrimaryColor.Indigo;
        private AccentColor _accentColor = AccentColor.Pink;
        private DarkThemeMode _themeMode = DarkThemeMode.Auto;
        private int _windowWidth;
        private IJSObjectReference? _jsModule;
        private DotNetObjectReference<MainLayout>? _objectReference;
        ErrorBoundary? errorBoundary;

        private bool Persistent => _windowWidth >= 992;

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        protected override void OnInitialized()
        {
            Navigation!.LocationChanged += OnLoactionChanged;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _objectReference = DotNetObjectReference.Create(this);
                _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                     "./_content/MduiBlazor.Shared/Shared/MainLayout.razor.js");
                await _jsModule!.InvokeVoidAsync("AddWindowWidthListener", _objectReference);
                await _jsModule!.InvokeVoidAsync("AddScrollListener", _objectReference, "body");
                var width = await _jsModule!.InvokeAsync<int>("GetWindowWidth");
                UpdateLayout(width);
                var scrollTop = await _jsModule!.InvokeAsync<double>("GetScrollTop");
                UpdateScrollToTop(scrollTop);
            }
        }

        protected override void OnParametersSet()
        {
            errorBoundary?.Recover();
            base.OnParametersSet();
        }

        [JSInvokable]
        public void UpdateLayout(int width)
        {
            _windowWidth = width;
            if (width >= 992)
            {
                if (!_open)
                {
                    _open = true;
                    StateHasChanged();
                }
            }
            else
            {
                if (_open)
                {
                    _open = false;
                    StateHasChanged();
                }
            }
        }

        [JSInvokable]
        public void UpdateScrollToTop(double scrollHeight)
        {
            if (scrollHeight >= 500)
            {
                if (_hideScrollToTop)
                {
                    _hideScrollToTop = false;
                    StateHasChanged();
                }
            }
            else
            {
                if (!_hideScrollToTop)
                {
                    _hideScrollToTop = true;
                    StateHasChanged();
                }
            }
        }

        private async Task ScrollToTopAsync()
        {
            await _jsModule!.InvokeVoidAsync("ScrollToTop");
        }

        private void OnLoactionChanged(object? sender, LocationChangedEventArgs args)
        {
            if (_windowWidth < 992 && _open)
            {
                _open = false;
                StateHasChanged();
            }
        }

        private void ToggleThemeDialog()
        {
            _showThemeDialog = !_showThemeDialog;
        }

        private void ResetTheme()
        {
            _primaryColor = PrimaryColor.Indigo;
            _accentColor = AccentColor.Pink;
            _themeMode = DarkThemeMode.Auto;
        }
    }
}
