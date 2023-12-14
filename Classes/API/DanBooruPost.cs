/*
    Post example for DanBooru.
    Use this as a guideline (NOT A COPY + PASTE) of how the provider should look.
    Licensed as GPL-3.0, see the branch LICENSE for full licensing.
*/

namespace DanBooruProvider;

public sealed class DanBooruPost : Post {
    // You can deserialize values on-top of overridden properties for ease of use (i apologize in advance for mixed naming styles)
    [JsonProperty("id")]
    public override uint PostId { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset created_at { get; set; }

    [JsonProperty("uploader_id")]
    public int uploader_id { get; set; }

    [JsonProperty("score")]
    public override int Score { get; set; }

    [JsonProperty("source")]
    public string source { get; set; }

    [JsonProperty("md5")]
    public override string FileHash { get; set; }

    [JsonProperty("last_comment_bumped_at")]
    public object last_comment_bumped_at { get; set; }

    [JsonProperty("rating")]
    public string rating { get; set; }

    [JsonProperty("image_width")]
    public int image_width { get; set; }

    [JsonProperty("image_height")]
    public int image_height { get; set; }

    [JsonProperty("tag_string")]
    public string tag_string { get; set; }

    [JsonProperty("fav_count")]
    public override int FavoriteCount { get; set; }

    [JsonProperty("file_ext")]
    public override string FileExtension { get; set; }

    [JsonProperty("last_noted_at")]
    public object last_noted_at { get; set; }

    [JsonProperty("parent_id")]
    public int? parent_id { get; set; }

    [JsonProperty("has_children")]
    public bool has_children { get; set; }

    [JsonProperty("approver_id")]
    public int? approver_id { get; set; }

    [JsonProperty("tag_count_general")]
    public int tag_count_general { get; set; }

    [JsonProperty("tag_count_artist")]
    public int tag_count_artist { get; set; }

    [JsonProperty("tag_count_character")]
    public int tag_count_character { get; set; }

    [JsonProperty("tag_count_copyright")]
    public int tag_count_copyright { get; set; }

    [JsonProperty("file_size")]
    public override long FileSize { get; set; }

    [JsonProperty("up_score")]
    public override int ScoreUp { get; set; }

    [JsonProperty("down_score")]
    public override int ScoreDown { get; set; }

    [JsonProperty("is_pending")]
    public bool is_pending { get; set; }

    [JsonProperty("is_flagged")]
    public bool is_flagged { get; set; }

    [JsonProperty("is_deleted")]
    public bool is_deleted { get; set; }

    [JsonProperty("tag_count")]
    public int tag_count { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset updated_at { get; set; }

    [JsonProperty("pixiv_id")]
    public uint? pixiv_id { get; set; }

    [JsonProperty("tag_string_artist")]
    public string tag_string_artist { get; set; }

    [JsonProperty("file_url")]
    public override string FileUrl { get; set; }

    #region Other API values (unused at this moment)
    //[JsonProperty("is_banned")]
    //public bool is_banned { get; set; }

    //[JsonProperty("last_commented_at")]
    //public object last_commented_at { get; set; }

    //[JsonProperty("has_active_children")]
    //public bool has_active_children { get; set; }

    //[JsonProperty("bit_flags")]
    //public int bit_flags { get; set; }

    //[JsonProperty("tag_count_meta")]
    //public int tag_count_meta { get; set; }

    //[JsonProperty("has_large")]
    //public bool has_large { get; set; }

    //[JsonProperty("has_visible_children")]
    //public bool has_visible_children { get; set; }

    //[JsonProperty("media_asset")]
    //public Media_Asset media_asset { get; set; }

    //[JsonProperty("tag_string_general")]
    //public string tag_string_general { get; set; }

    //[JsonProperty("tag_string_character")]
    //public string tag_string_character { get; set; }

    //[JsonProperty("tag_string_copyright")]
    //public string tag_string_copyright { get; set; }

    //[JsonProperty("tag_string_meta")]
    //public string tag_string_meta { get; set; }

    //[JsonProperty("large_file_url")]
    //public string large_file_url { get; set; }

    //[JsonProperty("preview_file_url")]
    //public string preview_file_url { get; set; }

    //[JsonProperty("DanBooruMediaAsset")]
    //public sealed class Media_Asset {

    //    [JsonProperty("id")]
    //    public int id { get; set; }

    //    [JsonProperty("created_at")]
    //    public DateTimeOffset created_at { get; set; }

    //    [JsonProperty("updated_at")]
    //    public DateTimeOffset updated_at { get; set; }

    //    [JsonProperty("md5")]
    //    public string md5 { get; set; }

    //    [JsonProperty("file_ext")]
    //    public string file_ext { get; set; }

    //    [JsonProperty("file_size")]
    //    public int file_size { get; set; }

    //    [JsonProperty("image_width")]
    //    public int image_width { get; set; }

    //    [JsonProperty("image_height")]
    //    public int image_height { get; set; }

    //    [JsonProperty("duration")]
    //    public object duration { get; set; }

    //    [JsonProperty("status")]
    //    public string status { get; set; }

    //    [JsonProperty("file_key")]
    //    public string file_key { get; set; }

    //    [JsonProperty("is_public")]
    //    public bool is_public { get; set; }

    //    [JsonProperty("pixel_hash")]
    //    public string pixel_hash { get; set; }

    //    [JsonProperty("variants")]
    //    public Variant[] variants { get; set; }
    //}

    //[JsonProperty("DanBooruVariant")]
    //public sealed class Variant {

    //    [JsonProperty("type")]
    //    public string type { get; set; }

    //    [JsonProperty("url")]
    //    public string url { get; set; }

    //    [JsonProperty("width")]
    //    public int width { get; set; }

    //    [JsonProperty("height")]
    //    public int height { get; set; }

    //    [JsonProperty("file_ext")]
    //    public string file_ext { get; set; }
    //}
    #endregion

    // These are required values. They require the 'set' field.
    public override bool PostDeleted { get => is_deleted || FileUrl.IsNullEmptyWhitespace(); set { } }
    public override string Rating {
        get {
            // We need to re-map the rating for danbooru to conformt.
            return rating switch {
                "g" or "general" => "s", // General should be 'safe' or 's'
                "s" or "sensitive" => "q", // Sensitive, should be 'questionable' or 'q'
                "q" or "questionable" => "q", // Questionable
                "e" or "explicit" => "e", // Explicit

                _ => "unknown",
            };
        }
        set { }
    }

    // These are optional. They don't require the 'set' field and just the getter can be overridden.
    public override string[] Tags => !tag_string.IsNullEmptyWhitespace() ? tag_string.SplitRemoveEmpty(' ') : [];
    public override string[] ArtistTags => !string.IsNullOrWhiteSpace(tag_string_artist) ? tag_string_artist.Split(' ') : [];
    public override string Author => uploader_id.ToString();
    public override object PostUpdateTime => updated_at;

    // You'll notice that they don't require 'JsonIgnore', that's because the parent is 'opt-in', as in they require an attribute to be serialized.
}
