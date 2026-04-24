using MiBusCR.Models;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Collections.ObjectModel;
using MiBusCR.Models;

namespace MiBusCR
{
    public partial class MainPage : ContentPage
    {

        private List<Ruta> _rutasMaestras = new List<Ruta>();
        public ObservableCollection<Ruta> RutasAMostrar { get; set; } = new ObservableCollection<Ruta>();



        public MainPage()
        {
            InitializeComponent();
            _ = VerificarConexionSupabase();
            _ = InicializarTodasLasVaras();
            this.BindingContext = this;
        }

        private async Task InicializarTodasLasVaras()
        {
            await VerificarConexionSupabase();
            await CargarRutasDesdeSupabaseAsync();
        }

        private async Task VerificarConexionSupabase()
        {
            try
            {
                var options = new SupabaseOptions { AutoConnectRealtime = true };
                var client = new Supabase.Client(
                    DatabaseConfig.SUPABASE_URL,
                    DatabaseConfig.SUPABASE_KEY,
                    options
                );

                await client.InitializeAsync();

                // Una prueba de consulta pa ver que todo esta bien 
                var resultado = await client
                    .From<UsuarioPrueba>()
                    .Where(x => x.Id == 5)
                    .Get();

                var usuarioEncontrado = resultado.Models.FirstOrDefault();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (usuarioEncontrado != null)
                    {

                        PruebaZe.Text = "Siuuu base de datos conectada :)";
                    }
                    else
                    {
                        PruebaZe.Text = "Se logro conectar pero, nada de data";
                    }
                });
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    PruebaZe.Text = "Error de conexión :(";
                    await DisplayAlert("Detalle del Error", ex.Message, "Entendido");
                });
            }
        }
        private async Task CargarRutasDesdeSupabaseAsync()
        {
            try
            {
                var cliente = App.SupabaseClient;
                if (cliente == null)
                {

                    var options = new SupabaseOptions { AutoConnectRealtime = true };
                    cliente = new Supabase.Client(DatabaseConfig.SUPABASE_URL, DatabaseConfig.SUPABASE_KEY, options);
                    await cliente.InitializeAsync();
                }

                var resultadoRutas = await cliente.From<Ruta>().Get();
                var rutas = resultadoRutas.Models;

                if (rutas == null || !rutas.Any()) return;

                var resultadoTarifas = await cliente.From<TarifaRuta>().Get();
                var tarifas = resultadoTarifas.Models;

                foreach (var ruta in rutas)
                {
                    if (tarifas != null)
                    {
                        var tarifa = tarifas.FirstOrDefault(t => t.RutaId == ruta.Id);
                        if (tarifa != null)
                        {
                            ruta.MontoTarifa = tarifa.MontoTarifa;
                            ruta.AceptaEfectivo = tarifa.AceptaEfectivo;
                            ruta.AceptaTarjeta = tarifa.AceptaTarjeta;
                            ruta.AceptaSinpe = tarifa.AceptaSinpe;
                        }
                    }
                }

                _rutasMaestras = rutas;
                ActualizarLista(_rutasMaestras);
            }
            catch (Exception ex)
            {

                await DisplayAlert("Error de Datos", $"Detalle: {ex.Message}", "OK");
            }
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            var textoBusqueda = e.NewTextValue?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(textoBusqueda))
            {
                ActualizarLista(_rutasMaestras);
                return;
            }

            var filtradas = _rutasMaestras.Where(r =>
                r.Nombre.ToLower().Contains(textoBusqueda) ||
                r.ProvinciaInicio.ToLower().Contains(textoBusqueda) ||
                r.ProvinciaFinal.ToLower().Contains(textoBusqueda)
            ).ToList();

            ActualizarLista(filtradas);
        }

        private void ActualizarLista(List<Ruta> rutas)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                RutasAMostrar.Clear();
                foreach (var ruta in rutas)
                {
                    RutasAMostrar.Add(ruta);
                }
                PruebaZe.Text = $"Rutas cargadas: {RutasAMostrar.Count}";
            });
        }

        //   private async void OnVerRutasClicked(object sender, EventArgs e)
        //   {
        //    var boton = (Button)sender;
        //     var rutaSeleccionada = (Ruta)boton.BindingContext;
        //
        //   if (rutaSeleccionada != null && !string.IsNullOrEmpty(rutaSeleccionada.Id))
        //    {
        //       await Navigation.PushAsync(new DetalleRuta(rutaSeleccionada.Id, rutaSeleccionada.Nombre));
        //  }
        //  else
        // {
        //      await DisplayAlert("Error", "No se pudo obtener el ID de la ruta.", "OK");
        //  }
        //  }

        private async void OnVerRutasClicked(object sender, EventArgs e)
        {
            var boton = (Button)sender;
            var rutaSeleccionada = (Ruta)boton.BindingContext;

            if (rutaSeleccionada != null && !string.IsNullOrEmpty(rutaSeleccionada.Id))
            {
                await Navigation.PushAsync(new DetalleRuta(rutaSeleccionada.Id, rutaSeleccionada.Nombre));
            }
            else
            {
                await DisplayAlert("Error", "No se pudo obtener el ID de la ruta.", "OK");
            }
        }

        private async void OnAdminClicked(object sender, EventArgs e)
        {
            // await Navigation.PushAsync(new AdminPage());
          await Navigation.PushAsync(new PanelAdministrador());
        }


    }


    }

