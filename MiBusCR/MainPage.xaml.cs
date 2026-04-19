using MiBusCR.Models;
using Supabase;

namespace MiBusCR
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

      

        public MainPage()
        {
            InitializeComponent();
            _ = VerificarConexionSupabase();
        }


        private async Task VerificarConexionSupabase()
        {
            try
            {
                // 1. Inicializar base de datos de Supabase
                var options = new SupabaseOptions { AutoConnectRealtime = true };
                var client = new Supabase.Client(
                    DatabaseConfig.SUPABASE_URL,
                    DatabaseConfig.SUPABASE_KEY,
                    options
                );

                await client.InitializeAsync();

                // 2. Una prueba de consulta pa ver que todo esta bien 
                var resultado = await client
                    .From<UsuarioPrueba>()
                    .Where(x => x.Id == 5)
                    .Get();

                var usuarioEncontrado = resultado.Models.FirstOrDefault();

                // 3. Actualización de la interfaz
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (usuarioEncontrado != null)
                    {

                        PruebaZe.Text = "la base de datos esta conectada :)";
                    }
                    else
                    {
                        PruebaZe.Text = "Conexión exitosa, pero no se pudieron traer los datos de prueba.";
                    }
                });
            }
            catch (Exception ex)
            {
                // En caso de error (red, llaves incorrectas, etc.)
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    PruebaZe.Text = "Error de conexión :(";
                    await DisplayAlert("Detalle del Error", ex.Message, "Entendido");
                });
            }
        }

        private async void OnVerRutasClicked(object sender, EventArgs e)
        {
            // Navigation.PushAsync "empuja" la nueva pantalla sobre la actual
            await Navigation.PushAsync(new InformacionRutasV2());
        }


    }


    }

