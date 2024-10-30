using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using MVVMDialogSample2.ViewModels;

namespace MVVMDialogSample2.Views;

public partial class CustomInteractionView : UserControl
{
    public CustomInteractionView()
    {
        InitializeComponent();
    }
    
    IDisposable? _selectFilesInteractionDisposable;

    protected override void OnDataContextChanged(EventArgs e)
    {
        _selectFilesInteractionDisposable?.Dispose();

        if (DataContext is CustomInteractionViewModel vm)
        {
            _selectFilesInteractionDisposable =
                vm.SelectFilesInteraction.RegisterHandler(InteractionHandler);
        }
        
        base.OnDataContextChanged(e);
        
    }

    public async Task<string[]?> InteractionHandler(string input)
    {
        TopLevel? topLevel = TopLevel.GetTopLevel(this);

        FilePickerOpenOptions filePickerOpenOptions = new()
        {
            AllowMultiple = true,
            Title = input
        };

        IReadOnlyList<IStorageFile> storageFiles = await topLevel!.StorageProvider.OpenFilePickerAsync(filePickerOpenOptions);
        
        return storageFiles?.Select(x => x.Name).ToArray();

    }
}