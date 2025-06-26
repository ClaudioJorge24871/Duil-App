using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Duil_App.Code
{
    public static class ListasHelper
    {
        public static List<SelectListItem> ObterListaDePaises()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Dinamarca", Text = "Dinamarca" },
                new SelectListItem { Value = "EUA", Text = "Estados Unidos da América" },
                new SelectListItem { Value = "França", Text = "França" },
                new SelectListItem { Value = "Holanda", Text = "Holanda" },
                new SelectListItem { Value = "Inglaterra", Text = "Inglaterra" },
                new SelectListItem { Value = "Suecia", Text = "Suécia"}
            };
        }
    }
}

