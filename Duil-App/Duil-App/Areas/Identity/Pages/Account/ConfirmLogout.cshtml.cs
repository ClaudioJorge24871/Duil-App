using Duil_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Duil_App.Areas.Identity.Pages.Account
{
    public class ConfirmLogoutModel : PageModel
    {
        private readonly SignInManager<Utilizadores> _signInManager;

        public ConfirmLogoutModel(SignInManager<Utilizadores> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}
