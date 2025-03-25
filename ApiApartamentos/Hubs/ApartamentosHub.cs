using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ApiApartamentos.Hubs
{
    public class ApartamentosHub : Hub
    {
        public async Task NotificarCambio()
        {
            await Clients.All.SendAsync("Recargar Datos");
        }
    }
}
