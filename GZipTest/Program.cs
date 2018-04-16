using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompDecomp
{
    class Program
    {
        static void Main(string[] args)
        {
            String pathin = args[1], pathout = args[2];
            Archiver.Compress(pathin, pathout);
            Archiver.Decompress(pathout, "checkdecompress.txt");
        }
    }
}