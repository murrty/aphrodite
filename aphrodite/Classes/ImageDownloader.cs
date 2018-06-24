using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace aphrodite {
    class ImageDownloader {

    #region Variables
        public string url;                      // The URL of the file to be downloaded.
        public string postID;                   // The ID of the post.

        public string header;                   // String for the header.
        public string saveTo;                   // String for the save directory.
        public string staticSaveTo;             // String for the initial saveTo string.
        public string graylist;                 // String for the graylist.
        public string blacklist;                // String for the blacklist.

        public bool separateRatings;            // Setting for separating ratings.
        public bool separateBlacklisted;        // Setting for separating blacklisted files.

        public bool saveInfo;                   // Global setting for saving images.nfo file.
        public bool ignoreFinish;               // Global setting for exiting after finishing.

        public bool separateArtists = false;    // Setting to separate files by artist.
        public int fileNameCode;                // How the file names will be named.
                                                // 0 = MD5
                                                // 1 = artist_MD5

        public static readonly string postJsonBase = "https://e621.net/post/show.json?id=";    // Json base url
    #endregion

    #region Downloader
        public bool downloadImage() {
            try {
            // Set the saveto to \\Images.
                if (!saveTo.EndsWith("\\Images"))
                    saveTo += "\\Images";

                staticSaveTo = saveTo;

            // Check the start URL to add the extra forward-slash for the split.
                if (url.StartsWith("http://")) {
                    url.Replace("http://", "https://");
                }
                if (!url.StartsWith("https://"))
                    url = "https://" + url;

            // Get postID from the split.
                postID = url.Split('/')[5];

            // New varaibles for the API parse.
                List<string> GraylistedTags = new List<string>(graylist.Split(' '));
                List<string> BlacklistedTags = new List<string>(blacklist.Split(' '));
                string imageInfo = string.Empty;
                string blacklistInfo = string.Empty;
                XmlDocument xmlDoc = new XmlDocument();

            // Begin to get the XML
                url = postJsonBase + postID;
                string postXML = apiTools.getJSON(postJsonBase + postID, header);

            // Check the XML.
                if (postXML == apiTools.emptyXML || string.IsNullOrWhiteSpace(postXML)) {
                    Debug.Print("Xml is empty, aborting.");
                    return false;
                }

            // Begin parsing XML.
                xmlDoc.LoadXml(postXML);
                Debug.Print("Gathering post information from XML");
                XmlNodeList xmlMD5 = xmlDoc.DocumentElement.SelectNodes("/root/md5");
                XmlNodeList xmlURL = xmlDoc.DocumentElement.SelectNodes("/root/file_url");
                XmlNodeList xmlTags = xmlDoc.DocumentElement.SelectNodes("/root/tags");
                XmlNodeList xmlArtist = xmlDoc.DocumentElement.SelectNodes("/root/artist/item");
                XmlNodeList xmlScore = xmlDoc.DocumentElement.SelectNodes("/root/score");
                XmlNodeList xmlRating = xmlDoc.DocumentElement.SelectNodes("/root/rating");
                XmlNodeList xmlDescription = xmlDoc.DocumentElement.SelectNodes("/root/description");
                XmlNodeList xmlExt = xmlDoc.DocumentElement.SelectNodes("/root/file_ext");

            // Get the artists.
                string foundArtists = string.Empty;
                for (int i = 0; i < xmlArtist.Count; i++) {
                    foundArtists += xmlArtist[i].InnerText + "\n";
                }
                foundArtists.TrimEnd('\n');

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

            // Read blacklists if it's blacklisted.
                List<string> foundTags = xmlTags[0].InnerText.Split(' ').ToList();
                bool isBlacklisted = false;
                string offendingTags = string.Empty;
                for (int i = 0; i < foundTags.Count; i++) {
                    if (GraylistedTags.Count > 0) {
                        for (int j = 0; j < GraylistedTags.Count; j++) {
                            if (foundTags[i] == GraylistedTags[j]) {
                                offendingTags += foundTags[i];
                                isBlacklisted = true;
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
                            }
                        }
                    }
                }

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
                }

                if (isBlacklisted && separateBlacklisted)
                    saveTo += "\\blacklisted";
                if (separateArtists)
                    saveTo += "\\" + xmlArtist[0].InnerText;

            // Create output directory.
                if (!Directory.Exists(saveTo))
                    Directory.CreateDirectory(saveTo);
                    

            // Work on the filename
                string fileName = string.Empty;
                switch (fileNameCode) {
                    case 0:
                        fileName = xmlMD5[0].InnerText + "." + xmlExt[0].InnerText;
                        break;
                    case 1:
                        fileName = xmlArtist[0].InnerText + "_" + xmlMD5[0].InnerText + "." + xmlExt[0].InnerText;
                        break;
                }

            // Check file before continuing.
                if (File.Exists(saveTo + "\\" + fileName))
                    return true;

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
                    }
                }

            // Download file.
                url = xmlURL[0].InnerText;
                using (ExWebClient wc = new ExWebClient()) {
                    wc.Proxy = WebRequest.GetSystemWebProxy();
                    wc.Method = "GET";
                    wc.Headers.Add(header);
                    Debug.Print("Beginning download of file " + xmlURL[0].InnerText);

                    wc.DownloadFile(xmlURL[0].InnerText, saveTo + "\\" + fileName);
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
    #endregion

    }
}