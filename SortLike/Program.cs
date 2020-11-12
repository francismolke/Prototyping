using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortLike
{
    class Program
    {
        static void Main(string[] args)
        {
            SortedList<int, LikeabilityQuellen> slClips = new SortedList<int, LikeabilityQuellen>();
            List<double> ld = new List<double>();
            List<LikeabilityQuellen> lg = new List<LikeabilityQuellen>();
            List<LikeabilityQuellen> lgg = new List<LikeabilityQuellen>();
          //  using (StreamReader sr = new StreamReader("Quellen_Likeability.txt", Encoding.UTF8))
            using (StreamReader sr = new StreamReader("Quellen_Likeability_Vorlage.txt", Encoding.UTF8))
            {
                string line;
                string likestringValue;
                double likeValue;
                int n = 0;
                while ((line = sr.ReadLine()) != null)
                {

                //    likestringValue = line.Substring(line.IndexOf("/ - ") + 3).Trim();
                    likestringValue = line.Substring(line.IndexOf(".mp4") + 6).Trim();
                    likeValue = double.Parse(likestringValue, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("de-DE"));
                    //  line = line.Remove(line.IndexOf("/ - "));
                    line = line.Remove(line.IndexOf(".mp4") +4);
                    //line = line.Substring(3);
                    lg.Add(new LikeabilityQuellen(likeValue, line));

                    lgg = lg.OrderBy(o => o.Likeability).ToList();
                    lgg.Reverse();



                    
                }
                
                foreach (var name in lgg)
                {
                    var haha = name.Link;
                }
                List<string> hehe = GetAllVideosFromFolder();
                List<LikeabilityQuellen> hoho = new List<LikeabilityQuellen>();
                foreach(string name in hehe)
                {
                    hoho.Add(new LikeabilityQuellen(0,(name.Substring(name.IndexOf(" - ") + 2).Trim())));
                    var ehehe = name.Substring(name.IndexOf(" - ") + 2).Trim();
                    var dasf = lgg.Find(o => o.Link == ehehe);

                }

                //var lggg = lgg.Where(b => b.Link ==)
                //var andere = hoho.
                var das = lgg.Find(o => o.Link == lgg.Skip(n).First().ToString());
                n++;
            }
        }

        private static List<string> GetAllVideosFromFolder()
        {
            string filepath = @"C:\Users\Agrre\Desktop\testproject";
            string[] filePath;
            List<string> lfn = new List<string>();
            filePath = Directory.GetFiles(filepath, "*.mp4");
            foreach (string file in filePath)
            {
               // lfn.Add(Path.GetFileNameWithoutExtension(file));
                lfn.Add(Path.GetFileName(file));
            }
            //  fileNames = new DirectoryInfo(filePath).GetFiles().Select(o => o.Name).ToArray();
            return lfn;
        }
    }
}
