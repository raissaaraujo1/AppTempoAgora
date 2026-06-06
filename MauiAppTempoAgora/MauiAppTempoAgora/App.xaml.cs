using MauiAppTempoAgora.Helpers;

namespace MauiAppTempoAgora
{
    public partial class App : Application
    {
        // Instância única do banco SQLite usada em toda a aplicação
        static SQLiteDatabaseHelper? database;

        public static SQLiteDatabaseHelper? Database
        {
            get
            {
                // Cria a instância do banco se ainda não existir
                if (database == null)
                {
                    // Pasta de dados da aplicação 
                    var local_instalacao = Environment.SpecialFolder.ApplicationData;

                    // Caminho completo para a pasta de dados
                    var caminho_instalacao = Environment.GetFolderPath(local_instalacao);

                    // Arquivo SQLite dentro da pasta de dados
                    string arquivo_sqlite = Path.Combine(caminho_instalacao, "tempo.db3");

                    // Inicializa o helper do SQLite, que cria a tabela se necessário
                    database = new SQLiteDatabaseHelper(arquivo_sqlite);
                }

                return database;
            }
        }

        public App()
        {
            InitializeComponent(); 
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var w = new Window(new AppShell());

            // Define tamanho padrão da janela 
            w.Height = 1000;
            w.Width = 500;

            return w;
        }
    }
}