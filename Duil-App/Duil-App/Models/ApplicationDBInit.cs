namespace Duil_App.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;

    public class ApplicationDBInit
    {
        /// <summary>
        /// Seed da base de dados, com roles e utilizadores iniciais
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static async Task SeedAsync(IServiceProvider serviceProvider, IConfiguration config)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Utilizadores>>();

            string[] roles = { "Admin", "Cliente", "Funcionario", "Utilizador"}; // Roles

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Criar admin
            string adminEmail = config["AdminUser:Email"] ?? " ";
            string adminPassword = config["AdminUser:Password"] ?? " ";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new Utilizadores
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Nome = config["AdminUser:Nome"] ?? "N/A",
                    Pais = config["AdminUser:Pais"] ?? "Portugal",
                };

                var result = await userManager.CreateAsync(user, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // Criar Funcionario
            string funcionarioEmail = "funcionario@duil.com";
            string funcionarioPassword = "Funcionario@1234";
            var funcionarioUser = await userManager.FindByEmailAsync(funcionarioEmail);

            if (funcionarioUser == null)
            {
                var user = new Utilizadores
                {
                    UserName = funcionarioEmail,
                    Email = funcionarioEmail,
                    EmailConfirmed = true,
                    Nome = "Funcionario",
                    Pais = "Portugal",
                };

                var result = await userManager.CreateAsync(user, funcionarioPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Funcionario");
                }
            }

            // Criar Cliente
            string clienteEmail = "cliente@duil.com";
            string clientePassword = "Cliente@1234";
            var clienteUser = await userManager.FindByEmailAsync(clienteEmail);

            if (clienteUser == null)
            {
                var user = new Utilizadores
                {
                    UserName = clienteEmail,
                    Email = clienteEmail,
                    EmailConfirmed = true,
                    Nome = "Cliente",
                    Pais = "Portugal",
                };

                var result = await userManager.CreateAsync(user, clientePassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Cliente");
                }
            }

        }
    }
}
