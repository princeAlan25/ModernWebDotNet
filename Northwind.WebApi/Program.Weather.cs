partial class Program
{
    static string[] summaries { get; set; } = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];
    internal static WeatherForecast[]? GetWeather(int days)
    {
        WeatherForecast[]? weatherForecasts = Enumerable.Range(1, days).Select(
            weather => new WeatherForecast(
                DateOnly.FromDateTime(DateTime.Now.AddDays(weather)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            )).ToArray();
        return weatherForecasts;
    }
}

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}