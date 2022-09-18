using KCode.Ed2kHash;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Ed2kGui
{
    public partial class Ed2kLinkForm : Form
    {
        private readonly BlockingCollection<string> _fpaths = new();
        private readonly BlockingCollection<string> _results = new();
        private readonly Progress<int> _computeWorkerProgress;

        public Ed2kLinkForm()
        {
            InitializeComponent();
            _computeWorkerProgress = new Progress<int>((c) => _worker.ReportProgress(c));
            InitEventHandler();

            _worker.RunWorkerAsync();
            _resultAppender.RunWorkerAsync();
        }

        private void InitEventHandler()
        {
            DragOver += (s, e) => e.Effect = e.Data?.GetDataPresent(DataFormats.FileDrop) ?? false ? DragDropEffects.Copy : DragDropEffects.None;
            DragDrop += (s, e) =>
            {
                object? fileData = e.Data?.GetData(DataFormats.FileDrop);
                if (fileData == null) return;

                var filePaths = (string[])fileData;
                foreach (var fpath in filePaths) _fpaths.Add(fpath);
            };

            _worker.DoWork += (s, e) =>
            {
                //var fpath = _fpaths.Take();
                //e.Result = Ed2k.Compute(fpath, _computeWorkerProgress);

                while (true)
                {
                    var fpath = _fpaths.Take();
                    FileInfo fi = new(fpath);
                    var hash = Ed2k.Compute(fpath, _computeWorkerProgress);
                    var link = new Ed2kFileLink(fi.Name, fi.Length, hash);
                    _results.Add(link.ToString());
                }
            };
            _worker.ProgressChanged += (s, e) => _progress.Value = e.ProgressPercentage;

            _resultAppender.DoWork += (s, e) =>
            {
                while (true)
                {
                    var link = _results.Take();
                    Results.Invoke(() => Results.Text += $"{link}\r\n");
                }
            };
        }

        private void Calc(string fpath)
        {
            var fi = new FileInfo(fpath);

            var ed2k = Ed2k.Compute(fpath, new Progress<int>((c) => Results.Text += $"{c}\r\n"));
            var link = new Ed2kFileLink(fi.Name, fi.Length, ed2k);
            Debug.WriteLine($"ed2k {link}");
            Results.Text += $"{link}\r\n";
        }
    }
}
