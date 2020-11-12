using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategoryQuellentest
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lTop = new List<string>();
            List<string> lBetter = new List<string>();
            List<string> lGood = new List<string>();
            List<string> lMiddle = new List<string>();
            List<string> lFiller = new List<string>();
            List<string> lEnd = new List<string>();
            for (int i = 1; i <= 6; i++)
            {
                FillCategoryListWithProfiles(i, lTop, lBetter, lGood, lMiddle, lFiller, lEnd);
            }
        }

        private static List<string> FillCategoryListWithProfiles(int i, List<string> lTop, List<string> lBetter, List<string> lGood, List<string> lMiddle, List<string> lFiller, List<string> lEnd)
        {
            string line;
            string profilename;
            switch (i)
            {
                case 1:
                    using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\ProfilListen\\Top.txt", Encoding.UTF8))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            //   profilename = line.Substring(26).Trim();
                            profilename = line.Substring(26);
                            profilename = profilename.Substring(0, profilename.Length - 1);
                            lTop.Add(profilename);
                        }
                    }
                    return lTop;

                case 2:
                    using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\ProfilListen\\Better.txt", Encoding.UTF8))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            //   profilename = line.Substring(26).Trim();
                            profilename = line.Substring(26);
                            profilename = profilename.Substring(0, profilename.Length - 1);
                            lTop.Add(profilename);
                        }
                    }
                    return lBetter;
                case 3:
                    using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\ProfilListen\\Good.txt", Encoding.UTF8))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            //   profilename = line.Substring(26).Trim();
                            profilename = line.Substring(26);
                            profilename = profilename.Substring(0, profilename.Length - 1);
                            lTop.Add(profilename);
                        }
                    }
                    return lGood;
                case 4:
                    using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\ProfilListen\\Middle.txt", Encoding.UTF8))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            //   profilename = line.Substring(26).Trim();
                            profilename = line.Substring(26);
                            profilename = profilename.Substring(0, profilename.Length - 1);
                            lTop.Add(profilename);
                        }
                    }
                    return lMiddle;
                case 5:
                    using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\ProfilListen\\Filler.txt", Encoding.UTF8))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            //   profilename = line.Substring(26).Trim();
                            profilename = line.Substring(26);
                            profilename = profilename.Substring(0, profilename.Length - 1);
                            lTop.Add(profilename);
                        }
                    }
                    return lFiller;
                case 6:
                    using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\ProfilListen\\End.txt", Encoding.UTF8))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            //   profilename = line.Substring(26).Trim();
                            profilename = line.Substring(26);
                            profilename = profilename.Substring(0, profilename.Length - 1);
                            lTop.Add(profilename);
                        }
                    }
                    return lEnd;
            }
            // nicht null returnen vl
            return null;
        }









    }
}
