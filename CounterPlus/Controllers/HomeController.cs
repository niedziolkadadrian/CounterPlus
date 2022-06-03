using System.Buffers.Text;
using System.Diagnostics;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using CounterPlus.Models;

namespace CounterPlus.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Counter(string Id)
    {
        var idBytes = Convert.FromBase64String(HttpUtility.UrlDecode(Id));
        int id = BitConverter.ToInt32(idBytes, 0);
        return View(id);
    }
    
    //[HttpPost]
    public IActionResult NewCounter()
    {
        int id = new Random().Next();
        string base64id = HttpUtility.UrlEncode(Convert.ToBase64String(BitConverter.GetBytes(id)));
        return RedirectToAction("Counter", new {Id = base64id});
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}