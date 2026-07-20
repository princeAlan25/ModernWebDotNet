using Northwind.Web.Components.Pages;
using Northwind.DataContext.Sqlite;

var builder = WebApplication.CreateBuilder(args);

#region Blazor Service registration
builder.Services.AddRazorComponents();
builder.Services.AddNorthwindContext();
#endregion

var app = builder.Build();


app.MapGet("/env", () => $"Environment is {app.Environment.EnvironmentName}");


app.MapGet("/data", () => Results.Json(
    new{
        firstName =  "prince",
        lastName = "alana",
        age = 21
    }
));

app.MapGet("/content", () => Results.Content(
   content: $"""
  <!doctype html>
  <html lang="en">
  <head>
    <title>Welcome to Northwind Web!</title>
  </head>
  <body>
    <h1>Welcome to Northwind Web!</h1>
  </body>
  </html>
  """,
  contentType: "text/html"
    ));

#region HTTP/HTTPS pipeline configuration
if (!app.Environment.IsDevelopment()) app.UseHsts();
app.UseHttpsRedirection().UseDefaultFiles().UseStaticFiles();
#endregion

#region Blazor pipeline configuration
app.MapRazorComponents<App>();
app.UseAntiforgery();
#endregion

app.Run();
