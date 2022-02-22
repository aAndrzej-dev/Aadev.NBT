namespace Aadev.NBT
{



    public static class NWriter
    {
        public static void WriteToFile(string filename, INTag tag, Endianness endianness = Endianness.Big) => File.WriteAllBytes(filename, tag.ToByteArray(endianness));
        public static byte[] WriteToByteArray(INTag tag, Endianness endianness = Endianness.Big) => tag.ToByteArray(endianness);

    }
}