using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiHeadTr<TItem>
    {
        private bool Checked => Table?.Rows.Count == Table?.SelectedItems.Count();

        [CascadingParameter]
        public ITable<TItem>? Table { get; set; }

        private void OnCheckedChanged(bool @checked)
        {
            if (@checked)
            {
                Table?.SelectAllRows();
            }
            else
            {
                Table?.ClearSelectedRows();
            }
        }

        internal void ReRender()
        {
            StateHasChanged();
        }
    }
}
