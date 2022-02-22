# Aadev.NBT
## Named Binary Tag
> NBT (Named Binary Tag) is a tag based binary format designed to carry large amounts of binary data with smaller amounts of additional data.

## Usage

### Reading
 - Gzipped file
```c#
 NTag tag = NReader.FromGzippedFile("exaplne.nbt", Endianness.Big);
 ```
 - Raw file
```c#
 NTag tag = NReader.FromRawFile("exaplne.nbt", Endianness.Big);
 ```
 - Gzipped Byte array
```c#
 NTag tag = NReader.FromGzippedByteArray(byteArray, Endianness.Big);
 ```
  - Raw Byte array
```c#
 NTag tag = NReader.FromByteArray(byteArray, Endianness.Big);
 ```
 ### Writing
 - To file
 ```c#
 NWriter.WriteToFile(filename, tag, Endianness.Big);
 ```
 - To byte array
 ```c#
 byte[] bytes = NWriter.WriteToByteArray(tag, Endianness.Big);
 ```
 ### Endianness
 Use Big-endian for Java Edition NBT Files and Litle-endian for Bedrock Edition NBT Files

 ## Special Thanks
This project is based on documentation from [wiki.vg](https://wiki.vg/NBT) and [fandom.com](https://minecraft.fandom.com/wiki/NBT_format)
