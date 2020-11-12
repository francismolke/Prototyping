using SautinSoft;
using System;

namespace PdfToWord
{
    class Program
    {
        static void Main(string[] args)
        {
            PdfFocus f = new PdfFocus();

            f.OpenPdf(@"C:\Users\Agrre\Desktop\alte\InsertTitleOnVideo\PdfToWord\Büro_Bildschirmarbeitsplatz.pdf");

            if(f.PageCount > 0)
            {
                f.WordOptions.Format = PdfFocus.CWordOptions.eWordDocument.Docx;
                f.ToWord(@"C:\Users\Agrre\Desktop\alte\InsertTitleOnVideo\PdfToWord\Büro_Bildschirmarbeitsplatz.docx");
            }
        }
    }
}
