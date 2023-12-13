/*
    IProviderInfo example for DanBoouro.
    Use this as a guideline (NOT A COPY + PASTE) of how the provider should look.
    Licensed as GPL-3.0, see the branch LICENSE for full licensing.
*/

namespace DanBooruProvider;

internal sealed class ProviderInfo : IProviderInfo {
    public string Name => "DanBooru";
    public string FolderName => "danbooru";
    public string SiteAccessName => "danbooru.donmai.us";
    public System.Drawing.Image Icon => Properties.Resources.Favicon;

    public void Dispose() {
        this.Icon.Dispose();
    }
}
