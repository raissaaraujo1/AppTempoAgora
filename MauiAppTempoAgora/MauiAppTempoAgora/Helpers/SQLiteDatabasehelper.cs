using MauiAppTempoAgora.Models;
using SQLite;

namespace MauiAppTempoAgora.Helpers
{
    // Classe responsável por centralizar o acesso ao banco SQLite
    public class SQLiteDatabaseHelper
    {
        // Conexão assíncrona com o banco SQLite
        readonly SQLiteAsyncConnection con;

        // Inicializa conexão e cria a tabela se não existir
        public SQLiteDatabaseHelper(string path)
        {
            con = new SQLiteAsyncConnection(path);

            // Cria a tabela baseada no model Tempo (se ainda não existir)
            con.CreateTableAsync<Tempo>().Wait();
        }

        // Retorna todos os registros da tabela "Tempo"
        public Task<List<Tempo>> GetAllRows()
        {
            return con.Table<Tempo>()
                .OrderByDescending(i => i.Id) 
                .ToListAsync();
        }

        // Insere um novo registro no banco
        public Task<int> Insert(Tempo model)
        {
            return con.InsertAsync(model);
        }

        // Busca simples por nome da cidade usando LIKE
        public Task<List<Tempo>> Search(string q)
        {
            string sql =
                $"SELECT * FROM Tempo WHERE Cidade LIKE '%{q}%'";

            return con.QueryAsync<Tempo>(sql);
        }

        // Busca registros filtrando por cidade e intervalo de datas
        public Task<List<Tempo>> SearchByPeriod(string cidade, DateTime inicio, DateTime fim)
        {
            string sql =
                "SELECT * FROM Tempo " +
                "WHERE Cidade LIKE ? " +              
                "AND DataConsulta BETWEEN ? AND ? " +  
                "ORDER BY DataConsulta DESC";          

            // Se cidade estiver vazia, usa "%" para retornar todas
            string filtroCidade = string.IsNullOrWhiteSpace(cidade)
                ? "%"
                : $"%{cidade}%";

            return con.QueryAsync<Tempo>(sql, filtroCidade, inicio, fim);
        }

    } // fim da classe
} // fim do namespace