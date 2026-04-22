using MiBusCR.Models;
using Supabase;

namespace MiBusCR;

public partial class Feedback : ContentPage
{
    private string _categoriaSeleccionada = string.Empty;
    private readonly string _rutaId;
    private readonly string _nombreRuta;

    public Feedback(string rutaId, string nombreRuta)
    {
        InitializeComponent();
        _rutaId = rutaId;
        _nombreRuta = nombreRuta;

        LabelNombreRuta.Text = nombreRuta;
        LabelDetalleNombreRuta.Text = nombreRuta;
        LabelDetalleFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        BorderDetalleRuta.IsVisible = true;
    }

    private void OnCategoriaInfoIncorrecta(object sender, TappedEventArgs e)
        => SeleccionarCategoria("informacion_incorrecta");

    private void OnCategoriaProblemaBus(object sender, TappedEventArgs e)
        => SeleccionarCategoria("problema_bus");

    private void OnCategoriaOtro(object sender, TappedEventArgs e)
        => SeleccionarCategoria("otro");

    private void SeleccionarCategoria(string categoria)
    {
        _categoriaSeleccionada = categoria;
        LabelCategoriaError.IsVisible = false;

        ResetCategoriaBorder(BorderInfoIncorrecta, CheckInfoIncorrecta);
        ResetCategoriaBorder(BorderProblemaBus, CheckProblemaBus);
        ResetCategoriaBorder(BorderOtro, CheckOtro);

        switch (categoria)
        {
            case "informacion_incorrecta":
                ActivarCategoriaBorder(BorderInfoIncorrecta, CheckInfoIncorrecta);
                break;
            case "problema_bus":
                ActivarCategoriaBorder(BorderProblemaBus, CheckProblemaBus);
                break;
            case "otro":
                ActivarCategoriaBorder(BorderOtro, CheckOtro);
                break;
        }
    }

    private static void ResetCategoriaBorder(Border border, Label check)
    {
        border.BackgroundColor = Color.FromArgb("#F9FAFB");
        border.Stroke = new SolidColorBrush(Color.FromArgb("#D1D5DB"));
        check.Text = "○";
        check.TextColor = Color.FromArgb("#D1D5DB");
    }

    private static void ActivarCategoriaBorder(Border border, Label check)
    {
        border.BackgroundColor = Color.FromArgb("#EFF6FF");
        border.Stroke = new SolidColorBrush(Color.FromArgb("#2563EB"));
        check.Text = "●";
        check.TextColor = Color.FromArgb("#2563EB");
    }

    private async void OnEnviarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_categoriaSeleccionada))
        {
            LabelCategoriaError.IsVisible = true;
            return;
        }

        BtnEnviar.IsEnabled = false;
        BtnEnviar.IsVisible = false;
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;

        try
        {
            var cliente = App.SupabaseClient;
            if (cliente == null)
            {
                var options = new SupabaseOptions { AutoConnectRealtime = false };
                cliente = new Supabase.Client(DatabaseConfig.SUPABASE_URL, DatabaseConfig.SUPABASE_KEY, options);
                await cliente.InitializeAsync();
            }

            var reporte = new ReporteUsuario
            {
                RutaId = _rutaId,
                TipoReporte = _categoriaSeleccionada,
                Comentario = EditorComentario.Text ?? string.Empty,
                Estado = "Pendiente"
            };

            await cliente.From<ReporteUsuario>().Insert(reporte);

            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;

            await DisplayAlert(
                "Reporte enviado",
                $"Tu reporte sobre \"{_nombreRuta}\" fue enviado exitosamente. Gracias por ayudarnos a mejorar el servicio.",
                "Aceptar"
            );

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
            BtnEnviar.IsVisible = true;
            BtnEnviar.IsEnabled = true;

            await DisplayAlert("Error", $"No se pudo enviar el reporte: {ex.Message}", "Aceptar");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}