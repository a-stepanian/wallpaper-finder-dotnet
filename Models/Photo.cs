namespace Wallpaper.API.Models;
public class Photo
{
    public string? Id { get; set; }
    public string? Slug { get; set; }
    public object? AlternativeSlugs { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? PromotedAt { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string? Color { get; set; }
    public string? BlurHash { get; set; }
    public string? Description { get; set; }
    public string? AltDescription { get; set; }
    public List<object>? Breadcrumbs { get; set; }
    public object Urls { get; set; }
    public object Links { get; set; }
    public int Likes { get; set; }
    public bool LikedByUser { get; set; }
    public List<object>? CurrentUserCollections { get; set; }
    public object? Sponsorship { get; set; }
    public object? TopicSubmissions { get; set; }
    public string? AssetType { get; set; }
    public object User { get; set; }
}