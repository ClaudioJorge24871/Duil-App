using Azure.Identity;
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
        public override async Task OnConnectedAsync()
        {
            // lOg de id de conexão
            _logger.LogInformation("OnConnectedAsync: {0}", Context.ConnectionId);

            // Adiciona o utilizador ao grupo de Funcionarios caso assim o seja
            // Permite que o Func receba notificações quando NotificarFuncionarios acontece
            var user = Context.User;

            if (user != null && user.IsInRole("Funcionario")) 
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Funcionarios");
            }
            
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Notifica os funcionarios quando um Cliente cria uma encomenda
        /// </summary>
        public async Task NotificarFuncionarios (string nomeCliente, DateTime data, decimal precoTotal, int quantidadeTotal)
        {
            await Clients.Group("Funcionarios").SendAsync("ReceberNotificacao", new
            {
                nomeCliente,
                data = data.ToString("dd/MM/yyyy"),
                precoTotal,
                quantidadeTotal
            });
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
