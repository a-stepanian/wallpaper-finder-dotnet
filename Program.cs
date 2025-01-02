using Microsoft.OpenApi.Models;
using System.Net.Http;
using System.Threading.Tasks;

// Env vars
DotNetEnv.Env.Load();
var host = Environment.GetEnvironmentVariable("HOST");
var apiKey = Environment.GetEnvironmentVariable("ACCESS_KEY");

// Builder config
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
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

// App config 
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

// Minimal API
app.MapGet("/photos", async (HttpClient client, int page = 1, int perPage = 1, string searchTerm = "junkyard") =>
{
    try
    {
        // Add auth header
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", $"Client-ID {apiKey}");

        string unsplashApiUrl = $"https://api.unsplash.com/search/photos?query={searchTerm}&page={page}&per_page={perPage}";
        HttpResponseMessage response = await client.GetAsync(unsplashApiUrl);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Results.Content(content, "application/json");
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        return Results.Json(new { error = response.ReasonPhrase, details = errorContent }, statusCode: (int)response.StatusCode);
    }
    catch (Exception ex)
    {
        return Results.Json(new { error = "Internal Server Error", message = ex.Message }, statusCode: 500);
    }
});
    
app.Run();
