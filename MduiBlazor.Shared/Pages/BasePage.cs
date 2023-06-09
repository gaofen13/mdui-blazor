using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MduiBlazor.Shared.Pages
{
    public class BasePage : ComponentBase
    {
        private IJSObjectReference? _jsModule;

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                     "./_content/MduiBlazor.Shared/Shared/MainLayout.razor.js");
            }
        }

        protected async Task<bool> ScrollToElementById(string elementId)
        {
            return await _jsModule!.InvokeAsync<bool>("ScrollToElementById", elementId);
        }
    }
}