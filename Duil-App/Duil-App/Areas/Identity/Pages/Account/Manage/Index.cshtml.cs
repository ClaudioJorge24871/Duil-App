// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Duil_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Duil_App.Code;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Duil_App.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Utilizadores> _userManager;
        private readonly SignInManager<Utilizadores> _signInManager;

        public IndexModel(
            UserManager<Utilizadores> userManager,
            SignInManager<Utilizadores> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Email { get; set; }

        public string Pais {  get; set; }


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

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
            [Display(Name = "Telemovel", ResourceType = typeof(Resources.Resource))]
            public string Telemovel { get; set; }

            [Display(Name = "Nome", ResourceType = typeof(Resources.Resource))]
            public string Nome { get; set; }

            [Display(Name = "Morada", ResourceType = typeof(Resources.Resource))]
            public string Morada { get; set; }

            [Display(Name = "CodPostal", ResourceType = typeof(Resources.Resource))]
            public string CodPostal { get; set; }

        }

        private async Task LoadAsync(Utilizadores user)
        {
            var userName = user.UserName;
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var nome = user.Nome;
            var morada = user.Morada;
            var codPostal = user.CodPostal;
            
            Email = userName;
            Pais = user.Pais;

            Input = new InputModel
            {
                Telemovel = phoneNumber,
                Nome = nome,
                Morada = morada,
                CodPostal = codPostal,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (!ValidaTelemovel(Input.Telemovel, user.Pais))
            {
                StatusMessage = "Error: " + Resources.Resource.telemovelnao;
                return RedirectToPage();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.Telemovel != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.Telemovel);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = Resources.Resource.errotelm;
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Os seus dados foram atualizados";
            return RedirectToPage();
        }


        /// <summary>
        ///  Valida o numero de telemovel a partir do pais do user
        /// </summary>
        /// <returns></returns>
        public bool ValidaTelemovel(string Telemovel, string pais)
        {
            if (pais.Trim().ToLowerInvariant() == "portugal")
            {
                //Validação do telemóvel português
                if (!System.Text.RegularExpressions.Regex.IsMatch(Telemovel, @"^9[1236][0-9]{7}$"))
                    return false;
            }
            else if (pais.Trim().ToLowerInvariant() == "dinamarca")
            {
                // Validação do telemóvel dinamarquês (começa geralmente por 2, 3, 4, 5, 6 ou 7 + 7 dígitos)
                if (!string.IsNullOrWhiteSpace(Telemovel) && !System.Text.RegularExpressions.Regex.IsMatch(Telemovel, @"^[2-7]\d{7}$"))
                    return false;
            }
            else if (pais.Trim().ToLowerInvariant() == "eua" || pais.Trim().ToLowerInvariant() == "estados unidos da américa")
            {
                // Telemóvel: 10 dígitos
                if (!string.IsNullOrWhiteSpace(Telemovel) && !System.Text.RegularExpressions.Regex.IsMatch(Telemovel, @"^\d{10}$"))
                    return false;
            }
            else if (pais.Trim().ToLowerInvariant() == "frança" || pais.Trim().ToLowerInvariant() == "franca")
            {
                // Telemóvel francês: começa com 06 ou 07 e tem 10 dígitos
                // Fonte: https://www.my-french-house.com/blog/article/75391/telephone-numbers-in-france
                if (!string.IsNullOrWhiteSpace(Telemovel) &&
                    !System.Text.RegularExpressions.Regex.IsMatch(Telemovel, @"^0[67]\d{8}$"))
                    return false;
            }
            else if (pais.Trim().ToLowerInvariant() == "holanda")
            {
                // Telemóvel holandês: começa com 06 seguido de 8 dígitos (total 10 dígitos)
                if (!string.IsNullOrWhiteSpace(Telemovel) && !System.Text.RegularExpressions.Regex.IsMatch(Telemovel, @"^06\d{8}$"))
                    return false;
            }
            else if (pais.Trim().ToLowerInvariant() == "inglaterra" || pais.Trim().ToLowerInvariant() == "uk")
            {
                // Telemóvel: começa com 07 seguido de 9 dígitos (total 11)
                if (!string.IsNullOrWhiteSpace(Telemovel) && !System.Text.RegularExpressions.Regex.IsMatch(Telemovel, @"^07\d{9}$"))
                    return false;
            }
            else if (pais.Trim().ToLowerInvariant() == "suecia" || pais.Trim().ToLowerInvariant() == "suécia")
            {
                // Telemóvel sueco: começa por 07 e seguido de 8 dígitos (10 no total)
                if (!string.IsNullOrWhiteSpace(Telemovel) &&
                    !System.Text.RegularExpressions.Regex.IsMatch(Telemovel, @"^07\d{8}$"))
                    return false;
            }
            else
            {
                return false;
            }


            return true;
        }
    }
}
