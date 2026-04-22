using MiBusCR.Models;
using Supabase;

namespace MiBusCR;

public partial class TarifasMetodosPago : ContentPage
{
    private readonly string? _rutaId;
    private readonly string? _nombreRuta;

    public TarifasMetodosPago(string? rutaId = null, string? nombreRuta = null)
    {
        InitializeComponent();
        _rutaId = rutaId;
        _nombreRuta = nombreRuta;
        _ = CargarTarifa();
    }

    private async Task CargarTarifa()
    {
        MostrarCargando(true);

        try
        {
            var cliente = App.SupabaseClient;
            if (cliente == null)
            {
                var options = new SupabaseOptions { AutoConnectRealtime = false };
                cliente = new Supabase.Client(DatabaseConfig.SUPABASE_URL, DatabaseConfig.SUPABASE_KEY, options);
                await cliente.InitializeAsync();
            }

            TarifaRuta? tarifa = null;

            if (!string.IsNullOrEmpty(_rutaId))
            {
                var resultado = await cliente
                    .From<TarifaRuta>()
                    .Where(x => x.RutaId == _rutaId)
                    .Get();
                tarifa = resultado.Models.FirstOrDefault();
            }
            else
            {
                var resultado = await cliente
                    .From<TarifaRuta>()
                    .Limit(1)
                    .Get();
                tarifa = resultado.Models.FirstOrDefault();
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                MostrarCargando(false);

                if (tarifa != null)
                    MostrarTarifa(tarifa);
                else
                    MostrarError("No se encontró información de tarifa para esta ruta.");
            });
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                MostrarCargando(false);
                MostrarError(ex.Message);
            });
        }
    }

    private void MostrarCargando(bool cargando)
    {
        LoadingIndicator.IsRunning = cargando;
        LoadingIndicator.IsVisible = cargando;
        BorderError.IsVisible = false;
    }

    private void MostrarError(string mensaje)
    {
        BorderError.IsVisible = true;
        LabelErrorMsg.Text = mensaje;
        BorderTarifa.IsVisible = false;
        BorderMetodosPago.IsVisible = false;
    }

    private void MostrarTarifa(TarifaRuta tarifa)
    {
        LabelNombreRuta.Text = _nombreRuta ?? "Ruta";

        string moneda = tarifa.Moneda ?? "CRC";
        string simbolo = moneda == "CRC" ? "?" : "$";
        LabelMonto.Text = $"{simbolo}{tarifa.MontoTarifa:N0}";
        LabelMoneda.Text = $"Colones costarricenses ({moneda})";

        BorderTarifa.IsVisible = true;
        BorderTiposPasajero.IsVisible = true;

        bool algunMetodo = false;

        if (tarifa.AceptaEfectivo) { BorderEfectivo.IsVisible = true; algunMetodo = true; }
        if (tarifa.AceptaTarjeta) { BorderTarjeta.IsVisible = true; algunMetodo = true; }
        if (tarifa.AceptaSinpe) { BorderSinpe.IsVisible = true; algunMetodo = true; }

        LabelSinMetodos.IsVisible = !algunMetodo;
        BorderMetodosPago.IsVisible = true;

        LabelActualizacion.Text = $"Información actualizada el {DateTime.Now:dd/MM/yyyy}";
        LabelActualizacion.IsVisible = true;
    }

    private async void OnReintentarClicked(object sender, EventArgs e)
    {
        await CargarTarifa();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}