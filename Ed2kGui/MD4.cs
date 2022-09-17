using static Ed2kGui.MD4Internals;

namespace Ed2kGui;

public ref struct MD4
{
    //private Span<byte> _digest = InitialABCD.ToArray();

    private uint A => BitConverter.ToUInt32(InitialABCD[0..4]);
    private uint B => BitConverter.ToUInt32(InitialABCD[4..8]);
    private uint C => BitConverter.ToUInt32(InitialABCD[8..12]);
    private uint D => BitConverter.ToUInt32(InitialABCD[12..16]);
    //private uint A => BitConverter.ToUInt32(_digest[0..4]);
    //private uint B => BitConverter.ToUInt32(_digest[4..8]);
    //private uint C => BitConverter.ToUInt32(_digest[8..12]);
    //private uint D => BitConverter.ToUInt32(_digest[12..16]);
    //private Span<byte> A => _digest[0..4];
    //private Span<byte> B => _digest[4..8];
    //private Span<byte> C => _digest[8..12];
    //private Span<byte> D => _digest[12..16];

    public ReadOnlySpan<byte> Digest
    {
        get
        {
            var a = BitConverter.GetBytes(A);
            var b = BitConverter.GetBytes(B);
            var c = BitConverter.GetBytes(C);
            var d = BitConverter.GetBytes(D);
            return new byte[] { a[0], a[1], a[2], a[3], b[0], b[1], b[2], b[3], c[0], c[1], c[2], c[3], d[0], d[1], d[2], d[3], };
        }
    }

    //public byte[] Digest => new byte[16] { A[0], A[1], A[2], A[3], B[0], B[1], B[2], B[3], C[0], C[1], C[2], C[3], D[0], D[1], D[2], D[3], };
    //public string DigestString => string.Join("", MemoryMarshal.ToEnumerable(Digest).Select(x => $"{x:H}"));
    public string DigestString => Convert.ToHexString(Digest);

    public MD4(Span<byte> input)
    {
        var inputPadded = PadInput(input);
        Guard.IsTrue(inputPadded.Length % 64 == 0);

        for (int i = 0; i < inputPadded.Length; i += 64)
        {
            Span<byte> block = inputPadded[i..(i + 64)];
            ProcessBlock(block);
        }
    }

    /// <remarks>In 3 rounds</remarks>
    public void ProcessBlock(Span<byte> block)
    {
        Guard.HasSizeEqualTo(block, 16);

        // RFC names the block `X`.
        var x = block;

        uint aa = A;
        uint bb = B;
        uint cc = C;
        uint dd = D;

        // Round 1
        // Let [A B C D i s] denote A = (A + f(B, C, D) + X[i]) <<< s

        // [A B C D 0 3]  =>  A = (A + f(B, C, D) + X[i]) <<< s
        A = (A + f(B, C, D) + x[i]) <<< s;


        // [A B C D i s]  =>  A = (A + f(B,C,D) + X[i]) <<< s
        //   with parameter mutation
        //   over 16 bits
        //for (int bitIndex = 0; bitIndex < 8; ++bitIndex)
        //{
        //    var value = BitValue()
        //}
        //for (int bitIndex = 0; bitIndex < 16; ++bitIndex)
        //{
        //    var xBitValue = x[bitIndex];
        //    A = A + f(A, B, C) +
        //}
    }

    private static byte BitValue(byte value, int bitIndex)
    {
        return (byte)((value << bitIndex) & 0b1000_000u);
    }

    public static void Process16WordsBlock(Span<byte> block)
    {
        //ushort x = block[]
    }
}
