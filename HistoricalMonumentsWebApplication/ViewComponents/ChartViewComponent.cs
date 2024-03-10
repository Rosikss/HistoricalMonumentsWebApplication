using Microsoft.AspNetCore.Mvc;

namespace HistoricalMonumentsWebApplication.ViewComponents
{
    public class ChartViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
