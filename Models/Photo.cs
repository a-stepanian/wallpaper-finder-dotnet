namespace Wallpaper.API.Models;
public class Photo
{
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? PromotedAt { get; set; } // Nullable, as not all photos are promoted
    public int? Width { get; set; } // Nullable, in case the width is missing
    public int? Height { get; set; } // Nullable, in case the height is missing
    public string Color { get; set; }
    public string Description { get; set; } // Nullable, if the description is missing
    public string AltDescription { get; set; } // Nullable, if the alt description is missing
    public string Url { get; set; }
    public int Likes { get; set; }
    public int Downloads { get; set; }
    public bool IsLikedByUser { get; set; }
    public string Location { get; set; } // Nullable, location may be missing
    public User User { get; set; }
    public PhotoUrls Urls { get; set; }
}