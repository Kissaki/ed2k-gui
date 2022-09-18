namespace KCode.Ed2kHash;

public class Ed2k
{
    private const int _blockSize = 9728000;

    public static string Compute(string fpath, IProgress<int>? progress = null)
    {
        var pool = ArrayPool<byte>.Shared;
        using var r = new BufferedStream(File.OpenRead(fpath));
        List<byte> digests = new();

        FileInfo fi = new(fpath);
        int blockCount = (int)(fi.Length / _blockSize) + 1;

        bool endReached = false;
        int i = 0;
        while (!endReached)
        {
            progress?.Report(i++ * 100 / blockCount);

            byte[] arr = pool.Rent(_blockSize);
            int read = r.Read(arr, 0, _blockSize);
            ReadOnlySpan<byte> block = arr.AsSpan(0, read);
            endReached = read != _blockSize;

            var md4 = new MD4(block).Digest;
            pool.Return(arr, clearArray: false);

            // Opportunity: Should copy entire range instead of byte-by-byte
            foreach (var b in md4) digests.Add(b);
            //digests.AddRange(md4);
        }
        progress?.Report(100);

        return new MD4(CollectionsMarshal.AsSpan(digests)).DigestString;
    }
}
