using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm;


namespace MVVMDialogSample2.Core;

public sealed class Interaction<TInput, TOutput> : IDisposable, ICommand
{

    private Func<TInput, Task<TOutput>>? _handler;

    public Task<TOutput> HandleAsync(TInput input)
    {
        if (_handler is null)
        {
            throw new InvalidOperationException("Handler was not registered.");
        }
        
        return _handler(input);
    }

    public IDisposable RegisterHandler(Func<TInput, Task<TOutput>?> handler)
    {
        if (_handler is null)
        {
            throw new InvalidOperationException("Handler was already registered.");
        }
        
        _handler = handler;
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        return this;
    }

    public void Dispose()
    {
        _handler = null;
    }
    
    public bool CanExecute(object? parameter) => _handler is not null;
    
    public void Execute(object? parameter) => HandleAsync((TInput?)parameter!);

    public event EventHandler? CanExecuteChanged;
}