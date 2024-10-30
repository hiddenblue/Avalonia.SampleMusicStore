using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

namespace MVVMDialogSample.ViewModels;

public class InteractionViewModel : ViewModelBase
{
    public InteractionViewModel()
    {
        _selectFilesInteraction = new Interaction<string?, string[]?>();

        SelectFilesCommand = ReactiveCommand.CreateFromTask(SelectFiles);

    }

    private readonly Interaction<string?, string[]?> _selectFilesInteraction;
    
    public Interaction<string?, string[]?> SelectFilesInteraction => this._selectFilesInteraction;
    
    public ICommand SelectFilesCommand { get; }
    
    private string[]? _selectedFiles;

    public string[]? SelectedFiles
    {
        get => _selectedFiles;

        set => this.RaiseAndSetIfChanged(ref _selectedFiles, value);
    }

    private async Task SelectFiles()
    {
        SelectedFiles= await _selectFilesInteraction.Handle("Hello from InteractionViewModel");
    }

}