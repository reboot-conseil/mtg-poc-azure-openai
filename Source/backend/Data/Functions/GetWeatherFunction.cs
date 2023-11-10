using Azure.AI.OpenAI;
using System.Text.Json;
using System;

namespace IASquad.Poc.AzureOpenAi.Data.Functions;

public class GetWeatherFunction
{
    static public string Name = "recuperer_meteo_fonction";

    // Return the function metadata
    static public FunctionDefinition GetFunctionDefinition()
    {
        return new FunctionDefinition()
        {
            Name = Name,
            Description = "Récupère la météo actuelle du lieu renseigné",
            Parameters = BinaryData.FromObjectAsJson(
            new
            {
                Type = "object",
                Properties = new
                {
                    Location = new
                    {
                        Type = "string",
                        Description = "La ville, la région ou le pays",
                    },
                    Unit = new
                    {
                        Type = "string",
                        Enum = new[] { "Celsius", "Fahrenheit" },
                    }
                },
                Required = new[] { "location" },
            },
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
        };
    }

    static public Weather GetWeather(string location, string unit)
    {
        // Appeler une API pour déterminer la météo
        return new Weather() { Temperature = 31, Unit = unit };
    }
}

public class WeatherInput
{
    public string Location { get; set; } = string.Empty;
    public string Unit { get; set; } = "Celsius";
}

public class Weather
{
    public int Temperature { get; set; }
    public string Unit { get; set; } = "Celsius";
}
