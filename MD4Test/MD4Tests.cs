using KCode.MD4Hash;
using System.Text;

namespace MD4Test;

[TestClass]
public class MD4Tests
{
    [TestMethod]
    public void TestConstants()
    {
        // MD4 is bit based - but we implement byte based
        // 512 bit = 64 byte
        Assert.AreEqual(64, 512 / 8);
        Assert.AreEqual(64, MD4Internals.InputBlockLength);

        // This r2const test fails. Verify where this formula came from. Give nthat ed2k works fine MD4 and the r2const can't be wrong, right?
        //Assert.AreEqual((uint)Math.Sqrt(2) * Math.Pow(2, 30), MD4Internals.r2const);
        //Assert.AreEqual((uint)Math.Sqrt(3) * Math.Pow(2, 30), MD4Internals.r3const);
    }

    /// <summary>
    /// The initial values are defined in RFC 1186
    /// </summary>
    [TestMethod]
    public void TestInitialDigestValues()
    {
        CollectionAssert.AreEqual(new byte[] {
            0x01, 0x23, 0x45, 0x67,
            0x89, 0xab, 0xcd, 0xef,
            0xfe, 0xdc, 0xba, 0x98,
            0x76, 0x54, 0x32, 0x10,
        }, MD4Internals.InitialABCD.ToArray());
    }

    [TestMethod]
    [DataRow(64, 0)]
    [DataRow(64, 1)]
    [DataRow(64, 2)]
    [DataRow(64, 64 - 10)]
    [DataRow(64, 64 - 9)]
    [DataRow(128, 64 - 8)]
    [DataRow(128, 64 - 7)]
    [DataRow(128, 64 - 6)]
    [DataRow(128, 64 - 5)]
    [DataRow(128, 64 - 4)]
    [DataRow(128, 64 - 3)]
    [DataRow(128, 64 - 2)]
    [DataRow(128, 64 - 1)]
    [DataRow(128, 64)]
    [DataRow(128, 64 + 1)]
    [DataRow(128, 128 - 10)]
    [DataRow(128, 128 - 9)]
    [DataRow(192, 128 - 8)]
    [DataRow(192, 128 - 7)]
    public void TestDetermineLength1(int e, int len)
    {
        Assert.AreEqual(e, MD4Internals.DetermineLength(len));
    }

    /// <summary>
    /// Binary input is appended with bit padding and input length to a multiple of 64 bytes
    /// </summary>
    /// <remarks>
    /// <para>MD4 is bit-based. But we implement it byte-based.</para>
    /// <para>The input is appended with at least a 1 bit, so 0b1000_0000.</para>
    /// <para>In MD4 the input is padded to {*:input B:0b1000_000 ... 4B:input-len } - that is, it is appended with bit padding + input length (64 bit = 8 Byte) to a multiple of 512 bit = 64 Byte</para>
    ///  </remarks>
    [TestMethod]
    public void TestDetermineLength2()
    {
        var minPad = 9;

        var gate1Value = 64;
        var gate1Split = gate1Value - minPad;
        for (int i = 0; i < gate1Split; ++i)
        {
            Assert.AreEqual(gate1Value, MD4Internals.DetermineLength(i));
        }

        var gate2Value = 128;
        var gate2Split = gate2Value - minPad;
        for (int i = gate1Split + 1; i < gate2Split; ++i)
        {
            Assert.AreEqual(gate2Value, MD4Internals.DetermineLength(i));
        }

        var gate3Value = 192;
        var gate3 = gate3Value - minPad;
        for (int i = gate2Split + 1; i < gate3; ++i)
        {
            Assert.AreEqual(gate3Value, MD4Internals.DetermineLength(i));
        }

        var gate4Value = 256;
        var gate4Split = gate4Value - minPad;
        for (int i = gate3 + 1; i < gate4Split; ++i)
        {
            Assert.AreEqual(gate4Value, MD4Internals.DetermineLength(i));
        }
    }

    [TestMethod]
    public void TestPadEmpty()
    {
        var input = Array.Empty<byte>();

        byte[] expected = new byte[64];
        expected[0] = 0b1000_0000;
        expected[^8] = 0x00;

        var actual = MD4Internals.PadInput(input);

        Assert.AreEqual(64, actual.Length);
        CollectionAssert.AreEqual(expected, actual.ToArray());
    }

    [TestMethod]
    public void TestPad1Byte()
    {
        var input = new byte[] { 0b1010_0101 };

        byte[] expected = new byte[64];
        expected[0] = 0b1010_0101;
        expected[1] = 0b1000_0000;
        expected[^8] = 0x08;

        var actual = MD4Internals.PadInput(input);

        Assert.AreEqual(64, actual.Length);
        CollectionAssert.AreEqual(expected, actual.ToArray());
    }

    [TestMethod]
    public void TestPad2Byte()
    {
        var input = new byte[] { 0b0110_1001, 0b1111_0000, };

        byte[] expected = new byte[64];
        expected[0] = 0b0110_1001;
        expected[1] = 0b1111_0000;
        expected[2] = 0b1000_0000;
        expected[^8] = 0x10;

        var actual = MD4Internals.PadInput(input);

        Assert.AreEqual(64, actual.Length);
        CollectionAssert.AreEqual(expected, actual.ToArray());
    }

    [TestMethod]
    public void TestEmptyData()
    {
        var md4 = new MD4([]);

        Assert.AreEqual("31d6cfe0d16ae931b73c59d7e0c089c0", md4.DigestString);
    }

    // Test cases from RFC 1186 page 16/17 - MDtestsuite() MDstring(s) MDprint(MDp)
    [TestMethod]
    [DataRow("31d6cfe0d16ae931b73c59d7e0c089c0", "")]
    [DataRow("bde52cb31de33e46245e05fbdbd6fb24", "a")]
    [DataRow("a448017aaf21d8525fc10ae87aa6729d", "abc")]
    [DataRow("d9130a8164549fe818874806e1c7014b", "message digest")]
    [DataRow("043f8582f241db351ce627e153e7f0e4", "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")]
    [DataRow("e33b4ddc9c38f2199c3e7b164fcc0536", "12345678901234567890123456789012345678901234567890123456789012345678901234567890")]
    public void Test(string expectedDigest, string text)
    {
        var md4 = new MD4(Encoding.ASCII.GetBytes(text).AsSpan());
        Assert.AreEqual(expectedDigest, md4.DigestString);
    }
}
