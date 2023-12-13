/*
    IDanBooruPoolProvider example for DanBooru.
    Use this as a guideline (NOT A COPY + PASTE) of how the provider should look.
    Licensed as GPL-3.0, see the branch LICENSE for full licensing.
*/

namespace DanBooruProvider;

[ApiFormat(ApiFormat.JSON, StartingPageIndex = 1, PageIndexOffset = 1, PostsPerPage = Const.PostsPerPage, DateTimeFormat = Const.DateTimeFormat)]
[PoolFormat(PoolFormat.ExtraDownloadsRequired)]
public sealed class PoolProvider : IDanBooruPoolProvider<DanBooruPool, DanBooruPost> {
    public string BaseApiUrl => "https://danbooru.donmai.us/pools/{0}.json";
    public string BasePoolSearchApiUrl => "https://danbooru.donmai.us/posts.json?tags=pool%3A{0}&page={1}&limit={2}";
    public string BaseFrontendUrl => "https://danbooru.donmai.us/pools/{0}";

    public void GenerateDownloadClientData(HttpClientHeaders Client) { /* Nothing to be done */ }
    public void Dispose() { /* Nothing to be done */ }

    public bool ValidPoolLink(string Url) {
        return PoolLinkRegex.IsMatch(Url);
    }
    public bool ValidPoolId(string Id) {
        return DownloadVerification.IsNumericOnly(Id);
    }
    public string GetPoolId(UrlData UrlData) {
        return UrlData.LastPath.Path;
    }

    private static readonly Regex PoolLinkRegex = new(@"^(http(s)?:\/\/)?danbooru\.donmai\.us\/pools\/\d+", RegexOptions.IgnoreCase);
}
