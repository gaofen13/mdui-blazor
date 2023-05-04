using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Windows.Input;

namespace MduiBlazor
{
    public class MduiButtonBase : MduiComponentBase
    {
        /// <summary>
        /// The HTML element that will be rendered in the root by the component
        /// By default, is a button
        /// </summary>
        [Parameter]
        public string HtmlTag { get; set; } = "button";

        /// <summary>
        /// The button Type (Button, Submit, Refresh)
        /// </summary>
        [Parameter]
        public ButtonType ButtonType { get; set; } = ButtonType.Button;

        /// <summary>
        /// The color of the component. It supports the theme colors.
        /// </summary>
        [Parameter]
        public string? Color { get; set; }

        [Parameter]
        public bool Text { get; set; }

        [Parameter]
        public bool DisableRipple { get; set; }

        [Parameter]
        public bool Dense { get; set; }

        /// <summary>
        /// If set to a URL, clicking the button will open the referenced document. Use Target to specify where
        /// </summary>
        [Parameter]
        public string? Href { get; set; }

        /// <summary>
        /// The target attribute specifies where to open the link, if Link is specified. Possible values: _blank | _self | _parent | _top | <i>framename</i>
        /// </summary>
        [Parameter]
        public string? Target { get; set; }

        /// <summary>
        /// If true, the button will be disabled.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Command executed when the user clicks on an element.
        /// </summary>
        [Parameter]
        public ICommand? Command { get; set; }

        /// <summary>
        /// Command parameter.
        /// </summary>
        [Parameter]
        public object? CommandParameter { get; set; }

        /// <summary>
        /// Button click event.
        /// </summary>
        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        protected virtual async Task OnClickHandler(MouseEventArgs ev)
        {
            if (Disabled)
                return;
            await OnClick.InvokeAsync(ev);
            if (Command?.CanExecute(CommandParameter) ?? false)
            {
                Command.Execute(CommandParameter);
            }
        }

        protected override void OnInitialized()
        {
            SetDefaultValues();
        }

        protected override void OnParametersSet()
        {
            //if params change, must set default values again
            SetDefaultValues();
        }

        //Set the default value for HtmlTag, Link and Target 
        private void SetDefaultValues()
        {
            if (Disabled)
            {
                HtmlTag = "button";
                Href = null;
                Target = null;
                return;
            }

            // Render an anchor element if Link property is set and is not disabled
            if (Href != null)
            {
                HtmlTag = "a";
            }
        }

        public ValueTask FocusAsync() => Element.FocusAsync();
    }
}
