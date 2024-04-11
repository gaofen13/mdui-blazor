using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTable<TItem> : MduiComponentBase, ITable<TItem>, ITreeTable
    {
        private MduiHeadTr<TItem>? _header;

        public Dictionary<Guid, MduiTr<TItem>> Rows { get; } = [];

        public Dictionary<Guid, MduiTr<TItem>> SelectedRows { get; set; } = [];

        protected string Classname =>
            new ClassBuilder("mdui-table")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass("mdui-table-hoverable", Hoverable)
            .AddClass(Class)
            .Build();

        internal static TItem DefaultValue
        {
            get
            {
                TItem? item = default;
                if (item == null)
                {
                    return Activator.CreateInstance<TItem>();
                }
                else
                {
                    return item;
                }
            }
        }

        public bool HasTreeData => TreeChildren is not null;

        public List<TItem> AllItems { get; set; } = [];

        [Parameter]
        public IEnumerable<TItem>? Items { get; set; }

        [Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
        public IEnumerable<TItem> SelectedItems
#pragma warning restore BL0007 // Component parameters should be auto properties
        {
            get => SelectedRows.Values.Select(r => r.Item);
            set
            {
                if ((value?.Count() ?? 0) != SelectedRows.Values.Count)
                {
                    foreach (var row in Rows)
                    {
                        row.Value.CheckRow();
                    }
                }
            }
        }

        [Parameter]
        public EventCallback<IEnumerable<TItem>> SelectedItemsChanged { get; set; }

        [Parameter]
        public bool MultiSelection { get; set; }

        [Parameter]
        public bool Hoverable { get; set; }

        [Parameter, EditorRequired]
        public RenderFragment<TItem> Columns { get; set; } = default!;

        [Parameter]
        public RenderFragment? HeaderContent { get; set; }

        /// <summary>
        /// 缩进量，默认16px
        /// </summary>
        [Parameter]
        public int IndentSize { get; set; } = 16;

        [Parameter]
        public Func<TItem, IEnumerable<TItem>>? TreeChildren { get; set; }

        public void AddRow(MduiTr<TItem> row)
        {
            Rows.TryAdd(row.Key, row);
        }

        public void RemoveRow(MduiTr<TItem> row)
        {
            if (Rows.Remove(row.Key))
            {
                SelectedRows.Remove(row.Key);
            }
        }

        public void AddSelectedRow(MduiTr<TItem> row)
        {
            var shouldReRenderHeader = SelectedRows.Count == Rows.Count - 1;
            if (SelectedRows.TryAdd(row.Key, row))
            {
                _ = SelectedItemsChanged.InvokeAsync(SelectedItems);
                if (shouldReRenderHeader)
                {
                    _header?.ReRender();
                }
            }
        }

        public void RemoveSelectedRow(MduiTr<TItem> row)
        {
            var shouldReRenderHeader = SelectedRows.Count == Rows.Count;
            if (SelectedRows.Remove(row.Key))
            {
                _ = SelectedItemsChanged.InvokeAsync(SelectedItems);
                if (shouldReRenderHeader)
                {
                    _header?.ReRender();
                }
            }
        }

        public void SelectAllRows()
        {
            if (Rows.Count > 0)
            {
                SelectedRows = Rows.ToDictionary(); //深度复制
                _ = SelectedItemsChanged.InvokeAsync(SelectedItems);
                StateHasChanged();
            }
        }

        public void ClearSelectedRows()
        {
            if (Rows.Count > 0)
            {
                SelectedRows.Clear();
                _ = SelectedItemsChanged.InvokeAsync(SelectedItems);
                StateHasChanged();
            }
        }
    }
}