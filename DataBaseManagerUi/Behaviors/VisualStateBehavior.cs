using System.Windows;
using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;

namespace DataBaseManagerUi.Behaviors;

public class VisualStateBehavior : Behavior<FrameworkElement>
{
	public static readonly DependencyProperty StateProperty =
		DependencyProperty.Register(
			"State",
			typeof(string),
			typeof(VisualStateBehavior),
			new PropertyMetadata(null, OnStateChanged));

	public string State
	{
		get => (string)GetValue(StateProperty);
		set => SetValue(StateProperty, value);
	}

	private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		var behavior = (VisualStateBehavior)d;
		if (behavior.AssociatedObject == null)
			return;

		var state = e.NewValue?.ToString();
		if (!string.IsNullOrEmpty(state))
		{
			behavior.AssociatedObject.Dispatcher.InvokeAsync(() =>
			{
				VisualStateManager.GoToState(
					behavior.AssociatedObject,
					state,
					true);
			}, System.Windows.Threading.DispatcherPriority.Render);
		}
	}
}