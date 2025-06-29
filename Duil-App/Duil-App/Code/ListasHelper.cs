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
                new SelectListItem { Value = "Dinamarca", Text = Resources.Resource.Dinamarca },
                new SelectListItem { Value = "EUA", Text = Resources.Resource.EstadosUnidosdaAmerica },
                new SelectListItem { Value = "França", Text = Resources.Resource.Franca },
                new SelectListItem { Value = "Holanda", Text = Resources.Resource.Holanda },
                new SelectListItem { Value = "Inglaterra", Text = Resources.Resource.Inglaterra },
                new SelectListItem { Value = "Suecia", Text = Resources.Resource.Suecia},
            };
        }
    }
}

