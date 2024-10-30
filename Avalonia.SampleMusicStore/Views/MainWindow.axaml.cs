using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using Avalonia.SampleMusicStore.ViewModels;
using ReactiveUI;

namespace Avalonia.SampleMusicStore.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    
    public MainWindow()
    {
        InitializeComponent();
        

        this.WhenActivated(action =>
            action(ViewModel!.ShowDialogInteraction.RegisterHandler(DoShowDialogAsync)));
        
    }

    private async Task DoShowDialogAsync(InteractionContext<MusicStoreViewModel, AlbumViewModel?> interaction)
    {

        var dialog = new MusicStoreWindow();
        dialog.DataContext = interaction.Input;
        
        AlbumViewModel? result = await dialog.ShowDialog<AlbumViewModel?>(this);
        interaction.SetOutput(result);

    }
}