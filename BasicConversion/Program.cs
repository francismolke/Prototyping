using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicConversion
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = new MediaFile { Filename = @"C:\Users\Agrre\Desktop\gege\MondoDrag.webm" };
            var outputFile = new MediaFile { Filename = @"C:\Users\Agrre\Desktop\gege\MondoDrag.mp4" };

            using (var engine = new Engine())
            {
                engine.Convert(inputFile, outputFile);
            }
        }
    }
}
