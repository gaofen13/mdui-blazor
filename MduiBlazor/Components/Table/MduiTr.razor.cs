using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTr<TItem> : MduiComponentBase, IDisposable
    {
        private string Classname =>
            new ClassBuilder()
            .AddClass("mdui-table-row-selected", Checked)
            .AddClass(Class)
            .Build();

        private bool Checked => Table?.SelectedRows?.ContainsKey(Key) == true;

        internal Guid Key { get; } = Guid.NewGuid();

        [CascadingParameter]
        public ITable<TItem>? Table { get; set; }

        [Parameter, EditorRequired]
        public TItem Item { get; set; } = default!;

        protected override void OnInitialized()
        {
            if (Table?.MultiSelection == true)
            {
                Table.AddRow(this);
                CheckRow();
            }
            base.OnInitialized();
        }

        internal void CheckRow()
        {
            if (Table?.SelectedItems.Contains(Item) == true)
            {
                Table.AddSelectedRow(this);
            }
        }

        private void OnCheckedChanged(bool @checked)
        {
            if (@checked)
            {
                Table?.AddSelectedRow(this);
            }
            else
            {
                Table?.RemoveSelectedRow(this);
            }
        }

        void IDisposable.Dispose()
        {
            if (Table?.MultiSelection == true)
            {
                Table.RemoveRow(this);
            }
            GC.SuppressFinalize(this);
        }
    }
}
