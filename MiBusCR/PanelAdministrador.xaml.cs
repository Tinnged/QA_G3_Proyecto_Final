using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace MiBusCR
{
    public partial class PanelAdministrador : ContentPage
    {
        private ObservableCollection<ParadaAdminLocal> paradas =
            new ObservableCollection<ParadaAdminLocal>();

        private ParadaAdminLocal paradaSeleccionada;

        public PanelAdministrador()
        {
            InitializeComponent();

            // lista visual de paradas
            ParadasCollectionView.ItemsSource = paradas;

        }

        private async void OnGuardarRutaClicked(object sender, EventArgs e)
        {
            string nombreRuta = NombreRutaEntry.Text?.Trim();
            string provincia = ProvinciaEntry.Text?.Trim();
            string precioTexto = PrecioEntry.Text?.Trim();
            string latitudOrigenTexto = LatitudOrigenEntry.Text?.Trim();
            string longitudOrigenTexto = LongitudOrigenEntry?.Text?.Trim();
            string latitudDestinoTexto = LatitudDestinoEntry.Text?.Trim();
            string longitudDestinoTexto = LongitudDestinoEntry?.Text?.Trim();

            if (string.IsNullOrWhiteSpace(nombreRuta) ||
                string.IsNullOrWhiteSpace(provincia) ||
                string.IsNullOrWhiteSpace(precioTexto) ||
                string.IsNullOrWhiteSpace(latitudOrigenTexto) ||
                string.IsNullOrWhiteSpace(longitudOrigenTexto) ||
                string.IsNullOrWhiteSpace(latitudDestinoTexto) ||
                String.IsNullOrWhiteSpace(longitudDestinoTexto))

            {
                MostrarMensaje("Todos los campos son obligatorios.", "#DC2626");
                return;
            }

            MostrarMensaje("Ruta creada correctamente.", "#16A34A");

            await DisplayAlert("Exito", "La ruta se registro correctamente.", "OK");

            LimpiarCampos();

        }

        private void LimpiarCampos()
        {
            NombreRutaEntry.Text = "";
            ProvinciaEntry.Text = "";
            PrecioEntry.Text = "";
            LatitudOrigenEntry.Text = "";
            LongitudOrigenEntry.Text = "";
            LatitudDestinoEntry.Text = "";
            LongitudDestinoEntry.Text = "";

        }

        private void MostrarMensaje(string mensaje, string colorHex)
        {
            MensajeLabel.Text = mensaje;
            MensajeLabel.TextColor = Color.FromRgba(colorHex);
            MensajeLabel.IsVisible = true;

        }

        private async void OnAgregarParadaClicked(object sender, EventArgs e)
        {

            string ruta = RutaParadaEntry.Text?.Trim();
            string nombre = NombreParadaEntry.Text?.Trim();
            string ordenTexto = OrdenParadaEntry.Text?.Trim();
            string latitudTexto = LatitudParadaEntry.Text?.Trim();
            string longitudTexto = LongitudParadaEntry?.Text?.Trim();

            if (string.IsNullOrWhiteSpace(ruta) ||
                string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(ordenTexto) ||
                string.IsNullOrWhiteSpace(latitudTexto) ||
                string.IsNullOrWhiteSpace(longitudTexto))
            {
                MostrarMensajeParada
                    ("Todos los campos de la parada son ablogatorios.", "#DC2626");
                return;

            }

            if (!int.TryParse(ordenTexto, out int orden)) 

            {

                MostrarMensajeParada
                    ("El orden debe ser numero entero.", "#DC2626");
                return;
            }

            if (!double.TryParse(latitudTexto, out double latitud) ||
                !double.TryParse(longitudTexto, out double longitud))
            {

                MostrarMensajeParada
                    ("Latitud y longitud invalidas.", "#DC2626");
                return; 
            }

            var nuevaParada = new ParadaAdminLocal
            {
                RutaAsociada = ruta,
                Nombre = nombre,
                Orden = orden,
                Latitud = latitud,
                Longitud = longitud,
            };

            paradas.Add(nuevaParada);

            MostrarMensajeParada
                ("Parada agregada correctamente.", "#16A34A");

            await DisplayAlert
                ("Exito", "Parada agregada correctamente.", "OK");

            LimpiarCamposParada();

        }

        private void OnParadaSeleccionada(object sender, SelectionChangedEventArgs e)

        {
            paradaSeleccionada = e.CurrentSelection.FirstOrDefault() as ParadaAdminLocal;

            if (paradaSeleccionada != null )
            {
                RutaParadaEntry.Text = paradaSeleccionada.RutaAsociada;

                NombreParadaEntry.Text = paradaSeleccionada.Nombre;

                OrdenParadaEntry.Text = paradaSeleccionada.Orden.ToString();

                LatitudParadaEntry.Text = paradaSeleccionada.Latitud.ToString();

                LongitudParadaEntry.Text = paradaSeleccionada.Longitud.ToString();
            }

        }

        private async void OnActualizarParadaClicked(object sender, EventArgs e)
        {
            if (paradaSeleccionada == null)

            {
                MostrarMensajeParada
                    ("Seleccione una parada.", "#DC2626");
                return;

            }
            
            paradaSeleccionada.RutaAsociada = RutaParadaEntry.Text;

            paradaSeleccionada.Nombre = NombreParadaEntry.Text;

            paradaSeleccionada.Orden = int.Parse(OrdenParadaEntry.Text);

            paradaSeleccionada.Latitud = double.Parse(LatitudParadaEntry.Text);

            paradaSeleccionada.Longitud = double.Parse (LongitudParadaEntry.Text);

            ParadasCollectionView.ItemsSource = null;
            ParadasCollectionView.ItemsSource = paradas;

            MostrarMensajeParada
                ("Parada actualizada.", "#16A34A");

            await DisplayAlert("Exito", "Parada actualizada correctamente.", "OK");

            LimpiarCamposParada();
            paradaSeleccionada = null;

        }

        private void MostrarMensajeParada(string mensaje, string colorHex)

        {
            MensajeParadaLabel.Text = mensaje;
            MensajeParadaLabel.TextColor = Color.FromArgb(colorHex);
            MensajeParadaLabel.IsVisible = true;

        }

        private void OnEditarParadaClicked(object sender, EventArgs e)

        {

            if (sender is Button boton && boton.BindingContext is ParadaAdminLocal parada)
            {

                paradaSeleccionada = parada;

                RutaParadaEntry.Text = paradaSeleccionada.RutaAsociada;
                NombreParadaEntry.Text = paradaSeleccionada.Nombre;
                OrdenParadaEntry.Text = paradaSeleccionada.Orden.ToString();
                LatitudParadaEntry.Text = paradaSeleccionada.Latitud.ToString();
                LongitudParadaEntry.Text = paradaSeleccionada.Longitud.ToString();

                MostrarMensajeParada("Parada cargada para editar.", "#16A34A");

            }
        }

        private void LimpiarCamposParada()

        {
            RutaParadaEntry.Text = "";
            NombreParadaEntry.Text = "";
            OrdenParadaEntry.Text = "";
            LatitudParadaEntry.Text = "";
            LongitudParadaEntry.Text = "";

        }

    }


    public class ParadaAdminLocal
    {
        public string RutaAsociada {  get; set; }
        public string Nombre {  get; set; }
        public int Orden { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }

        public string Detalle =>
            $"Orden: {Orden} | Lat: {Latitud} | Lon: {Longitud}";

    }



}
