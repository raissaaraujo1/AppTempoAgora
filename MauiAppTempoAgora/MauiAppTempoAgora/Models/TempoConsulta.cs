using SQLite;

namespace MauiAppTempoAgora.Models
{
    public class TempoConsulta
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string? Cidade { get; set; }

        public double? Temperatura { get; set; }

        public string? Icone { get; set; }

        public DateTime DataConsulta { get; set; }
    }
}