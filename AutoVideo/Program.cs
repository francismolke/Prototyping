using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AutoVideo
{
    class Program
    {
        static void Main(string[] args)
        {

            CreateBlankProjekt();
        }


        private static void CreateBlankProjekt()
        {
            string projektDatei = "Slamdank1.wlmp";
            string projektName = "Slamdank1";
            //  string filepath = @"C:\Users\Agrre\Desktop\alte\InsertTitleOnVideo\InsertTitleOnVideo\bin\Debug\sorryjungs\bearboob - B8rppWTgaaq.mp4";


            // INTRO AUSWÄHLEN
            // Holt die Videos aus dem Ordner
            string[] filepath = GetAllVideosFromFolder();
            int width = 1920;
            int height = 1080;
            double duration = 30.93;
            using (XmlWriter writer = XmlWriter.Create(projektDatei))
            {
                writer.WriteStartDocument();
            }
            string newDirectiory = Directory.GetCurrentDirectory() + @"\" + projektDatei;

            // CREATES PROJECT/MEDIAITEMS/MEDIAITEM on EMPTY XML
            new XDocument(
                    new XElement("Project", new XAttribute("name", projektName), new XAttribute("themeId", 0), new XAttribute("version", 65540), new XAttribute("templateID", "SimpleProjectTemplate"),
                                new XElement(new XElement("MediaItems", new XElement("MediaItem", new XAttribute("id", 1), new XAttribute("filePath", filepath[0]), new XAttribute("arWidth", width), new XAttribute("arHeight", height), new XAttribute("duration", duration), new XAttribute("songTitle", ""), new XAttribute("songArtist", ""), new XAttribute("songAlbum", ""), new XAttribute("songCopyrightUrl", ""), new XAttribute("songArtistUrl", ""), new XAttribute("songAudioFileUrl", ""), new XAttribute("stabilizationMode", 0), new XAttribute("mediaItemType", 1)))),
                                new XElement("Extents", ""))).Save("Slamdank1.wlmp");
            XDocument doc = XDocument.Load(newDirectiory);

            CreateProject(newDirectiory, filepath.Length);

        }



        // Holt die Videos aus dem Ordner
        private static string[] GetAllVideosFromFolder()
        {
            string filepath = @"C:\Users\Agrre\Desktop\testproject";
            string[] fileNames;
            fileNames = Directory.GetFiles(filepath, "*.mp4");
            return fileNames;
        }

        private static void CreateProject(string newDirectiory, int amountOfMediaItems)
        {
            int mediaItemID = 1;

            // Creates Project / MediaItems 
            int letzteZahl = MakeOneMediaItemsWithOneNode(newDirectiory, mediaItemID, amountOfMediaItems);
            letzteZahl += 5;
            // v-- braucht man nicht
            CreateMediaClipXElement();

            //Zähle wie viele mediaItems
            //int lastExtentRefNumber = 0;
            //int letzteZahl = 0;
            //var currentDirectory = Directory.GetCurrentDirectory() + @"\Slamdank1.wlmp";
            //XDocument doc = XDocument.Load(currentDirectory);
            //IEnumerable<XElement> ExtentSelectorVideo = from el in doc.Descendants("ExtentSelector") where (int)el.Attribute("extentID") == 1 select el;
            //IEnumerable<XElement> ExtentRefVideo = from el in ExtentSelectorVideo.Descendants("ExtentRef") where (int?)el.Attribute("id") != null select el;
            //List<string> ExtentRefElementVideo = new List<string>();
            //letzteZahl = GetHighestIDNumber(lastExtentRefNumber, ExtentRefVideo, ExtentRefElementVideo);




            // Creates TitleClips in Extents
            string checkedFileName = "ändern";


            // Insert ExtentSelector
            MakeExtentSelectorForAutomation(newDirectiory);

            // Insert BoundPlaceholders
            MakeBoundPlaceholders(newDirectiory);

            // Insert BoundProperties
            MakeBoundProperties(newDirectiory);

            // Insert ThemeOperationLog
            MakeThemeOperationLog(newDirectiory);

            // Insert AudioDuckingProperties
            MakeAudioDuckingProperties(newDirectiory);

            // Insert ExtentRefs id=1
            int extent4ID = MakeExtentRefForExtentRefs1(newDirectiory, letzteZahl, amountOfMediaItems);

            MakeExtentRefForExtentRef2(newDirectiory, extent4ID, amountOfMediaItems);

            // Insert VideoClips
            // MakeOneVideoClipItemWithOneNode(newDirectiory);

            //  List<string> retrievedVideoList = RetrieveVideos();

            // InsertTitleClipAndSave(letzteZahl, newDirectiory, checkedFileName, amountOfMediaItems);

        }

    }
}
