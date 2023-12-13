/*
    IImageProvider example for DanBooru.
    Use this as a guideline (NOT A COPY + PASTE) of how the provider should look.
    Licensed as GPL-3.0, see the branch LICENSE for full licensing.
*/

namespace DanBooruProvider;

[ApiFormat(ApiFormat.JSON, DateTimeFormat = Const.DateTimeFormat)]
public sealed class ImageProvider : IImageProvider<DanBooruPost> {
    public string BaseApiUrl => "https://danbooru.donmai.us/posts/{0}.json";
    public string BaseFrontendUrl => "https://danbooru.donmai.us/posts/{0}";

    public void GenerateDownloadClientData(HttpClientHeaders Client) { /* Nothing to be done */ }
    public void Dispose() { /* Nothing to be done */ }

    public bool ValidImageLink(string Url) {
        return Regex.IsMatch(Url, @"^(http(s)?:\/\/)?danbooru\.donmai\.us\/posts\/\d+", RegexOptions.IgnoreCase);
    }
    public bool ValidImageId(string Id) {
        return DownloadVerification.IsNumericOnly(Id);
    }
    public string GetImageId(UrlData UrlData) {
        return UrlData.LastPath.Path;
    }
}
