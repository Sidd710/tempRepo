using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexDump
{
    public static class HexTools
    {
        public static void DumpFile(string inputFile, string outputFile)
        {
            const int MAX_ITEMS_PER_LINE = 12;

            using (FileStream inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
            {
                Console.WriteLine("Dumping {0} to {1}", inputFile, outputFile);
                BinaryReader binaryReader = new BinaryReader(inputFileStream);
                
                using (FileStream outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (TextWriter writer = new StreamWriter(outputFileStream, Encoding.ASCII))
                    {
                        writer.WriteLine("#define RAW_ASSEMBLY_LENGTH {0}", inputFileStream.Length);
                        writer.WriteLine("unsigned char rawData[{0}] = {{", inputFileStream.Length);
                        long bytesRead = 0;
                        int itemsPerLine = 0;
                        while (bytesRead != inputFileStream.Length)
                        {
                            // Uncomment if you would like to compare the output with HxD HexEditor Save As C output, now commented to save space
                            //if (itemsPerLine == 0)
                            //    writer.Write("\t");
                            //else
                            //    writer.Write(" ");

                            byte fileByte = binaryReader.ReadByte();
                            writer.Write("0x{0:X2},", fileByte ^ 'H');
                            itemsPerLine++;
                            if (itemsPerLine == MAX_ITEMS_PER_LINE)
                            {
                                writer.WriteLine();
                                itemsPerLine = 0;
                            }
                            bytesRead++;
                        }

                        writer.WriteLine("};");
                    }
                }
            }
        }
    }
}
