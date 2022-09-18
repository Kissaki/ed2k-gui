using KCode.MD4;
using System.Buffers;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace Ed2k;

public class Ed2k
{
    private const int _blockSize = 9728000;

    public static string Compute(string fpath)
    {
        var pool = ArrayPool<byte>.Shared;
        using var r = new BufferedStream(File.OpenRead(fpath));
        List<byte> digests = new();

        bool endReached = false;
        while (!endReached)
        {
            var arr = pool.Rent(_blockSize);
            var read = r.Read(arr, 0, _blockSize);
            endReached = read != _blockSize;

            var md4 = new MD4(arr.AsSpan(0, _blockSize)).Digest;
            pool.Return(arr, clearArray: false);

            // Opportunity: Should copy entire range instead of byte-by-byte
            foreach (var b in md4) digests.Add(b);
            //digests.AddRange(md4);
        }

        return new MD4(CollectionsMarshal.AsSpan(digests)).DigestString;
    }

    //private BlockingCollection<byte[]> _queue = new();
    //private ArrayPool<byte> _pool = new();

    //public Ed2k(string fpath)
    //{
    //    var pool = ArrayPool<byte>.Shared;
    //    using var r = new BufferedStream(File.OpenRead(fpath));
    //    List<byte> digests = new();

    //    bool endReached = false;
    //    while (!endReached)
    //    {
    //        var arr = pool.Rent(_blockSize);
    //        var read = r.Read(arr, 0, _blockSize);
    //        endReached = read != _blockSize;

    //        var md4 = new MD4(arr.AsSpan()).Digest;
    //        pool.Return(arr, clearArray: false);

    //        // Opportunity: Should copy entire range instead of byte-by-byte
    //        foreach (var b in md4) digests.Add(b);
    //        //digests.AddRange(md4);
    //    }

    //    return new MD4(CollectionsMarshal.AsSpan(digests)).DigestString;
    //}
}
