using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using MVVMDialogSample.ViewModels;
using ReactiveUI;

namespace MVVMDialogSample.Views;

public partial class InteractionView : ReactiveUserControl<InteractionViewModel>
{
    public InteractionView()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            d(ViewModel.SelectFilesInteraction.RegisterHandler(this.InteractionHandler));
        });
        
        
    }
    

    private async Task InteractionHandler(InteractionContext<string?, string[]?> context)
    {
        TopLevel? topLevel = TopLevel.GetTopLevel(this);

        var filePickerOpenOption = new FilePickerOpenOptions
        {
            AllowMultiple = true,
            Title = context.Input
        };

        IReadOnlyList<IStorageFile>? storageFiles = await topLevel!.StorageProvider
            .OpenFilePickerAsync(filePickerOpenOption);
        
        // 找到文件之后进行返回

        var outputArray = storageFiles?.Select(x => x.Name).ToArray();
        
        context.SetOutput(outputArray);
    }
}