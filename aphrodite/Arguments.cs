using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace aphrodite {

#pragma warning disable CS0162 // Unreachable code detected
    /// <summary>
    /// The argument parser used to manipulate the program outside of the main form.
    /// </summary>
    internal class Arguments {

        #region Fields
        /// <summary>
        /// Whether the arguments have been parsed.
        /// </summary>
        public static bool ArgumentsParsed {
            get; private set;
        } = false;

        /// <summary>
        /// The <see cref="DownloadType"/> that was pushed, to either download or activate the tab on the requested form.
        /// </summary>
        public static ArgumentType ArgumentType {
            get; private set;
        } = ArgumentType.NoArguments;

        /// <summary>
        /// The string accompanied with the argument.
        /// An example is the tags to download, pool ID, image ID, or pool information to add to the pool wishlist.
        /// </summary>
        public static string ArgumentData {
            get; private set;
        } = null;

        /// <summary>
        /// The array accompanied with an argument that supports it.
        /// <para>Pool Wishlist array: 0 = Pool URL, 1 = Pool Name</para>
        /// <para>InkBunny array: 0 = Keywords, 1 = Artists' Gallery, 2 = Users' Favorites</para>
        /// </summary>
        public static string[] ArgumentDataArray {
            get; private set;
        } = null;
        #endregion

        #region Parser
        /// <summary>
        /// Parses the command line arguments.
        /// </summary>
        /// <returns>True if the arguments were fully parsed; otherwise, false.</returns>
        public static bool ParseArguments() => ParseArguments(Environment.GetCommandLineArgs());
        
        /// <summary>
        /// Decodes a URL encoded argument into an array.
        /// </summary>
        /// <param name="Argument"></param>
        /// <returns></returns>
        public static string[] DecodeUrlEncodedArgument(string Argument) {
            try {
                return Argument.StartsWith("aphrodite:") ?
                    Regex.Matches(HttpUtility.UrlDecode(Argument), @"\""(\""\""|[^\""])+\""|[^ ]+", RegexOptions.ExplicitCapture)
                        .Cast<Match>()
                        .Select(x => x.Value)
                        .Select(x => x.StartsWith("\"") && x.EndsWith("\"") ? x[1..^1].Replace("\"\"", "\"") : x)
                        .Select(x => x.StartsWith("\\\"") && x.EndsWith("\"") ? x[1..^1] : x)
                        .ToArray() : null;

                return Regex.Split(HttpUtility.UrlDecode(Argument), "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
            }
            catch (Exception ex) {
                Log.ReportException(ex);
                return new string[] { Argument };
            }
        }

        /// <summary>
        /// Parses an array of arguments.
        /// </summary>
        /// <param name="args">The array of arguments that should be parsed.</param>
        /// <returns>True if the arguments were fully parsed; otherwise, false.</returns>
        public static bool ParseArguments(string[] Arguments) {
            if (!ArgumentsParsed) {
                try {
                    if (Arguments.Length > 0) {
                        
                        if (Program.PrintArgsToLog) {
                            for (int i = 0; i < Arguments.Length; i++) {
                                Log.WriteNoDate($"ARG {i:000.##}: [{Arguments[i]}]");
                            }
                        }
                        
                        if (Arguments[0].StartsWith("aphrodite:")) {
                            // We'll have to assume it's encoded.
                            Arguments = DecodeUrlEncodedArgument(Arguments[0]) ?? Arguments;
                            Arguments[0] = Arguments[0][10..];
                        }

                        string NewArgumentData = Arguments.Length > 1 ? Arguments[1] : null;

                        switch (Arguments[0].ToLower()) {

                            #region Tags
                            case "t":
                            case "-t":
                            case "tag":
                            case "-tag":
                            case "tags":
                            case "-tags": {
                                switch (NewArgumentData) {
                                    case "settings":
                                    case "configuresettings": {
                                        ArgumentType = ArgumentType.ConfigureTagsSettings;
                                    } break;

                                    case "":
                                    case null:
                                    case "start": {
                                        Log.Write("tags argument was null, empty, or start, and will show the main form.");
                                        ArgumentType = ArgumentType.PushTags;
                                        ArgumentData = null;
                                    } break;

                                    default: {
                                        if (ApiTools.IsValidPageWithTagsLink(NewArgumentData)) {
                                            if (NewArgumentData.IndexOf("?tags=") > 0) {
                                                NewArgumentData = NewArgumentData[NewArgumentData.IndexOf("?tags=")..].Replace("?tags=", "").Split('&')[0];
                                            }
                                            else if (NewArgumentData.IndexOf("&tags=") > 0) {
                                                NewArgumentData = NewArgumentData[NewArgumentData.IndexOf("&tags=")..].Replace("&tags=", "").Split('&')[0];
                                            }
                                            else {
                                                throw new ArgumentParsingUrlException("Valid tags page, but couldn't find tags in the url.", NewArgumentData);
                                            }
                                        }

                                        if (Config.Settings.Initialization.AutoDownloadWithArguments) {
                                            using frmArgument ArgumentDialog = new(NewArgumentData, DownloadType.Tags);
                                            switch (ArgumentDialog.ShowDialog()) {
                                                case System.Windows.Forms.DialogResult.Yes: {
                                                    ArgumentType = ArgumentType.DownloadTags;
                                                    ArgumentData =
                                                        ArgumentDialog.AppendedArgument.Split(' ').Length > 6 ?
                                                            string.Join(" ", ArgumentDialog.AppendedArgument.Split(' '), 0, 6) :
                                                            ArgumentDialog.AppendedArgument;
                                                }
                                                break;

                                                case System.Windows.Forms.DialogResult.No: {
                                                    ArgumentType = ArgumentType.PushTags;
                                                    ArgumentData = NewArgumentData.Replace("+", " ").Trim();
                                                }
                                                break;

                                                case System.Windows.Forms.DialogResult.Cancel: {
                                                    ArgumentType = ArgumentType.CancelledArgumentsDownload;
                                                    ArgumentData = null;
                                                }
                                                break;
                                            }
                                        }
                                        else {
                                            Log.Write("tags: argument has tags, but the program is not configured to auto-download.");
                                            ArgumentType = ArgumentType.PushTags;
                                            ArgumentData = NewArgumentData;
                                        }
                                    } break;
                                }
                            } break;
                            #endregion

                            #region Page
                            case "pg":
                            case "-pg":
                            case "page":
                            case "-page": {
                                switch (NewArgumentData) {
                                    case "settings":
                                    case "configuresettings": {
                                        ArgumentType = ArgumentType.ConfigureTagsSettings;
                                    } break;

                                    case "":
                                    case null:
                                    case "start": {
                                        Log.Write("page argument was null, empty, or start, and will show the main form.");
                                        ArgumentType = ArgumentType.PushTags;
                                        ArgumentData = null;
                                    } break;

                                    default: {
                                        if (ApiTools.IsValidPageLink(NewArgumentData)) {
                                            if (Config.Settings.Initialization.AutoDownloadWithArguments) {
                                                using frmArgument ArgumentDialog = new(NewArgumentData, DownloadType.Page);
                                                switch (ArgumentDialog.ShowDialog()) {
                                                    case System.Windows.Forms.DialogResult.Yes: {
                                                        ArgumentType = ArgumentType.DownloadPage;
                                                        ArgumentData =
                                                            ArgumentDialog.AppendedArgument.Split(' ').Length > 6 ?
                                                                string.Join(" ", ArgumentDialog.AppendedArgument.Split(' '), 0, 6) :
                                                                ArgumentDialog.AppendedArgument;
                                                    }
                                                    break;

                                                    case System.Windows.Forms.DialogResult.No: {
                                                        ArgumentType = ArgumentType.PushTags;
                                                        ArgumentData =
                                                            (NewArgumentData.IndexOf("?tags=") > 0 ? 
                                                                NewArgumentData[NewArgumentData.IndexOf("?tags=")..].Replace("?tags=", "").Split('&')[0] :
                                                                NewArgumentData.IndexOf("&tags=") > 0 ?
                                                                    NewArgumentData[NewArgumentData.IndexOf("&tags=")..].Replace("&tags=", "").Split('&')[0] :
                                                            throw new ArgumentParsingUrlException("Valid tags page, but couldn't find the tags in the url.", NewArgumentData))
                                                                .Replace("+", " ").Trim();
                                                    }
                                                    break;

                                                    case System.Windows.Forms.DialogResult.Cancel: {
                                                        ArgumentType = ArgumentType.CancelledArgumentsDownload;
                                                        ArgumentData = null;
                                                    }
                                                    break;
                                                }
                                            }
                                            else {
                                                Log.Write("tags: argument has tags, but the program is not configured to auto-download.");
                                                ArgumentType = ArgumentType.PushTags;
                                                ArgumentData = null;
                                            }
                                        }
                                    } break;
                                }
                            } break;
                            #endregion

                            #region Pools
                            case "p":
                            case "-p":
                            case "pool":
                            case "-pool":
                            case "pools":
                            case "-pools": {
                                switch (NewArgumentData) {
                                    case "settings":
                                    case "configuresettings": {
                                        ArgumentType = ArgumentType.ConfigurePoolsSettings;
                                    } break;

                                    case "":
                                    case null:
                                    case "start": {
                                        Log.Write("-pool argument was null, empty, or start, and will show the main form.");
                                        ArgumentType = ArgumentType.PushPools;
                                        ArgumentData = null;
                                    } break;

                                    default: {
                                        NewArgumentData = NewArgumentData.Split('?')[0];
                                        if (ApiTools.IsValidPoolLink(NewArgumentData)) {
                                            NewArgumentData = NewArgumentData[(NewArgumentData.IndexOf("/pools/") + 7)..];
                                        }

                                        if (ApiTools.IsNumericOnly(NewArgumentData)) {
                                            if (Config.Settings.Initialization.AutoDownloadWithArguments) {
                                                using frmArgument ArgumentDialog = new(NewArgumentData, DownloadType.Pools);
                                                switch (ArgumentDialog.ShowDialog()) {
                                                    case System.Windows.Forms.DialogResult.Yes: {
                                                        ArgumentType = ArgumentType.DownloadPools;
                                                        ArgumentData = ArgumentDialog.AppendedArgument;
                                                    }
                                                    break;

                                                    case System.Windows.Forms.DialogResult.No: {
                                                        ArgumentType = ArgumentType.PushPools;
                                                        ArgumentData = NewArgumentData;
                                                    }
                                                    break;

                                                    case System.Windows.Forms.DialogResult.Cancel: {
                                                        ArgumentType = ArgumentType.CancelledArgumentsDownload;
                                                        ArgumentData = null;
                                                    }
                                                    break;
                                                }
                                            }
                                            else {
                                                Log.Write("tags argument has tags, but the program is not configured to auto-download.");
                                                ArgumentType = ArgumentType.PushPools;
                                                ArgumentData = NewArgumentData;
                                            }
                                        }
                                    } break;
                                }
                            } break;
                            #endregion

                            #region Image
                            case "i":
                            case "-i":
                            case "image":
                            case "-image":
                            case "images":
                            case "-images": {
                                switch (NewArgumentData) {
                                    case "settings":
                                    case "configuresettings": {
                                        ArgumentType = ArgumentType.ConfigureImagesSettings;
                                    } break;

                                    case "":
                                    case null:
                                    case "start": {
                                        Log.Write("-image argument was null, empty, or start, and will show the main form.");
                                        ArgumentType = ArgumentType.PushImages;
                                        ArgumentData = null;
                                    } break;

                                    default: {
                                        NewArgumentData = NewArgumentData.Split('?')[0];
                                        if (ApiTools.IsValidImageLink(NewArgumentData)) {
                                            NewArgumentData = NewArgumentData[(NewArgumentData.IndexOf("/posts/") + 7)..];
                                        }

                                        if (ApiTools.IsNumericOnly(NewArgumentData)) {
                                            if (Config.Settings.Initialization.AutoDownloadWithArguments) {
                                                using frmArgument ArgumentDialog = new(NewArgumentData, DownloadType.Images);
                                                switch (ArgumentDialog.ShowDialog()) {
                                                    case System.Windows.Forms.DialogResult.Yes: {
                                                        ArgumentType = ArgumentType.DownloadImages;
                                                        ArgumentData = ArgumentDialog.AppendedArgument;
                                                    }
                                                    break;

                                                    case System.Windows.Forms.DialogResult.No: {
                                                        ArgumentType = ArgumentType.PushImages;
                                                        ArgumentData = NewArgumentData;
                                                    }
                                                    break;

                                                    case System.Windows.Forms.DialogResult.Cancel: {
                                                        ArgumentType = ArgumentType.CancelledArgumentsDownload;
                                                        ArgumentData = null;
                                                    }
                                                    break;
                                                }
                                            }
                                            else {
                                                Log.Write("tags argument has tags, but the program is not configured to auto-download.");
                                                ArgumentType = ArgumentType.PushImages;
                                                ArgumentData = NewArgumentData;
                                            }
                                        }
                                    } break;
                                }
                            } break;
                            #endregion

                            #region Pool wishlist
                            case "wl":
                            case "-wl":
                            case "pwl":
                            case "-pwl":
                            case "poolwl":
                            case "-poolwl":
                            case "poolwishlist":
                            case "-poolwishlist": {
                                switch (Arguments.Length) {
                                    case > 2: {
                                        if (ApiTools.IsValidPoolLink(Arguments[1])) {
                                            ArgumentType = ArgumentType.AddToPoolWishlist;
                                            ArgumentDataArray = new string[] { Arguments[1], Arguments[2] ?? "" };
                                        }
                                    } break;

                                    case 2: {
                                        if (ApiTools.IsValidPoolLink(Arguments[1])) {
                                            ArgumentType = ArgumentType.AddToPoolWishlist;
                                            ArgumentDataArray = new string[] { Arguments[1], "" };
                                        }
                                    } break;

                                    default: {
                                        ArgumentType = ArgumentType.ShowPoolWishlist;
                                        ArgumentData = NewArgumentData;       
                                    } break;
                                }
                            } break;
                            #endregion

                            #region Redownloader
                            case "rd":
                            case "-rd":
                            case "redownload":
                            case "-redownload":
                            case "redownloader":
                            case "-redownloader": {
                                ArgumentType = ArgumentType.ShowRedownloader;
                            } break;
                            #endregion

                            #region Blacklist
                            case "b":
                            case "g":
                            case "-b":
                            case "-g":
                            case "graylist":
                            case "-graylist":
                            case "blacklist":
                            case "-blacklist": {
                                ArgumentType = ArgumentType.ShowBlacklist;
                            } break;
                            #endregion

                            #region Settings
                            case "c":
                            case "s":
                            case "-c":
                            case "-s":
                            case "config":
                            case "-config":
                            case "settings":
                            case "-settings":
                            case "configuration":
                            case "-configuration":
                            case "configuresettings": {
                                if (Arguments.Length > 1) {
                                    switch (Arguments[1].ToLower()) {
                                        case "t":
                                        case "-t":
                                        case "tag":
                                        case "-tag":
                                        case "tags":
                                        case "-tags": {
                                            ArgumentType = ArgumentType.ConfigureTagsSettings;
                                        } break;

                                        case "p":
                                        case "-p":
                                        case "pool":
                                        case "-pool":
                                        case "pools":
                                        case "-pools": {
                                            ArgumentType = ArgumentType.ConfigurePoolsSettings;
                                        } break;

                                        case "i":
                                        case "-i":
                                        case "image":
                                        case "-image":
                                        case "images":
                                        case "-images": {
                                            ArgumentType = ArgumentType.ConfigureImagesSettings;
                                        } break;

                                        case "m":
                                        case "-m":
                                        case "misc":
                                        case "-misc": {
                                            ArgumentType = ArgumentType.ConfigureMiscSettings;
                                        } break;

                                        case "pr":
                                        case "-pr":
                                        case "protocol":
                                        case "-protocol":
                                        case "protocols":
                                        case "-protocols": {
                                            ArgumentType = ArgumentType.ConfigureProtocolSettings;
                                        } break;

                                        case "s":
                                        case "-s":
                                        case "schema":
                                        case "-schema":
                                        case "schemas":
                                        case "-schemas": {
                                            ArgumentType = ArgumentType.ConfigureSchemaSettings;
                                        } break;

                                        case "import":
                                        case "export": {
                                            ArgumentType = ArgumentType.ConfigureImportExportSettings;
                                        } break;

                                        case "po":
                                        case "-po":
                                        case "ini":
                                        case "-ini":
                                        case "portable":
                                        case "-portable": {
                                            ArgumentType = ArgumentType.ConfigurePortableSettings;
                                        } break;

                                        default: {
                                            ArgumentType = ArgumentType.ConfigureSettings;
                                        } break;
                                    }
                                }
                                else {
                                    ArgumentType = ArgumentType.ConfigureSettings;
                                }
                            } break;
                            #endregion

                            #region Protocols
                            case "up":
                            case "-up":
                            case "updateprotocol":
                            case "-updateprotocol":
                            case "installprotocol":
                            case "-installprotocol": {
                                ArgumentType = ArgumentType.UpdateProtocol;
                            } break;

                            case "pr":
                            case "-pr":
                            case "protocol":
                            case "-protocol": {
                                ArgumentType = ArgumentType.ConfigureProtocolSettings;
                            }
                            break;
                            #endregion

                            #region Portable
                            case "portable":
                            case "-portable": {
                                ArgumentType = ArgumentType.ConfigurePortableSettings;
                            } break;
                            #endregion

                            #region Schema
                            case "schema":
                            case "-schema": {
                                ArgumentType = ArgumentType.ConfigureSchemaSettings;
                            } break;
                            #endregion

                            #region FurryBooru
                            case "fb":
                            case "-fb":
                            case "furrybooru":
                            case "-furrybooru": {
                                if (Arguments.Length > 1) {
                                    switch (Arguments[1].ToLower()) {
                                        case "configuresettings": {
                                            ArgumentType = ArgumentType.ConfigureSettings;
                                        } break;

                                        default: {
                                            if (ApiTools.IsValidPageWithTagsLink(NewArgumentData)) {
                                                if (NewArgumentData.IndexOf("?tags=") > 0) {
                                                    NewArgumentData = NewArgumentData[NewArgumentData.IndexOf("?tags=")..].Replace("?tags=", "").Split('&')[0];
                                                }
                                                else if (NewArgumentData.IndexOf("&tags=") > 0) {
                                                    NewArgumentData = NewArgumentData[NewArgumentData.IndexOf("&tags=")..].Replace("&tags=", "").Split('&')[0];
                                                }
                                                else {
                                                    throw new ArgumentParsingUrlException("Valid tags page, but couldn't find tags in the url.", NewArgumentData);
                                                }
                                            }

                                            if (Config.Settings.Initialization.AutoDownloadWithArguments) {
                                                using frmArgument ArgumentDialog = new(NewArgumentData, DownloadType.FurryBooru);
                                                switch (ArgumentDialog.ShowDialog()) {
                                                    case System.Windows.Forms.DialogResult.Yes: {
                                                        ArgumentType = ArgumentType.DownloadFurryBooru;
                                                        ArgumentData =
                                                            ArgumentDialog.AppendedArgument.Split(' ').Length > 6 ?
                                                                string.Join(" ", ArgumentDialog.AppendedArgument.Split(' '), 0, 6) :
                                                                ArgumentDialog.AppendedArgument;
                                                    }
                                                    break;

                                                    case System.Windows.Forms.DialogResult.No: {
                                                        ArgumentType = ArgumentType.PushFurryBooru;
                                                        ArgumentData = NewArgumentData.Replace("+", " ").Trim();
                                                    }
                                                    break;

                                                    case System.Windows.Forms.DialogResult.Cancel: {
                                                        ArgumentType = ArgumentType.CancelledArgumentsDownload;
                                                        ArgumentData = null;
                                                    }
                                                    break;
                                                }
                                            }
                                            else {
                                                Log.Write("tags: argument has tags, but the program is not configured to auto-download.");
                                                ArgumentType = ArgumentType.PushFurryBooru;
                                                ArgumentData = NewArgumentData;
                                            }

                                        } break;
                                    }
                                }
                                else {
                                    ArgumentType = ArgumentType.ShowFurryBooru;
                                }
                            } break;
                            #endregion

                            #region InkBunny
                            case "-ib":
                            case "-inkbunny": {
                                if (Arguments.Length > 1) {
                                    switch (Arguments[1].ToLower()) {
                                        case "configuresettings": {
                                            ArgumentType = ArgumentType.ConfigureSettings;
                                        } break;

                                        default: {
                                            int Index = Array.FindIndex(Arguments, X => X.ToLower() == "keywords" || X.ToLower() == "-keywords");
                                            string KeywordsTemp = Index > -1 && Arguments.Length >= Index + 1 ? Arguments[Index] : null;

                                            Index = Array.FindIndex(Arguments, X => X.ToLower() == "artist" || X.ToLower() == "-artist");
                                            string ArtistGalleryTemp = Index > -1 && Arguments.Length >= Index + 1 ? Arguments[Index] : null;

                                            Index = Array.FindIndex(Arguments, X => X.ToLower() == "userfav" || X.ToLower() == "-userfav");
                                            string UserFavsTemp = Index > -1 && Arguments.Length >= Index + 1 ? Arguments[Index] : null;

                                            if (KeywordsTemp == null && ArtistGalleryTemp == null && UserFavsTemp == null) {
                                                if (Arguments.Length > 2) {
                                                    if (Config.Settings.Initialization.AutoDownloadWithArguments) {
                                                        using frmArgument ArgumentDialog = new(NewArgumentData, DownloadType.InkBunny);
                                                        switch (ArgumentDialog.ShowDialog()) {
                                                            case System.Windows.Forms.DialogResult.Yes: {
                                                                ArgumentType = ArgumentType.DownloadInkBunny;
                                                                ArgumentData = NewArgumentData;
                                                            } break;

                                                            case System.Windows.Forms.DialogResult.No: {
                                                                ArgumentType = ArgumentType.PushInkBunny;
                                                                ArgumentData = NewArgumentData;
                                                            } break;

                                                            case System.Windows.Forms.DialogResult.Cancel: {
                                                                ArgumentType = ArgumentType.CancelledArgumentsDownload;
                                                                ArgumentData = null;
                                                            } break;
                                                        }
                                                    }
                                                    else {
                                                        Log.Write("tags argument has tags, but the program is not configured to auto-download.");
                                                        ArgumentType = ArgumentType.PushInkBunny;
                                                        ArgumentData = NewArgumentData;
                                                    }
                                                }
                                                else {
                                                    ArgumentType = ArgumentType.PushInkBunny;
                                                    ArgumentData = null;
                                                }
                                            }
                                            else {
                                                string[] InkBunnyArguments = new string[] { KeywordsTemp, ArtistGalleryTemp, UserFavsTemp };
                                                if (Config.Settings.Initialization.AutoDownloadWithArguments) {
                                                    using frmArgument ArgumentDialog = new(NewArgumentData, DownloadType.InkBunny);
                                                    switch (ArgumentDialog.ShowDialog()) {
                                                        case System.Windows.Forms.DialogResult.Yes: {
                                                            ArgumentType = ArgumentType.DownloadInkBunny;
                                                            ArgumentDataArray = ArgumentDialog.BaseArguments;
                                                        } break;

                                                        case System.Windows.Forms.DialogResult.No: {
                                                            ArgumentType = ArgumentType.PushInkBunny;
                                                            ArgumentDataArray = InkBunnyArguments;
                                                        } break;

                                                        case System.Windows.Forms.DialogResult.Cancel: {
                                                            ArgumentType = ArgumentType.CancelledArgumentsDownload;
                                                            ArgumentData = null;
                                                        } break;
                                                    }
                                                }
                                                else {
                                                    Log.Write("tags argument has tags, but the program is not configured to auto-download.");
                                                    ArgumentType = ArgumentType.PushInkBunny;
                                                    ArgumentDataArray = InkBunnyArguments;
                                                }
                                            }
                                        } break;
                                    }
                                }
                                else {
                                    ArgumentType = ArgumentType.ShowInkBunny;
                                }
                            } break;
                            #endregion

                            #region Imgur
                            case "-ig":
                            case "-imgur": {
                                if (Arguments.Length > 1) {
                                    switch (Arguments[1].ToLower()) {
                                        case "configuresettings": {
                                            ArgumentType = ArgumentType.ConfigureSettings;
                                        } break;

                                        default: {
                                            if (ApiTools.IsValidImgurLink(NewArgumentData)) {
                                                NewArgumentData = NewArgumentData[(NewArgumentData.IndexOf("/a/") + 3)..]
                                                    .Split('/')[0]
                                                    .Split('?')[0]
                                                    .Split('#')[0];
                                            }

                                            if (ApiTools.IsNumericOnly(NewArgumentData)) {
                                                if (Config.Settings.Initialization.AutoDownloadWithArguments) {
                                                    using frmArgument ArgumentDialog = new(NewArgumentData, DownloadType.Imgur);
                                                    switch (ArgumentDialog.ShowDialog()) {
                                                        case System.Windows.Forms.DialogResult.Yes: {
                                                            ArgumentType = ArgumentType.DownloadImgur;
                                                            ArgumentData = ArgumentDialog.AppendedArgument;
                                                        }
                                                        break;

                                                        case System.Windows.Forms.DialogResult.No: {
                                                            ArgumentType = ArgumentType.PushImgur;
                                                            ArgumentData = NewArgumentData;
                                                        }
                                                        break;

                                                        case System.Windows.Forms.DialogResult.Cancel: {
                                                            ArgumentType = ArgumentType.CancelledArgumentsDownload;
                                                            ArgumentData = null;
                                                        }
                                                        break;
                                                    }
                                                }
                                                else {
                                                    Log.Write("tags argument has tags, but the program is not configured to auto-download.");
                                                    ArgumentType = ArgumentType.PushImgur;
                                                    ArgumentData = NewArgumentData;
                                                }
                                            }
                                        } break;
                                    }
                                }
                                else {
                                    ArgumentType = ArgumentType.ShowImgur;
                                }
                            } break;
                            #endregion

                            default: {
                                Log.Write($"The argument {Arguments[0]} is not handled by the program.");
                            } break;

                        }

                    }

                    ArgumentsParsed = true;
                }
                catch (Exception ex) {
                    Log.ReportException(ex);
                    ArgumentsParsed = false;
                }
            }
            return ArgumentsParsed;
        }
        #endregion

    }
#pragma warning restore CS0162 // Unreachable code detected
}
