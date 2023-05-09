using MduiBlazor.Shared.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace MduiBlazor.Shared.Pages
{
    public partial class MaterialIcons
    {
        private bool _show;
        private string? _searchInput;
        private string? _selectedIcon;
        private IEnumerable<MaterialIcon> _showIcons = Enumerable.Empty<MaterialIcon>();

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        [Inject]
        private HttpClient HttpClient { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private MaterialIconList MaterialIconList { get; set; } = new();

        private MarkupString? IconCodeContents { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (MaterialIconList.IconList?.Any() != true)
            {
                if (HttpClient.BaseAddress == null)
                {
                    HttpClient.BaseAddress = new Uri(Navigation.BaseUri);
                }
                MaterialIconList.IconList = await HttpClient.GetFromJsonAsync<MaterialIcon[]>("_content/MduiBlazor.Shared/data/material_icon.json");
            }

            if (MaterialIconList.IconList != null)
            {
                _showIcons = MaterialIconList.IconList;
            }
        }

        private void OnSearchChanged(string value)
        {
            if (MaterialIconList.IconList?.Any() == true)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _showIcons = MaterialIconList.IconList;
                }
                else
                {
                    _showIcons = MaterialIconList.IconList.Where(i => i.Name?.Contains(value.Trim()) == true);
                }
                StateHasChanged();
            }
        }

        private async Task OnClickIconItem(string? name)
        {
            var codeString = $"<MduiIcon Name=\"{name}\" />";
            var res = await JSRuntime.InvokeAsync<string>("HighlightCode", codeString);
            if (!string.IsNullOrWhiteSpace(res))
            {
                IconCodeContents = new MarkupString(res);
            }
            _selectedIcon = name;
            _show = true;
        }
    }
}