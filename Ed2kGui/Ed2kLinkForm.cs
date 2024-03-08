using KCode.Ed2kHash;
using System.Collections.Concurrent;

namespace Ed2kGui
{
    public partial class Ed2kLinkForm : Form
    {
        private readonly BlockingCollection<string> _fpaths = [];
        private readonly BlockingCollection<string> _results = [];
        private readonly Progress<int> _computeWorkerProgress;

        public Ed2kLinkForm()
        {
            InitializeComponent();
            _computeWorkerProgress = new Progress<int>(_worker.ReportProgress);
            InitEventHandlers();

            _worker.RunWorkerAsync();
            _resultAppender.RunWorkerAsync();
        }

        private void InitEventHandlers()
        {
            InitDropEventHandlers();
            InitBackgroundWorker();
            InitBackgroundResultAppender();

            void InitDropEventHandlers()
            {
                DragOver += (s, e) => e.Effect = e.Data?.GetDataPresent(DataFormats.FileDrop) ?? false ? DragDropEffects.Copy : DragDropEffects.None;
                DragDrop += (s, e) =>
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
                };
            }

            void InitBackgroundWorker()
            {
                _worker.DoWork += (s, e) =>
                {
                    while (true)
                    {
                        // Blocking take
                        var fpath = _fpaths.Take();
                        FileInfo fi = new(fpath);
                        var hash = Ed2k.Compute(fpath, _computeWorkerProgress);
                        var link = new Ed2kFileLink(fi.Name, fi.Length, hash);
                        _results.Add(link.ToString());
                    }
                };
                _worker.ProgressChanged += (s, e) => _progressbar.Value = e.ProgressPercentage;
            }

            void InitBackgroundResultAppender()
            {
                _resultAppender.DoWork += (s, e) =>
                {
                    while (true)
                    {
                        // Blocking take
                        var link = _results.Take();
                        _ = Results.Invoke(() => Results.Text += $"{link}\r\n");
                    }
                };
            }
        }
    }
}
