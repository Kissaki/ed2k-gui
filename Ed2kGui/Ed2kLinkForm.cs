using KCode.Ed2kHash;
using System.Collections.Concurrent;

namespace Ed2kGui
{
    public partial class Ed2kLinkForm : Form
    {
        private readonly BlockingCollection<string> _fpaths = [];
        private readonly BlockingCollection<string> _results = [];
        private readonly Progress<int> _computeWorkerProgress;
        private readonly Ed2kLinkFormDragHandler _dragHandler;

        public Ed2kLinkForm()
        {
            InitializeComponent();
            _dragHandler = new(this, _fpaths);
            _computeWorkerProgress = new Progress<int>(_worker.ReportProgress);
            InitEventHandlers();

            _worker.RunWorkerAsync();
            _resultAppender.RunWorkerAsync();
        }

        public void DisposeManaged()
        {
            _dragHandler.Dispose();
        }

        private void InitEventHandlers()
        {
            InitBackgroundWorker();
            InitBackgroundResultAppender();

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
