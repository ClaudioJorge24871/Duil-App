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
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
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
                        Body = $"Por favor confirme o seu email clicando aqui: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>link</a>"
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
    }
}
