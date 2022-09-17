using KCode.MD4;
using System.Diagnostics;

namespace Ed2kGui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //var fpath = @"C:\temp\test.mkv";
            //Calc(fpath);
        }

        public void Calc(string fpath)
        {
            var fi = new FileInfo(fpath);

            var digest = new MD4(File.ReadAllBytes(fi.FullName)).DigestString;
            var md4 = $"md4://{fi.Name}/{digest}/";
            Debug.WriteLine($"md4 {md4}");
            Results.Text += $"{md4}\r\n";
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var file in files)
            {
                Calc(file);
            }
        }

        private void Home_DragDrop(object sender, DragEventArgs e)
        {
        }

        private void Form1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = e.Data?.GetDataPresent(DataFormats.FileDrop) ?? false ? DragDropEffects.Copy : DragDropEffects.None;
        }
    }
}