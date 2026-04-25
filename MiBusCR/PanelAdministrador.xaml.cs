using MiBusCR.Models;
using Supabase;
using Microsoft.Maui.Graphics;

namespace MiBusCR
{
    public partial class PanelAdministrador : ContentPage
    {
        private const string EmpresaId = "c36b2779-a595-4e7f-bc63-ec8fcbc875d5";
        private Supabase.Client _supabase;

        public PanelAdministrador()
        {
            InitializeComponent();
            _ = InicializarAsync();
        }

        private async Task InicializarAsync()
        {
            try
            {
                _supabase = App.SupabaseClient;
                if (_supabase == null)
                {
                    var options = new SupabaseOptions { AutoConnectRealtime = true };
                    _supabase = new Supabase.Client(
                        DatabaseConfig.SUPABASE_URL,
                        DatabaseConfig.SUPABASE_KEY,
                        options);
                    await _supabase.InitializeAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo inicializar: {ex.Message}", "OK");
            }
        }

        private async void OnGuardarRutaClicked(object sender, EventArgs e)
        {
            string nombre = NombreRutaEntry.Text?.Trim();
            string provinciaInicio = ProvinciaInicioEntry.Text?.Trim();
            string provinciaFinal = ProvinciaFinalEntry.Text?.Trim();
            string destino = DestinoEntry.Text?.Trim();
            string montoTexto = MontoTarifaEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(provinciaInicio) ||
                string.IsNullOrWhiteSpace(provinciaFinal) ||
                string.IsNullOrWhiteSpace(destino) ||
                string.IsNullOrWhiteSpace(montoTexto))
            {
                MostrarMensaje("Todos los campos son obligatorios.", "#DC2626");
                return;
            }

            if (!decimal.TryParse(montoTexto, out decimal monto))
            {
                MostrarMensaje("El monto de la tarifa debe ser un número válido.", "#DC2626");
                return;
            }

            try
            {
                var nuevaRuta = new Ruta
                {
                    Nombre = nombre,
                    ProvinciaInicio = provinciaInicio,
                    ProvinciaFinal = provinciaFinal,
                    Destino = destino,
                    EstaActiva = true,
                    EmpresaId = EmpresaId,
                    FechaCreacion = DateTime.UtcNow
                };

                var rutaInsertada = await _supabase.From<Ruta>().Insert(nuevaRuta);
                var rutaId = rutaInsertada.Models.First().Id;

                var tarifa = new TarifaRuta
                {
                    RutaId = rutaId,
                    MontoTarifa = monto,
                    Moneda = "CRC",
                    AceptaEfectivo = CheckEfectivo.IsChecked,
                    AceptaTarjeta = CheckTarjeta.IsChecked,
                    AceptaSinpe = CheckSinpe.IsChecked
                };

                await _supabase.From<TarifaRuta>().Insert(tarifa);

                MostrarMensaje("Ruta creada correctamente.", "#16A34A");
                await DisplayAlert("Éxito", "La ruta se registró correctamente.", "OK");
                LimpiarCamposRuta();
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al guardar.", "#DC2626");
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void MostrarMensaje(string mensaje, string colorHex)
        {
            MensajeLabel.Text = mensaje;
            MensajeLabel.TextColor = Color.FromArgb(colorHex);
            MensajeLabel.IsVisible = true;
        }

        private void LimpiarCamposRuta()
        {
            NombreRutaEntry.Text = "";
            ProvinciaInicioEntry.Text = "";
            ProvinciaFinalEntry.Text = "";
            DestinoEntry.Text = "";
            MontoTarifaEntry.Text = "";
            CheckEfectivo.IsChecked = false;
            CheckTarjeta.IsChecked = false;
            CheckSinpe.IsChecked = false;
        }
    }
}