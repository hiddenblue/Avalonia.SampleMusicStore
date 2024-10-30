using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;

namespace DialogManageSample.Services;

public class DialogManager
{
    private static readonly Dictionary<object, Visual> RegistrationMapper = new Dictionary<object, Visual>();

    public static readonly AttachedProperty<object?> RegisterProperty =
        AvaloniaProperty.RegisterAttached<DialogManager, Visual, object?>("Register");
    
    
    public static void SetRegister(AvaloniaObject element, object? value) => element.SetValue(RegisterProperty, value);
    
    public static object? GetRegister(AvaloniaObject element) => element.GetValue(RegisterProperty);

    static DialogManager()
    {
        RegisterProperty.Changed.AddClassHandler<Visual>(RegisterChanged);
    }

    private static void RegisterChanged(Visual sender, AvaloniaPropertyChangedEventArgs e)
    {
        if( sender is null )
            throw new InvalidOperationException("The DialogManager can not be registered on a visual");

        if (e.OldValue != null)
        {
            RegistrationMapper.Remove(sender);
        }

        if (e.NewValue != null)
        {
            RegistrationMapper.Add(e.NewValue, sender);
        }
    }
    
    public static Visual? GetVisualForContext(object context) =>
        RegistrationMapper.TryGetValue(context, out var result) ? result : null;

    public static TopLevel? GetTopLevelForContext(object context) =>
        TopLevel.GetTopLevel(GetVisualForContext(context));
}

