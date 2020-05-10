using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace InsertTitleOnVideo
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 0;
            int lastExtentRefNumber = 0;
            int amountOfMediaItems = 0;
            //var currentDirectory = Directory.GetCurrentDirectory() + @"\SD2801.wlmp";
            var currentDirectory = Directory.GetCurrentDirectory() + @"\zumcoden.wlmp";

            XDocument doc = XDocument.Load(currentDirectory);



            // Select ExtentSelector für VIDEOS (Oberster Parent) 
            IEnumerable<XElement> ExtentSelectorVideo = from el in doc.Descendants("ExtentSelector") where (int)el.Attribute("extentID") == 1 select el;
            // Select ExtentRef innerhalb von ExtentSelector (Kind von ExtentSelector)
            IEnumerable<XElement> ExtentRefVideo = from el in ExtentSelectorVideo.Descendants("ExtentRef") where (int?)el.Attribute("id") != null select el;
            List<string> ExtentRefElementVideo = new List<string>();
            lastExtentRefNumber = GetHighestIDNumber(lastExtentRefNumber, ExtentRefVideo, ExtentRefElementVideo);
            string reihenFolgeVideo = ExtentRefElementVideo.Skip(n).FirstOrDefault();

            amountOfMediaItems = GetAmountOfMediaItems(amountOfMediaItems, doc);


            // Select ExtentSelector für TEXT (Oberster Parent) 
            IEnumerable<XElement> ExtentSelectorText = from el in doc.Descendants("ExtentSelector") where (int)el.Attribute("extentID") == 4 select el;
            // Select ExtentRef innerhalb von ExtentSelector (Kind von ExtentSelector)
            IEnumerable<XElement> ExtentRefText = from el in ExtentSelectorText.Descendants("ExtentRef") where (int?)el.Attribute("id") != null select el;
            List<string> ExtentRefElementText = new List<string>();
            foreach (XElement el in ExtentRefText)
            {
                ExtentRefElementText.Add(el.FirstAttribute.Value);
            }
            string extentTextID = ExtentRefElementText.Skip(n).FirstOrDefault();






            IEnumerable<XElement> textgapBefore = from el in doc.Descendants("TitleClip") where (string)el.Attribute("extentID") == extentTextID select el;
            List<string> TextGapBefore = new List<string>();
            foreach (XElement el in textgapBefore)
            {

            }










            // Gibt die id der Länge des Videos
            IEnumerable<XElement> VideoClipID = from al in doc.Descendants("VideoClip") where (string)al.Attribute("extentID") == reihenFolgeVideo select al;
            List<string> VideoClipElement = new List<string>();
            foreach (XElement al in VideoClipID)
            {
                VideoClipElement.Add(al.FirstAttribute.Value);
            }
            string VextentID = VideoClipElement.Skip(n).FirstOrDefault();




            // Erhalte die erweiterte ID des Videosclips, um mit mediaItemID arbeiten zu können
            // the Order of videos in MovieMaker  ExtentID
            IEnumerable<XElement> mediaItemID = from al in doc.Descendants("VideoClip") where (string)al.Attribute("extentID") == reihenFolgeVideo select al;
            List<string> media_ItemElement = new List<string>();
            foreach (XElement al in mediaItemID)
            {
                media_ItemElement.Add(al.Attribute("mediaItemID").Value);
            }
            //  verpackt die erhaltenen daten in der schleife zugreifbare variablen
            string VmediaID = media_ItemElement.Skip(n).FirstOrDefault();



            IEnumerable<XElement> MediaID = from el in doc.Descendants("MediaItem") where (string)el.Attribute("id") == VmediaID select el;
            List<string> MediaElement = new List<string>();
            foreach (XElement el in MediaID)
            {
                MediaElement.Add(el.FirstAttribute.Value);
            }

            //  verpackt die erhaltenen daten in der schleife zugreifbare variablen
            string mediaID = MediaElement.Skip(n).FirstOrDefault();

           // double mdD;
         //   mdD = GetDurationOfClip(doc, n);
            //  verpackt die erhaltenen daten in eine global zugreifbare variable



            //string mediaItemName = getFileNameFromMediaItem(n, doc, reihenFolgeVideo);



            ////var zwischenspeicher = mediaItemName.Split('\\');
            ////var mediaName = zwischenspeicher[zwischenspeicher.Length - 1];

            //string mediaFileName = Path.GetFileName(mediaItemName);






            // Video start nach schnitt von vorne
            IEnumerable<XElement> VideoInTime = from al in doc.Descendants("VideoClip") where (string)al.Attribute("mediaItemID") == mediaID select al;
            List<string> VideoInTimeElement = new List<string>();
            foreach (XElement al in VideoInTime)
            {
                VideoInTimeElement.Add(al.Attribute("inTime").Value);
            }
            string VideoInTimeString = VideoInTimeElement.Skip(n).FirstOrDefault();
            double VideoInTimeDouble = double.Parse(VideoInTimeString, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-GB"));


            // Gibt die ID an ob ein Video gekürzt wurde
            IEnumerable<XElement> VoutTime = from al in doc.Descendants("VideoClip") where (string)al.Attribute("mediaItemID") == mediaID select al;
            List<string> MediaVoutTime = new List<string>();
            foreach (XElement al in VoutTime)
            {
                MediaVoutTime.Add(al.Attribute("outTime").Value);

            }
            //  verpackt die erhaltenen daten in der schleife zugreifbare variablen
            string videoOutTime = MediaVoutTime.FirstOrDefault();
            double videoOutTDouble = double.Parse(videoOutTime, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-GB"));


            /// TITLECLIP, THROWED NULL EXCEPTION WEIL KEINE TITELCLIP DRINNEN
            // Eingefügte Text die "gapBefore"
            //IEnumerable<XElement> TitleGapBefore = from al in doc.Descendants("TitleClip") where (string)al.Attribute("extentID") == extentTextID select al;
            //List<string> TextGabBefore = new List<string>();
            //foreach (XElement al in TitleGapBefore)
            //{
            //    TextGabBefore.Add(al.Attribute("gapBefore").Value);
            //}
            //string gapBeforeText = TextGabBefore.Skip(n).FirstOrDefault();
            //double gapBeforeTextDouble = double.Parse(gapBeforeText, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-GB"));

            // Eingefügte Text die Länge (duration)
            //IEnumerable<XElement> TitleDuration = from al in doc.Descendants("TitleClip") where (string)al.Attribute("extentID") == extentTextID select al;
            //List<string> TextDuration = new List<string>();
            //foreach (XElement al in TitleDuration)
            //{
            //    TextDuration.Add(al.Attribute("duration").Value);
            //}
            //string durationText = TextDuration.Skip(n).FirstOrDefault();
            //double durationTextDouble = double.Parse(durationText, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-GB"));





             CreateBlankProjekt(lastExtentRefNumber, amountOfMediaItems);


            // amountOfMediaItems
            //while (n > anzahl_der_videoclips)
            MakeExtentSelectorForText(doc, lastExtentRefNumber, currentDirectory, amountOfMediaItems, reihenFolgeVideo);
            //     MakeTitleClipXML(doc);

            //     GetExtentSelectorRefsOfVideoClips(doc);


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
            // the Order of videos in MovieMaker  ExtentID
            IEnumerable<XElement> mediaItemID = from al in doc.Descendants("VideoClip") where (string)al.Attribute("extentID") == reihenFolgeVideo select al;
            List<string> media_ItemElement = new List<string>();
            foreach (XElement al in mediaItemID)
            {
                media_ItemElement.Add(al.Attribute("mediaItemID").Value);
            }
            //  verpackt die erhaltenen daten in der schleife zugreifbare variablen
            string VmediaID = media_ItemElement.Skip(n).FirstOrDefault();

            IEnumerable<XElement> MediaID = from el in doc.Descendants("MediaItem") where (string)el.Attribute("id") == VmediaID select el;
            List<string> MediaElement = new List<string>();
            foreach (XElement el in MediaID)
            {
                MediaElement.Add(el.FirstAttribute.Value);
            }

            //  verpackt die erhaltenen daten in der schleife zugreifbare variablen
            string mediaID = MediaElement.Skip(n).FirstOrDefault();





            double mdD = 0;
            // Gibt die id der Länge des Videos
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
            n++;
            return mdD;
            
        }

        private static string getFileNameFromMediaItem(int n, XDocument doc, string reihenFolgeVideo)
        {
            
            //string mediaID = GetMediaAndVideoID(n, doc, reihenFolgeVideo);

            // Gibt den Namen des Videos her
            IEnumerable<XElement> MediaFileName = from al in doc.Descendants("MediaItem") where (int)al.Attribute("id") == n+1 select al;
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


        private static int GetAmountOfMediaItems(int amountOfMediaItems, XDocument doc)
        {
            IEnumerable<XElement> MediaIDhoechsteZahl = from el in doc.Descendants("MediaItem") where (string)el.Attribute("id") != null select el;
            List<string> MediaElementHoechsteZahl = new List<string>();
            foreach (XElement el in MediaIDhoechsteZahl)
            {
                MediaElementHoechsteZahl.Add(el.FirstAttribute.Value);
                amountOfMediaItems = MediaIDhoechsteZahl.Count();

            }

            return amountOfMediaItems;
        }

        private void GetDurationOfMediaItems()
        {

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

            return letzteZahl+1;
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
                if (n+1 != amountOfMediaItems)
                {

                    XElement doc = new XElement(
                    new XElement("ExtentRef", new XAttribute("id", idIterator)));
                    docY.Descendants("ExtentSelector").Where(i => i.Attribute("extentID").Value == "4").Descendants("ExtentRefs").FirstOrDefault().AddFirst(doc);
                    //getFileNameFromMediaItem(n, docY, reihenFolgeVideo);
                    fileName = getFileNameFromMediaItem(n, docY, reihenFolgeVideo);
                }
             

                // Hier noch Prüfen ob das ein Instagram/Youtube etc Video ist
                string checkedFileName = CheckFileNameForSource(fileName);
                var docTitleClip = CreateTitleClipXElement(letzteZahl, checkedFileName, n);
                docY.Root.Descendants("Extents").FirstOrDefault().AddFirst(docTitleClip);
                

                docY.Save(currentDirectoryC);
                

                n++;
            }
        }

        private static string CheckFileNameForSource(string fileName)
        {
            Regex mr1 = new Regex(@"^[a-z._A-Z0-9]+\s-\s.{11}\.mp4$");

            if (mr1.IsMatch(fileName))
            {
                // INSTAGRAM PATH
                var index = fileName.IndexOf('-');
                return fileName.Substring(0, index).Trim();
            }
            {
                Console.WriteLine("was anderes");
                return "keinName";
            }


        }

        private static XElement CreateTitleClipXElement(int idIterator, string fileName, int n)
        {
            idIterator -= 1;
            var currentDirectory = Directory.GetCurrentDirectory() + @"\zumcoden.wlmp";
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
                    new XElement("BoundPropertyStringElement", new XAttribute("Value", "@"+fileName))),
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

        private static void MakeOneExtentRefWithOneNode(XDocument docX, string currentDirectoryC, int letzteZahl, int amountOfMediaItems)
        {
            XElement doc = new XElement(
            new XElement("ExtentRefs", new XElement("ExtentRef", new XAttribute("id", letzteZahl-1+amountOfMediaItems))));

            docX.Descendants("ExtentSelector").Where(i => i.Attribute("extentID").Value == "4").Descendants("BoundProperties").FirstOrDefault().AddAfterSelf(doc);
            docX.Save(currentDirectoryC);
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


        private static void CreateBlankProjekt(int lastExtentRefNumber, int amountOfMediaItems)
        {
            string projektDatei = "Slamdank1.wlmp";
            string projektName = "Slamdank1";
            string filepath = "";
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
                                new XElement(new XElement("MediaItems", new XElement("MediaItem", new XAttribute("id", 1), new XAttribute("filePath", filepath), new XAttribute("arWidth", width), new XAttribute("arHeight", height), new XAttribute("duration", duration), new XAttribute("songTitle", ""), new XAttribute("songArtist", ""), new XAttribute("songAlbum", ""), new XAttribute("songCopyrightUrl", ""), new XAttribute("songArtistUrl", ""), new XAttribute("songAudioFileUrl", ""), new XAttribute("stabilizationMode", 0), new XAttribute("mediaItemType", 1)))),
                                new XElement("Extents", ""))).Save("Slamdank1.wlmp");
            XDocument doc = XDocument.Load(newDirectiory);

            CreateProject(lastExtentRefNumber, newDirectiory, doc, amountOfMediaItems);

        }

        private static void CreateProject(int letzteZahl, string newDirectiory, XDocument doc, int amountOfMediaItems)
        {
            int mediaItemID = 1;

            // Creates Project / MediaItems 
            MakeOneMediaItemsWithOneNode(newDirectiory, mediaItemID, amountOfMediaItems);
            CreateMediaClipXElement();

            // Creates TitleClips in Extents
            string checkedFileName = "";
            InsertTitleClipAndSave(letzteZahl, newDirectiory, checkedFileName, amountOfMediaItems);


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


        }


        private static List<string> RetrieveVideos()
        {
            //Directory.GetCurrentDirectory() + @"\zumcoden.wlmp"
            string videoLocation = @"C:\Users\LjHa\Desktop\sorryjungs";
            var myDir = Directory.GetFiles(videoLocation);
            List<string> fileNameList = new List<string>();
            foreach (var fileName in myDir)
            {
                var fn = Path.GetFileName(fileName);
                var index = fn.IndexOf('-');
                fn = fn.Substring(0, index).Trim();
                fileNameList.Add(fn);
            }
            return fileNameList;
        }


        private static void MakeExtentRefForExtentRef2(string newDirectiory, int extent4ID, int amountOfMediaItems)
        {
            XDocument doc = XDocument.Load(newDirectiory);
            int n = 0;
            extent4ID = extent4ID + amountOfMediaItems;
            while (n < amountOfMediaItems)
            {
                extent4ID--;
            XElement docY = new XElement(
            new XElement("ExtentRef", new XAttribute("id", extent4ID)));
            doc.Descendants("ExtentSelector").Where(i => i.Attribute("extentID").Value == "4").Descendants("ExtentRefs").FirstOrDefault().AddFirst(docY);
            n++;
            doc.Save(newDirectiory);
            }

        }

        private static int MakeExtentRefForExtentRefs1(string newDirectiory, int letzteZahl, int amountOfMediaItems)
        {
            XDocument doc = XDocument.Load(newDirectiory);

            amountOfMediaItems = GetAmountOfMediaItems(0, doc);

            int n = 0;
            int idIterator = 5;
            int extentID = idIterator + amountOfMediaItems;
            int extent4Id = extentID;
            //string fileName = "";
            while (n < amountOfMediaItems)
            {
                extentID--;
                
                if (n != amountOfMediaItems)
                {
                    XElement docY = new XElement(
                    new XElement("ExtentRef", new XAttribute("id", extentID)));
                    doc.Descendants("ExtentSelector").Where(i => i.Attribute("extentID").Value == "1").Descendants("ExtentRefs").FirstOrDefault().AddFirst(docY);
                    //getFileNameFromMediaItem(n, docY, reihenFolgeVideo);
                }
                n++;
                doc.Save(newDirectiory);

            }
            return extent4Id;
        }

        private static void MakeAudioDuckingProperties(string newDirectiory)
        {
            XDocument doc = XDocument.Load(newDirectiory);

            var audioDuckingProperties = new XElement("AudioDuckingProperties", new XAttribute("emphasisPlaceholderID", "Narration"));
            doc.Root.Descendants("ThemeOperationLog").LastOrDefault().AddAfterSelf(audioDuckingProperties);

            doc.Save(newDirectiory);


        }

        private static void MakeThemeOperationLog(string newDirectiory)
        {
            XDocument doc = XDocument.Load(newDirectiory);

            var createdThemeOperationLog = new XElement(new XElement("ThemeOperationLog", new XAttribute("themeID", 0), new XElement("MonolithicThemeOperations")));

            doc.Root.Descendants("BoundProperties").LastOrDefault().AddAfterSelf(createdThemeOperationLog);

            doc.Save(newDirectiory);

        }

        private static void MakeBoundProperties(string newDirectiory)
        {
            XDocument doc = XDocument.Load(newDirectiory);

            var createdBoundProperties = new XElement(new XElement("BoundProperties", new XElement("BoundPropertyFloatSet", new XAttribute("Name", "AspectRatio"), new XElement("BoundPropertyFloatElement", new XAttribute("Value", "1.7777776718139648"))), new XElement("BoundPropertyFloat", new XAttribute("Name", "DuckedNarrationAndSoundTrackMix"), new XAttribute("Value", "0.5")),
                                                                                                                                                                                                                                                              new XElement("BoundPropertyFloat", new XAttribute("Name", "DuckedVideoAndNarrationMix"), new XAttribute("Value", "0")),
                                                                                                                                                                                                                                                              new XElement("BoundPropertyFloat", new XAttribute("Name", "DuckedVideoAndSoundTrackMix"), new XAttribute("Value", "0")),
                                                                                                                                                                                                                                                              new XElement("BoundPropertyFloat", new XAttribute("Name", "SoundTrackMix"), new XAttribute("Value", "0"))));


            doc.Root.Descendants("BoundPlaceholders").FirstOrDefault().AddAfterSelf(createdBoundProperties);

            doc.Save(newDirectiory);

        }

        private static void MakeBoundPlaceholders(string newDirectiory)
        {
            XDocument doc = XDocument.Load(newDirectiory);
            var createdBoundPlaceHolders = new XElement(new XElement("BoundPlaceholders", new XElement("BoundPlaceholder", new XAttribute("placeholderID", "SingleExtentView"), new XAttribute("extentID", 0)),
                                                           new XElement("BoundPlaceholder", new XAttribute("placeholderID", "Main"), new XAttribute("extentID", 1)),
                                                           new XElement("BoundPlaceholder", new XAttribute("placeholderID", "SoundTrack"), new XAttribute("extentID", 2)),
                                                           new XElement("BoundPlaceholder", new XAttribute("placeholderID", "Text"), new XAttribute("extentID", 4)),
                                                           new XElement("BoundPlaceholder", new XAttribute("placeholderID", "Narration"), new XAttribute("extentID", 3))));
            doc.Root.Descendants("Extents").FirstOrDefault().AddAfterSelf(createdBoundPlaceHolders);

            doc.Save(newDirectiory);
        }


        // ExtentSelector
        private static void MakeExtentSelectorForAutomation(string newDirectiory)
        {
            XDocument doc = XDocument.Load(newDirectiory);

            var createdExtentSelector1 = new XElement(new XElement("ExtentSelector", new XAttribute("extentID", 1), new XAttribute("gapBefore", 0), new XAttribute("primaryTrack", "true"), new XElement("Effects"), new XElement("Transitions"), new XElement("BoundProperties"), new XElement("ExtentRefs", "")));
            var createdExtentSelector2 = new XElement(new XElement("ExtentSelector", new XAttribute("extentID", 2), new XAttribute("gapBefore", 0), new XAttribute("primaryTrack", "false"), new XElement("Effects"), new XElement("Transitions"), new XElement("BoundProperties"), new XElement("ExtentRefs")));
            var createdExtentSelector3 = new XElement(new XElement("ExtentSelector", new XAttribute("extentID", 3), new XAttribute("gapBefore", 0), new XAttribute("primaryTrack", "false"), new XElement("Effects"), new XElement("Transitions"), new XElement("BoundProperties"), new XElement("ExtentRefs")));
            var createdExtentSelector4 = new XElement(new XElement("ExtentSelector", new XAttribute("extentID", 4), new XAttribute("gapBefore", 0), new XAttribute("primaryTrack", "false"), new XElement("Effects"), new XElement("Transitions"), new XElement("BoundProperties"), new XElement("ExtentRefs", "")));

            doc.Root.Descendants("Extents").FirstOrDefault().Add(createdExtentSelector1);
            doc.Root.Descendants("Extents").FirstOrDefault().Add(createdExtentSelector2);
            doc.Root.Descendants("Extents").FirstOrDefault().Add(createdExtentSelector3);
            doc.Root.Descendants("Extents").FirstOrDefault().Add(createdExtentSelector4);
            doc.Save(newDirectiory);

        }


        // VideoClip

        private static void InsertTitleClipAndSave(int lastExtentRefNumber, string newDirectiory, string checkedFileName, int amountOfMediaItems)
        {
            XDocument doc = XDocument.Load(newDirectiory);
            int n = 0;
            while (n < amountOfMediaItems)
            {

            var createdTitleClips = CreateTitleClipElement(lastExtentRefNumber, checkedFileName);
            doc.Root.Descendants("Extents").FirstOrDefault().AddFirst(createdTitleClips);
            doc.Save(newDirectiory);
                n++;
                lastExtentRefNumber++;
            }
        }


        private static void CreateMediaClipXElement()
        {
            int width = 1920;
            int height = 1080;
            double duration = 30.93;
            string filepath = "";
            new XElement(new XElement("MediaItem", new XAttribute("id", 1), new XAttribute("filePath", filepath), new XAttribute("arWidth", width), new XAttribute("arHeight", height), new XAttribute("duration", duration), new XAttribute("songTitle", ""), new XAttribute("songArtist", ""), new XAttribute("songAlbum", ""), new XAttribute("songCopyrightUrl", ""), new XAttribute("songArtistUrl", ""), new XAttribute("songAudioFileUrl", ""), new XAttribute("stabilizationMode", 0), new XAttribute("mediaItemType", 1)));
        
        }

        private static void MakeOneMediaItemsWithOneNode(string newDirectiory, int mediaItemID, int amountOfMediaItems)
        {
            int n = 0;
            int extentID = 5;
            XDocument docY = XDocument.Load(newDirectiory);
            bool ersterUmlauf = true;
            string filepath = "";
            int width = 1920;
            int height = 1080;
            double duration = 30.93;
            while (n < amountOfMediaItems)
            {
                mediaItemID++;
                XElement doc =
                new XElement(new XElement("MediaItem", new XAttribute("id", mediaItemID), new XAttribute("filePath", filepath), new XAttribute("arWidth", width), new XAttribute("arHeight", height), new XAttribute("duration", duration), new XAttribute("songTitle", ""), new XAttribute("songArtist", ""), new XAttribute("songAlbum", ""), new XAttribute("songCopyrightUrl", ""), new XAttribute("songArtistUrl", ""), new XAttribute("songAudioFileUrl", ""), new XAttribute("stabilizationMode", 0), new XAttribute("mediaItemType", 1)));

                //docX.Save(doc);
                docY.Descendants("MediaItems").FirstOrDefault().AddFirst(doc);
                docY.Save(newDirectiory);
                ersterUmlauf = true;
                MakeOneVideoClipItemWithOneNode(newDirectiory, mediaItemID, extentID, docY, ersterUmlauf);
                extentID++;
                n++;
            }
            if (n == amountOfMediaItems)
            {
                ersterUmlauf = false;
            MakeOneVideoClipItemWithOneNode(newDirectiory, mediaItemID, extentID, docY, ersterUmlauf);
            }
        }


        private static void MakeOneVideoClipItemWithOneNode(string newDirectiory, int mediaItemID, int extentID, XDocument docY, bool ersterUmlauf)
        {
            if (ersterUmlauf == true)
            { 
            mediaItemID -= 1;
            }
            else if (ersterUmlauf == false)
            {
            }
            //   XDocument doc = XDocument.Load(newDirectiory);

            var createdVideoClips = new XElement(new XElement("VideoClip", new XAttribute("extentID", extentID), new XAttribute("gapBefore", 0), new XAttribute("mediaItemID", mediaItemID), new XAttribute("inTime", 0), new XAttribute("speed", 1), new XAttribute("stabilizationMode", 0),
                new XElement("Effects"),
                new XElement("Transitions"),
                new XElement("BoundProperties", new XElement("BoundPropertyBool", new XAttribute("Name", "Mute"), new XAttribute("Value", "false")), new XElement("BoundPropertyInt", new XAttribute("Name", "rotateStepNinety"), new XAttribute("Value", "0")), new XElement("BoundPropertyFloat", new XAttribute("Name", "Volume"), new XAttribute("Value", "1")))));
            docY.Root.Descendants("Extents").FirstOrDefault().Add(createdVideoClips);
            docY.Save(newDirectiory);

        }



        // FALSCH? ? ? ? ? ? ? ?
        private static XElement CreateTitleClipElement(int idIterator, string fileName)
        {
            XElement titleDocX = new XElement(
                new XElement("TitleClip", new XAttribute("extentID", idIterator + 1), new XAttribute("gapBefore", 0), new XAttribute("duration", 0),
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

    }
}






///     B   A   C   K   U   P   ///

//private static void MakeExtentSelectorForText(XDocument docX, int letzteZahl, string currentDirectoryC)
//{
//    //var currentDirectory = Directory.GetCurrentDirectory() + @"\Extents4.wlmp";
//    //var currentDirectoryC = Directory.GetCurrentDirectory() + @"\zumcoden.wlmp";

//    //     XDocument docX = XDocument.Load(currentDirectoryC);

//    StringBuilder sb = new StringBuilder();

//    XElement doc = new XElement(
//        //              HIER WERTE VERÄNDERN
//        new XElement("ExtentSelector", new XAttribute("extentID", 4), new XAttribute("gapBefore", 0), new XAttribute("primaryTrack", "false"),
//            //new XElement("Effects"),
//            //new XElement("Transitions"),
//            //new XElement("BoundProperties"),
//            // WERT HIER ÄNDERN UND DIE ANDEREN EINFÜGEN?
//            new XElement("ExtentRefs", new XElement("ExtentRef", new XAttribute("id", 34)))));
//    //var nodes = docX.Elements().Where(x => x.Element("ExtentRefs"))
//    //var v = from n in docX.Descendants("ExtentSelector") where n.Element("ExtentRefs").Value == null select n;

//    docX.Descendants("ExtentSelector").Where(i => i.Attribute("extentID").Value == "4").Descendants("BoundProperties").FirstOrDefault().AddAfterSelf(doc);
//    //docX.Descendants("Extents").FirstOrDefault().AddAfterSelf(doc);
//    //  var hehe = from al in docX.Descendants("ExtentSelector") where (int)al.Attribute("extentID") == 4 select al;
//    docX.Save(currentDirectoryC);
//}






//    // var currentDirectory = Directory.GetCurrentDirectory() + @"\SDTEST.wlmp";
//    //var currentDirectory = Directory.GetCurrentDirectory() + @"\Extents.wlmp";
//    var currentDirectory = Directory.GetCurrentDirectory() + @"\zumcoden.wlmp";

//    //   XDocument docX = XDocument.Load(currentDirectory);

//    RemoveExtentRefsFromXML(docX);
//    //  docX.Elements("Extents").First().Add((titleDocX));
//    //docX.Element("Extents").Add(titleDocX);
//    //from al in doc.Descendants("VideoClip") where (string)al.Attribute("mediaItemID") == mediaID select al;
//    //docX.Add(titleDoc);
//    //doc.Add(titleDoc);

//    // INSERT TEXT IDS EXTENTREF
//    //  docX.Descendants("ExtentSelector").Where(i => i.Attribute("extentID").Value == "4").Descendants("BoundProperties").FirstOrDefault().AddAfterSelf(titleDocX);


//    var iesafj = docX.Root.Descendants("Extents");
//    iesafj.FirstOrDefault().AddFirst(titleDocX);



//    docX.Save(currentDirectory);
//}
