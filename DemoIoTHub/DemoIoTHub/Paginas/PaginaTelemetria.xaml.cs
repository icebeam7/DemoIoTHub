using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using DemoIoTHub.Modelos;
using DemoIoTHub.Servicios;

namespace DemoIoTHub.Paginas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaginaTelemetria : ContentPage
	{
        ServicioIoTHub hub;

		public PaginaTelemetria ()
		{
			InitializeComponent ();

            hub = new ServicioIoTHub();
		}

        public async void RegistrarDispositivo_Clicked(object sender, EventArgs e)
        {
            await hub.RegistrarDispositivo();

            if (hub.Device != null)
            {
                await DisplayAlert("IoTHub", "Dispositivo registrado", "OK");
                RegistrarDispositivo.IsEnabled = false;
                ConectarDispositivo.IsEnabled = true;
            }
        }

        public async void ConectarDispositivo_Clicked(object sender, EventArgs e)
        {
            await hub.ConectarHub();

            if (hub.Client != null)
            {
                await DisplayAlert("IoTHub", "Dispositivo conectado", "OK");
                ConectarDispositivo.IsEnabled = false;
                EnviarTelemetria.IsEnabled = true;
            }
        }

        public async void EnviarTelemetria_Clicked(object sender, EventArgs e)
        {
            EnviarTelemetria.IsEnabled = false;

            for (int i = 1; i < 6; i++)
            {
                var bateria = ServicioBateria.ObtenerBateriaActual();
                var ubicacion = await ServicioUbicacion.ObtenerUbicacionActual();

                if (bateria != null)
                {
                    Bateria.Text = bateria.Porcentaje.ToString();
                    Status.Text = bateria.Status;
                    Fuente.Text = bateria.Fuente;
                }

                if (ubicacion != null)
                {
                    Latitud.Text = ubicacion.Latitud.ToString();
                    Longitud.Text = ubicacion.Longitud.ToString();
                }

                if (bateria != null && ubicacion != null)
                {
                    var telemetria = new Telemetria()
                    {
                        Bateria = bateria,
                        Ubicacion = ubicacion,
                        Fecha = DateTime.UtcNow
                    };

                    EnviarTelemetria.Text = $"Enviando dato {i}...";
                    await hub.EnviarTelemetria(telemetria);
                    EnviarTelemetria.Text = $"Dato {i} enviado!!";
                }

                await Task.Delay(15000);
            }

            await DisplayAlert("IoTHub", "Mensajes enviados al hub", "OK");
            EnviarTelemetria.IsEnabled = true;
        }
    }
}