using Microsoft.OpenApi.Models;
using System.Text.Json;
using Wallpaper.API.Models;

DotNetEnv.Env.Load();
var host = Environment.GetEnvironmentVariable("HOST");
var apiKey = Environment.GetEnvironmentVariable("ACCESS_KEY");

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins(host)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wallpaper Finder API", Description = "Free High Quality Wallpaper", Version = "v1" });
});
    
var app = builder.Build();
app.UseCors("AllowAngularApp");
    
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI(c =>
   {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wallpaper Finder API V1");
   });
}

app.MapGet("/photos", async (int page = 1, int per_page = 10, string searchTerm = "junkyard") =>
{
    var client = new HttpClient();
    try
    {
        var response = await client.GetAsync($"https://api.unsplash.com/photos?query={searchTerm}&page={page}&per_page={per_page}&client_id={apiKey}");
        
        if (!response.IsSuccessStatusCode)
        {
            return Results.Json(new { message = "Error fetching photos", statusCode = response.StatusCode }, statusCode: (int)response.StatusCode);
        }

        // Read the response content as a string
        var responseContent = await response.Content.ReadAsStringAsync();

        // Deserialize the JSON response into a list of photos
        var photos = JsonSerializer.Deserialize<List<Photo>>(responseContent);

        // Assuming the total count of photos is available in the Unsplash response headers (or through another API call)
        var totalCount = 1000;  // Replace with actual total count from Unsplash API if needed

        // Return a PaginatedDto response
        return Results.Ok(new PaginatedDto<Photo>
        {
            Total = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / per_page),
            Results = photos
        });
    }
    catch (HttpRequestException ex)
    {
        // Handle errors related to the HTTP request (e.g., network issues)
        Console.Error.WriteLine($"Error making HTTP request: {ex}");
        return Results.Json(new { message = "Internal server error while fetching photos", details = ex.Message }, statusCode: 500);
    }
    catch (JsonException ex)
    {
        // Handle errors related to deserialization
        Console.Error.WriteLine($"Error deserializing response: {ex.Message}");
        return Results.Json(new { message = "Error deserializing photo data", details = ex.Message }, statusCode: 500);
    }
    catch (Exception ex)
    {
        // Catch any other unexpected errors
        Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        return Results.Json(new { message = "An unexpected error occurred", details = ex.Message }, statusCode: 500);
    }
});
    
app.Run();