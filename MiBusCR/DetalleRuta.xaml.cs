namespace MiBusCR;

public partial class DetalleRuta : ContentPage
{

    private readonly string _rutaId;
    private readonly string _nombreRuta;

    public DetalleRuta(string rutaId, string nombreRuta)
    {
        InitializeComponent();
        _rutaId = rutaId;
        _nombreRuta = nombreRuta;
        LblNombreRuta.Text = nombreRuta;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnTarifasClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new TarifasMetodosPago(_rutaId, _nombreRuta));
    }

    private async void OnReportarClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new Feedback(_rutaId, _nombreRuta));
    }

    private async void OnVerParadasClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new InformacionRutasV2(_rutaId, _nombreRuta));
    }

}