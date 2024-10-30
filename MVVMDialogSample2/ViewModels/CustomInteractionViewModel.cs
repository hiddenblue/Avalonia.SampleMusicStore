using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVMDialogSample2.Core;

namespace MVVMDialogSample2.ViewModels;

public partial class CustomInteractionViewModel : ViewModelBase
{
    public Interaction<string?, string[]?> SelectFilesInteraction { get; } = new Interaction<string?, string[]?>();


    [ObservableProperty]
    private string[]? _selectedFiles;

    [RelayCommand]
    public async Task SelectFilesAsync()
    {
        SelectedFiles =   await SelectFilesInteraction.HandleAsync("Hello from avalonia");
    }
    
    public string Greeting => "Hello from avalonia!";
    
    
    
}