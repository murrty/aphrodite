/*
    Pool example for DanBooru.
    Use this as a guideline (NOT A COPY + PASTE) of how the provider should look.
    Licensed as GPL-3.0, see the branch LICENSE for full licensing.
*/

namespace DanBooruProvider;

[Serializable]
public class DanBooruPool : Pool {
    [JsonProperty("id")]
    public uint PoolId { get; set; }

    [JsonProperty("name")]
    public override string PoolName { get; set; }

    [JsonProperty("created_at")]
    public DateTimeOffset created_at { get; set; }

    [JsonProperty("updated_at")]
    public DateTimeOffset updated_at { get; set; }

    [JsonProperty("description")]
    public string description { get; set; }

    [JsonProperty("is_active")]
    public bool is_active { get; set; }

    [JsonProperty("is_deleted")]
    public override bool PoolDeleted {get; set; }

    [JsonProperty("post_ids")]
    public override uint[] OrderedPostIds { get; set; }

    [JsonProperty("category")]
    public string category { get; set; }

    [JsonProperty("post_count")]
    public override int PageCount { get; set; }

    // Since this is just a getter in this case, ignore the set field. 'JsonIgnore' is not required.
    public override object PoolUpdateTime { get => updated_at; set { }  }

    // If this was a 'PostsIncluded' pool, this would point to the array (or deserialize into, if possible).
    // Since it's not, this is just here for examples sake.
    public override Post[] PoolPosts { get; set; }
}
