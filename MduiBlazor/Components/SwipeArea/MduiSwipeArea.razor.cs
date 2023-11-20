using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace MduiBlazor
{
    public partial class MduiSwipeArea : MduiComponentBase
    {
        private double? _xDown;
        private double? _yDown;

        /// <summary>
        /// 识别精度，单位像素点，默认30px
        /// </summary>
        [Parameter]
        public int Accuracy { get; set; } = 30;

        [Parameter]
        public EventCallback<SwipeDirection> OnSwipe { get; set; }

        private void OnTouchStart(TouchEventArgs args)
        {
            _xDown = args.Touches[0].ClientX;
            _yDown = args.Touches[0].ClientY;
        }

        private void OnTouchEnd(TouchEventArgs args)
        {
            if (OnSwipe.HasDelegate)
            {
                if (_xDown == null || _yDown == null)
                {
                    return;
                }
                var xDiff = _xDown.Value - args.ChangedTouches[0].ClientX;
                var yDiff = _yDown.Value - args.ChangedTouches[0].ClientY;
                if (Math.Abs(xDiff) < Accuracy && Math.Abs(yDiff) < Accuracy)
                {
                    _xDown = null;
                    _yDown = null;
                    return;
                }
                if (Math.Abs(xDiff) > Math.Abs(yDiff))
                {
                    if (xDiff > 0)
                    {
                        OnSwipe.InvokeAsync(SwipeDirection.RightToLeft);
                    }
                    else
                    {
                        OnSwipe.InvokeAsync(SwipeDirection.LeftToRight);
                    }
                }
                else
                {
                    if (yDiff > 0)
                    {
                        OnSwipe.InvokeAsync(SwipeDirection.BottomToTop);
                    }
                    else
                    {
                        OnSwipe.InvokeAsync(SwipeDirection.TopToBottom);
                    }
                }
                _xDown = null;
                _yDown = null;
            }
        }

        private void OnTouchCancel(TouchEventArgs args)
        {
            _xDown = null;
            _yDown = null;
        }
    }
}