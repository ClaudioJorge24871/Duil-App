using Duil_App.Data;
using Microsoft.AspNetCore.SignalR;


namespace Duil_App.Services
{
    /// <summary>
    /// Hub do Signal R. 
    /// Ponto Central na API Signal R que lida com conexões, grupos e mensagens.
    /// </summary>
    public class RealTimeHub:Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RealTimeHub> _logger;

        public RealTimeHub(ApplicationDbContext context, ILogger<RealTimeHub> logger) 
        {
            _context = context;
            _logger = logger;

        }

        /// <summary>
        /// Quando há uma ligação ao Signal R,
        /// este é o primeiro método a ser executado
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            // lOg de id de conexão
            _logger.LogInformation("OnConnectedAsync: {0}", Context.ConnectionId);
            
            //lógica para guardar o id da conexão para poder enviar mensagens ao utilizador mais tarde.

            return base.OnConnectedAsync();
        }


        /// <summary>
        /// Envia notificação a todos os clientes
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
