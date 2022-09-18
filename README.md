# Markdown File

http://wiki.amule.org/t/index.php?title=MD4_hash

ed2k hash is based on/uses MD4. the algorithm for computing ed2k hash is as follows:

1. split the file into blocks of 9728000 bytes
2. compute MD4 hash digest of each file block separately
3. concatenate the list of block digests into one big byte array
4. compute MD4 of array created in step #3. this is the ed2k hash.

[MD4 - RFC 6150](https://www.rfc-editor.org/rfc/rfc6150)

<details><summary>MD4 - RFC 6150, RFC 1320, RFC 1186</summary>

* [RFC 6150](https://www.rfc-editor.org/rfc/rfc6150) *(retires MD4 as historic, obsoletes RFC 1320)*
* [RFC 1320](https://www.rfc-editor.org/rfc/rfc1320) *(obsoletes RFC 1186)*
* [RFC 1186](https://www.rfc-editor.org/rfc/rfc1186)

</details>
