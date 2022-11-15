using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace App2.CustomControls
{
    public class ScrollHostWithStepButtons : Control
    {
        public static ScrollViewer _scrollViewer;
        public static ScrollViewer scroll;
        private static long HorizontalOffsetPropertyCallbackToken;
        private static long ScrollableWidthPropertyCallbackToken;
        private Button LeftButton;
        private Button RightButton;
        private int width = 360;

        public ScrollHostWithStepButtons()
        {
            this.DefaultStyleKey = typeof(ScrollHostWithStepButtons);
        }
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _scrollViewer = this.GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
            scroll = _scrollViewer;
            if (scroll.HorizontalOffset == 0) //vertical offset 0 => at the top of the scrollviewer
                VisualStateManager.GoToState(this, "LeftDisabled", false);
            else
                VisualStateManager.GoToState(this, "LeftEnabled", false);


            if (scroll.HorizontalOffset +
                scroll.ActualWidth < scroll.ExtentWidth)
                VisualStateManager.GoToState(this, "RightEnabled", false);
            else
                VisualStateManager.GoToState(this, "RightDisabled", false);
            scroll.SizeChanged += (f, g) =>
            {
                if (scroll.HorizontalOffset == 0) //vertical offset 0 => at the top of the scrollviewer
                    VisualStateManager.GoToState(this, "LeftDisabled", false);
                else
                    VisualStateManager.GoToState(this, "LeftEnabled", false);


                if (scroll.HorizontalOffset +
                    g.NewSize.Width < scroll.ExtentWidth)
                    VisualStateManager.GoToState(this, "RightEnabled", false);
                else
                    VisualStateManager.GoToState(this, "RightDisabled", false);
            };
            scroll.ViewChanged += (s, c) =>
            {
                if (scroll.HorizontalOffset == 0) //vertical offset 0 => at the top of the scrollviewer
                    VisualStateManager.GoToState(this, "LeftDisabled", false);
                else
                    VisualStateManager.GoToState(this, "LeftEnabled", false);


                if (scroll.HorizontalOffset +
                    scroll.ActualWidth < scroll.ExtentWidth)
                    VisualStateManager.GoToState(this, "RightEnabled", false);
                else
                    VisualStateManager.GoToState(this, "RightDisabled", false);
            };
            _scrollViewer.Tag = this;
            LeftButton = this.GetTemplateChild("Left") as Button;
            LeftButton.Tapped += Up_Tapped;
            RightButton = this.GetTemplateChild("Right") as Button;
            RightButton.Tapped += Down_Tapped;
        }
        private void Down_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ScrollListHorizontal(shouldScrollDow: true);
        }

        private void Up_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ScrollListHorizontal(shouldScrollDow: false);
        }

        private void ScrollListHorizontal(bool shouldScrollDow)
        {
            if (shouldScrollDow)
            {
                _scrollViewer.ChangeView(
                    _scrollViewer.HorizontalOffset + 360, null, null);
                //width += 360;
            }
            else
            {
                //width = -360;
                _scrollViewer.ChangeView(
                    _scrollViewer.HorizontalOffset - 360, null, null);
                //width -= 360;
            }
        }
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public static readonly DependencyProperty ContentProperty =
  DependencyProperty.Register(
     "Content",
     typeof(object),
     typeof(ScrollHostWithStepButtons),
     new PropertyMetadata(null, new PropertyChangedCallback(OnContentChanged)));

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null)
            {
                if (e.NewValue is ListViewBase)
                {
                    Debug.WriteLine("it's a ListView");
                }
            }
        }
    }
}
