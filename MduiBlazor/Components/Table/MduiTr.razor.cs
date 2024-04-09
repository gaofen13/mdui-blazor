using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTr<TItem>
    {
        private string TrClass =>
            new ClassBuilder()
            .AddClass("mdui-table-row-selected", Checked)
            .AddClass(Class)
            .Build();

        private bool Checked => Table?.SelectedItems?.Contains(Item) == true;

        [CascadingParameter]
        public ITable<TItem>? Table { get; set; }

        [Parameter, EditorRequired]
        public TItem Item { get; set; } = default!;

        private void OnCheckedChanged(bool @checked)
        {
            if (@checked)
            {
                Table?.AddSelectedItem(Item);
            }
            else
            {
                Table?.RemoveSelectedItem(Item);
            }
        }
    }
}
