using System.Windows.Controls;
using System.Windows;

namespace DataBaseManagerUi.Helpers;

public static class PasswordBoxHelper
{
    public static readonly DependencyProperty BoundPasswordProperty =
        DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxHelper),
            new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

    public static string GetBoundPassword(DependencyObject dp)
    {
        return (string)dp.GetValue(BoundPasswordProperty);
    }

    public static void SetBoundPassword(DependencyObject dp, string value)
    {
        dp.SetValue(BoundPasswordProperty, value);
    }

    private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PasswordBox passwordBox)
        {
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            if (!(bool)passwordBox.GetValue(IsUpdatingProperty))
            {
                passwordBox.Password = e.NewValue as string;
            }

            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }
    }

    private static readonly DependencyProperty IsUpdatingProperty =
        DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordBoxHelper));

    private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is PasswordBox passwordBox)
        {
            passwordBox.SetValue(IsUpdatingProperty, true);
            SetBoundPassword(passwordBox, passwordBox.Password);
            passwordBox.SetValue(IsUpdatingProperty, false);
        }
    }
}