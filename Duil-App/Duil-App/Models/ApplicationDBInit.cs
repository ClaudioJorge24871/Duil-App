namespace Duil_App.Models
{
    using Duil_App.Data;
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
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

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
                    NIF = "250165487",
                    Nome = "Cliente Exemplo",
                    Pais = "Portugal",
                };

                var result = await userManager.CreateAsync(user, clientePassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Cliente");
                }
            }

            // Adicionar Cliente à base de dados
            if (!context.Clientes.Any(c => c.Email == clienteEmail))
            {
                var cliente = new Clientes
                {
                    Nif = "250165487",
                    Nome = "Cliente Exemplo",
                    Email = clienteEmail,
                    Telemovel = "912345678",
                    Pais = "Portugal",
                };

                await context.Clientes.AddAsync(cliente);
                await context.SaveChangesAsync();
            }


            // Criar Fabrica
            if (!context.Fabricas.Any(f => f.Nome == "Fábrica Exemplo"))
            {
                var fabricaDefault = new Fabricas
                {
                    Nif = "250123456",
                    Nome = "Fábrica Exemplo",
                    Email = "fabrica@exemplo.com",
                    Telemovel = "912345678",
                    Pais = "Portugal",
                };

                await context.Fabricas.AddAsync(fabricaDefault);
                await context.SaveChangesAsync();

            }
        }
    }
}
