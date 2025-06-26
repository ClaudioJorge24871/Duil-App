// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Duil_App.Code;
using Duil_App.Data;
using Duil_App.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Duil_App.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<Utilizadores> _signInManager;
        private readonly UserManager<Utilizadores> _userManager;
        private readonly IUserStore<Utilizadores> _userStore;
        private readonly IUserEmailStore<Utilizadores> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly Ferramentas _ferramenta;



        public RegisterModel(
            UserManager<Utilizadores> userManager,
            IUserStore<Utilizadores> userStore,
            SignInManager<Utilizadores> signInManager,
            ILogger<RegisterModel> logger,
            Ferramentas ferramenta,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            this._ferramenta = ferramenta;
            _context = context;
            _emailStore = GetEmailStore();
        }


        /// <summary>
        /// Este objecto será usado para fazer a transposição de dados entre este
        /// ficheiro (de programação)  e a sua respetiva visualização
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// Link que após a autenticação será redirecionado.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        /// Lista de Paises disponiveis
        /// </summary>
        public List<SelectListItem> ListaPaises { get; set; }


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///  Email do novo utilizador
            /// </summary>
            [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
            [EmailAddress(ErrorMessage ="Tem de escrever um {0} válido")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
            [StringLength(100, ErrorMessage = "A {0} Tem de ter {2} e no máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirmar password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            /// <summary>
            /// Incorporação dos dados de um utilizador 
            /// no formulario de Registo
            /// </summary>
            public Utilizadores Utilizador { get; set; }

        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ListaPaises = ListasHelper.ObterListaDePaises();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var nomeEmUso = _context.Users.Any(user => user.Nome == Input.Utilizador.Nome);

            if(nomeEmUso)
            {
                ModelState.AddModelError("Input.Utilizador.Nome", "Já existe um utilizador com esse nome.");
                return Page();
            }

            if (ModelState.IsValid)
            {
                var user = new Utilizadores
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Nome = Input.Utilizador.Nome,
                    Morada = Input.Utilizador.Morada,
                    CodPostal = Input.Utilizador.CodPostal,
                    Pais = Input.Utilizador.Pais,
                    NIF = Input.Utilizador.NIF,
                    Telemovel = Input.Utilizador.Telemovel,
                    PhoneNumber = Input.Utilizador.Telemovel
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));


                    await _userManager.AddToRoleAsync(user, "Utilizador");
                    await _context.SaveChangesAsync();

                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);


                    Models.Email email = new Models.Email
                    {
                        Destinatario = Input.Email,
                        Subject = "Confirmação de email",
                        Body = GetEmailBody(callbackUrl)
                    };

                    var resposta = await _ferramenta.EnviaEmailAsync(email);

                    string texto = "Email enviado em: " + DateTime.Now.ToString() +
                     "\r\n" + "Destinatário: " + email.Destinatario +
                     "\r\n" + "Assunto: " + email.Subject +
                     "\r\n" + "Corpo Email: " + email.Body;

                    await _ferramenta.EscreveLogAsync("Home", "Index", texto, "");

                    if (resposta == 0)
                    {
                        TempData["Mensagem"] = "S#:Email Enviado com sucesso.";
                    }
                    else
                    {
                        TempData["Mensagem"] = "E#:Ocorreu um erro com o envio do Email.";
                    }

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IUserEmailStore<Utilizadores> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<Utilizadores>)_userStore;
        }

        private string GetEmailBody(string callbackUrl)
        {
            string Body = $@"
                <!DOCTYPE html>
                <html lang='pt'>
                <head>
                    <meta charset='UTF-8' />
                    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                    <title>Confirmação de Email</title>
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
                        <p>Obrigado por se registar na nossa plataforma.</p>
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
