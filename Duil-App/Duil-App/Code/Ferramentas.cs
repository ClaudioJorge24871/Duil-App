namespace Duil_App.Code
{

    using Duil_App.Models;

    using System.Net;
    using System.Net.Mail;

    /// <summary>
    /// Ferramentas de uso genério para enviar Emails e escrever logs de eventos.
    /// Fonte: https://github.com/jcnpereira/SendEmail/blob/main/SendEmail/SendEmail/Code/Ferramentas.cs
    /// </summary>
    public class Ferramentas
    {

        private readonly IConfiguration _configuracao;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Ferramentas(IConfiguration configuracao,
            IWebHostEnvironment webHostEnvironment)
        {
            this._configuracao = configuracao;
            this._webHostEnvironment = webHostEnvironment;
        }

        public async Task<int> EscreveLogAsync(string nomeController, string metodo, string acao, string pessoa)
        {
            int resultado = 0; 

            string dataAtual = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            if (!Directory.Exists(Path.Combine(_webHostEnvironment.ContentRootPath, "Logs")))
            {
                Directory.CreateDirectory(Path.Combine(_webHostEnvironment.ContentRootPath, "Logs"));
            }
            var caminhoCompleto = Path.Combine(_webHostEnvironment.ContentRootPath, "Logs", dataAtual + ".txt");

            var logFile = System.IO.File.Create(caminhoCompleto);
            var logWriter = new System.IO.StreamWriter(logFile);
            await logWriter.WriteLineAsync("Data: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            await logWriter.WriteLineAsync("User: " + pessoa);
            await logWriter.WriteLineAsync("Controller: " + nomeController);
            await logWriter.WriteLineAsync("Método: " + metodo);
            await logWriter.WriteLineAsync("Ação executada: " + acao);
            await logWriter.DisposeAsync();

            return resultado;
        }


        public async Task<int> EnviaEmailAsync(Email email, bool bcc = false)
        {

            int resultado = 0;

            string emailSenderEmail = _configuracao["AppSettings:Email:SenderEmail"];
            string emailUserName = _configuracao["AppSettings:Email:Username"];
            string emailPassword = _configuracao["AppSettings:Email:Password"];
            string emailHost = _configuracao["AppSettings:Email:Host"];
            string emailPort = _configuracao["AppSettings:Email:Port"];



            using (var client = new SmtpClient())
            {
             
                try
                {
                    client.Host = emailHost;
                    client.Port = Convert.ToInt32(emailPort);
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(emailUserName, emailPassword);

                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(emailSenderEmail, "app Envio de Emails");
                        message.To.Add(email.Destinatario);
                        

                        message.Subject = email.Subject;
                        message.Body = email.Body;
                        message.IsBodyHtml = true;

                        await client.SendMailAsync(message);
                    } 
                }
                catch (Exception ex)
                {
                    string auxErro = "Erro no envio do email.\r\n" + ex.Message + "\r\n\r\nInnerException\r\n" + ex.InnerException + "\r\n\r\nStackTrace\r\n" + ex.StackTrace;
                    await EscreveLogAsync("Envio Email", "", auxErro, "");
                    resultado = 1;
                }

            } 


            return resultado; 
        }




    }
}
