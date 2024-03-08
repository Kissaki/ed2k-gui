using System.Collections.Concurrent;

namespace Ed2kGui;

/// <summary>Handles form drag events</summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "False positive. It's 'ed2k'.")]
public sealed class Ed2kLinkFormDragHandler : IDisposable
{
    /// <summary>Handle drag events on <paramref name="form"/>, add file paths to <paramref name="fpaths"/></summary>
    public Ed2kLinkFormDragHandler(Form form, BlockingCollection<string> fpaths)
    {
        _form = form;
        _fpaths = fpaths;
        _dragOverHandler = new(HandleDragOver);
        _dragDropHandler = new(HandleDragDrop);
        _form.DragOver += _dragOverHandler;
        _form.DragDrop += _dragDropHandler;
    }

    public void Dispose()
    {
        _form.DragOver -= _dragOverHandler;
        _form.DragDrop -= _dragDropHandler;
    }

    private readonly Form _form;
    private readonly BlockingCollection<string> _fpaths;

    private readonly DragEventHandler _dragOverHandler;
    private readonly DragEventHandler _dragDropHandler;

    private void HandleDragOver(object? sender, DragEventArgs e)
    {
        e.Effect = e.Data?.GetDataPresent(DataFormats.FileDrop) ?? false ? DragDropEffects.Copy : DragDropEffects.None;
    }

    private void HandleDragDrop(object? sender, DragEventArgs e)
    {
        object? fileData = e.Data?.GetData(DataFormats.FileDrop);
        if (fileData == null) return;

        var filePaths = (string[])fileData;
        foreach (var fpath in filePaths)
        {
            var isFile = File.Exists(fpath);
            if (isFile) _fpaths.Add(fpath);

            var isDir = Directory.Exists(fpath);
            if (isDir)
            {
                var di = new DirectoryInfo(fpath);
                var fiRecursive = di.GetFiles("", SearchOption.AllDirectories);
                foreach (var fi in fiRecursive)
                {
                    _fpaths.Add(fi.FullName);
                }
            }
        }
    }
}
