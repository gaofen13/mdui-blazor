using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTable<TItem> : MduiComponentBase, ITable<TItem>
    {
        private IEnumerable<TItem> _items = Enumerable.Empty<TItem>();
        private List<TItem> _selectedItems = [];

        protected string Classname =>
            new ClassBuilder("mdui-table")
            .AddClass("mdui-typo", UseMduiTypo)
            .AddClass("mdui-table-hoverable", Hoverable)
            .AddClass(Class)
            .Build();

        private static TItem DefaultValue
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

        [Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
        public IEnumerable<TItem>? Items
#pragma warning restore BL0007 // Component parameters should be auto properties
        {
            get => _items;
            set
            {
                if (!EqualityComparer<IEnumerable<TItem>>.Default.Equals(value, _items))
                {
                    if (value == null)
                    {
                        _items = Enumerable.Empty<TItem>();
                    }
                    else
                    {
                        _items = value;
                    }
                    if (_selectedItems.Count > 0)
                    {
                        _selectedItems.Clear();
                        SelectedItemsChanged.InvokeAsync(_selectedItems);
                    }
                }
            }
        }

        [Parameter]
#pragma warning disable BL0007 // Component parameters should be auto properties
        public IEnumerable<TItem> SelectedItems
#pragma warning restore BL0007 // Component parameters should be auto properties
        {
            get => _selectedItems;
            set
            {
                if (!EqualityComparer<IEnumerable<TItem>>.Default.Equals(value, _selectedItems))
                {
                    _selectedItems = value?.ToList() ?? [];
                    _ = SelectedItemsChanged.InvokeAsync(_selectedItems);
                }
            }
        }

        [Parameter]
        public EventCallback<IEnumerable<TItem>> SelectedItemsChanged { get; set; }

        [Parameter]
        public bool MultiSelection { get; set; }

        [Parameter]
        public bool Hoverable { get; set; }

        [Parameter]
        public RenderFragment<TItem>? Columns { get; set; }

        [Parameter]
        public RenderFragment? HeaderContent { get; set; }

        public void AddSelectedItem(TItem item)
        {
            if (!_selectedItems.Contains(item))
            {
                _selectedItems.Add(item);
                _ = SelectedItemsChanged.InvokeAsync(_selectedItems);
            }
        }

        public void RemoveSelectedItem(TItem item)
        {
            if (!_selectedItems.Contains(item))
            {
                return;
            }
            _selectedItems.Remove(item);
            _ = SelectedItemsChanged.InvokeAsync(_selectedItems);
        }

        public void SelectAllItems()
        {
            SelectedItems = _items;
        }

        public void ClearSelectedItems()
        {
            _selectedItems.Clear();
            _ = SelectedItemsChanged.InvokeAsync(_selectedItems);
        }
    }
}
