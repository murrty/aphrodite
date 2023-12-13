/*
    IDanBooruTagProvider example for DanBooru.
    Use this as a guideline (NOT A COPY + PASTE) of how the provider should look.
    Licensed as GPL-3.0, see the branch LICENSE for full licensing.
*/

namespace DanBooruProvider;

[ApiFormat(ApiFormat.JSON, StartingPageIndex = 1, PageIndexOffset = 1, PostsPerPage = Const.PostsPerPage, DateTimeFormat = Const.DateTimeFormat)]
public sealed class TagsProvider : IDanBooruTagProvider<DanBooruPost> {
    public string BaseApiUrl => "https://danbooru.donmai.us/posts.json?tags={0}+status%3Aany&page={1}&limit={2}";
    public string BaseTagsPageApiUrl => "https://danbooru.donmai.us/posts.json?tags={1}+status%3Aany&page={0}";
    public string BasePageApiUrl => "https://danbooru.donmai.us/posts.json?page={0}";
    public string BaseFrontendUrl => "https://danbooru.donmai.us/posts.json?tags={0}+status%3Aany";
    public bool SupportsPageDownload => true;

    public void GenerateDownloadClientData(HttpClientHeaders Client) { /* Nothing to be done */ }
    public void Dispose() { /* Nothing to be done */ }

    public bool ValidPageLink(string Url) {
        return ValidPageRegex.IsMatch(Url);
    }
    public bool ValidPageWithTagsLink(string Url) {
        return ValidPageTagsRegex.IsMatch(Url);
    }
    public string GetTags(UrlData UrlData) {
        return UrlData.LastPath.GetQuery("tags");
    }
    public int GetPageNumber(UrlData UrlData) {
        if (UrlData.LastPath is null) {
            return 1;
        }
        if (UrlData.LastPath.TryGetQuery("page", out var val) && int.TryParse(val, out int page)) {
            return page;
        }
        return 1;
    }
    public void BuildTags(string Tags, out string UrlTags, out string Identifier) {
        Tags = TagsReplaceRegex.Replace(Tags, string.Empty)
            .ReplaceWhitespace(" ")
            .Trim();

        Identifier = Tags;
        while ((Tags = Uri.UnescapeDataString(Tags)) != Identifier) {
            Identifier = Tags;
        }

        UrlTags = Uri.EscapeDataString(Tags)
            .ReplaceWhitespace('+');
    }

    private static readonly Regex ValidPageRegex = new(@"^(http(s)?:\/\/)?danbooru\.donmai\.us(\/posts)?(?!\/)", RegexOptions.IgnoreCase);
    private static readonly Regex ValidPageTagsRegex = new(@"^(http(s)?:\/\/)?danbooru\.donmai\.us\/posts(?!\/).*?(tags=)(?!&).", RegexOptions.IgnoreCase);
    private static readonly Regex TagsReplaceRegex = new("(score|favcount|status)(:|%3A)(<|>|<>|><)?(=)?((.+)[ +]|(.+)[ +]?)", RegexOptions.IgnoreCase);
}
