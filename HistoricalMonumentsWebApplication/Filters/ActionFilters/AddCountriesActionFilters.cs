using HistoricalMonumentsWebApplication.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HistoricalMonumentsWebApplication.Filters.ActionFilters
{
    public class AddCountriesActionFilters : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();

            if (context.Controller is HomeController homeController)
            {
                var translations = new Dictionary<string, string>()
                {
                    ["Україна"] = "Ukraine",
                    ["Німеччина"] = "Germany",
                    ["Франція"] = "France",
                    ["Італія"] = "Italy",
                    ["Чехія"] = "CZ",
                    
                };

                homeController.ViewBag.CountryNames = translations;
            }


        }
    }
}
