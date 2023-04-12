﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace MduiBlazor.Shared.Shared
{
    public partial class MainLayout
    {
        bool _open;
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
                var width = await _jsModule!.InvokeAsync<int>("GetWindowWidth");
                UpdateLayout(width);
                StateHasChanged();
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

        private void OnLoactionChanged(object? sender, LocationChangedEventArgs args)
        {
            if (_windowWidth < 992 && _open)
            {
                _open = false;
                StateHasChanged();
            }
        }

        private void ReflashPage()
        {
            Navigation.NavigateTo(Navigation.Uri, true, true);
        }
    }
}
