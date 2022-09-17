# Markdown File

http://wiki.amule.org/t/index.php?title=MD4_hash

http://www.faqs.org/rfcs/rfc1186.html

Although ed2k hash is based on MD4, it is not MD4. the algorithm for computing ed2k hash is as follows:

1. split the file into blocks of 9728000 bytes
2. compute MD4 hash digest of each file block separately
3. concatenate the list of block digests into one big byte array
4. compute MD4 of array created in step #3. this is the ed2k hash.
