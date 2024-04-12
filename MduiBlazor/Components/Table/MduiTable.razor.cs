using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTable<TItem> : MduiComponentBase, ITable<TItem>, ITreeTable
    {
        private MduiHeadTr<TItem>? _header;
        private List<TItem> _selectedItems = [];

        public Dictionary<Guid, MduiTr<TItem>> Rows { get; } = [];

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

        [Parameter]
        public IEnumerable<TItem>? Items { get; set; }

        [Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
        public IEnumerable<TItem> SelectedItems
#pragma warning restore BL0007 // Component parameters should be auto properties
        {
            get => _selectedItems;
            set
            {
                if (_selectedItems.Count != value?.Count())
                {
                    _selectedItems = value?.ToList() ?? [];
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
            var shouldReRenderHeader = _selectedItems.Count == Rows.Count;
            if (Rows.TryAdd(row.Key, row) && shouldReRenderHeader)
            {
                _header?.ReRender();
            }
        }

        public void RemoveRow(MduiTr<TItem> row)
        {
            if (Rows.Remove(row.Key))
            {
                RemoveSelectedItem(row.Item);
            }
        }

        public void AddSelectedItem(TItem item)
        {
            if (!_selectedItems.Contains(item))
            {
                _selectedItems.Add(item);
                _ = SelectedItemsChanged.InvokeAsync(_selectedItems);

                if (_selectedItems.Count == Rows.Count)
                {
                    _header?.ReRender();
                }
            }
        }

        public void RemoveSelectedItem(TItem item)
        {
            if (_selectedItems.Contains(item))
            {
                _selectedItems.Remove(item);
                _ = SelectedItemsChanged.InvokeAsync(_selectedItems);

                if (_selectedItems.Count + 1 == Rows.Count)
                {
                    _header?.ReRender();
                }
            }
        }

        public void SelectAllRows()
        {
            if (Rows.Count > 0)
            {
                _selectedItems = Rows.Values.Select(r => r.Item).ToList(); //深度复制
                _ = SelectedItemsChanged.InvokeAsync(_selectedItems);
                StateHasChanged();
            }
        }

        public void ClearSelectedRows()
        {
            if (Rows.Count > 0)
            {
                _selectedItems.Clear();
                _ = SelectedItemsChanged.InvokeAsync(_selectedItems);
                StateHasChanged();
            }
        }
    }
}