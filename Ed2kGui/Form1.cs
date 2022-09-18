using Ed2k;
using KCode.MD4;
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
            DragDrop += (s, ev) =>
            {
                object? fileData = ev.Data?.GetData(DataFormats.FileDrop);
                if (fileData == null) return;
                var filePaths = (string[])fileData;
                foreach (var fpath in filePaths) Calc(fpath);
            };
        }

        public void Calc(string fpath)
        {
            var fi = new FileInfo(fpath);

            var bytes = File.ReadAllBytes(fi.FullName);
            var digest = new MD4(bytes).DigestString;
            var md4 = $"md4://{fi.Name}/{digest}/";
            Debug.WriteLine($"md4 {md4}");
            Results.Text += $"{md4}\r\n";

            var ed2k = Ed2k.Ed2k.Compute(fpath);
            var link = new Ed2kFileLink(fi.Name, fi.Length, ed2k);
            Debug.WriteLine($"ed2k {link}");
            Results.Text += $"{link}\r\n";
        }
    }
}
