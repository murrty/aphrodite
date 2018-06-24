using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace aphrodite_min {
    class ImageDownloader {
        #region Variables
        public bool debugMode = false;          // Debug mode.
        public bool minMode = false;            // Determine if aphrodite_min.

        public string header;                   // String for the header.
        public string saveTo;                   // String for the save directory.
        public string staticSaveTo;             // String for the initial saveTo string.
        public string graylist;                 // String for the graylist.
        public string blacklist;                // String for the blacklist.
 
        public bool saveInfo;                   // Global setting for saving images.nfo file.
        public bool ignoreFinish;               // Global setting for exiting after finishing.

        public bool separateRatings;            // Setting for separating ratings.
        public bool separateBlacklisted;        // Setting for separating blacklisted files.


        public bool separateArtists = false;    // Setting to separate files by artist.
        public int fileNameCode;                // How the file names will be named. 
                                                // 0 = MD5
                                                // 1 = artist_MD5

        public static readonly string postJsonBase = "https://e621.net/post/show.json?id=";    // Json base url
        #endregion

        public bool downloadImage(string url) {
            if (minMode)
                Console.Clear();
            
            try {
            // Set the saveto to \\Images.
                if (!saveTo.EndsWith("\\Images"))
                    saveTo += "\\Images";

                staticSaveTo = saveTo;
                if (minMode)
                    writeToConsole("Set output to " + saveTo);

            // Check the start URL to add the extra forward-slash for the split.
                if (url.StartsWith("http://")) {
                    url.Replace("http://", "https://");
                }
                if (!url.StartsWith("https://"))
                    url = "https://" + url;

            // Get postID from the split.
                string postID = url.Split('/')[5];
                writeToConsole("Saving image ID " + postID, true);

            // New varaibles for the API parse.
                List<string> GraylistedTags = new List<string>(graylist.Split(' '));        // The list of graylisted tags, used for blacklist checking.
                List<string> BlacklistedTags = new List<string>(blacklist.Split(' '));      // The list of blacklisted tags, used for blacklist checking.
                string imageInfo = string.Empty;                                            // The images.nfo file buffer.
                string blacklistInfo = string.Empty;                                        // The images.blacklisted.nfo file buffer.
                XmlDocument xmlDoc = new XmlDocument();                                     // The XML document.
                writeToConsole("Configured variables");

            // Begin to get the XML
                writeToConsole("Starting JSON download...", true);
                url = postJsonBase + postID;
                string postXML = apiTools.getJSON(postJsonBase + postID, header);
                writeToConsole("JSON Downloaded.", true);

            // Check the XML.
                writeToConsole("Checking XML...");
                if (postXML == apiTools.emptyXML || string.IsNullOrWhiteSpace(postXML)) {
                    writeToConsole("XML is empty. Press any key to continue...");
                    waitInConsole();
                    return false;
                }
                writeToConsole("XML is valid.");

            // Begin parsing XML.
                xmlDoc.LoadXml(postXML);
                writeToConsole("Gathering post information from XML");
                XmlNodeList xmlMD5 = xmlDoc.DocumentElement.SelectNodes("/root/md5");
                XmlNodeList xmlURL = xmlDoc.DocumentElement.SelectNodes("/root/file_url");
                XmlNodeList xmlTags = xmlDoc.DocumentElement.SelectNodes("/root/tags");
                XmlNodeList xmlArtist = xmlDoc.DocumentElement.SelectNodes("/root/artist/item");
                XmlNodeList xmlScore = xmlDoc.DocumentElement.SelectNodes("/root/score");
                XmlNodeList xmlRating = xmlDoc.DocumentElement.SelectNodes("/root/rating");
                XmlNodeList xmlDescription = xmlDoc.DocumentElement.SelectNodes("/root/description");
                XmlNodeList xmlExt = xmlDoc.DocumentElement.SelectNodes("/root/file_ext");

            // Get the artists.
                string foundArtists = string.Empty;         // Used for listing all available artists on a post.
                for (int i = 0; i < xmlArtist.Count; i++) {
                    foundArtists += xmlArtist[i].InnerText + "\n";
                }
                foundArtists.TrimEnd('\n');
                writeToConsole("Artist(s) info gathered");

            // Get the rating.
                string rating = xmlRating[0].InnerText;
                switch (rating) {
                    case "e":
                        rating = "Explicit";
                        break;
                    case "q":
                        rating = "Questionable";
                        break;
                    case "s":
                        rating = "Safe";
                        break;
                }
                writeToConsole("Rating info set");

            // Read blacklists if it's blacklisted.
                writeToConsole("Starting blacklist filtering...");
                List<string> foundTags = xmlTags[0].InnerText.Split(' ').ToList();      // The list of all the tags on a post.
                bool isBlacklisted = false;
                string offendingTags = string.Empty;
                for (int i = 0; i < foundTags.Count; i++) { 
                    if (GraylistedTags.Count > 0) {
                        for (int j = 0; j < GraylistedTags.Count; j++) {
                            if (foundTags[i] == GraylistedTags[j]) {
                                offendingTags += foundTags[i];
                                isBlacklisted = true;
                                writeToConsole("This image is on the graylist.", true);
                            }
                        }
                    }
                    // Since the image downloader is user specific, the blacklist won't cancel downloading.
                    // Image downloading is basically the user consenting to downloading it, therefore they want it despite the blacklist.
                    // Worst-case, just delete it after downloading.
                    if (BlacklistedTags.Count > 0) {
                        for (int j = 0; j < BlacklistedTags.Count; j++) {
                            if (foundTags[i] == BlacklistedTags[j]) {
                                offendingTags += foundTags[i];
                                isBlacklisted = true;
                                writeToConsole("This image is on the blacklist.",true );
                            }
                        }
                    }
                }

                if (!isBlacklisted)
                    writeToConsole("This image is not graylisted nor blacklisted.", true);

            // set the .nfo buffer.
                if (isBlacklisted) {
                    if (separateBlacklisted) {
                        blacklistInfo += "BLACKLISTED POST " + postID + ":\n    MD5: " + xmlMD5[0].InnerText + "\n    URL: https://e621.net/post/show/" + postID + "\n    ARTIST(S): " + foundArtists + "\n    TAGS: " + xmlTags[0].InnerText + "\n    SCORE: " + xmlScore[0].InnerText + "\n    RATING: " + rating + "\n    OFFENDING TAGS: " + offendingTags + "\n    DESCRIPITON:\n\"    " + xmlDescription[0].InnerText + "\"\n\n";
                    }
                    else {
                        imageInfo += "POST " + postID + ":\n    MD5: " + xmlMD5[0].InnerText + "\n    URL: https://e621.net/post/show/" + postID + "\n    ARTIST(S): " + foundArtists + "\n    TAGS: " + xmlTags[0].InnerText + "\n    SCORE: " + xmlScore[0].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"    " + xmlDescription[0].InnerText + "\"\n\n";
                    }
                }
                else {
                    imageInfo += "POST " + postID + ":\n    MD5: " + xmlMD5[0].InnerText + "\n    URL: https://e621.net/post/show/" + postID + "\n    ARTIST(S): " + foundArtists + "\n    TAGS: " + xmlTags[0].InnerText + "\n    SCORE: " + xmlScore[0].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"    " + xmlDescription[0].InnerText + "\"\n\n";
                }

            // Trim the excess in the buffer.
                imageInfo = imageInfo.TrimEnd('\n');
                blacklistInfo = blacklistInfo.TrimEnd('\n');
                writeToConsole("Set the .nfo buffer");

            // Start working on the saveTo string.
                if (separateRatings) {
                    switch (xmlRating[0].InnerText) {
                        case "e":
                            saveTo += "\\explicit";
                            break;
                        case "q":
                            saveTo += "\\questionable";
                            break;
                        case "s":
                            saveTo += "\\safe";
                            break;
                    }
                    writeToConsole("Image has been separated to " + saveTo.Split('\\')[saveTo.Split('\\').Length - 1]);
                }

                if (isBlacklisted && separateBlacklisted)
                    saveTo += "\\blacklisted";
                if (separateArtists)
                    saveTo += "\\" + xmlArtist[0].InnerText;

            // Create output directory.
                if (!Directory.Exists(saveTo)) {
                    Directory.CreateDirectory(saveTo);
                    writeToConsole("Output directory did not exist, created it.");
                }

            // Work on the filename
                string fileName = string.Empty;     // The file name for the WebClient filename.
                switch (fileNameCode) {
                    case 0:
                        fileName = xmlMD5[0].InnerText + "." + xmlExt[0].InnerText;
                        break;
                    case 1:
                        fileName = xmlArtist[0].InnerText + "_" + xmlMD5[0].InnerText + "." + xmlExt[0].InnerText;
                        break;
                    default:
                        fileName = xmlArtist[0].InnerText + "_" + xmlMD5[0].InnerText + "." + xmlExt[0].InnerText;
                        break;
                }
                writeToConsole("Set the filename to " + fileName);

            // Check file before continuing.
                if (File.Exists(saveTo + "\\" + fileName)) {
                    writeToConsole("File already exists. Press any key to continue...", true);
                    waitInConsole();
                    return true;
                }

            // Save image.nfo.
                if (saveInfo) {
                    if (isBlacklisted && separateBlacklisted) {
                        if (!File.Exists(staticSaveTo + "\\images.blacklisted.nfo")) {
                            File.WriteAllText(staticSaveTo + "\\images.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                        }
                        else {
                            blacklistInfo = "\n\n" + blacklistInfo;

                            string readInfo = File.ReadAllText(staticSaveTo + "\\images.blacklisted.nfo");
                            if (!readInfo.Contains("MD5: " + xmlMD5[0].InnerText))
                                File.AppendAllText(staticSaveTo + "\\images.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                        }
                        writeToConsole("images.blacklisted.nfo has been wrote to.");
                    }
                    else {
                        if (!File.Exists(staticSaveTo + "\\images.nfo")) {
                            File.WriteAllText(staticSaveTo + "\\images.nfo", imageInfo, Encoding.UTF8);
                        }
                        else {
                            imageInfo = "\n\n" + imageInfo;

                            string readInfo = File.ReadAllText(staticSaveTo + "\\images.nfo");
                            if (!readInfo.Contains("MD5: " + xmlMD5[0].InnerText))
                                File.AppendAllText(staticSaveTo + "\\images.nfo", imageInfo, Encoding.UTF8);
                        }
                        writeToConsole("images.nfo has been wrote to.");
                    }
                }

            // Download file.
                url = xmlURL[0].InnerText;
                writeToConsole("Downloading url " + url + "\n", true);
                string outputBar = "";  // The progress bar on the webclient download.
                using (ExWebClient wc = new ExWebClient()) {
                    wc.DownloadProgressChanged += (s, e) => {
                        int prog = e.ProgressPercentage;
                        if (prog < 1)
                            outputBar = "|----------| ";
                        else if (prog > 10 && prog < 20)
                            outputBar = "|#---------| ";
                        else if (prog > 20 && prog < 30)
                            outputBar = "|##--------| ";
                        else if (prog > 30 && prog < 40)
                            outputBar = "|###-------| ";
                        else if (prog > 40 && prog < 50)
                            outputBar = "|####------| ";
                        else if (prog > 50 && prog < 60)
                            outputBar = "|#####-----| ";
                        else if (prog > 60 && prog < 70)
                            outputBar = "|######----| ";
                        else if (prog > 70 && prog < 80)
                            outputBar = "|#######---| ";
                        else if (prog > 80 && prog < 90)
                            outputBar = "|########--| ";
                        else if (prog > 90 && prog <= 100)
                            outputBar = "|#########-| ";
                        Console.Write("\r" + outputBar + prog + "% Complete of url " + url);
                    };
                    wc.DownloadFileCompleted += (s, e) => {
                        lock (e.UserState) {
                            Console.Write("\r|##########| Download completed.\n");
                            Monitor.Pulse(e.UserState);
                        }
                    };


                    wc.Proxy = WebRequest.GetSystemWebProxy();
                    wc.Headers.Add(header);
                    wc.Method = "GET";
                    var sync = new Object();
                    lock (sync) {
                        wc.DownloadFileAsync(new Uri(xmlURL[0].InnerText), saveTo + "\\" + fileName, sync);
                        Monitor.Wait(sync);
                    }
                    //wc.DownloadFile(xmlURL[0].InnerText, saveTo + "\\" + fileName);
                }

            // Finish the job.
                if (!ignoreFinish) {
                    writeToConsole("Image saved. Press any key to continue...", true);
                    waitInConsole();
                }
                return true;
            }
            catch (ThreadAbortException thrEx) {
                Debug.Print("Thread was requested to be, and has been, aborted.");
                Debug.Print("========== BEGIN THREADABORTEXCEPTION ==========");
                Debug.Print(thrEx.ToString());
                Debug.Print("========== END THREADABORTEXCEPTION ==========");
                return false;
                throw thrEx;
            }
            catch (WebException WebE) {
                Debug.Print("A WebException has occured.");
                Debug.Print("========== BEGIN WEBEXCEPTION ==========");
                Debug.Print(WebE.ToString());
                Debug.Print("========== END WEBEXCEPTION ==========");
                apiTools.webError(WebE, url);
                return false;
                throw WebE;
            }
            catch (Exception ex) {
                Debug.Print("A gneral exception has occured.");
                Debug.Print("========== BEGIN EXCEPTION ==========");
                Debug.Print(ex.ToString());
                Debug.Print("========== END EXCEPTION ==========");
                return false;
                throw ex;
            }
        }

        public void writeToConsole(string message, bool important = false) {
            if (minMode) {
                if (important) {
                    Console.WriteLine(message);
                    return;
                }
                else if (debugMode) {
                    Console.WriteLine(message);
                    return;
                }
                Debug.Print(message);
            }
                //Console.WriteLine(message);
        }
        public void waitInConsole() {
            Console.ReadLine();
        }
    }
}

// .. and the day goes on.