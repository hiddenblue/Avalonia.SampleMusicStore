using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace DialogManageSample.Services;

public static class DialogHelper
{
    public static async Task<IEnumerable<string>?> OpenFileDialogAsync(
        this object? context, string? title = null, bool selectMany = true)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var topLevel = DialogManager.GetTopLevelForContext(context);

        if (topLevel != null)
        {
            FilePickerOpenOptions filePickerOpenOptions = new()
            {
                AllowMultiple = selectMany,
                Title =  title ??  "Select any file(s) "
            };
            var storageFiles = await topLevel.StorageProvider.OpenFilePickerAsync(
                filePickerOpenOptions);

            return storageFiles.Select(f => f.Name);
        }

        return null;
    }
}