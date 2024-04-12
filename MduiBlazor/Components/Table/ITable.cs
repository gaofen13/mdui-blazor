using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public interface ITable<TItem>
    {
        bool MultiSelection { get; set; }

        Dictionary<Guid, MduiTr<TItem>> Rows { get; }

        IEnumerable<TItem> SelectedItems { get; set; }

        Func<TItem, IEnumerable<TItem>>? TreeChildren { get; set; }

        RenderFragment<TItem> Columns { get; set; }

        void AddRow(MduiTr<TItem> item);

        void RemoveRow(MduiTr<TItem> item);

        void AddSelectedItem(TItem item);

        void RemoveSelectedItem(TItem item);

        void SelectAllRows();

        void ClearSelectedRows();
    }
}
