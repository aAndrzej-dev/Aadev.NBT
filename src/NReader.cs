using Aadev.NBT.Tags;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Aadev.NBT
{


    public static class NReader
    {
        public static NTag FromRawFile(string filename, Endianness endianness = Endianness.Big) => Read(File.ReadAllBytes(filename), endianness);
        public static NTag FromGzippedFile(string filename, Endianness endianness = Endianness.Big)
        {
            using MemoryStream compressedStream = new MemoryStream(File.ReadAllBytes(filename));
            using GZipStream zipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
            using MemoryStream resultStream = new MemoryStream();
            zipStream.CopyTo(resultStream);

            return Read(resultStream.ToArray(), endianness);

        }
        public static NTag FromByteArray(ReadOnlySpan<byte> bytes, Endianness endianness = Endianness.Big) => Read(bytes, endianness);
        public static NTag FromGzippedByteArray(byte[] bytes, Endianness endianness = Endianness.Big)
        {
            using MemoryStream compressedStream = new MemoryStream(bytes);
            using GZipStream zipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
            using MemoryStream resultStream = new MemoryStream();
            zipStream.CopyTo(resultStream);

            return Read(resultStream.ToArray(), endianness);
        }

        private static NTag Read(ReadOnlySpan<byte> bytes, Endianness endianness)
        {
            try
            {
                int offset = 0;

                return Read(null, ref offset, bytes, endianness);
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot read!\n{ex.Message}");
            }
        }


        private static ReadOnlySpan<byte> ReadBytes(int count, ref int offset, ReadOnlySpan<byte> bytes)
        {
            ReadOnlySpan<byte> buffer = bytes.Slice(offset, count);
            offset += count;
            return buffer;
        }

        private static int ReadInt(ref int offset, ReadOnlySpan<byte> bytes, Endianness endianness) => endianness is Endianness.Big
            ? bytes[offset++] << 24 | bytes[offset++] << 16 | bytes[offset++] << 8 | bytes[offset++]
            : bytes[offset++] | bytes[offset++] << 8 | bytes[offset++] << 16 | bytes[offset++] << 24;
        private static short ReadShort(ref int offset, ReadOnlySpan<byte> bytes, Endianness endianness) => endianness is Endianness.Big
            ? (short)(bytes[offset++] << 8 | bytes[offset++])
            : (short)(bytes[offset++] | bytes[offset++] << 8);
        private static byte ReadByte(ref int offset, ReadOnlySpan<byte> bytes) => bytes[offset++];
        private static long ReadLong(ref int offset, ReadOnlySpan<byte> bytes, Endianness endianness)
        {
            return endianness is Endianness.Big
                ? (long)bytes[offset++] << 56 | (long)bytes[offset++] << 48 | (long)bytes[offset++] << 40 | (long)bytes[offset++] << 32 | (long)bytes[offset++] << 24 | (long)bytes[offset++] << 16 | (long)bytes[offset++] << 8 | bytes[offset++]
                : bytes[offset++] | (long)bytes[offset++] << 8 | (long)bytes[offset++] << 16 | (long)bytes[offset++] << 24 | (long)bytes[offset++] << 32 | (long)bytes[offset++] << 40 | (long)bytes[offset++] << 48 | (long)bytes[offset++] << 56;
        }


        private static NTag Read(INTagParent? parent, ref int offset, ReadOnlySpan<byte> bytes, Endianness endianness)
        {
            NTag current;

            if (parent is INTagArray array)
            {
                current = array.ChildType!.Instance;
            }
            else
            {
                byte typeId = ReadByte(ref offset, bytes);
                current = NTagType.GetTagTypeById(typeId).Instance;

                if (current.HasName)
                {
                    short nameLenght = ReadShort(ref offset, bytes, endianness);

                    current.Name = Encoding.UTF8.GetString(ReadBytes(nameLenght, ref offset, bytes));
                }
            }

            if (current is NTagCompound compound)
            {
                bool notEnded = true;
                while (notEnded)
                {
                    NTag tag = Read(compound, ref offset, bytes, endianness);
                    compound.AddChild(tag);

                    notEnded = !(tag is NTagEnd);
                }
            }
            else if (current is NTagList list)
            {
                byte arrayTypeId = ReadByte(ref offset, bytes);

                list.ChildType = NTagType.GetTagTypeById(arrayTypeId);

                int arrayLenght = ReadInt(ref offset, bytes, endianness);

                for (int i = 0; i < arrayLenght; i++)
                {
                    list.AddChild(Read(list, ref offset, bytes, endianness));
                }
            }
            else if (current is INTagArray ntagArray)
            {
                int arrayLenght = ReadInt(ref offset, bytes, endianness);

                for (int i = 0; i < arrayLenght; i++)
                {
                    ntagArray.AddChild(Read(ntagArray, ref offset, bytes, endianness));
                }
            }
            else if (current is NTagString tagString)
            {
                short stringLenght = ReadShort(ref offset, bytes, endianness);

                tagString.StringValue = Encoding.UTF8.GetString(ReadBytes(stringLenght, ref offset, bytes));
            }
            else if (current.HasValue)
            {
                switch (current)
                {
                    case NTagByte nTagByte: nTagByte.Value = ReadByte(ref offset, bytes); break;
                    case NTagShort nTagShort: nTagShort.Value = ReadShort(ref offset, bytes, endianness); break;
                    case NTagInt nTagInt: nTagInt.Value = ReadInt(ref offset, bytes, endianness); break;
                    case NTagLong nTagLong: nTagLong.Value = ReadLong(ref offset, bytes, endianness); break;
                    case NTagFloat nTagFloat:
                        if (endianness is Endianness.Big)
                        {
                            Span<byte> b = ReadBytes(4, ref offset, bytes).ToArray().AsSpan();
                            MemoryExtensions.Reverse(b);
                            nTagFloat.Value = BitConverter.ToSingle(b);
                        }
                        else
                        {
                            BitConverter.ToSingle(ReadBytes(4, ref offset, bytes));

                        }
                        break;
                    case NTagDouble nTagDouble:
                        if (endianness is Endianness.Big)
                        {
                            Span<byte> b = ReadBytes(8, ref offset, bytes).ToArray().AsSpan();
                            MemoryExtensions.Reverse(b);
                            nTagDouble.Value = BitConverter.ToDouble(b);
                        }
                        else
                        {
                            nTagDouble.Value = BitConverter.ToDouble(ReadBytes(8, ref offset, bytes));
                        }

                        break;
                    default: throw new IndexOutOfRangeException(nameof(current));
                }
            }

            return current;
        }
    }
}