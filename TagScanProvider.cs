/*
    IPostScannerProvider example for DanBooru.
    Use this as a guideline (NOT A COPY + PASTE) of how the provider should look.
    Licensed as GPL-3.0, see the branch LICENSE for full licensing.
*/

namespace DanBooruProvider;

[ApiFormat(ApiFormat.JSON, DateTimeFormat = Const.DateTimeFormat)]
public sealed class TagScanProvider : IPostScannerProvider<DanBooruPost> {
    public string BaseApiUrl => "https://danbooru.donmai.us/posts/{0}.json";
    public string BaseFrontendUrl => "https://danbooru.donmai.us/posts/{0}";
    public string BaseFrontendTagSearchUrl => "https://danbooru.donmai.us/posts?tags={0}";

    public void GenerateDownloadClientData(HttpClientHeaders Client) { /* Nothing to be done */ }
    public void Dispose() { /* Nothing to be done */ }

    public bool ValidPostId(string Id) {
        return DownloadVerification.IsNumericOnly(Id);
    }
    public bool ValidPostLink(string Url) {
        return ValidLinkRegex.IsMatch(Url);
    }
    public string GetPostId(UrlData UrlData) {
        return UrlData.LastPath.Path;
    }

    private static readonly Regex ValidLinkRegex = new(@"^(http(s)?:\/\/)?danbooru\.donmai\.us\/posts\/\d+", RegexOptions.IgnoreCase);
}
