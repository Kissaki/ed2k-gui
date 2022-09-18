using Ed2k;
using System.Diagnostics;

namespace Ed2kGui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitEventHandler();
        }

        private void InitEventHandler()
        {
            DragOver += (s, e) => e.Effect = e.Data?.GetDataPresent(DataFormats.FileDrop) ?? false ? DragDropEffects.Copy : DragDropEffects.None;
            DragDrop += (s, e) =>
            {
                object? fileData = e.Data?.GetData(DataFormats.FileDrop);
                if (fileData == null) return;
                var filePaths = (string[])fileData;
                foreach (var fpath in filePaths) Calc(fpath);
            };
            Worker.DoWork += (s, e) =>
            {

            };
        }

        public void Calc(string fpath)
        {
            var fi = new FileInfo(fpath);

            var ed2k = Ed2k.Ed2k.Compute(fpath, new Progress<int>((c) => Results.Text += $"{c}\r\n"));
            var link = new Ed2kFileLink(fi.Name, fi.Length, ed2k);
            Debug.WriteLine($"ed2k {link}");
            Results.Text += $"{link}\r\n";
        }
    }
}
