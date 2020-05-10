﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GondoAssist_Tags
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Program los = new Program();
            los.Run();
            
        }

        public void Run()
        {
            int lastExtentRefNumber = 0;
            int n = 0;
         //   var currentDirectory = Directory.GetCurrentDirectory() + @"\Slamdank1.wlmp";
            var currentDirectory = @"C:\Users\Agrre\Desktop\alte\InsertTitleOnVideo\GondoAssist_AutoVideo\bin\Debug\SD284.wlmp";
            // var currentDirectory = Directory.GetCurrentDirectory() + @"\SD2801.wlmp";

            XDocument doc = XDocument.Load(currentDirectory);

            #region Select From XML
            // Select ExtentSelector für VIDEOS (Oberster Parent) 
            IEnumerable<XElement> ExtentSelectorVideo = from el in doc.Descendants("ExtentSelector") where (int)el.Attribute("extentID") == 1 select el;
            // Select ExtentRef innerhalb von ExtentSelector (Kind von ExtentSelector)
            IEnumerable<XElement> ExtentRefVideo = from el in ExtentSelectorVideo.Descendants("ExtentRef") where (int?)el.Attribute("id") != null select el;
            List<string> ExtentRefElementVideo = new List<string>();
            lastExtentRefNumber = GetHighestIDNumber(lastExtentRefNumber, ExtentRefVideo, ExtentRefElementVideo);
            string reihenFolgeVideo = ExtentRefElementVideo.Skip(n).FirstOrDefault();
            #endregion

            int amountOfMediaItems = GetAmountOfMediaItems(doc);


         //   lastExtentRefNumber = GetHighestIDNumber(lastExtentRefNumber, ExtentRefVideo, ExtentRefElementVideo);

            MakeExtentSelectorForText(doc, lastExtentRefNumber, currentDirectory, amountOfMediaItems, reihenFolgeVideo);
        }

        private static void MakeExtentSelectorForText(XDocument docX, int letzteZahl, string currentDirectoryC, int amountOfMediaItems, string reihenFolgeVideo)
        {
            RemoveExtentRefsFromXML(docX);
            MakeOneExtentRefWithOneNode(docX, currentDirectoryC, letzteZahl, amountOfMediaItems);
            XDocument docY = XDocument.Load(currentDirectoryC);

            //  XElement docExtentRefs = new XElement("ExtentRefs", new XElement("ExtentRef"));
            // docX.Descendants("ExtentSelector").Where(i => i.Attribute("extentID").Value == "4").Descendants("BoundProperties").FirstOrDefault().Add(docExtentRefs);

            int n = 0;
            int idIterator = letzteZahl - 1 + amountOfMediaItems;
            int titleID = letzteZahl;
            string fileName = "";
            while (n < amountOfMediaItems)
            {
                idIterator--;
                letzteZahl++;
                // n + 1
                if (n + 1 != amountOfMediaItems)
                {

                    XElement doc = new XElement(
                    new XElement("ExtentRef", new XAttribute("id", idIterator)));
                    docY.Descendants("ExtentSelector").Where(i => i.Attribute("extentID").Value == "4").Descendants("ExtentRefs").FirstOrDefault().AddFirst(doc);

                    //getFileNameFromMediaItem(n, docY, reihenFolgeVideo);
                    fileName = getFileNameFromMediaItem(n, docY, reihenFolgeVideo, currentDirectoryC);
                }
                else
                {
                    // eins zu wenig?
                }


                // Hier noch Prüfen ob das ein Instagram/Youtube etc Video ist
                string checkedFileName = CheckFileNameForSource(fileName);
                var docTitleClip = CreateTitleClipXElement(letzteZahl, checkedFileName, n);
                docY.Root.Descendants("Extents").FirstOrDefault().AddFirst(docTitleClip);


                docY.Save(currentDirectoryC);


                n++;
            }
        }

        private static void RemoveExtentRefsFromXML(XDocument docX)
        {
            var hoho = from al in docX.Descendants("ExtentSelector") where (int)al.Attribute("extentID") == 4 select al.Descendants("ExtentRefs");

            foreach (var item in hoho)
            {

                foreach (var itemx in item)
                {
                    if (itemx.Name == "ExtentRefs")
                    {
                        itemx.Remove();
                        break;
                    }
                }

            }
        }

        private static void MakeOneExtentRefWithOneNode(XDocument docX, string currentDirectoryC, int letzteZahl, int amountOfMediaItems)
        {
            XElement doc = new XElement(
            new XElement("ExtentRefs", new XElement("ExtentRef", new XAttribute("id", letzteZahl - 1 + amountOfMediaItems))));

            docX.Descendants("ExtentSelector").Where(i => i.Attribute("extentID").Value == "4").Descendants("BoundProperties").FirstOrDefault().AddAfterSelf(doc);
            docX.Save(currentDirectoryC);
        }

        private static string getFileNameFromMediaItem(int n, XDocument doc, string reihenFolgeVideo, string currentDirectoryC)
        {

            //string mediaID = GetMediaAndVideoID(n, doc, reihenFolgeVideo);

            // Gibt den Namen des Videos her
         //   int videoclipmediaitemID = GetCurrentClipWithExtendID(currentDirectoryC);

           IEnumerable<XElement> MediaFileName = from al in doc.Descendants("MediaItem") where (int)al.Attribute("id") == n + 1 select al;
       //     IEnumerable<XElement> MediaFileName = from al in doc.Descendants("MediaItem") where (int)al.Attribute("id") == videoclipmediaitemID select al;
            IEnumerable<XElement> MediaFilePath = from el in MediaFileName.Descendants("MediaItem") where (string)el.Attribute("filePath") != null select el;
            List<string> MediaItemNameElement = new List<string>();
            foreach (XElement al in MediaFileName)
            {
                MediaItemNameElement.Add(al.Attribute("filePath").Value);
            }
            string mediaItemName = MediaItemNameElement.FirstOrDefault();
            mediaItemName = Path.GetFileName(mediaItemName);
            if (mediaItemName == null)
            {
                return "leer";
            }
            return mediaItemName;
        }

        private static int GetCurrentClipWithExtendID(string currentDirectory)
        {
            XDocument doc = XDocument.Load(currentDirectory);

            IEnumerable<XElement> ExtentSelectorVideo = from el in doc.Descendants("ExtentSelector") where (int)el.Attribute("extentID") == 1 select el;
            // Select ExtentRef innerhalb von ExtentSelector (Kind von ExtentSelector)
            IEnumerable<XElement> ExtentRefVideo = from el in ExtentSelectorVideo.Descendants("ExtentRef") where (int?)el.Attribute("id") != null select el;
            List<string> ExtentRefElementVideo = new List<string>();
           // lastExtentRefNumber = GetHighestIDNumber(lastExtentRefNumber, ExtentRefVideo, ExtentRefElementVideo);
            string reihenFolgeVideo = ExtentRefElementVideo.Skip(0).FirstOrDefault();


            // Erhalte die erweiterte ID des Videosclips, um mit mediaItemID arbeiten zu können
            // the Order of videos in MovieMaker ExtentID
            IEnumerable<XElement> mediaItemID = from al in doc.Descendants("VideoClip") where (string)al.Attribute("extentID") == reihenFolgeVideo select al;
            List<string> media_ItemElement = new List<string>();
            foreach (XElement al in mediaItemID)
            {
                media_ItemElement.Add(al.Attribute("mediaItemID").Value);
            }
            //  verpackt die erhaltenen daten in der schleife zugreifbare variablen
            string VmediaID = media_ItemElement.Skip(0).FirstOrDefault();

            IEnumerable<XElement> MediaID = from el in doc.Descendants("MediaItem") where (string)el.Attribute("id") == VmediaID select el;
            List<string> MediaElement = new List<string>();
            foreach (XElement el in MediaID)
            {
                MediaElement.Add(el.FirstAttribute.Value);
            }




            //  verpackt die erhaltenen daten in der schleife zugreifbare variablen
            //         string mediaID = MediaElement.Skip(n).FirstOrDefault();
            string mediaID = MediaElement.FirstOrDefault();
            return Int32.Parse(mediaID);

        }


        //private static void takeFromVideoClipmediaID(XDocument doc)
        //{
        //    IEnumerable<XElement> extendVideoMediaItemID = from al in doc.Descendants()
        //}
        private static string XgetFileNameFromMediaItem(int n, XDocument doc, string reihenFolgeVideo)
        {

            //string mediaID = GetMediaAndVideoID(n, doc, reihenFolgeVideo);

            // Gibt den Namen des Videos her
            IEnumerable<XElement> MediaFileName = from al in doc.Descendants("MediaItem") where (int)al.Attribute("id") == n + 1 select al;
            IEnumerable<XElement> MediaFilePath = from el in MediaFileName.Descendants("MediaItem") where (string)el.Attribute("filePath") != null select el;
            List<string> MediaItemNameElement = new List<string>();
            foreach (XElement al in MediaFileName)
            {
                MediaItemNameElement.Add(al.Attribute("filePath").Value);
            }
            string mediaItemName = MediaItemNameElement.FirstOrDefault();
            mediaItemName = Path.GetFileName(mediaItemName);
            if (mediaItemName == null)
            {
                return "leer";
            }
            return mediaItemName;
        }


        private static string CheckFileNameForSource(string fileName)
        {
            Regex mr1 = new Regex(@"^[a-z._A-Z0-9]+\s-\s.{11}\.mp4$");
            Regex mr2 = new Regex(@"^[a-z._A-Z0-9]+\s-\s.{23}\.mp4$");
            if (mr1.IsMatch(fileName))
            {
                // INSTAGRAM PATH
                var index = fileName.IndexOf('-');
                return fileName.Substring(0, index).Trim();
            }
            if(mr2.IsMatch(fileName))
            {
                var index = fileName.IndexOf('-');
                return fileName.Substring(0, index).Trim();

            }
            {
                Console.WriteLine("was anderes");
                return fileName;
            }


        }

        private static int GetAmountOfMediaItems(XDocument doc)
        {
            int amountOfMediaItems = 0;
            IEnumerable<XElement> MediaIDhoechsteZahl = from el in doc.Descendants("MediaItem") where (string)el.Attribute("id") != null select el;
            List<string> MediaElementHoechsteZahl = new List<string>();
            foreach (XElement el in MediaIDhoechsteZahl)
            {
                MediaElementHoechsteZahl.Add(el.FirstAttribute.Value);
                amountOfMediaItems = MediaIDhoechsteZahl.Count();

            }

            return amountOfMediaItems;
        }




        private static XElement CreateTitleClipXElement(int idIterator, string fileName, int n)
        {
            idIterator -= 1;
            // var currentDirectory = Directory.GetCurrentDirectory() + @"\Slamdank1.wlmp";
            var currentDirectory = @"C:\Users\Agrre\Desktop\alte\InsertTitleOnVideo\GondoAssist_AutoVideo\bin\Debug\SD284.wlmp";

            //  var currentDirectory = Directory.GetCurrentDirectory() + @"\SD2801.wlmp";
            XDocument doc = XDocument.Load(currentDirectory);
            double duration = GetDurationOfClip(doc, n);
            XElement titleDocX = new XElement(
                new XElement("TitleClip", new XAttribute("extentID", idIterator), new XAttribute("gapBefore", 0), new XAttribute("duration", duration),
                    new XElement("Effects",                                                                 // Bei Intro = 2 ? ansonsten immer 0 ?
                    new XElement("TextEffect", new XAttribute("effectTemplateID", "TextEffectStretchTemplate"), new XAttribute("TextScriptId", 0),
                    new XElement("BoundProperties",
                    new XElement("BoundPropertyBool", new XAttribute("Name", "automatic"), new XAttribute("Value", "false")),
                    new XElement("BoundPropertyFloatSet", new XAttribute("Name", "color"),
                    new XElement("BoundPropertyFloatElement", new XAttribute("Value", 1)),
                    new XElement("BoundPropertyFloatElement", new XAttribute("Value", 1)),
                    new XElement("BoundPropertyFloatElement", new XAttribute("Value", 1))),
                    new XElement("BoundPropertyStringSet", new XAttribute("Name", "family"),
                    new XElement("BoundPropertyStringElement", new XAttribute("Value", "Rocket Rinder"))),
                    new XElement("BoundPropertyBool", new XAttribute("Name", "horizontal"), new XAttribute("Value", "true")),
                    new XElement("BoundPropertyStringSet", new XAttribute("Name", "justify"),
                    new XElement("BoundPropertyStringElement", new XAttribute("Value", "MIDDLE"))),
                    new XElement("BoundPropertyBool", new XAttribute("Name", "leftToRight"), new XAttribute("Value", "true")),
                    new XElement("BoundPropertyFloatSet", new XAttribute("Name", "length")),
                    new XElement("BoundPropertyFloat", new XAttribute("Name", "maxExtent"), new XAttribute("Value", 0)),
                    new XElement("BoundPropertyFloatSet", new XAttribute("Name", "outlineColor"),
                    new XElement("BoundPropertyFloatElement", new XAttribute("Value", 0)),
                    new XElement("BoundPropertyFloatElement", new XAttribute("Value", 0)),
                    new XElement("BoundPropertyFloatElement", new XAttribute("Value", 0))),
                    new XElement("BoundPropertyInt", new XAttribute("Name", "outlineSizeIndex"), new XAttribute("Value", 1)),
                    new XElement("BoundPropertyFloatSet", new XAttribute("Name", "position"),
                    // HIER SIND DIE POSITIONSVALUES X Y Z
                    new XElement("BoundPropertyFloatElement", new XAttribute("Value", 3)),
                    new XElement("BoundPropertyFloatElement", new XAttribute("Value", 3)),
                    new XElement("BoundPropertyFloatElement", new XAttribute("Value", 3))),
                    new XElement("BoundPropertyFloat", new XAttribute("Name", "size"), new XAttribute("Value", 0.5)),
                    new XElement("BoundPropertyStringSet", new XAttribute("Name", "string"),
                    // HIER IST DIE NAMENS / TEXT VARIABLE
                    new XElement("BoundPropertyStringElement", new XAttribute("Value", "@" + fileName))),
                    new XElement("BoundPropertyString", new XAttribute("Name", "style"), new XAttribute("Value", "Plain")),
                    new XElement("BoundPropertyFloat", new XAttribute("Name", "transparency"), new XAttribute("Value", 0))))),
                    new XElement("Transitions"),
                    new XElement("BoundProperties", new XElement("BoundPropertyFloatSet", new XAttribute("Name", "diffuseColor"),
                    new XElement("BoundPropertyFloatElement", new XAttribute("Value", 0.75)),
                    new XElement("BoundPropertyFloatElement", new XAttribute("Value", 1)),
                    new XElement("BoundPropertyFloatElement", new XAttribute("Value", 0))),
                    new XElement("BoundPropertyFloat", new XAttribute("Name", "transparency"), new XAttribute("Value", 1)))));
            return titleDocX;
        }

        private static double GetDurationOfClip(XDocument doc, int n)
        {
            int lastExtentRefNumber = 0;
            //    int n = 0;

            // Select ExtentSelector für VIDEOS (Oberster Parent) 
            IEnumerable<XElement> ExtentSelectorVideo = from el in doc.Descendants("ExtentSelector") where (int)el.Attribute("extentID") == 1 select el;
            // Select ExtentRef innerhalb von ExtentSelector (Kind von ExtentSelector)
            IEnumerable<XElement> ExtentRefVideo = from el in ExtentSelectorVideo.Descendants("ExtentRef") where (int?)el.Attribute("id") != null select el;
            List<string> ExtentRefElementVideo = new List<string>();
            lastExtentRefNumber = GetHighestIDNumber(lastExtentRefNumber, ExtentRefVideo, ExtentRefElementVideo);
            string reihenFolgeVideo = ExtentRefElementVideo.Skip(n).FirstOrDefault();


            // Erhalte die erweiterte ID des Videosclips, um mit mediaItemID arbeiten zu können
            // the Order of videos in MovieMaker ExtentID
            IEnumerable<XElement> mediaItemID = from al in doc.Descendants("VideoClip") where (string)al.Attribute("extentID") == reihenFolgeVideo select al;
            List<string> media_ItemElement = new List<string>();
            foreach (XElement al in mediaItemID)
            {
                media_ItemElement.Add(al.Attribute("mediaItemID").Value);
            }
            //  verpackt die erhaltenen daten in der schleife zugreifbare variablen
            string VmediaID = media_ItemElement.Skip(0).FirstOrDefault();

            IEnumerable<XElement> MediaID = from el in doc.Descendants("MediaItem") where (string)el.Attribute("id") == VmediaID select el;
            List<string> MediaElement = new List<string>();
            foreach (XElement el in MediaID)
            {
                MediaElement.Add(el.FirstAttribute.Value);
            }




            //  verpackt die erhaltenen daten in der schleife zugreifbare variablen
            //         string mediaID = MediaElement.Skip(n).FirstOrDefault();
            string mediaID = MediaElement.FirstOrDefault();


            double cutDuration = CheckIfClipWasCut(doc, reihenFolgeVideo);
            double mdD = 0;
            if (cutDuration == 0)
            {


                // Nimmt die id, für die Länge des Videos
                IEnumerable<XElement> MediaItemDur = from al in doc.Descendants("MediaItem") where (string)al.Attribute("id") == mediaID select al;
                List<string> MediaItemDurElement = new List<string>();
                foreach (XElement al in MediaItemDur)
                {
                    MediaItemDurElement.Add(al.Attribute("duration").Value);
                }
                string mediaDuration = MediaItemDurElement.FirstOrDefault();
                if (mediaDuration != null)
                {
                    mdD = double.Parse(mediaDuration, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-GB"));
                    double mediaDurDouble = mdD;
                }
                else
                {
                    //    break;
                }
            }
            else
            {
                mdD = cutDuration;
            }
            n++;
            return mdD;

        }

        private static double CheckIfClipWasCut(XDocument doc, string reihenFolgeVideo)
        {
            double duration = 0;
            // Erhalte die erweiterte ID des Videosclips, um die inTime(gekürzte duration von vorne (i)) zu erhalten
            IEnumerable<XElement> mediaItemID = from al in doc.Descendants("VideoClip") where (string)al.Attribute("extentID") == reihenFolgeVideo select al;
            List<string> media_InTimeElement = new List<string>();
            foreach (XElement al in mediaItemID)
            {
                media_InTimeElement.Add(al.Attribute("inTime").Value);
            }
            //  verpackt die erhaltenen daten in der schleife zugreifbare variablen
            string VmediaInTimeElement = media_InTimeElement.Skip(0).FirstOrDefault();
            double VmediaInTimeInt = double.Parse(VmediaInTimeElement, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-GB"));


            // Erhalte die erweiterte ID des Videosclips, um die outTime(gekürzte duration von vorne (O)) zu erhalten
            List<string> media_OutTimeElement = new List<string>();
            foreach (XElement al in mediaItemID)
            {
                media_OutTimeElement.Add(al.Attribute("outTime").Value);
            }
            //  verpackt die erhaltenen daten in der schleife zugreifbare variablen
            string VmediaOutTimeElement = media_OutTimeElement.Skip(0).FirstOrDefault();
            double VmediaOutTimeInt = double.Parse(VmediaOutTimeElement, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-GB"));



            // CUT BOTH I & O (intime & outtime) (Wenn nur vorne gecutet dann ist es wie wenn beide seiten gecutet
            if (VmediaInTimeInt != 0 && VmediaOutTimeInt != 0)
            {
                duration = VmediaOutTimeInt - VmediaInTimeInt;
            }
            // CUT O (outtime)
            else if (VmediaInTimeInt == 0 && VmediaOutTimeInt != 0)
            {
                duration = VmediaOutTimeInt;
            }
            // C
            return duration;
        }

        private static int GetHighestIDNumber(int letzteZahl, IEnumerable<XElement> ExtentRefVideo, List<string> ExtentRefElementVideo)
        {
            foreach (XElement el in ExtentRefVideo)
            {

                ExtentRefElementVideo.Add(el.FirstAttribute.Value);
                if (letzteZahl == 0)
                {
                    letzteZahl = Int32.Parse(el.FirstAttribute.Value);

                }
                int ausgewählteZahl = Int32.Parse(el.FirstAttribute.Value);
                if (ausgewählteZahl >= letzteZahl)
                {
                    letzteZahl = ausgewählteZahl;
                }
            }

            return letzteZahl + 1;
        }



    }
}
