using Newtonsoft.Json.Linq;
using MauiAppTempoAgora.Models;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetTempo(string cidade)
        {
            Tempo? t = null;

            string chave = "53fb370ac763ee7ac16d575813fd7064"; // API Key do OpenWeatherMap
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url); // Faz a requisição HTTP

                if (resp.IsSuccessStatusCode) // Só continua se a API respondeu corretamente
                {
                    string json = await resp.Content.ReadAsStringAsync(); // Lê o JSON retornado

                    var rascunho = JObject.Parse(json); // Converte JSON para objeto manipulável

                    // OpenWeather retorna sunrise/sunset em Unix timestamp (segundos)
                    DateTime baseTime = new();

                    DateTime sunrise = baseTime.AddSeconds((double)rascunho["sys"]["sunrise"]);
                    DateTime sunset = baseTime.AddSeconds((double)rascunho["sys"]["sunset"]);

                    // Mapeia os dados da API para o modelo Tempo
                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["main"],

                        temp_max = (double)rascunho["main"]["temp_max"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp = (double)rascunho["main"]["temp"],
                        feels_like = (double)rascunho["main"]["feels_like"],

                        visibility = (int)rascunho["visibility"],
                        timezone = (int)rascunho["timezone"],

                        sunrise = sunrise,
                        sunset = sunset,

                        icon = (string)rascunho["weather"][0]["icon"]
                    };
                }
            }

            return t; 
        }
    }
}