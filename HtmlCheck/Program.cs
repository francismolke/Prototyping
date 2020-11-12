using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace HtmlCheck
{
    class Program
    {
        static void Main(string[] args)
        {

            string targetPath = @"C:\Users\Agrre\Desktop\SD291";
            CopyQuellenToDebugFolder(targetPath);
            //var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Download.htm";
            //var htmlDoc = new HtmlDocument();
            //htmlDoc.Load(path);
            //// zweite


            //// Check if Bot was not blocked by Instagram
            //var node31 = htmlDoc.DocumentNode.SelectNodes("//pre[@style='word-wrap: break-word; white-space: pre-wrap;']");
            //var doesPreExists = htmlDoc.DocumentNode.Descendants("pre").Any();

            //if (doesPreExists == true)
            //{

            //}
            //if (doesPreExists == false)
            //{

            //}
        }

        private static void CopyQuellenToDebugFolder(string targetPath)
        {
            string qline;
            File.Copy(System.IO.Path.Combine(targetPath, "Quellen.txt"), System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Quellen.txt"), true);

            List<string> quellenvergleich = new List<string>();
            using (StreamReader srquellen = new StreamReader("Quellen.txt", Encoding.UTF8))
            {
                while ((qline = srquellen.ReadLine()) != null)
                {
                    quellenvergleich.Add(qline);
                }
            }


        }
    }
}
