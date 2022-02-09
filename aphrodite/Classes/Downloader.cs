using System;
using System.Linq;
using System.Collections.Generic;

namespace aphrodite {

    /// <summary>
    /// Skeleton class that is used as a derivitive for all download info classes.
    /// </summary>
    public abstract class DownloadInfo {
        /// <summary>
        /// Whether the download info is valid.
        /// </summary>
        public bool Valid;

        /// <summary>
        /// The string array of graylisted tags.
        /// </summary>
        public string[] Graylist;
        /// <summary>
        /// The string array of blacklisted tags.
        /// </summary>
        public string[] Blacklist;
        /// <summary>
        /// The string array of undesired tags.
        /// </summary>
        public string[] UndesiredTags;

        /// <summary>
        /// The path to save the file(s) to.
        /// </summary>
        public string DownloadPath;
        /// <summary>
        /// Whether the download info is valid.
        /// </summary>
        public bool SaveInfo;
        /// <summary>
        /// Whether to automatically close the download when finished.
        /// </summary>
        public bool IgnoreFinish;
        /// <summary>
        /// Whether to open the download path after the download finishes.
        /// </summary>
        public bool OpenAfter;

        /// <summary>
        /// Whether Graylisted files should be downloaded.
        /// Does not get checked when downloading single files.
        /// </summary>
        public bool SaveGraylistedFiles;

        /// <summary>
        /// Whether Graylisted files should be downloaded.
        /// Does not get checked when downloading single files.
        /// Requires overriding, otherwise will be false.
        /// </summary>
        public bool SaveBlacklistedFiles;

        public DownloadInfo() {
            Graylist = Config.Settings.General.Graylist.Split(' ');
            Blacklist = Config.Settings.General.Blacklist.Split(' ');
            UndesiredTags = Config.Settings.General.undesiredTags.Split(' ');

            DownloadPath = String.IsNullOrWhiteSpace(Config.Settings.General.saveLocation) ?
                Environment.CurrentDirectory : Config.Settings.General.saveLocation;

            SaveInfo = Config.Settings.General.saveInfo;
            IgnoreFinish = Config.Settings.General.ignoreFinish;
            OpenAfter = Config.Settings.General.openAfter;

            SaveGraylistedFiles = Config.Settings.General.saveGraylisted;
            SaveBlacklistedFiles = false;
        }

    }

    /// <summary>
    /// Class for tag downloads, containing instance-related information for that current download.
    /// </summary>
    public class TagDownloadInfo : DownloadInfo {
        /// <summary>
        /// The <see cref="DownloadSite"/> to determine what site this info will be geared towards.
        /// </summary>
        public DownloadSite Site;
        /// <summary>
        /// Whether the download is from a URL. Used for page downloading.
        /// </summary>
        public bool FromUrl;
        /// <summary>
        /// The (mutable) tags that the download info is downloading.
        /// </summary>
        public string Tags;
        /// <summary>
        /// The (immutable) tags that the download info is downloading. Used for display.
        /// </summary>
        public readonly string BaseTags;
        /// <summary>
        /// The URL of the page being downloaded.
        /// </summary>
        public string PageUrl;
        /// <summary>
        /// The number of the page being downloaded.
        /// </summary>
        public string PageNumber;

        /// <summary>
        /// Whether to only download files with a score equal to, or greater than, the minimum score.
        /// </summary>
        public bool UseMinimumScore;
        /// <summary>
        /// Whether to use the minimum score as a tag.
        /// </summary>
        public bool MinimumScoreAsTag;
        /// <summary>
        /// The minimum score a file is allowed to have.
        /// </summary>
        public int MinimumScore;
        /// <summary>
        /// Whether to save explicit files.
        /// </summary>
        public bool SaveExplicit;
        /// <summary>
        /// Whether to save questionable files.
        /// </summary>
        public bool SaveQuestionable;
        /// <summary>
        /// Whether to save safe files.
        /// </summary>
        public bool SaveSafe;
        /// <summary>
        /// Whether to separate ratings into their own folder.
        /// </summary>
        public bool SeparateRatings;
        /// <summary>
        /// Whether to separate non-images into their own folder.
        /// </summary>
        public bool SeparateNonImages;
        /// <summary>
        /// Whether to skip existing files.
        /// </summary>
        public bool SkipExistingFiles;
        /// <summary>
        /// The amount of images allowed to be downloaded.
        /// </summary>
        public int ImageLimit;
        /// <summary>
        /// The amount of pages allowed to be parsed.
        /// </summary>
        public int PageLimit;
        /// <summary>
        /// Whether to download newest-to-oldest.
        /// </summary>
        public bool DownloadNewestToOldest;
        /// <summary>
        /// Whether the favorite count should be used as a tag.
        /// </summary>
        public bool FavoriteCountAsTag;
        /// <summary>
        /// The minimum favorite count a file is allowed to have.
        /// </summary>
        public int FavoriteCount;
        /// <summary>
        /// The file name schema of the files to be downloaded.
        /// </summary>
        public string FileNameSchema;
        /// <summary>
        /// The amount of posts displayed on the page.
        /// </summary>
        public string PageDisplayedImagesCount;

        /// <summary>
        /// Initializes a new <see cref="TagDownloadInfo"/> for downloading specified tags.
        /// </summary>
        /// <param name="InputTags"></param>
        public TagDownloadInfo(string InputTags, DownloadSite Site = DownloadSite.None) {
            if (string.IsNullOrWhiteSpace(InputTags) || Site == DownloadSite.None) {
                Valid = false;
                return;
            }

            this.Site = Site;

            BaseTags = InputTags
                .Replace("%25-2F", "/")
                .Replace("+", " ")
                .Replace("%2B", "+")
                .Replace("%3A", ":")
                .Replace("%3B", ";")
                .Replace("%3D", "=")
                .Replace("%24", "$")
                .Replace("%40", "@");

            Tags = InputTags
                .Replace("/", "%25-2F")
                .Replace(" ", "+")
                .Replace("+", "%2B")
                .Replace(":", "%3A")
                .Replace(";", "%3B")
                .Replace("=", "%3D")
                .Replace("$", "%24")
                .Replace("@", "%40");

            PageUrl = null;
            FromUrl = false;

            DownloadPath +=
                Site switch {
                    DownloadSite.FurryBooru => "\\FurryBooru",
                    _ => ""
                } + "\\Tags";

            PageDisplayedImagesCount = Site switch {
                DownloadSite.FurryBooru => "100",   // 100 is the max supported on furrybooru.
                DownloadSite.e621 => "320",         // 320 is the max supported on e621.
                _ => "50"                           // 50 is a generous guess at the maximum to one that isn't defined.
            };

            UseMinimumScore = Config.Settings.Tags.enableScoreMin;
            MinimumScoreAsTag = Config.Settings.Tags.scoreAsTag;
            MinimumScore = Config.Settings.Tags.scoreMin;
            ImageLimit = Config.Settings.Tags.imageLimit;
            PageLimit = Config.Settings.Tags.pageLimit;
            SeparateRatings = Config.Settings.Tags.separateRatings;
            SeparateNonImages = Config.Settings.Tags.separateNonImages;
            SaveExplicit = Config.Settings.Tags.Explicit;
            SaveQuestionable = Config.Settings.Tags.Questionable;
            SaveSafe = Config.Settings.Tags.Safe;
            FileNameSchema = ApiTools.ReplaceIllegalCharacters(Config.Settings.Tags.fileNameSchema.ToLower());
            SaveBlacklistedFiles = Config.Settings.Tags.downloadBlacklisted;
            DownloadNewestToOldest = Config.Settings.Tags.DownloadNewestToOldest;
            FavoriteCountAsTag = Config.Settings.Tags.FavoriteCountAsTag;
            FavoriteCount = Config.Settings.Tags.FavoriteCount;
        }

        /// <summary>
        /// Initializes a new <see cref="TagDownloadInfo"/> for downloading a page.
        /// </summary>
        /// <param name="InputPage"></param>
        /// <param name="IsPage"></param>
        public TagDownloadInfo(Uri InputPage, DownloadSite Site = DownloadSite.None) {
            this.Site = Site;
            PageUrl = InputPage.AbsoluteUri;
            FromUrl = true;

            if (string.IsNullOrWhiteSpace(PageUrl) || !ApiTools.IsValidPageLink(PageUrl) || Site != DownloadSite.e621 && Site != DownloadSite.FurryBooru) {
                Valid = false;
                return;
            }

            switch (Site) {
                case DownloadSite.e621: {
                    Tags =
                        PageUrl.IndexOf("?tags=") > -1 ?
                        PageUrl.Split('?')[1].Split('&')[0].Substring(6) :
                        PageUrl.IndexOf("&tags=") > -1 ?
                        PageUrl.Substring(PageUrl.IndexOf("&tags=") + 6).Split('&')[0] :
                        "[no tags]";

                    PageNumber =
                        PageUrl.IndexOf("?page=") > -1 ?
                        PageUrl.Split('?')[1].Split('&')[0].Substring(6) :
                        PageUrl.IndexOf("&page=") > -1 ?
                        PageUrl.Substring(PageUrl.IndexOf("&page=") + 6).Split('&')[0] :
                        "1";

                    PageDisplayedImagesCount =
                        PageUrl.IndexOf("?limit=") > -1 ?
                        PageUrl.Split('?')[1].Split('&')[0].Substring(7) :
                        PageUrl.IndexOf("&limit=") > -1 ?
                        PageUrl.Substring(PageUrl.IndexOf("&limit=") + 7).Split('&')[0] :
                        "75"; // 75 is the default.
                } break;

                case DownloadSite.FurryBooru: {
                    Tags =
                        PageUrl.IndexOf("?tags=") > -1 ?
                        PageUrl.Split('?')[1].Split('&')[0].Substring(6) :
                        PageUrl.IndexOf("&tags=") > -1 ?
                        PageUrl.Substring(PageUrl.IndexOf("&tags=") + 6).Split('&')[0] :
                        "all";

                    PageNumber =
                        PageUrl.IndexOf("?pid=") > -1 ?
                        PageUrl.Split('?')[1].Split('&')[0].Substring(5) :
                        PageUrl.IndexOf("&pid=") > -1 ?
                        PageUrl.Substring(PageUrl.IndexOf("&pid=") + 5).Split('&')[0] :
                        "0";

                    PageDisplayedImagesCount =
                        PageUrl.IndexOf("?limit=") > -1 ?
                        PageUrl.Split('?')[1].Split('&')[0].Substring(7) :
                        PageUrl.IndexOf("&limit=") > -1 ?
                        PageUrl.Substring(PageUrl.IndexOf("&limit=") + 7).Split('&')[0] :
                        "40"; // 40 is the default.
                } break;
            }

            BaseTags = Site == DownloadSite.FurryBooru && Tags.ToLower() == "all" ? "[no tags]" :
                Tags.Replace("%2B", "+")
                    .Replace("+", " ")
                    .Replace("%25-2F", "/")
                    .Replace("%3A", ":")
                    .Replace("%3B", ";")
                    .Replace("%3D", "=")
                    .Replace("%24", "$")
                    .Replace("%40", "@");

            Tags = Tags
                .Replace("+", "%2B")
                .Replace(" ", "+")
                .Replace("/", "%25-2F")
                .Replace(":", "%3A")
                .Replace(";", "%3B")
                .Replace("=", "%3D")
                .Replace("$", "%24")
                .Replace("@", "%40");

            DownloadPath += $"{(Site == DownloadSite.FurryBooru ? "\\FurryBooru" : "")}\\Page";

            UseMinimumScore = Config.Settings.Tags.enableScoreMin;
            MinimumScoreAsTag = Config.Settings.Tags.scoreAsTag;
            MinimumScore = Config.Settings.Tags.scoreMin;
            ImageLimit = Config.Settings.Tags.imageLimit;
            PageLimit = Config.Settings.Tags.pageLimit;
            SeparateRatings = Config.Settings.Tags.separateRatings;
            SeparateNonImages = Config.Settings.Tags.separateNonImages;
            SaveExplicit = Config.Settings.Tags.Explicit;
            SaveQuestionable = Config.Settings.Tags.Questionable;
            SaveSafe = Config.Settings.Tags.Safe;
            FileNameSchema = ApiTools.ReplaceIllegalCharacters(Config.Settings.Tags.fileNameSchema.ToLower());
            SaveBlacklistedFiles = Config.Settings.Tags.downloadBlacklisted;
            DownloadNewestToOldest = Config.Settings.Tags.DownloadNewestToOldest;
            FavoriteCountAsTag = Config.Settings.Tags.FavoriteCountAsTag;
            FavoriteCount = Config.Settings.Tags.FavoriteCount;
        }

    }

    /// <summary>
    /// Class for pool downloads, containing instance-related information for that current download.
    /// </summary>
    public class PoolDownloadInfo : DownloadInfo {

        /// <summary>
        /// The ID of the pool that will be downloaded.
        /// </summary>
        public string PoolId;

        /// <summary>
        /// Whether graylisted pages should be merged into the main folder.
        /// </summary>
        public bool MergeGraylistedPages;
        /// <summary>
        /// Whether blacklisted pages should be merged into the main folder.
        /// </summary>
        public bool MergeBlacklistedPages;
        /// <summary>
        /// The file name schema of the files to be downloaded.
        /// </summary>
        public string FileNameSchema;

        /// <summary>
        /// Initializes a new <see cref="PoolDownloadInfo"/> for downloading a pool.
        /// </summary>
        /// <param name="RequestedPool">The ID or URL of the pool to download.</param>
        public PoolDownloadInfo(string RequestedPool) {
            if (string.IsNullOrWhiteSpace(RequestedPool)) {
                Valid = false;
                return;
            }
            if (ApiTools.IsValidPoolLink(RequestedPool)) {
                PoolId = ApiTools.GetPoolOrPostId(RequestedPool);
            }
            else if (ApiTools.IsNumericOnly(RequestedPool)) {
                PoolId = RequestedPool;
            }
            else {
                Valid = false;
                return;
            }

            PoolId = RequestedPool;

            DownloadPath += "\\Pools";

            SaveBlacklistedFiles = Config.Settings.Pools.downloadBlacklisted;
            MergeGraylistedPages = Config.Settings.Pools.mergeGraylisted;
            MergeBlacklistedPages = Config.Settings.Pools.mergeBlacklisted;
            FileNameSchema = ApiTools.ReplaceIllegalCharacters(Config.Settings.Pools.fileNameSchema);
        }
    }

    /// <summary>
    /// Class for image downloads, containing instance-related information for that current download.
    /// </summary>
    public class ImageDownloadInfo : DownloadInfo {
        /// <summary>
        /// Whether the image downloader should use the form.
        /// If false, the download will proceed in the background.
        /// </summary>
        public bool UseForm;
        /// <summary>
        /// The URL of the image.
        /// </summary>
        public string ImageUrl;
        /// <summary>
        /// The ID of the post.
        /// </summary>
        public string PostId;
        /// <summary>
        /// The name of the file.
        /// </summary>
        public string FileName;

        /// <summary>
        /// Whether to separate ratings into their own folder.
        /// </summary>
        public bool SeparateRatings;
        /// <summary>
        /// Whether to separate graylisted files into their own folder.
        /// </summary>
        public bool SeparateGraylisted;
        /// <summary>
        /// Whether to separate non-image files into their own folder.
        /// </summary>
        public bool SeparateNonImages;
        /// <summary>
        /// Whether to separate files into a folder based on the artist tags.
        /// </summary>
        public bool SeparateArtists;
        /// <summary>
        /// Whether the separate blacklisted files into their own folder.
        /// </summary>
        public bool SeparateBlacklisted;

        /// <summary>
        /// The file name schema of the files to be downloaded.
        /// </summary>
        public string FileNameSchema;

        /// <summary>
        /// Initializes a new <see cref="ImageDownloadInfo"/> for downloading single images.
        /// </summary>
        /// <param name="Image">The ID or URL of the image to download.</param>
        public ImageDownloadInfo(string Image) {
            if (string.IsNullOrWhiteSpace(Image)) {
                Valid = false;
                return;
            }

            if (ApiTools.IsValidImageLink(Image)) {
                ImageUrl = Image;
                PostId = ApiTools.GetPoolOrPostId(Image); //Image.Substring(Image.IndexOf("e621.net")).Split('/')[2].Split('?')[0];
            }
            else if (ApiTools.IsNumericOnly(Image)){
                PostId = Image;
            }
            else {
                Valid = false;
                return;
            }

            DownloadPath += "\\Images";

            FileNameSchema = ApiTools.ReplaceIllegalCharacters(Config.Settings.Images.fileNameSchema);
            SeparateRatings = Config.Settings.Images.separateRatings;
            SeparateGraylisted = Config.Settings.Images.separateGraylisted;
            SeparateBlacklisted = Config.Settings.Images.separateBlacklisted;
            SeparateNonImages = Config.Settings.Images.separateNonImages;
            SeparateArtists = Config.Settings.Images.separateArtists;

            UseForm = Config.Settings.Images.useForm;
        }

    }

    /// <summary>
    /// Class for inkbunny downloads, containing instance-related information for that current download.
    /// </summary>
    public class InkBunnyDownloadInfo : DownloadInfo {
        /// <summary>
        /// The (mutable) keywords that will be used to search.
        /// </summary>
        public string SearchKeywords;
        /// <summary>
        /// The (immutable) keywords that will be used to search. Used for display.
        /// </summary>
        public readonly string BaseSearchKeywords;
        /// <summary>
        /// The (mutable) artists' gallery that will be used to search.
        /// </summary>
        public string SearchArtistGallery;
        /// <summary>
        /// The (immutable) artists' gallery that will be used to search. Used for display.
        /// </summary>
        public readonly string BaseSearchArtistsGallery;
        /// <summary>
        /// The (mutable) users' favorites that will be used to search.
        /// </summary>
        public string SearchUsersFavorites;
        /// <summary>
        /// The (immutable) artists' gallery that will be used to search. Used for display.
        /// </summary>
        public readonly string BaseSearchUsersFavorites;
        /// <summary>
        /// The identifier that will be used to determine if it's already being downloaded.
        /// </summary>
        public readonly string Identifier;

        /// <summary>
        /// The session ID used to connect to InkBunny.
        /// </summary>
        public string SessionID;
        /// <summary>
        /// The file name schema of the files to be downloaded.
        /// </summary>
        public string FileNameSchema;
        /// <summary>
        /// The file name schema of the files to be downloaded that contain multiple images.
        /// </summary>
        public string FileNameSchemaMultiPost;

        /// <summary>
        /// Whether General-rated posts will be downloaded.
        /// </summary>
        public bool General;
        /// <summary>
        /// Whether Mature (Nudity)-rated posts will be downloaded.
        /// </summary>
        public bool MatureNudity;
        /// <summary>
        /// Whether Mature (Violence)-rated posts will be downloaded.
        /// </summary>
        public bool MatureViolence;
        /// <summary>
        /// Whether Adult (Sexual Themes)-rated posts will be downloaded.
        /// </summary>
        public bool AdultSexualThemes;
        /// <summary>
        /// Whether Adult (Strong Violence)-rated posts will be downloaded.
        /// </summary>
        public bool AdultStrongViolence;

        /// <summary>
        /// Whether both mature-rated submissions are false.
        /// </summary>
        public bool SkipMature;
        /// <summary>
        /// Whether both adult-rated submissions are false.
        /// </summary>
        public bool SkipAdult;

        /// <summary>
        /// Whether the api should search in keywords.
        /// </summary>
        public bool SearchInKeywords;
        /// <summary>
        /// Whether the api should search in the submission title.
        /// </summary>
        public bool SearchInTitle;
        /// <summary>
        /// Whether the api should search in the description or story.
        /// </summary>
        public bool SearchInDescriptionOrStory;
        /// <summary>
        /// Whether the api should search in the md5 hash (requires the keywords to be occupied with the hash).
        /// </summary>
        public bool SearchInMd5Hash;

        /// <summary>
        /// Whether Picture/Pinup-type posts will be downloaded.
        /// </summary>
        public bool PicturePinup;
        /// <summary>
        /// Whether Sketch-type posts will be downloaded.
        /// </summary>
        public bool Sketch;
        /// <summary>
        /// Whether Picture series-type posts will be downloaded.
        /// </summary>
        public bool PictureSeries;
        /// <summary>
        /// Whether Comic-type posts will be downloaded.
        /// </summary>
        public bool Comic;
        /// <summary>
        /// Whether Portfolio-type posts will be downloaded.
        /// </summary>
        public bool Portfolio;
        /// <summary>
        /// Whether Flash(Animation)-type posts will be downloaded.
        /// </summary>
        public bool FlashAnimation;
        /// <summary>
        /// Whether Flash (Interactive)-type posts will be downloaded.
        /// </summary>
        public bool FlashInteractive;
        /// <summary>
        /// Whether Video (Animation)-type posts will be downloaded.
        /// </summary>
        public bool VideoAnimation;
        /// <summary>
        /// Whether Music (Single Track)-type posts will be downloaded.
        /// </summary>
        public bool MusicSingleTrack;
        /// <summary>
        /// Whether Music (Album)-type posts will be downloaded.
        /// </summary>
        public bool MusicAlbum;
        /// <summary>
        /// Whether Video (Feature Length)-type posts will be downloaded.
        /// </summary>
        public bool VideoFeatureLength;
        /// <summary>
        /// Whether Writing (Document)-type posts will be downloaded.
        /// </summary>
        public bool WritingDocument;
        /// <summary>
        /// Whether Character Sheet-type posts will be downloaded.
        /// </summary>
        public bool CharacterSheet;
        /// <summary>
        /// Whether Photography-type posts will be downloaded.
        /// </summary>
        public bool Photography;

        /// <summary>
        /// Whether to separate non-images into their own folder.
        /// </summary>
        public bool SeparateNonImages;
        /// <summary>
        /// Whether multiple image posts will be spearated into their own folder.
        /// </summary>
        public bool SeparateMultiPosts;
        /// <summary>
        /// The amount of images allowed to be downloaded.
        /// </summary>
        public int ImageLimit;
        /// <summary>
        /// The amount of pages allowed to be parsed.
        /// </summary>
        public int PageLimit;

        /// <summary>
        /// Initializes a new <see cref="InkBunnyDownloadInfo"/> for downloading files off InkBunny.
        /// </summary>
        /// <param name="Keywords">The Keywords (or MD5 hash) to search InkBunny with.</param>
        /// <param name="ArtistGallery">The Artist Username (or ID?) to search InkBunny with.</param>
        /// <param name="UsersFavorites">The User ID to search InkBunny with.</param>
        public InkBunnyDownloadInfo(string Keywords, string ArtistGallery, string UsersFavorites) {
            if (string.IsNullOrWhiteSpace(Keywords) && string.IsNullOrWhiteSpace(ArtistGallery) && string.IsNullOrWhiteSpace(UsersFavorites)) {
                this.Valid = false;
                return;
            }
            
            if (Config.Settings.InkBunny.GuestPriority && !string.IsNullOrWhiteSpace(Config.Settings.InkBunny.GuestSessionID)) {
                this.SessionID = Config.Settings.InkBunny.GuestSessionID;
                this.General = Config.Settings.InkBunny.GuestGeneral;
                this.MatureNudity = Config.Settings.InkBunny.GuestMatureNudity;
                this.MatureViolence = Config.Settings.InkBunny.GuestMatureViolence;
                this.AdultSexualThemes = Config.Settings.InkBunny.GuestAdultSexualThemes;
                this.AdultStrongViolence = Config.Settings.InkBunny.GuestAdultStrongViolence;
            }
            else if (!string.IsNullOrWhiteSpace(Config.Settings.InkBunny.SessionID)) {
                this.SessionID = Config.Settings.InkBunny.SessionID;
                this.General = Config.Settings.InkBunny.General;
                this.MatureNudity = Config.Settings.InkBunny.MatureNudity;
                this.MatureViolence = Config.Settings.InkBunny.MatureViolence;
                this.AdultSexualThemes = Config.Settings.InkBunny.AdultSexualThemes;
                this.AdultStrongViolence = Config.Settings.InkBunny.AdultStrongViolence;
            }
            else {
                this.Valid = false;
                return;
            }

            this.DownloadPath += "\\InkBunny";

            this.SearchKeywords = Keywords;
            this.BaseSearchKeywords = Keywords;
            this.SearchArtistGallery = ArtistGallery;
            this.BaseSearchArtistsGallery = ArtistGallery;
            this.SearchUsersFavorites = UsersFavorites;
            this.BaseSearchUsersFavorites = UsersFavorites;
            this.Identifier = (BaseSearchKeywords + "&&" + BaseSearchArtistsGallery + "&&" + BaseSearchUsersFavorites).Trim('&');

            this.FileNameSchema = Config.Settings.InkBunny.FileNameSchema;
            this.FileNameSchemaMultiPost = Config.Settings.InkBunny.FileNameSchemaMultiPost;

            this.SaveGraylistedFiles = Config.Settings.InkBunny.DownloadGraylistedSubmissions;
            this.SaveBlacklistedFiles = Config.Settings.InkBunny.DownloadBlacklistedSubmissions;

            this.SearchInKeywords = Config.Settings.InkBunny.SearchInKeywords;
            this.SearchInTitle = Config.Settings.InkBunny.SearchInTitle;
            this.SearchInDescriptionOrStory = Config.Settings.InkBunny.SearchInDescriptionOrStory;
            this.SearchInMd5Hash = Config.Settings.InkBunny.SearchInMd5Hash;

            this.PicturePinup = Config.Settings.InkBunny.PicturePinup;
            this.Sketch = Config.Settings.InkBunny.Sketch;
            this.PictureSeries = Config.Settings.InkBunny.PictureSeries;
            this.Comic = Config.Settings.InkBunny.Comic;
            this.Portfolio = Config.Settings.InkBunny.Portfolio;
            this.FlashAnimation = Config.Settings.InkBunny.FlashAnimation;
            this.FlashInteractive = Config.Settings.InkBunny.FlashInteractive;
            this.VideoAnimation = Config.Settings.InkBunny.VideoAnimation;
            this.MusicSingleTrack = Config.Settings.InkBunny.MusicSingleTrack;
            this.MusicAlbum = Config.Settings.InkBunny.MusicAlbum;
            this.VideoFeatureLength = Config.Settings.InkBunny.VideoFeatureLength;
            this.WritingDocument = Config.Settings.InkBunny.WritingDocument;
            this.CharacterSheet = Config.Settings.InkBunny.CharacterSheet;
            this.Photography = Config.Settings.InkBunny.Photography;
            this.SeparateMultiPosts = Config.Settings.InkBunny.SeparateMultiPosts;
            this.SeparateNonImages = Config.Settings.InkBunny.SeparateNonImages;
            this.ImageLimit = Config.Settings.InkBunny.ImageLimit;
            this.PageLimit = Config.Settings.InkBunny.PageLimit;
        }

    }

    /// <summary>
    /// Class for imgur downloads, containing instance-related information for that current download.
    /// </summary>
    public class ImgurDownloadInfo : DownloadInfo {
        /// <summary>
        /// The album id that will be downloaded.
        /// </summary>
        public string Album;

        /// <summary>
        /// The client ID used to connect to InkBunny.
        /// </summary>
        public string ClientID;

        /// <summary>
        /// The file name schema of the files to be downloaded.
        /// </summary>
        public string FileNameSchema;
        /// <summary>
        /// Whether to separate non-images into their own folder.
        /// </summary>
        public bool SeparateNonImages;
        /// <summary>
        /// The amount of images allowed to be downloaded.
        /// </summary>
        public int ImageLimit;

        /// <summary>
        /// Initializes a new <see cref="ImgurDownloadInfo"/> for downloading albums from Imgur.
        /// </summary>
        /// <param name="Album">The album ID or URL to download.</param>
        public ImgurDownloadInfo(string Album) {
            if (string.IsNullOrWhiteSpace(Album)) {
                Valid = false;
                return;
            }

            this.Album = Album;

            DownloadPath += "\\Imgur";

            ClientID = Config.Settings.Imgur.ClientID;
            FileNameSchema = !string.IsNullOrWhiteSpace(Config.Settings.Imgur.FileNameSchema) ? Config.Settings.Imgur.FileNameSchema : "%imageindex%_%imageid%";
            SeparateNonImages = Config.Settings.Imgur.SeparateNonImages;
            ImageLimit = Config.Settings.Imgur.ImageLimit;
        }
    }

    /// <summary>
    /// Class for non-image file types.
    /// </summary>
    public class NonImageFileTypeInfo {
        public bool Webm = false;
        public bool Gif = false;
        public bool Apng = false;
        public bool Swf = false;
    }

    /// <summary>
    /// Reference-only inkbunny known non-image types.
    /// </summary>
    public class NonImageFileTypeInkBunnyInfo {
        // 0 = gif, 1 = swf, 2 = flv, 3 = mp4, 4 = mp3, 5 = doc, 6 = rtf, 7 = txt
        public bool Gif = false;
        public bool Swf = false;
        public bool Flv = false;
        public bool Mp4 = false;
        public bool Mp3 = false;
        public bool Doc = false;
        public bool Rtf = false;
        public bool Txt = false;
    }

    /// <summary>
    /// Non-overriding based downloading
    /// </summary>
    class Downloader {

        /// <summary>
        /// The list of tags currently being downloaded.
        /// </summary>
        public static readonly List<string> TagsBeingDownloaded = new();
        /// <summary>
        /// The list of page urls currently being downloaded.
        /// </summary>
        public static readonly List<string> PagesBeingDownloaded = new();
        /// <summary>
        /// The list of pool ids currently being downloaded.
        /// </summary>
        public static readonly List<string> PoolsBeingDownloaded = new();
        /// <summary>
        /// The list of post ids currently being downloaded.
        /// </summary>
        public static readonly List<string> ImagesBeingDownloaded = new();
        /// <summary>
        /// The list of furry booru tags currently being downloaded.
        /// </summary>
        public static readonly List<string> FurryBooruTagsBeingDownloaded = new();
        /// <summary>
        /// The list of imgur albums currently being downloaded.
        /// </summary>
        public static readonly List<string> ImgurAlbumsBeingDownloaded = new();
        /// <summary>
        /// The list of inkbunny searches currently being downloaded.
        /// </summary>
        public static readonly List<string> InkBunnySearchesBeingDownloaded = new();

        /// <summary>
        /// Creates a new tag downloader.
        /// </summary>
        /// <param name="Tags">Tags to download.</param>
        /// <param name="UseDialog">Whether the form should be a dialog.</param>
        public static void DownloadTags(string Tags, bool UseDialog) {
            using frmTagDownloader DownloadForm = new(new(Tags, DownloadSite.e621));
            if (UseDialog) {
                DownloadForm.ShowDialog();
            }
            else {
                DownloadForm.Show();
            }
        }

        /// <summary>
        /// Creates a new page downloader.
        /// </summary>
        /// <param name="PageUrl">Page URL to download.</param>
        /// <param name="UseDialog">Whether the form should be a dialog.</param>
        public static void DownloadPage(string PageUrl, bool UseDialog) {
            using frmTagDownloader DownloadForm = new(new(new Uri(PageUrl), DownloadSite.e621));
            if (UseDialog) {
                DownloadForm.ShowDialog();
            }
            else {
                DownloadForm.Show();
            }
        }

        /// <summary>
        /// Creates a new pool downloader.
        /// </summary>
        /// <param name="PoolId">The Pool ID to download.</param>
        /// <param name="UseDialog">Whether the form should be a dialog.</param>
        public static void DownloadPool(string PoolId, bool UseDialog) {
            using frmPoolDownloader DownloadForm = new(new(PoolId));
            if (UseDialog) {
                DownloadForm.ShowDialog();
            }
            else {
                DownloadForm.Show();
            }
        }

        /// <summary>
        /// Creates a new image downloader.
        /// </summary>
        /// <param name="ImageId">The image ID or URL to download.</param>
        /// <param name="UseDialog">Whether the form should be a dialog.</param>
        public static void DownloadImage(string ImageId, bool UseDialog) {
            using frmImageDownloader DownloadForm = new(new(ImageId));
            if (UseDialog) {
                DownloadForm.ShowDialog();
            }
            else {
                DownloadForm.Show();
            }
        }

        /// <summary>
        /// Creates a new furry booru tag downloader.
        /// </summary>
        /// <param name="Tags">Tags to download.</param>
        /// <param name="UseDialog">Whether the form should be a dialog.</param>
        public static void DownloadFurryBooru(string Tags, bool UseDialog) {
            using frmTagDownloader DownloadForm = new(new(Tags, DownloadSite.FurryBooru));
            if (UseDialog) {
                DownloadForm.ShowDialog();
            }
            else {
                DownloadForm.Show();
            }
        }

        /// <summary>
        /// Creates a new imgur album downloader.
        /// </summary>
        /// <param name="Album">The album ID or url to download.</param>
        /// <param name="UseDialog">Whether the form should be a dialog.</param>
        public static void DownloadImgurAlbum(string Album, bool UseDialog) {
            using frmImgurDownloader DownloadForm = new(new(Album));
            if (UseDialog) {
                DownloadForm.ShowDialog();
            }
            else {
                DownloadForm.Show();
            }
        }

        /// <summary>
        /// Creates a new inkbunny downloader. A minimum of one search term is required.
        /// </summary>
        /// <param name="Keywords">The keywords that will be downloaded.</param>
        /// <param name="ArtistsGallery">The artists' username that will be used to search in their gallery.</param>
        /// <param name="UsersFavorites">The users' ID to search in their favorites.</param>
        /// <param name="UseDialog">Whether the form should be a dialog.</param>
        public static void DownloadInkbunny(string Keywords, string ArtistsGallery, string UsersFavorites, bool UseDialog) {
            if (!string.IsNullOrWhiteSpace(Config.Settings.InkBunny.SessionID) || !string.IsNullOrWhiteSpace(Config.Settings.InkBunny.GuestSessionID)
            && !string.IsNullOrWhiteSpace(Keywords) && !string.IsNullOrWhiteSpace(ArtistsGallery) && !string.IsNullOrWhiteSpace(UsersFavorites)) {
                frmInkBunnyDownloader DownloadForm = new(new(Keywords, ArtistsGallery, UsersFavorites));
                if (UseDialog) {
                    DownloadForm.ShowDialog();
                }
                else {
                    DownloadForm.Show();
                }
            }
        }

    }

    /// <summary>
    /// Class to hold download helper logic, such as string formatters.
    /// </summary>
    class DownloadHelpers {

        /// <summary>
        /// Converts the input <paramref name="BytesRecieved"/> and <paramref name="BytesToRecieve"/> into a legible and formatted string.
        /// </summary>
        /// <param name="BytesRecieved">The <see cref="Int64"/> bytes downloaded of the file to format.</param>
        /// <param name="BytesToRecieve">The <see cref="Int64"/> total size of the file in bytes to format.</param>
        /// <returns>The string, formatted as "(RecievedBytes) / (TotalBytes) (KiB|MiB)".</returns>
        public static string GetTransferRate(long BytesRecieved, long BytesToRecieve) =>
            BytesToRecieve switch {
                < 1048576 => $"{BytesRecieved / 1024f:#0.00} / {BytesToRecieve / 1024f:#0.00} KiB",
                _ => $"{BytesRecieved / 1024f / 1024f:#0.00} / {BytesToRecieve / 1024f / 1024f:#0.00} MiB"
            };

    }

    /// <summary>
    /// Class to handle filtering artist username tags, to filter out tags that are considered informational.
    /// </summary>
    class UndesiredTags {
        /// <summary>
        /// Read-only array of undesired tags e621 is known to include in the artist tags.
        /// (As of 2022, this may be outdated)
        /// </summary>
        public static readonly string[] HardcodedUndesiredTags = { "avoid_posting", "conditional_dnp", "sound_warning", "unknown_artist_signature" };

        /// <summary>
        /// Determine if a tag is an undesired artist name tag.
        /// </summary>
        /// <param name="tag">The tag to check.</param>
        /// <returns>True if <paramref name="Tag"/> is undesired, null, empty, or whitespace; otherwise, false.</returns>
        public static bool IsUndesiredHardcoded(string Tag) =>
            string.IsNullOrWhiteSpace(Tag) || HardcodedUndesiredTags.Any(x => x == Tag);

        /// <summary>
        /// Determine if a tag is an undesired artist name tag.
        /// </summary>
        /// <param name="tag">The tag to check.</param>
        /// <param name="BadTags">The array of tags that are considered bad.</param>
        /// <returns>True if <paramref name="Tag"/> is in the <paramref name="BadTags"/> array, null, empty, or whitespace; otherwise, false.</returns>
        public static bool IsUndesired(string Tag, string[] BadTags) =>
            string.IsNullOrWhiteSpace(Tag) || BadTags.Any(x => x == Tag);
    }

}