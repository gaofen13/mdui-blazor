using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiHeadTr<TItem>
    {
        private string TrClass =>
            new ClassBuilder()
            .AddClass(Class)
            .Build();

        private bool Checked => Table?.SelectedItems?.Count() == Table?.Items?.Count();

        [CascadingParameter]
        public ITable<TItem>? Table { get; set; }

        private void OnCheckedChanged(bool @checked)
        {
            if (@checked)
            {
                Table?.SelectAllItems();
            }
            else
            {
                Table?.ClearSelectedItems();
            }
        }
    }
}
