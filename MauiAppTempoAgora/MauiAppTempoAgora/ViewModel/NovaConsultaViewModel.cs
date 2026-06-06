using System.ComponentModel;
using System.Windows.Input;
using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora.ViewModels
{
    public class NovaConsultaViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // Campos privados que armazenam os valores reais
        string? cidade;
        string? previsao;
        string? mapa;
        string? iconeClima;
        bool temDados = false;

        // Controla se há dados carregados para mostrar na UI
        public bool TemDados
        {
            get => temDados;
            set
            {
                temDados = value;
                PropertyChanged(this, new PropertyChangedEventArgs("TemDados"));
            }
        }

        // Cidade digitada pelo usuário
        public string? Cidade
        {
            get => cidade;
            set
            {
                cidade = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Cidade"));
                TemDados = false; // Sempre que muda a cidade, não há dados carregados ainda
            }
        }

        // Texto com previsão detalhada
        public string? Previsao
        {
            get => previsao;
            set
            {
                previsao = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Previsao"));
            }
        }

        // URL do mapa para o WebView
        public string? Mapa
        {
            get => mapa;
            set
            {
                mapa = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Mapa"));
            }
        }

        // Ícone do clima a ser exibido
        public string? IconeClima
        {
            get => iconeClima;
            set
            {
                iconeClima = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IconeClima"));
            }
        }

        // Consultar o clima para a cidade
        public ICommand ConsultarTempo
        {
            get => new Command(async () =>
            {
                try
                {
                    // Verifica se o usuário informou a cidade
                    if (string.IsNullOrEmpty(Cidade))
                        throw new Exception("Informe a cidade.");

                    // Chamada ao serviço que consulta a API do OpenWeather
                    Tempo? t = await DataService.GetTempo(Cidade);

                    if (t != null)
                    {
                        // Atualiza flag para exibir dados na UI
                        TemDados = true;

                        // Monta o texto completo da previsão
                        Previsao =
                            $"Latitude: {t.lat}\n" +
                            $"Longitude: {t.lon}\n" +
                            $"Temperatura: {t.temp}\n" +
                            $"Sensação Térmica: {t.feels_like}\n" +
                            $"Temp Max: {t.temp_max}\n" +
                            $"Temp Min: {t.temp_min}\n" +
                            $"Nascer do Sol: {t.sunrise}\n" +
                            $"Pôr do Sol: {t.sunset}";

                        // Converte o código do ícone da API em imagem local
                        if (t.icon.StartsWith("01"))
                            IconeClima = "sol.png";
                        else if (t.icon.StartsWith("02"))
                            IconeClima = "parcialmente_nublado.png";
                        else if (t.icon.StartsWith("03") || t.icon.StartsWith("04"))
                            IconeClima = "nublado.png";
                        else if (t.icon.StartsWith("09") || t.icon.StartsWith("10"))
                            IconeClima = "chuva.png";
                        else if (t.icon.StartsWith("11"))
                            IconeClima = "tempestade.png";
                        else if (t.icon.StartsWith("13"))
                            IconeClima = "neve.png";
                        else if (t.icon.StartsWith("50"))
                            IconeClima = "neblina.png";

                        // Salva no banco local para histórico
                        Tempo c = new Tempo
                        {
                            Cidade = Cidade,
                            lon = t.lon,
                            lat = t.lat,
                            temp = t.temp,
                            temp_min = t.temp_min,
                            temp_max = t.temp_max,
                            feels_like = t.feels_like,
                            visibility = t.visibility,
                            timezone = t.timezone,
                            sunrise = t.sunrise,
                            sunset = t.sunset,
                            description = t.description,
                            icon = iconeClima,
                            DataConsulta = DateTime.Now
                        };

                        await App.Database.Insert(c);

                        // Monta a URL do mapa para o WebView
                        // Substitui vírgula por ponto para não quebrar a URL
                        Mapa =
                            $"https://embed.windy.com/embed.html?" +
                            $"type=map&location=coordinates&metricRain=mm&" +
                            $"metricTemp=°C&metricWind=km/h&zoom=5&overlay=wind&" +
                            $"product=ecmwf&level=surface&" +
                            $"lat={t.lat.ToString().Replace(",", ".")}&" +
                            $"lon={t.lon.ToString().Replace(",", ".")}";
                    }
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlertAsync("Ops", ex.Message, "OK");
                }
            });
        }
    }
}