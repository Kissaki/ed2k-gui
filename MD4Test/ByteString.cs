using CommunityToolkit.Diagnostics;
using System.Globalization;

namespace MD4Test;

public class HexByteString
{
    public string Text { get; }
    public byte[] Data { get; }

    public HexByteString(string value)
    {
        value = value.Trim();
        Guard.IsTrue(value.Length % 2 == 0);

        Text = value;

        Data = new byte[value.Length / 2];
        for (int i = 0; i < value.Length / 2; ++i) Data[i / 2] = byte.Parse(value[i..(i + 2)], NumberStyles.HexNumber);
    }

    public HexByteString(byte[] bytes)
    {
        Data = bytes;
        Text = string.Join(" ", bytes.Select(x => "X2"));
    }

    public static implicit operator string(HexByteString value) => value.Text;
    public static implicit operator byte[](HexByteString value) => value.Data;
    public static implicit operator HexByteString(string value) => new(value);
    public static implicit operator HexByteString(byte[] value) => new(value);
}
