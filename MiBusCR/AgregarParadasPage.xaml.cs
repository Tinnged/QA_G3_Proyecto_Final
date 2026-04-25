using MiBusCR.Models;
using Supabase;
using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;

namespace MiBusCR
{
    public partial class AgregarParadasPage : ContentPage
    {
        private const string EmpresaId = "c36b2779-a595-4e7f-bc63-ec8fcbc875d5";

        private Supabase.Client _supabase;
        private List<Ruta> _rutas = new();
        private ObservableCollection<ParadaViewModel> _paradasVista = new();

        public AgregarParadasPage()
        {
            InitializeComponent();
            ParadasCollectionView.ItemsSource = _paradasVista;
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

                await CargarRutasAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo inicializar: {ex.Message}", "OK");
            }
        }

        private async Task CargarRutasAsync()
        {
            try
            {
                var resultado = await _supabase
                    .From<Ruta>()
                    .Filter("empresa_id", Supabase.Postgrest.Constants.Operator.Equals, EmpresaId)
                    .Get();

                _rutas = resultado.Models;

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    RutaPicker.ItemsSource = null;
                    RutaPicker.ItemsSource = _rutas;
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudieron cargar las rutas: {ex.Message}", "OK");
            }
        }

        private async void OnRutaPickerChanged(object sender, EventArgs e)
        {
            if (RutaPicker.SelectedItem is not Ruta rutaSeleccionada) return;
            await CargarParadasDeRutaAsync(rutaSeleccionada.Id);
        }

        private async Task CargarParadasDeRutaAsync(string rutaId)
        {
            try
            {
                _paradasVista.Clear();

                var paradasRutaRes = await _supabase
                    .From<ParadaRuta>()
                    .Filter("ruta_id", Supabase.Postgrest.Constants.Operator.Equals, rutaId)
                    .Get();

                var paradasRuta = paradasRutaRes.Models;
                if (!paradasRuta.Any()) return;

                var paradasRes = await _supabase.From<Parada>().Get();
                var todasParadas = paradasRes.Models;

                var lista = paradasRuta
                    .OrderBy(pr => pr.OrdenParada)
                    .Select(pr =>
                    {
                        var detalle = todasParadas.FirstOrDefault(p => p.Id == pr.ParadaId);
                        return new ParadaViewModel
                        {
                            Nombre = detalle?.Nombre ?? "Sin nombre",
                            Detalle = $"Orden: {pr.OrdenParada} | Lat: {detalle?.Latitud} | Lon: {detalle?.Longitud}"
                        };
                    });

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach (var p in lista)
                        _paradasVista.Add(p);
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudieron cargar las paradas: {ex.Message}", "OK");
            }
        }

        private async void OnAgregarParadaClicked(object sender, EventArgs e)
        {
            if (RutaPicker.SelectedItem is not Ruta rutaSeleccionada)
            {
                MostrarMensajeParada("Seleccione una ruta primero.", "#DC2626");
                return;
            }

            string nombre = NombreParadaEntry.Text?.Trim();
            string ordenTexto = OrdenParadaEntry.Text?.Trim();
            string latitudTexto = LatitudParadaEntry.Text?.Trim();
            string longitudTexto = LongitudParadaEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(ordenTexto) ||
                string.IsNullOrWhiteSpace(latitudTexto) ||
                string.IsNullOrWhiteSpace(longitudTexto))
            {
                MostrarMensajeParada("Todos los campos son obligatorios.", "#DC2626");
                return;
            }

            if (!int.TryParse(ordenTexto, out int orden))
            {
                MostrarMensajeParada("El orden debe ser un número entero.", "#DC2626");
                return;
            }

            if (!double.TryParse(latitudTexto, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out double latitud) ||
                !double.TryParse(longitudTexto, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out double longitud))
            {
                MostrarMensajeParada("Latitud y longitud inválidas.", "#DC2626");
                return;
            }

            try
            {
                // Desplazar paradas existentes con orden >= al nuevo
                var paradasExistentes = await _supabase
                    .From<ParadaRuta>()
                    .Filter("ruta_id", Supabase.Postgrest.Constants.Operator.Equals, rutaSeleccionada.Id)
                    .Filter("orden_parada", Supabase.Postgrest.Constants.Operator.GreaterThanOrEqual, orden.ToString())
                    .Get();

                foreach (var pr in paradasExistentes.Models)
                {
                    pr.OrdenParada += 1;
                    await _supabase.From<ParadaRuta>().Update(pr);
                }

                // Insert parada
                var nuevaParada = new Parada
                {
                    Nombre = nombre,
                    Latitud = latitud,
                    Longitud = longitud
                };

                var paradaInsertada = await _supabase.From<Parada>().Insert(nuevaParada);
                var paradaId = paradaInsertada.Models.First().Id;

                // Insert parada_ruta
                var paradaRuta = new ParadaRuta
                {
                    RutaId = rutaSeleccionada.Id,
                    ParadaId = paradaId,
                    OrdenParada = orden
                };

                await _supabase.From<ParadaRuta>().Insert(paradaRuta);

                MostrarMensajeParada("Parada agregada correctamente.", "#16A34A");
                await DisplayAlert("Éxito", "Parada agregada correctamente.", "OK");
                LimpiarCamposParada();
                await CargarParadasDeRutaAsync(rutaSeleccionada.Id);
            }
            catch (Exception ex)
            {
                MostrarMensajeParada("Error al agregar parada.", "#DC2626");
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void MostrarMensajeParada(string mensaje, string colorHex)
        {
            MensajeParadaLabel.Text = mensaje;
            MensajeParadaLabel.TextColor = Color.FromArgb(colorHex);
            MensajeParadaLabel.IsVisible = true;
        }

        private void LimpiarCamposParada()
        {
            NombreParadaEntry.Text = "";
            OrdenParadaEntry.Text = "";
            LatitudParadaEntry.Text = "";
            LongitudParadaEntry.Text = "";
        }
    }

    public class ParadaViewModel
    {
        public string Nombre { get; set; }
        public string Detalle { get; set; }
    }
}