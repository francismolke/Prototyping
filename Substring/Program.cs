using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Substring
{
    class Program
    {
        static void Main(string[] args)
        {
            //line.Substring(line.IndexOf(".mp4 | ") + 7).Trim();

            //string profilename = "https://www.instagram.com/retry.mp4/";
            //string neuerstring = profilename.Substring(26).Trim();
            //profilename = neuerstring.Remove(neuerstring.Length - 1);
            string downloadLink = "https://gondola.stravers.net/MondoDrag.webm";
            
            string connectstring = "files/video/";
            // MondoDrag.webm
            string filename = downloadLink.Substring(29).Trim();
            //https://gondola.stravers.net/
            string linkname = downloadLink.Substring(0, 29);
            //https://gondola.stravers.net/files/video/MondoDrag.webm
            string newDownloadLink = linkname + connectstring + filename;
        }


    }
}
