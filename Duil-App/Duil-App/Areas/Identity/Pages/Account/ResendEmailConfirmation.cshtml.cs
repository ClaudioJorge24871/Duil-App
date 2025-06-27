// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Duil_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Duil_App.Code;

namespace Duil_App.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<Utilizadores> _userManager;
        private readonly Ferramentas _ferramentas;

        public ResendEmailConfirmationModel(UserManager<Utilizadores> userManager, Ferramentas ferramentas)
        {
            _userManager = userManager;
            _ferramentas = ferramentas;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required (ErrorMessage = "O email é obrigatório")]
            [EmailAddress (ErrorMessage = "Formato de email inválido")]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email); //Obtem o user atraves do email
            if (user == null)
            {
                TempData["Mensagem"] = "Nenhum utilizador encontrado com esse email.";
                return Page();
            }

            // Caso o email já está confirmado
            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                TempData["Mensagem"] = "O email já foi confirmado anteriormente.";
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: Request.Scheme);

            Models.Email email = new Models.Email
            {
                Destinatario = Input.Email,
                Subject = "Reenvio - Confirmação de Email",
                Body = GetEmailBody(callbackUrl),
            };

            var resposta = await _ferramentas.EnviaEmailAsync(email);

            TempData["Mensagem"] = resposta == 0
                ? "Email de confirmação reenviado com sucesso."
                : "Ocorreu um erro ao enviar o email.";

            return RedirectToPage(); 
        }

        /// <summary>
        /// Email de confirmação de Email
        /// </summary>
        /// <param name="callbackUrl"></param>
        /// <returns></returns>
        private string GetEmailBody(string callbackUrl)
        {
            string Body = $@"
                <!DOCTYPE html>
                <html lang='pt'>
                <head>
                    <meta charset='UTF-8' />
                    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                    <title>Reenvio - Confirmação de Email</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f6f8;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            max-width: 600px;
                            background-color: #ffffff;
                            margin: 30px auto;
                            padding: 20px;
                            border-radius: 8px;
                            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
                            color: #333333;
                        }}
                        h1 {{
                            color: #007bff;
                            font-size: 24px;
                            margin-bottom: 20px;
                        }}
                        p {{
                            font-size: 16px;
                            line-height: 1.5;
                        }}
                        a.button {{
                            display: inline-block;
                            padding: 12px 24px;
                            margin-top: 20px;
                            background-color: #007bff;
                            color: #ffffff !important;
                            text-decoration: none;
                            border-radius: 5px;
                            font-weight: bold;
                        }}
                        a.button:hover {{
                            background-color: #0056b3;
                        }}
                        .footer {{
                            margin-top: 30px;
                            font-size: 12px;
                            color: #888888;
                            text-align: center;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Confirmação de Email</h1>
                        <p>Olá,</p>
                        <p>Por favor, confirme o seu endereço de email ao clicar no botão abaixo:</p>
                        <p style='text-align: center;'>
                            <a href='{HtmlEncoder.Default.Encode(callbackUrl)}' class='button'>Confirmar Email</a>
                        </p>
                        <div class='footer'>
                            &copy; {DateTime.Now.Year} Duil. Todos os direitos reservados.
                        </div>
                    </div>
                </body>
                </html>
                ";

            return Body;
        }
    }
}
