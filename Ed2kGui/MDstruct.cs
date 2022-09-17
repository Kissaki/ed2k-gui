namespace Ed2kGui;

/// <summary>
/// Data structure for a message digest computation.
/// </summary>
public struct MDstruct
{
    /// <summary>
    /// Holds 4-word result of MD computation
    /// </summary>
    uint[] buffer = new uint[4];

    /// <summary>
    /// Number of bits processed so far
    /// </summary>
    char[] count = new char[8];

    /// <summary>
    /// Nonzero means MD computation finished
    /// </summary>
    uint done = 0;

    public MDstruct()
    {
    }

    /// <summary>
    /// Initialize the MDstruct prepatory to doing a message digest computation.
    /// </summary>
    public void begin()
    {
        buffer[0] = 0x67_45_23_01u;
        buffer[1] = 0xef_cd_ab_89u;
        buffer[2] = 0x98_ba_dc_feu;
        buffer[3] = 0x10_32_54_76u;
        for (int i = 0; i < count.Length; ++i) count[i] = (char)0;
        done = 0;
    }

    /// <summary>
    ///
    /// </summary>
    public void MDupdate()
    {
    }
}
