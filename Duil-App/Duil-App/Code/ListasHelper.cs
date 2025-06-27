using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Duil_App.Code
{
    /// <summary>
    /// Helper Lista de Países para ser utilizado em outras partes da app
    /// </summary>
    public static class ListasHelper
    {
        /// <summary>
        /// Obtêm a Lista dos Países disponíveis
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> ObterListaDePaises()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Dinamarca", Text = "Dinamarca" },
                new SelectListItem { Value = "EUA", Text = "Estados Unidos da América" },
                new SelectListItem { Value = "França", Text = "França" },
                new SelectListItem { Value = "Holanda", Text = "Holanda" },
                new SelectListItem { Value = "Inglaterra", Text = "Inglaterra" },
                new SelectListItem { Value = "Suecia", Text = "Suécia"},
                new SelectListItem { Value = "Outro", Text = "Outro"} // Fallback de País. CAso outro cliente. Não deveria ser utilizado
            };
        }
    }
}

