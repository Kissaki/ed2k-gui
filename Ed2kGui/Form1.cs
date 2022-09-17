using sun.security.provider;
using System.Diagnostics;

namespace Ed2kGui
{
    public partial class Form1 : Form
    {
        private const int BlockSize = 9728000;

        public Form1()
        {
            InitializeComponent();

            var fpath = @"C:\temp\test.mkv";
            Calc(fpath);
        }

        public void Calc(string fpath)
        {
            var buf = new byte[BlockSize];
            var fs = File.OpenRead(fpath);
            var r = new BufferedStream(fs);
            var blockDigests = new List<byte[]>();

            var m = sun.security.provider.MD4.getInstance();
            m.update(buf);
            //for (int i = 0; i < fs.Length; i += BlockSize)
            //{
            //    Debug.WriteLine($"Block {i}…");
            //    r.Read(buf);
            //    MD4 md4 = new(buf.AsSpan());
            //    md4.DigestString;
            //    var md4 = MD4.getInstance();
            //    md4.i
            //    var digest = MD4.getInstance().(buf);
            //    blockDigests.Add(digest);
            //}

            var full = new List<byte>();
            foreach (var blockDigest in blockDigests) full.AddRange(blockDigest);

            //var ed2kDigest = MD4.getInstance().digest(full.ToArray());
            //var ed2k = string.Join("", ed2kDigest.Select(x => x.ToString("x")));
            //Debug.WriteLine($"ed2k {ed2k}");
        }
    }
}