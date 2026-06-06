using MauiAppTempoAgora.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MauiAppTempoAgora.ViewModels
{
    public class HistoricoConsultaViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Cidade usada no filtro de busca (pode ser nula para buscar todas)
        public string? Cidade { get; set; }

        // Intervalo padrão: últimos 7 dias até hoje
        DateTime dataInicio = DateTime.Now.AddDays(-7);
        public DateTime DataInicio
        {
            get => dataInicio;
            set
            {
                dataInicio = value;
                PropertyChanged(this, new PropertyChangedEventArgs("DataInicio")); 
            }
        }

        DateTime dataFim = DateTime.Now;
        public DateTime DataFim
        {
            get => dataFim;
            set
            {
                dataFim = value;
                PropertyChanged(this, new PropertyChangedEventArgs("DataFim"));
            }
        }

        // Horários padrão usados para complementar o filtro de data
        TimeSpan horaInicio = TimeSpan.Zero; // 00:00:00
        public TimeSpan HoraInicio
        {
            get => horaInicio;
            set
            {
                horaInicio = value;
                PropertyChanged(this, new PropertyChangedEventArgs("HoraInicio"));
            }
        }

        TimeSpan horaFim = new TimeSpan(23, 59, 59); // final do dia
        public TimeSpan HoraFim
        {
            get => horaFim;
            set
            {
                horaFim = value;
                PropertyChanged(this, new PropertyChangedEventArgs("HoraFim"));
            }
        }

        // Controle simples para evitar múltiplas requisições simultâneas (evita duplicação de carga)
        bool estaAtualizando = false;
        public bool EstaAtualizando
        {
            get => estaAtualizando;
            set
            {
                estaAtualizando = value;
                PropertyChanged(this, new PropertyChangedEventArgs("EstaAtualizando"));
            }
        }

        // Lista observável: quando muda, a UI atualiza automaticamente
        ObservableCollection<Tempo> listaConsultas = new();
        public ObservableCollection<Tempo> ListaConsultas
        {
            get => listaConsultas;
            set => listaConsultas = value;
        }

        // Carrega todo o histórico
        public ICommand CarregarTodos
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        // Evita execução duplicada (ex: usuário clicando várias vezes)
                        if (EstaAtualizando)
                            return;

                        EstaAtualizando = true;

                        // Busca todos os registros no SQLite
                        List<Tempo> tmp = await App.Database.GetAllRows();
                        ListaConsultas.Clear();
                        tmp.ForEach(i => ListaConsultas.Add(i));
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlertAsync("Ops", ex.Message, "OK");
                    }
                    finally
                    {
                        EstaAtualizando = false;
                    }
                });
            }
        }

        // Busca filtrando por cidade e período
        public ICommand BuscarPorPeriodo
        {

            get
            {

                return new Command(async () =>
                {
                    try
                    {
                        if (EstaAtualizando)
                            return;

                        // Junta data + hora para formar o intervalo completo de busca
                        DateTime inicio = DataInicio.Date + HoraInicio;
                        DateTime fim = DataFim.Date + HoraFim;

                        // Validação básica para evitar erro lógico na consulta
                        if (inicio > fim)
                            throw new Exception("A data/hora de início não pode ser maior que a data/hora fim.");

                        EstaAtualizando = true;

                        // Busca no banco filtrando cidade + período
                        List<Tempo> tmp = await App.Database.SearchByPeriod(Cidade ?? "", inicio, fim);

                        ListaConsultas.Clear();
                        tmp.ForEach(i => ListaConsultas.Add(i));

                        // Feedback caso não encontre resultados
                        if (!tmp.Any())
                            await Shell.Current.DisplayAlertAsync("Aviso", "Nenhum registro encontrado.", "OK");
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlertAsync("Ops", ex.Message, "OK");
                    }
                    finally
                    {
                        EstaAtualizando = false;
                    }
                });
            }
        }
    }
}