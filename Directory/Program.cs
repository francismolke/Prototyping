using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //C: \Users\Agrre\Desktop\alte\InsertTitleOnVideo\Directory\bin\Debug\InstagramProfileLists
            var BinFolder = Directory.GetParent(Directory.GetCurrentDirectory());
            var ParentOfBinFolder = BinFolder.Parent.FullName;
            string[] prosfileListsArray = Directory.GetFiles(ParentOfBinFolder + "\\ProjectItems\\InstagramProfileLists");

            //var asd = Directory.GetParent(profileListsArray);
            //GetFiles("InstagramProfileLists");

        }
    }
}
