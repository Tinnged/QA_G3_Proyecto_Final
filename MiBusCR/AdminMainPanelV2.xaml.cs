using MiBusCR.Models;
using Supabase;
using Supabase.Postgrest.Models;
using System.Collections.ObjectModel;


namespace MiBusCR;


public partial class AdminMainPanelV2 : ContentPage
{

    private const string AdminId = "6b11d06a-cddb-490c-8db1-19bef0e61426";

    public AdminMainPanelV2()
	{
		InitializeComponent();

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarDatosAdminAsync();
    }

    private async Task CargarDatosAdminAsync()
    {
        try
        {
            var supabase = App.SupabaseClient;
            if (supabase == null)
            {
                var options = new SupabaseOptions { AutoConnectRealtime = true };
                supabase = new Supabase.Client(DatabaseConfig.SUPABASE_URL, DatabaseConfig.SUPABASE_KEY, options);
                await supabase.InitializeAsync();
            }

            var adminResponse = await supabase
                .From<Administrador>()
                .Filter("id", Supabase.Postgrest.Constants.Operator.Equals, AdminId)
                .Single();

            if (adminResponse == null) return;

            LblNombreAdmin.Text = adminResponse.NombreCompleto;

            var empresaResponse = await supabase
                .From<Empresa>()
                .Filter("id", Supabase.Postgrest.Constants.Operator.Equals, adminResponse.EmpresaId)
                .Single();

            if (empresaResponse != null)
            {
                LblNombreEmpresa.Text = empresaResponse.NombreEmpresa;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar los datos: {ex.Message}", "OK");
        }
    }

    private void OnBackClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void OnFeedbackClicked(object sender, TappedEventArgs e)
    {
         await Navigation.PushAsync(new FeedbackAdmin());

    }

    private async void OnCrearRutaClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new PanelAdministrador());
    }

    private async void OnParadasClicked(object sender, TappedEventArgs e)
    {
        // await Navigation.PushAsync(new ParadasPage());
        await Navigation.PushAsync(new AgregarParadasPage());
    }

  





}