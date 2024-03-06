using KCode.MD4Hash;
using System.Text;

namespace MD4Test;

public class MD4Tests
{
    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Assertions", "xUnit2000:Constants and literals should be the expected argument", Justification = "<Pending>")]
    public void TestConstants()
    {
        // MD4 is bit based - but we implement byte based
        // 512 bit = 64 byte
        Assert.Equal(64, 512 / 8);
        Assert.Equal(64, MD4Internals.InputBlockLength);

        Assert.Equal((uint)Math.Sqrt(2) * Math.Pow(2, 30), MD4Internals.r2const);
        Assert.Equal((uint)Math.Sqrt(3) * Math.Pow(2, 30), MD4Internals.r3const);
    }

    /// <summary>
    /// The initial values are defined in RFC 1186
    /// </summary>
    [Fact]
    public void TestInitialDigestValues()
    {
        Assert.Equal(new byte[] {
            0x01, 0x23, 0x45, 0x67,
            0x89, 0xab, 0xcd, 0xef,
            0xfe, 0xdc, 0xba, 0x98,
            0x76, 0x54, 0x32, 0x10,
        }, MD4Internals.InitialABCD.ToArray());
    }

    [Theory]
    [InlineData(64, 0)]
    [InlineData(64, 1)]
    [InlineData(64, 2)]
    [InlineData(64, 64 - 10)]
    [InlineData(64, 64 - 9)]
    [InlineData(128, 64 - 8)]
    [InlineData(128, 64 - 7)]
    [InlineData(128, 64 - 6)]
    [InlineData(128, 64 - 5)]
    [InlineData(128, 64 - 4)]
    [InlineData(128, 64 - 3)]
    [InlineData(128, 64 - 2)]
    [InlineData(128, 64 - 1)]
    [InlineData(128, 64)]
    [InlineData(128, 64 + 1)]
    [InlineData(128, 128 - 10)]
    [InlineData(128, 128 - 9)]
    [InlineData(192, 128 - 8)]
    [InlineData(192, 128 - 7)]
    public void TestDetermineLength1(int e, int len)
    {
        Assert.Equal(e, MD4Internals.DetermineLength(len));
    }

    /// <summary>
    /// Binary input is appended with bit padding and input length to a multiple of 64 bytes
    /// </summary>
    /// <remarks>
    /// <para>MD4 is bit-based. But we implement it byte-based.</para>
    /// <para>The input is appended with at least a 1 bit, so 0b1000_0000.</para>
    /// <para>In MD4 the input is padded to {*:input B:0b1000_000 ... 4B:input-len } - that is, it is appended with bit padding + input length (64 bit = 8 Byte) to a multiple of 512 bit = 64 Byte</para>
    ///  </remarks>
    [Fact]
    public void TestDetermineLength2()
    {
        var minPad = 9;

        var gate1Value = 64;
        var gate1Split = gate1Value - minPad;
        for (int i = 0; i < gate1Split; ++i)
        {
            Assert.Equal(gate1Value, MD4Internals.DetermineLength(i));
        }

        var gate2Value = 128;
        var gate2Split = gate2Value - minPad;
        for (int i = gate1Split + 1; i < gate2Split; ++i)
        {
            Assert.Equal(gate2Value, MD4Internals.DetermineLength(i));
        }

        var gate3Value = 192;
        var gate3 = gate3Value - minPad;
        for (int i = gate2Split + 1; i < gate3; ++i)
        {
            Assert.Equal(gate3Value, MD4Internals.DetermineLength(i));
        }

        var gate4Value = 256;
        var gate4Split = gate4Value - minPad;
        for (int i = gate3 + 1; i < gate4Split; ++i)
        {
            Assert.Equal(gate4Value, MD4Internals.DetermineLength(i));
        }
    }

    [Fact]
    public void TestPadEmpty()
    {
        var input = Array.Empty<byte>();

        byte[] expected = new byte[64];
        expected[0] = 0b1000_0000;
        expected[^8] = 0x00;

        var actual = MD4Internals.PadInput(input);

        Assert.Equal(64, actual.Length);
        Assert.Equal<byte>(expected, actual.ToArray());
    }

    [Fact]
    public void TestPad1Byte()
    {
        var input = new byte[] { 0b1010_0101 };

        byte[] expected = new byte[64];
        expected[0] = 0b1010_0101;
        expected[1] = 0b1000_0000;
        expected[^8] = 0x08;

        var actual = MD4Internals.PadInput(input);

        Assert.Equal(64, actual.Length);
        Assert.Equal<byte>(expected, actual.ToArray());
    }

    [Fact]
    public void TestPad2Byte()
    {
        var input = new byte[] { 0b0110_1001, 0b1111_0000, };

        byte[] expected = new byte[64];
        expected[0] = 0b0110_1001;
        expected[1] = 0b1111_0000;
        expected[2] = 0b1000_0000;
        expected[^8] = 0x10;

        var actual = MD4Internals.PadInput(input);

        Assert.Equal(64, actual.Length);
        Assert.Equal<byte>(expected, actual.ToArray());
    }

    [Fact]
    public void TestEmptyData()
    {
        var md4 = new MD4([]);

        Assert.Equal("31d6cfe0d16ae931b73c59d7e0c089c0", md4.DigestString);
    }

    // Test cases from RFC 1186 page 16/17 - MDtestsuite() MDstring(s) MDprint(MDp)
    [Theory]
    [InlineData("31d6cfe0d16ae931b73c59d7e0c089c0", "")]
    [InlineData("bde52cb31de33e46245e05fbdbd6fb24", "a")]
    [InlineData("a448017aaf21d8525fc10ae87aa6729d", "abc")]
    [InlineData("d9130a8164549fe818874806e1c7014b", "message digest")]
    [InlineData("043f8582f241db351ce627e153e7f0e4", "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")]
    [InlineData("e33b4ddc9c38f2199c3e7b164fcc0536", "12345678901234567890123456789012345678901234567890123456789012345678901234567890")]
    public void Test(string expectedDigest, string text)
    {
        var md4 = new MD4(Encoding.ASCII.GetBytes(text).AsSpan());
        Assert.Equal(expectedDigest, md4.DigestString);
    }
}
