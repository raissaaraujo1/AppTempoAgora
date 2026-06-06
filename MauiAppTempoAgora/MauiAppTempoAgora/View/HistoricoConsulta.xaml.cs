using MauiAppTempoAgora.ViewModels;

namespace MauiAppTempoAgora.View;

public partial class HistoricoConsulta : ContentPage
{
    public HistoricoConsulta()
    {
        InitializeComponent(); 

        BindingContext = new HistoricoConsultaViewModel(); // Liga a View ao ViewModel (MVVM)
    }

    // Sempre que a página aparece, recarrega os dados do histórico
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is HistoricoConsultaViewModel vm)
            vm.CarregarTodos.Execute(null);
    }
}