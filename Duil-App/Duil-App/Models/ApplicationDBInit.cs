namespace Duil_App.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;

    public class ApplicationDBInit
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, IConfiguration config)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Utilizadores>>();

            string[] roles = { "Admin", "Cliente", "Funcionario", "Utilizador"};

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Criar admin se nao existe
            string adminEmail = config["AdminUser:Email"];
            string adminPassword = config["AdminUser:Password"]; // Possivel mudar para tornar mais seguro
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new Utilizadores
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Nome = config["AdminUser:Nome"],
                };

                var result = await userManager.CreateAsync(user, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

        }
    }
}
