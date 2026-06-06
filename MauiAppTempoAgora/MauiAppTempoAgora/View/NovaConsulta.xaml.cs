using MauiAppTempoAgora.ViewModels;

namespace MauiAppTempoAgora.View;

public partial class NovaConsulta : ContentPage
{
    public NovaConsulta()
    {
        InitializeComponent();
        BindingContext = new NovaConsultaViewModel(); // Liga a View ao ViewModel (MVVM)
    }
}