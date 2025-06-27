using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Duil_App.Code
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ValidaPorPaisAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var obj = validationContext.ObjectInstance;
            var type = obj.GetType();

            // usa reflection para obter Nif, Pais, Telemovel e CodPostal
            var nifProp = type.GetProperty("Nif") ?? type.GetProperty("NIF");
            var paisProp = type.GetProperty("Pais");
            var telProp = type.GetProperty("Telemovel");
            var postalProp = type.GetProperty("CodPostal");

            var Nif = nifProp?.GetValue(obj) as string ?? "";
            var pais = paisProp?.GetValue(obj) as string ?? "";
            var Telemovel = telProp?.GetValue(obj) as string ?? "";
            var CodPostal = postalProp?.GetValue(obj) as string ?? "";

            bool IsMatch(string pattern, string input)
                => Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);

            if (pais.Trim().ToLowerInvariant() == "portugal") {
                //Validação do NIF portugûes
                if (!System.Text.RegularExpressions.Regex.IsMatch(Nif, @"^[1-9][0-9]{8}$"))
                    return new ValidationResult("O NIF deve ser válido.", new[] { nameof(Nif) });

                //Validação do telemóvel português
                if (!string.IsNullOrWhiteSpace(CodPostal) && !System.Text.RegularExpressions.Regex.IsMatch(Telemovel, @"^9[1236][0-9]{7}$"))
                      return new ValidationResult("O Telemóvel deve ser válido.", new[] { nameof(Telemovel) });

                //Validação do Código Postal Português
                if (!string.IsNullOrWhiteSpace(CodPostal) &&
                    !System.Text.RegularExpressions.Regex.IsMatch(CodPostal, @"^[1-9][0-9]{3}-[0-9]{3}$"))
                      return new ValidationResult("O Código Postal deve ser válido.", new[] { nameof(CodPostal) });
            }else if (pais.Trim().ToLowerInvariant() == "dinamarca")
            {
                //CVR (Empresas)
                // Possui 8 digitos e pode começar com 0
                if (!System.Text.RegularExpressions.Regex.IsMatch(Nif, @"^\d{8}$")) //\d é equivalente a [0-9]
                      return new ValidationResult("O CVR deve ser válido", new[] { nameof(Nif) });

                // Validação do telemóvel dinamarquês (começa geralmente por 2, 3, 4, 5, 6 ou 7 + 7 dígitos)
                if (!string.IsNullOrWhiteSpace(Telemovel) && !System.Text.RegularExpressions.Regex.IsMatch(Telemovel, @"^[2-7]\d{7}$"))
                      return new ValidationResult("O Telemóvel deve ser válido.", new[] { nameof(Telemovel) });

                // Código postal dinamarquês (4 dígitos)
                if (!string.IsNullOrWhiteSpace(CodPostal) && !System.Text.RegularExpressions.Regex.IsMatch(CodPostal, @"^\d{4}$"))
                      return new ValidationResult("O Código Postal deve ser válido.", new[] { nameof(CodPostal) });
            }else if (pais.Trim().ToLowerInvariant() == "eua" || pais.Trim().ToLowerInvariant() == "estados unidos da américa")
            {
                // EIN: 9 dígitos, opcionalmente com hífen no formato XX-XXXXXXX
                if (!System.Text.RegularExpressions.Regex.IsMatch(Nif, @"^\d{2}-?\d{7}$"))
                      return new ValidationResult("O EIN deve ser válido.", new[] { nameof(Nif) });

                // Telemóvel: 10 dígitos
                if (!string.IsNullOrWhiteSpace(Telemovel) && !System.Text.RegularExpressions.Regex.IsMatch(Telemovel, @"^\d{10}$"))
                      return new ValidationResult("O Telemóvel deve conter 10 dígitos.", new[] { nameof(Telemovel) });

                // Código postal: 5 dígitos ou ZIP+4 (ex: 12345 ou 12345-6789)
                // Fonte: https://en.wikipedia.org/wiki/ZIP_Code
                if (!string.IsNullOrWhiteSpace(CodPostal) && !System.Text.RegularExpressions.Regex.IsMatch(CodPostal, @"^\d{5}(-\d{4})?$"))
                      return new ValidationResult("O Código Postal deve ser válido.", new[] { nameof(CodPostal) });

            }else if (pais.Trim().ToLowerInvariant() == "frança" || pais.Trim().ToLowerInvariant() == "franca")
            {
                // SIREN (9 dígitos)
                if (!System.Text.RegularExpressions.Regex.IsMatch(Nif, @"^\d{9}$"))
                      return new ValidationResult("O SIREN deve ser válido.", new[] { nameof(Nif) });

                // Telemóvel francês: começa com 06 ou 07 e tem 10 dígitos
                // Fonte: https://www.my-french-house.com/blog/article/75391/telephone-numbers-in-france
                if (!string.IsNullOrWhiteSpace(Telemovel) &&
                    !System.Text.RegularExpressions.Regex.IsMatch(Telemovel, @"^0[67]\d{8}$"))
                      return new ValidationResult("O Telemóvel francês deve começar por 06 ou 07 e conter 10 dígitos.", new[] { nameof(Telemovel) });

                // Código postal francês: exatamente 5 dígitos
                if (!string.IsNullOrWhiteSpace(CodPostal) &&
                    !System.Text.RegularExpressions.Regex.IsMatch(CodPostal, @"^\d{5}$"))
                      return new ValidationResult("O Código Postal deve ser válido.", new[] { nameof(CodPostal) });
            }else if(pais.Trim().ToLowerInvariant() == "holanda")
            {
                // RSIN: 9 dígitos, começando geralmente por 8 ou 9
                if (!System.Text.RegularExpressions.Regex.IsMatch(Nif, @"^[89]\d{8}$"))
                      return new ValidationResult("O RSIN deve ser válido.", new[] { nameof(Nif) });

                // Telemóvel holandês: começa com 06 seguido de 8 dígitos (total 10 dígitos)
                if (!string.IsNullOrWhiteSpace(Telemovel) && !System.Text.RegularExpressions.Regex.IsMatch(Telemovel, @"^06\d{8}$"))
                      return new ValidationResult("O Telemóvel deve ser válido.", new[] { nameof(Telemovel) });

                // Código Postal holandês: 4 dígitos + 2 letras maiúsculas
                if (!string.IsNullOrWhiteSpace(CodPostal) && !System.Text.RegularExpressions.Regex.IsMatch(CodPostal, @"^\d{4}[A-Z]{2}$"))
                      return new ValidationResult("O Código Postal deve ser válido.", new[] { nameof(CodPostal) });
            }else if (pais.Trim().ToLowerInvariant() == "inglaterra" || pais.Trim().ToLowerInvariant() == "uk")
            {
                // Company Number: 8 dígitos OU 2 letras + 6 dígitos 
                if (!System.Text.RegularExpressions.Regex.IsMatch(Nif, @"^([A-Z]{2}\d{6}|\d{8})$"))
                      return new ValidationResult("O Company Number deve ser válido.", new[] { nameof(Nif) });

                // Telemóvel: começa com 07 seguido de 9 dígitos (total 11)
                if (!string.IsNullOrWhiteSpace(Telemovel) && !System.Text.RegularExpressions.Regex.IsMatch(Telemovel, @"^07\d{9}$"))
                      return new ValidationResult("O Telemóvel deve ser válido.", new[] { nameof(Telemovel) });

                // Codigo Postal
                // Fonte: https://ideal-postcodes.co.uk/guides/postcode-validation
                if (!string.IsNullOrWhiteSpace(CodPostal) &&
                    !System.Text.RegularExpressions.Regex.IsMatch(CodPostal, @"^[a-z]{1,2}\d[a-z\d]?\s*\d[a-z]{2}$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))

                      return new ValidationResult("O Código Postal deve ser válido.", new[] { nameof(CodPostal) });
            }else if(pais.Trim().ToLowerInvariant() == "suecia" || pais.Trim().ToLowerInvariant() == "suécia")
            {
                // Organisationsnummer: 10 dígitos, com ou sem hífen
                if (!System.Text.RegularExpressions.Regex.IsMatch(Nif, @"^\d{6}[-]?\d{4}$"))
                      return new ValidationResult("O Organisationsnummer deve ser válido.", new[] { nameof(Nif) });

                // Telemóvel sueco: começa por 07 e seguido de 8 dígitos (10 no total)
                if (!string.IsNullOrWhiteSpace(Telemovel) &&
                    !System.Text.RegularExpressions.Regex.IsMatch(Telemovel, @"^07\d{8}$"))
                      return new ValidationResult("O Telemóvel deve ser válido.", new[] { nameof(Telemovel) });

                // Código postal da sueecia: 5 dígitos (opcional espaço entre o 3.º e 4.º dígito)
                if (!string.IsNullOrWhiteSpace(CodPostal) &&
                    !System.Text.RegularExpressions.Regex.IsMatch(CodPostal, @"^\d{3}\s?\d{2}$"))
                      return new ValidationResult("O Código Postal deve ser válido.", new[] { nameof(CodPostal) });
            }
            else // Caso seja injetado outro país
            {
                return new ValidationResult(
                        $"O país '{pais}' não é suportado para validação.",
                        new[] { "Pais" }
                    );
            }
                
            
            return ValidationResult.Success;
        }
    }
}
