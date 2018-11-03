using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using DemoIoTHub.Modelos;

namespace DemoIoTHub.Servicios
{
    public static class ServicioUbicacion
    {
        public static async Task<Ubicacion> ObtenerUbicacionActual()
        {
            Position ubicacion = null;

            try
            {
                var gps = CrossGeolocator.Current;
                gps.DesiredAccuracy = 100;

                if (!gps.IsGeolocationAvailable || !gps.IsGeolocationEnabled)
                    return null;

                ubicacion = await gps.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);
            }
            catch (Exception ex) { }

            if (ubicacion != null)
            {
                return new Ubicacion()
                {
                    Latitud = ubicacion.Latitude,
                    Longitud = ubicacion.Longitude
                };
            }

            return null;
        }
    }
}
