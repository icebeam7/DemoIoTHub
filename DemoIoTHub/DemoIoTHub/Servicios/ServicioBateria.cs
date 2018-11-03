using DemoIoTHub.Modelos;
using Plugin.Battery;

namespace DemoIoTHub.Servicios
{
    public static class ServicioBateria
    {
        public static Bateria ObtenerBateriaActual()
        {
            var info = CrossBattery.Current;

            return new Bateria()
            {
                Porcentaje = info.RemainingChargePercent,
                Status = info.Status.ToString(),
                Fuente = info.PowerSource.ToString()
            };
        }
    }
}
