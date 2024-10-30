using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.SampleMusicStore.Models;
using Avalonia.SampleMusicStore.ViewModels;
using System.Reactive.Concurrency;

namespace Avalonia.SampleMusicStore.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    
    public MainWindowViewModel()
    {
        ShowDialogInteraction = new Interaction<MusicStoreViewModel, AlbumViewModel?>();

        BuyMusicCommand = ReactiveCommand.CreateFromTask(BuyMusic);

        RxApp.MainThreadScheduler.Schedule(LoadAlbums);


    }
    
    public Interaction<MusicStoreViewModel, AlbumViewModel?> ShowDialogInteraction { get; }

    private async Task BuyMusic()
    {
        // 传了一个viewmodel给backend的Handler
        MusicStoreViewModel store = new MusicStoreViewModel();
        AlbumViewModel? result = await ShowDialogInteraction.Handle(store);
        if (result != null)
        {
            Albums.Add(result);
            await result.SaveToDiskAsync();
        }
    }
    public ICommand BuyMusicCommand { get; }

    public ObservableCollection<AlbumViewModel> Albums { get; } = new();


    private async void LoadAlbums()
    {
        var albums = (await Album.LoadFromCacheAsync()).Select(x => new AlbumViewModel(x));

        foreach (AlbumViewModel album in albums)
        {
            Albums.Add(album);
        }

        foreach (AlbumViewModel album in Albums)
        {
            await album.LoadCover();

        }
    }
    



}