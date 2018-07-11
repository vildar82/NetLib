using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using JetBrains.Annotations;

/// <summary>
/// https://stackoverflow.com/a/16317999
/// Behavior that will connect an UI event to a viewmodel Command,
/// allowing the event arguments to be passed as the CommandParameter.
/// </summary>
[PublicAPI]
public class EventToCommand : Behavior<FrameworkElement>
{
    private Delegate _handler;
    private EventInfo _oldEvent;

    // Event
    public string Event
    {
        get => (string) GetValue(EventProperty);
        set => SetValue(EventProperty, value);
    }
    public static readonly DependencyProperty EventProperty = DependencyProperty.Register("Event", typeof(string),
        typeof(EventToCommand), new PropertyMetadata(null, OnEventChanged));

    // Command
    public ICommand Command
    {
        get => (ICommand) GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand),
        typeof(EventToCommand), new PropertyMetadata(null));

    // PassArguments (default: false)
    public bool PassArguments
    {
        get => (bool) GetValue(PassArgumentsProperty);
        set => SetValue(PassArgumentsProperty, value);
    }
    public static readonly DependencyProperty PassArgumentsProperty = DependencyProperty.Register("PassArguments", typeof(bool),
        typeof(EventToCommand), new PropertyMetadata(false));


    private static void OnEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var beh = (EventToCommand) d;

        if (beh.AssociatedObject != null) // is not yet attached at initial load
            beh.AttachHandler((string) e.NewValue);
    }

    protected override void OnAttached()
    {
        AttachHandler(Event); // initial set
    }

    /// <summary>
    /// Attaches the handler to the event
    /// </summary>
    private void AttachHandler(string eventName)
    {
        // detach old event
        if (_oldEvent != null)
            _oldEvent.RemoveEventHandler(AssociatedObject, _handler);

        // attach new event
        if (!string.IsNullOrEmpty(eventName))
        {
            var ei = AssociatedObject.GetType().GetEvent(eventName);
            if (ei != null)
            {
                var mi = GetType().GetMethod("ExecuteCommand", BindingFlags.Instance | BindingFlags.NonPublic);
                _handler = Delegate.CreateDelegate(ei.EventHandlerType, this, mi);
                ei.AddEventHandler(AssociatedObject, _handler);
                _oldEvent = ei; // store to detach in case the Event property changes
            }
            else
                throw new ArgumentException(
                    $"The event '{eventName}' was not found on type '{AssociatedObject.GetType().Name}'");
        }
    }

    /// <summary>
    /// Executes the Command
    /// </summary>
    private void ExecuteCommand(object sender, EventArgs e)
    {
        object parameter = PassArguments ? e : null;
        if (Command != null)
        {
            if (Command.CanExecute(parameter))
                Command.Execute(parameter);
        }
    }
}