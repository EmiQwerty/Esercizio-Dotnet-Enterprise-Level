using Microsoft.AspNetCore.Mvc;

namespace OrderManagement.Web.Controllers;

// Controller minimale usato per la home e per una seconda pagina semplice.
public class HomeController : Controller
{
    // Restituisce la view Views/Home/Index.cshtml.
    public IActionResult Index()
    {
        return View();
    }

    // Restituisce la view Views/Home/Privacy.cshtml.
    public IActionResult Privacy()
    {
        return View();
    }
}
