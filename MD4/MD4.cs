using System.Numerics;
using System.Runtime.InteropServices;
using static KCode.MD4.MD4Internals;

namespace KCode.MD4;

public class MD4
{
    //private Span<byte> _digest = InitialABCD.ToArray();

    private uint A = BitConverter.ToUInt32(InitialABCD[0..4]);
    private uint B = BitConverter.ToUInt32(InitialABCD[4..8]);
    private uint C = BitConverter.ToUInt32(InitialABCD[8..12]);
    private uint D = BitConverter.ToUInt32(InitialABCD[12..16]);
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
    public string DigestString => Convert.ToHexString(Digest).ToLowerInvariant();

    public MD4(ReadOnlySpan<byte> input)
    {
        var inputPadded = PadInput(input);
        Guard.IsTrue(inputPadded.Length % InputBlockLength == 0);

        for (int i = 0; i < inputPadded.Length; i += InputBlockLength)
        {
            ReadOnlySpan<byte> block = inputPadded[i..(i + InputBlockLength)];
            ProcessBlock(block);
        }
    }

    /// <remarks>RFC names the block variable X</remarks>
    public void ProcessBlock(ReadOnlySpan<byte> block)
    {
        Guard.HasSizeEqualTo(block, InputBlockLength);
        Guard.HasSizeEqualTo(block, 64);

        uint aa = A;
        uint bb = B;
        uint cc = C;
        uint dd = D;

        // Round 1
        R1(block, ref A, B, C, D, 0, 3);
        R1(block, ref D, A, B, C, 1, 7);
        R1(block, ref C, D, A, B, 2, 11);
        R1(block, ref B, C, D, A, 3, 19);
        R1(block, ref A, B, C, D, 4, 3);
        R1(block, ref D, A, B, C, 5, 7);
        R1(block, ref C, D, A, B, 6, 11);
        R1(block, ref B, C, D, A, 7, 19);
        R1(block, ref A, B, C, D, 8, 3);
        R1(block, ref D, A, B, C, 9, 7);
        R1(block, ref C, D, A, B, 10, 11);
        R1(block, ref B, C, D, A, 11, 19);
        R1(block, ref A, B, C, D, 12, 3);
        R1(block, ref D, A, B, C, 13, 7);
        R1(block, ref C, D, A, B, 14, 11);
        R1(block, ref B, C, D, A, 15, 19);

        // Round 2
        R2(block, ref A, B, C, D, 0, 3);
        R2(block, ref D, A, B, C, 4, 5);
        R2(block, ref C, D, A, B, 8, 9);
        R2(block, ref B, C, D, A, 12, 13);
        R2(block, ref A, B, C, D, 1, 3);
        R2(block, ref D, A, B, C, 5, 5);
        R2(block, ref C, D, A, B, 9, 9);
        R2(block, ref B, C, D, A, 13, 13);
        R2(block, ref A, B, C, D, 2, 3);
        R2(block, ref D, A, B, C, 6, 5);
        R2(block, ref C, D, A, B, 10, 9);
        R2(block, ref B, C, D, A, 14, 13);
        R2(block, ref A, B, C, D, 3, 3);
        R2(block, ref D, A, B, C, 7, 5);
        R2(block, ref C, D, A, B, 11, 9);
        R2(block, ref B, C, D, A, 15, 13);

        // Round 3
        R3(block, ref A, B, C, D, 0, 3);
        R3(block, ref D, A, B, C, 8, 9);
        R3(block, ref C, D, A, B, 4, 11);
        R3(block, ref B, C, D, A, 12, 15);
        R3(block, ref A, B, C, D, 2, 3);
        R3(block, ref D, A, B, C, 10, 9);
        R3(block, ref C, D, A, B, 6, 11);
        R3(block, ref B, C, D, A, 14, 15);
        R3(block, ref A, B, C, D, 1, 3);
        R3(block, ref D, A, B, C, 9, 9);
        R3(block, ref C, D, A, B, 5, 11);
        R3(block, ref B, C, D, A, 13, 15);
        R3(block, ref A, B, C, D, 3, 3);
        R3(block, ref D, A, B, C, 11, 9);
        R3(block, ref C, D, A, B, 7, 11);
        R3(block, ref B, C, D, A, 15, 15);

        A += aa;
        B += bb;
        C += cc;
        D += dd;
    }

    /// <remarks>
    /// <para>Let [A B C D i s] denote A = (A + f(B, C, D) + X[i]) <<< s</para>
    /// <para>[abcd k s]  =>  a = (a + f(b,c,d) + X[k]) <<< s</para>
    /// </remarks>
    private static void R1(ReadOnlySpan<byte> block, ref uint a, uint b, uint c, uint d, int i, int s)
    {
        // nr = word index => byte index
        i *= 4;
        a = BitOperations.RotateLeft(a + f(b, c, d) + MemoryMarshal.Read<uint>(block[i..]), s);
    }

    /// <remarks>
    /// <para>Let [A B C D i s] denote A = (A + g(B, C, D) + X[i] + 5A827999) <<< s.</para>
    /// </remarks>
    private static void R2(ReadOnlySpan<byte> block, ref uint a, uint b, uint c, uint d, int i, int s)
    {
        // nr = word index => byte index
        i *= 4;
        a = BitOperations.RotateLeft(a + g(b, c, d) + MemoryMarshal.Read<uint>(block[i..]) + r2const, s);
    }

    /// <remarks>
    /// <para>Let [A B C D i s] denote A = (A + h(B, C, D) + X[i] + 6ED9EBA1) <<< s.</para>
    /// </remarks>
    private static void R3(ReadOnlySpan<byte> block, ref uint a, uint b, uint c, uint d, int i, int s)
    {
        // nr = word index => byte index
        i *= 4;
        a = BitOperations.RotateLeft(a + h(b, c, d) + MemoryMarshal.Read<uint>(block[i..]) + r3const, s);
    }
}
