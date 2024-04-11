using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiTreeTd : MduiComponentBase
    {
        private string Classname =>
            new ClassBuilder()
            .AddClass("mdui-collapse-open", IsExpanded)
            .AddClass(Class)
            .Build();

        private int Level => Node?.Level ?? 0;

        private int IndentSize => Table?.IndentSize ?? 0;

        [CascadingParameter]
        private ITreeTable? Table { get; set; }

        [CascadingParameter(Name = "TreeTableNode")]
        private ITreeTableNode? Node { get; set; }

        private bool IsExpanded
        {
            get => Node?.IsExpanded == true;
            set
            {
                Node?.OnExpandedChanged(value);
            }
        }

        private void OnArrowClicked()
        {
            IsExpanded = !IsExpanded;
        }
    }
}