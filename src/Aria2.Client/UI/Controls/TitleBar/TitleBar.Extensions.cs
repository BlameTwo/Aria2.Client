using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

namespace Aria2.Client.UI.Controls;

partial class TitleBar
{
    internal void UpDate()
    {
        if (Window == null) return;
        if (!IsExtendsContentIntoTitleBar)
        {
            Window.AppWindow.TitleBar.ExtendsContentIntoTitleBar = false;
            SizeChanged -= TitleBar_SizeChanged;
            return;
        }
        else
        {
            Window.AppWindow.TitleBar.PreferredHeightOption = TitleMode;
        }
        if(TitleMode == TitleBarHeightOption.Tall)
        {
            VisualStateManager.GoToState(this, "Tall",true);
        }
        else if(TitleMode == TitleBarHeightOption.Standard)
        {
            VisualStateManager.GoToState(this, "Standard", true);
        }
        if (Window.Content is FrameworkElement rootElement)
        {
            UpdateTitleBarCaption(rootElement);
            rootElement.ActualThemeChanged += (s, e) =>
            {
                UpdateTitleBarCaption(rootElement);
            };
        }
        MakeDragLength();


    }

    private void UpdateTitleBarCaption(FrameworkElement rootElement)
    {
        Window.AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
        Window.AppWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        if (rootElement.ActualTheme == ElementTheme.Dark)
        {
            Window.AppWindow.TitleBar.ButtonForegroundColor = Colors.White;
            Window.AppWindow.TitleBar.ButtonInactiveForegroundColor = Colors.DarkGray;
        }
        else
        {
            Window.AppWindow.TitleBar.ButtonForegroundColor = Colors.Black;
            Window.AppWindow.TitleBar.ButtonInactiveForegroundColor = Colors.DarkGray;
        }
    }

    private static void OnWindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if(d is TitleBar title)
        {
            title.SetTitleBar();
        }
    }

    private static void OnTitleBarModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TitleBar title && title.Window!=null)
            title.UpDate();
    }

    private static void OnMaxButtonVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if(d is TitleBar title && title.Window != null && e.NewValue is bool ismax)
        {
            if (title.Window.AppWindow.Presenter is OverlappedPresenter opr)
            {
                opr.IsMaximizable = ismax;
                title.UpDate();
            }
        }
    }

    private static void OnMinButtonVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TitleBar title && title.Window != null && e.NewValue is bool ismin)
        {
            if (title.Window.AppWindow.Presenter is OverlappedPresenter opr)
            {
                opr.IsMinimizable = ismin;
                title.UpDate();
            }
        }
    }


    private static void OnContentRectChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TitleBar title && title.Window != null)
            title.UpDate();
    }
}
