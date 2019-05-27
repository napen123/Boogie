# Boogie
A simple program to change a file's encoding.

## Usage
#### Change a file's encoding:
> Boogie <input encoding> [optional output encoding] <input file> <output file>


#### Change the encoding of entry names in a zip file:
> Boogie -z <input encoding> [optional output encoding] <input file> <output file>
* The outputted zip file is compressed in a fast way (non-optimally).

> Boogie -zo <input encoding> [optional output encoding] <input file> <output file>
* The outputted zip file is compressed in an optimal way (non-fast).
