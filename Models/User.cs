namespace Wallpaper.API.Models;
public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public string PortfolioUrl { get; set; } // Nullable, if the user hasn't set a portfolio
    public string Bio { get; set; } // Nullable, if the user hasn't provided a bio
    public string Location { get; set; } // Nullable, if the user hasn't set a location
    public string AvatarUrl { get; set; } // Nullable, if the user doesn't have an avatar
}