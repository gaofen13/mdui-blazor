using MduiBlazor.Shared.Data;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace MduiBlazor.Shared.Pages
{
    public partial class MaterialICLPage
    {
        private string? _searchInput;
        private IEnumerable<MaterialIcon> _showIcons = [];

        [Inject]
        private HttpClient HttpClient { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private MaterialIconList MaterialIconList { get; set; } = new();

        [Inject]
        private DialogService? DialogService { get; set; }

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

        private void OnClickIconItem(string? name)
        {
            var parameters = new ComponentParameters
            {
                { "IconName", name??string.Empty }
            };
            DialogService?.Show<IconDemoDialog>(parameters);
        }
    }
}