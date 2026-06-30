using System.Net.Http.Json;
using System.Text.Json;
using Thoitiet.DTOs;
using Thoitiet.Models;

public class OpenMeteoClient
{
    private readonly HttpClient
        _httpClient;


    public OpenMeteoClient(

        IHttpClientFactory factory)

    {
        _httpClient =
            factory.CreateClient();
    }

    public async Task<
    List<WeatherResponse>>

    GetWeather(

    WeatherConfiguration config

    )
    {
        var url =

        "https://api.open-meteo.com/v1/forecast?"

        + $"latitude={config.Latitude}"

        + $"&longitude={config.Longitude}"

        + $"&daily={config.Daily}"

        + $"&hourly={config.Hourly}"

        + $"&current={config.Current}";
        if (
            !string.IsNullOrWhiteSpace(
            config.Timezone
            ))
        {
            url +=
            $"&timezone={config.Timezone}";
        }
        var json =
        await _httpClient
        .GetStringAsync(url);

        if (json.TrimStart().StartsWith("["))
        {

            var list =
                JsonSerializer.Deserialize<List<WeatherResponse>>(json)
                ?? [];
            if (string.IsNullOrWhiteSpace(config.PositionName))
            {
                return list;

            }
            var positionNames = config.PositionName
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < list.Count; i++)
            {
                if (i < positionNames.Length)
                {
                    list[i].positionName = positionNames[i];
                }
            }

            return list;
        }

        var item =
            JsonSerializer.Deserialize<WeatherResponse>(json);

        if (item == null)
        {
            return [];
        }

        item.positionName = config.PositionName;
        Console.WriteLine("PositionNamePositionName");
        Console.WriteLine(item);
        return [item];
    }
}
