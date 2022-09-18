namespace Ed2k;

public record Ed2kFileLink(string Filename, long FilesizeB, string Digest, string? TopHash = null, string[]? Sources = null)
{
    public override string ToString()
    {
        return ToString(Filename, FilesizeB, Digest, TopHash, Sources);
    }

    /// <param name="aich"><seealso cref="https://en.wikipedia.org/wiki/Ed2k_URI_scheme#AICH"/></param>
    /// <param name="sources">IP:Port</param>
    public static string ToString(string fname, long fsize, string digest, string? aich, params string[]? sources)
    {
        var h = aich == null ? "" : $"h={aich}|";
        var srcAppend = sources == null || sources.Length == 0 ? "" : $"|sources,{string.Join(",", sources)}|/";
        return $"ed2k://|file|{fname}|{fsize}|{digest}|{h}/{srcAppend}";
    }
}
