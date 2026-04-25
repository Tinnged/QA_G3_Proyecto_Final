using MiBusCR.Models;
using Supabase;

namespace MiBusCR
{
    public partial class FeedbackAdmin : ContentPage
    {
        public FeedbackAdmin()
        {
            InitializeComponent();
        }

        private void OnBackClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void OnCargarFeedbackClicked(object sender, EventArgs e)
        {
            try
            {
                var cliente = App.SupabaseClient;
                if (cliente == null)
                {
                    var options = new SupabaseOptions { AutoConnectRealtime = false };
                    cliente = new Supabase.Client(
                        DatabaseConfig.SUPABASE_URL,
                        DatabaseConfig.SUPABASE_KEY,
                        options);
                    await cliente.InitializeAsync();
                }

                var respuesta = await cliente.From<Models.FeedbackAdmin>().Get();
                FeedbackCollectionView.ItemsSource = respuesta.Models;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo cargar el feedback: {ex.Message}", "Aceptar");
            }
        }
    }
}