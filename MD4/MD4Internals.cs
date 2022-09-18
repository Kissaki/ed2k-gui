namespace KCode.MD4;

public static class MD4Internals
{
    private static readonly byte[] _initialABCD = new byte[] {
        0x01, 0x23, 0x45, 0x67,
        0x89, 0xab, 0xcd, 0xef,
        0xfe, 0xdc, 0xba, 0x98,
        0x76, 0x54, 0x32, 0x10,
    };

    public static ReadOnlySpan<byte> InitialABCD => _initialABCD.AsSpan();

    public const int InputBlockLength = 64;

    // high-order digit first
    // This constant represents the square root of 2 * 2^30.
    // Octal value 013240474631
    public const uint r2const = 0x5A827999;

    // high-order digit first
    // This constant represents the square root of 3 * 2^30.
    // Octal value 015666365641
    public const uint r3const = 0x6ED9EBA1;

    /// <summary>
    /// Append to input the bit padding and the input length
    /// </summary>
    public static Span<byte> PadInput(ReadOnlySpan<byte> input)
    {
        byte[] padded = new byte[DetermineLength(input.Length)];

        int i = 0;
        for (; i < input.Length; ++i)
        {
            padded[i] = input[i];
        }

        padded[i] = 0b1000_0000;

        // MD4 is bit-based, so the length value must be in bit
        byte[] inLenValue = BitConverter.GetBytes((ulong)input.Length * 8L);
        padded[^1] = inLenValue[^1];
        padded[^2] = inLenValue[^2];
        padded[^3] = inLenValue[^3];
        padded[^4] = inLenValue[^4];
        padded[^5] = inLenValue[^5];
        padded[^6] = inLenValue[^6];
        padded[^7] = inLenValue[^7];
        padded[^8] = inLenValue[^8];

        return padded.AsSpan();
    }

    /// <summary>
    /// Determine the target padded input length
    /// </summary>
    /// <remarks>
    /// Input MUST be appended with 1-bit and 4-byte-length.
    /// So minimum is input.Length + 5.
    /// </remarks>
    public static int DetermineLength(int inputLength)
    {
        // a 1 bit is always appended, so our byte-based input will always be extended by at least one byte
        // Input length is always appended as 8 bytes integer
        var x = inputLength + 8 + 1;

        // Increase until a multiple of 64
        while (x % 64 != 0) ++x;

        return x;
    }

    /// <remarks>
    /// <para>f(X,Y,Z)  =  XY v not(X)Z</para>
    /// <para>f: in every bit position, x ? y : z</para>
    /// <para>*Side note: (The function f could have been defined using + instead of v since XY and not(X)Z will never have 1's in the same bit position.)*</para>
    /// <para>f(X,Y,Z)  ((X&Y) | ((~X)&Z))</para>
    /// </remarks>
    public static uint f(uint x, uint y, uint z)
    {
        return (x & y) | ((~x) & z);
    }

    /// <remarks>
    /// <para>g: in every bit position, majority fn, x + y + z > 1 ? 1 : 0</para>
    /// <para>*Side note: It is interesting to note that if the bits of X, Y, and Z are independent and unbiased, then each bit of f(X,Y,Z) will be independent and unbiased, and similarly each bit of g(X,Y,Z) will be independent and unbiased.*</para>
    /// <para>g(X,Y,Z)  ((X&Y) | (X&Z) | (Y&Z))</para>
    /// </remarks>
    public static uint g(uint x, uint y, uint z)
    {
        return (x & y) | (x & z) | (y & z);
    }

    /// <remarks>
    /// <para>h: bit-wise xor or parity function</para>
    /// <para>h(X,Y,Z)  (X^Y^Z)</para>
    /// </remarks>
    public static uint h(uint x, uint y, uint z)
    {
        return x ^ y ^ z;
    }

    // #define ff(A,B,C,D,i,s)      A = rot((A + f(B,C,D) + X[i]),s)
    // #define gg(A,B,C,D,i,s)      A = rot((A + g(B,C,D) + X[i] + C2),s)
    // #define hh(A,B,C,D,i,s)      A = rot((A + h(B,C,D) + X[i] + C3),s)
}
