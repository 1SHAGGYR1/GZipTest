using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace CompDecomp
{
    static class Archiver
    {
        const int step = 32 * 1024 * 1024;
        public static void Compress(string infile, string outfile)
        {
            using (Stream outStream = new FileInfo(outfile).Open(FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (Stream inStream = new FileInfo(infile).Open(FileMode.Open))
                {
                    using (GZipStream compresser = new GZipStream(outStream, CompressionMode.Compress))
                    {
                        Byte[] buffer;
                        int moreThanInt = 0, blockDivider = 0;
                        while (blockDivider < inStream.Length)
                        {
                            buffer = new Byte[(inStream.Length > blockDivider + moreThanInt * int.MaxValue + step) ? step : inStream.Length - (blockDivider + moreThanInt * int.MaxValue)];
                            inStream.Read(buffer, blockDivider, buffer.Length);
                            compresser.Write(buffer, 0, buffer.Length);
                            if (blockDivider + step > int.MaxValue)
                            {
                                moreThanInt++;
                                blockDivider = step - (int.MaxValue - blockDivider);
                                inStream.Position = moreThanInt * int.MaxValue;
                            }
                            else
                                blockDivider += step;
                        }
                        compresser.Flush();
                    }
                }
            }
        }
        public static void Decompress(string filein, string fileout)
        {
            using (Stream outStream = new FileInfo(fileout).Open(FileMode.Create, FileAccess.ReadWrite))
            {
                using (Stream inStream = new FileInfo(filein).Open(FileMode.Open))
                {
                    using (GZipStream decompresser = new GZipStream(outStream, CompressionMode.Decompress))
                    {
                        Byte[] buffer;
                        int moreThanInt = 0, blockDivider = 0;
                        while (blockDivider < inStream.Length)
                        {
                            buffer = new byte[(blockDivider + moreThanInt * int.MaxValue + step) > inStream.Length ? inStream.Length - (blockDivider + moreThanInt * int.MaxValue) : step];
                            inStream.Read(buffer, blockDivider, buffer.Length);
                            decompresser.Read(buffer, 0, buffer.Length);
                            if (blockDivider + step > int.MaxValue)
                            {
                                moreThanInt++;
                                blockDivider = step - (int.MaxValue - blockDivider);
                                inStream.Position = moreThanInt * int.MaxValue;
                            }
                            else
                                blockDivider += step;
                        }
                        decompresser.Flush();
                    }
                }
            }
        }
    }
}