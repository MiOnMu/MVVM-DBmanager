using System.Windows;

namespace DataBaseManagerUi.VisualStates;

public class AppStateManager
{
    public static string GetVisualStateProperty(DependencyObject obj)
    {
        return (string)obj.GetValue(VisualStatePropertyProperty);
    }

    public static void SetVisualStateProperty(DependencyObject obj, string value)
    {
        obj.SetValue(VisualStatePropertyProperty, value);
    }

    public static readonly DependencyProperty VisualStatePropertyProperty =
        DependencyProperty.RegisterAttached(
            "VisualStateProperty", typeof(string),
            typeof(AppStateManager),
            new PropertyMetadata((s, e) =>
            {
                var propertyName = (string)e.NewValue;
                var ctrl = s as FrameworkElement;
                if (ctrl == null)
                    throw new InvalidOperationException("");
                VisualStateManager.GoToElementState(ctrl, (string)e.NewValue, false);
            }));
}