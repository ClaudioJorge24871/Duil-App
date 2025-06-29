using Duil_App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Duil_App.Resources;

namespace Duil_App.Code
{
    public class EstadosTraduzidosHelper
    {
        /// <summary>
        /// Obtem os estados das encomendas traduzidos
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> GetEstadosTraduzidos()
        {
            var estados = Enum.GetValues(typeof(Estados)).Cast<Estados>();
            return estados.Select(e => new SelectListItem
            {
                Value = e.ToString(),
                Text = Resource.ResourceManager.GetString("Estado" + e.ToString())
            }).ToList();
        }

		public static string GetUmEstadoTraduzido(string estado)
		{
			var traducao = Resource.ResourceManager.GetString("Estado" + estado);

            return traducao ?? Resource.ResourceManager.GetString("semInfo");

		}
	}
}
