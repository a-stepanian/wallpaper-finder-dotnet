namespace Wallpaper.API.Models;
public class PaginatedDto<T>
{
    public int? Total { get; set; }
    public int? TotalPages { get; set; }
    public IEnumerable<T>? Results { get; set; }
}