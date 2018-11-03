using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Common.Exceptions;
using Newtonsoft.Json;
using DemoIoTHub.Helpers;
using DemoIoTHub.Modelos;

namespace DemoIoTHub.Servicios
{
    public class ServicioIoTHub
    {
        private RegistryManager rm;
        public Device Device { get; set; }
        public DeviceClient Client { get; set; }

        public ServicioIoTHub()
        {
            rm = RegistryManager.CreateFromConnectionString(Constantes.IoTHubConnectionString);
        }

        public async Task RegistrarDispositivo()
        {
            try
            {
                Device = await rm.AddDeviceAsync(new Device(Constantes.DeviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                Device = await rm.GetDeviceAsync(Constantes.DeviceId);
            }
            catch (Exception ex) { }
        }

        public async Task ConectarHub()
        {
            if (Device != null)
            {
                var key = Device.Authentication.SymmetricKey.PrimaryKey;
                var auth = new DeviceAuthenticationWithRegistrySymmetricKey(
                    Constantes.DeviceId, key);
                Client = DeviceClient.Create(Constantes.IoTHubURL, auth,
                    Microsoft.Azure.Devices.Client.TransportType.Mqtt);
            }
        }

        public async Task EnviarTelemetria(Telemetria telemetria)
        {
            if (Client != null)
            {
                var json = JsonConvert.SerializeObject(telemetria);
                var mensaje = new Microsoft.Azure.Devices.Client.Message(
                    Encoding.ASCII.GetBytes(json));
                await Client.SendEventAsync(mensaje);
            }
        }
    }
}
