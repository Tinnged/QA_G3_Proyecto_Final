using MiBusCR.Models;
using Supabase;
using System.Collections.ObjectModel;

namespace MiBusCR;

public partial class InformacionRutasV2 : ContentPage
{
    public ObservableCollection<ParadaRuta> ListaParadas { get; set; } = new ObservableCollection<ParadaRuta>();
    private readonly string _rutaId;
    private readonly string _nombreRuta;

    public InformacionRutasV2(string rutaId, string nombreRuta)
    {
        InitializeComponent();
        _rutaId = rutaId;
        _nombreRuta = nombreRuta;
        LblNombreRuta.Text = nombreRuta;
        this.BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarDetalleParadasAsync();
    }

    private async Task CargarDetalleParadasAsync()
    {
        MostrarCargando(true);

        try
        {
            var cliente = App.SupabaseClient;
            if (cliente == null)
            {
                var options = new SupabaseOptions { AutoConnectRealtime = true };
                cliente = new Supabase.Client(DatabaseConfig.SUPABASE_URL, DatabaseConfig.SUPABASE_KEY, options);
                await cliente.InitializeAsync();
            }

            var resultado = await cliente
                .From<ParadaRuta>()
                .Select("*, DetalleParada:paradas(*)")
                .Where(x => x.RutaId == _rutaId)
                .Order(x => x.OrdenParada, Supabase.Postgrest.Constants.Ordering.Ascending)
                .Get();

            if (resultado?.Models == null)
            {
                MostrarError("No se encontraron paradas para esta ruta.");
                return;
            }

            var paradasObtenidas = resultado.Models;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                ListaParadas.Clear();
                for (int i = 0; i < paradasObtenidas.Count; i++)
                {
                    paradasObtenidas[i].NoEsLaPrimera = i > 0;
                    paradasObtenidas[i].NoEsLaUltima = i < paradasObtenidas.Count - 1;
                    ListaParadas.Add(paradasObtenidas[i]);
                }

                MostrarCargando(false);
                BorderParadas.IsVisible = true;
                LabelTotalParadas.Text = $"{paradasObtenidas.Count} paradas en total";
            });
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(() => MostrarError(ex.Message));
        }
    }

    private void MostrarCargando(bool cargando)
    {
        LoadingIndicator.IsRunning = cargando;
        LoadingIndicator.IsVisible = cargando;
        BorderError.IsVisible = false;
        BorderParadas.IsVisible = false;
    }

    private void MostrarError(string mensaje)
    {
        LoadingIndicator.IsRunning = false;
        LoadingIndicator.IsVisible = false;
        BorderError.IsVisible = true;
        LabelErrorMsg.Text = mensaje;
        BorderParadas.IsVisible = false;
    }

    // ? Este era el método que faltaba
    private async void OnReintentarClicked(object sender, EventArgs e)
    {
        await CargarDetalleParadasAsync();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnVerEnMapaClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        var paradaRuta = button?.BindingContext as ParadaRuta;

        if (paradaRuta?.DetalleParada != null)
        {
            try
            {
                var location = new Location(paradaRuta.DetalleParada.Latitud, paradaRuta.DetalleParada.Longitud);
                var options = new MapLaunchOptions { Name = paradaRuta.DetalleParada.Nombre };
                await Map.Default.OpenAsync(location, options);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "No se pudo abrir el mapa: " + ex.Message, "OK");
            }
        }
    }
}