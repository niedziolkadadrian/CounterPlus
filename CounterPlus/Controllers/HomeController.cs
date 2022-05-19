﻿using System.Buffers.Text;
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

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Counter(string Id)
    {
        //var idBytes = Convert.FromBase64String(HttpUtility.UrlDecode(base64id));
        //int id = BitConverter.ToInt32(idBytes, 0);
        return View(432);
    }
    
    //[HttpPost]
    public IActionResult NewCounter()
    {
        int id = new Random().Next();
        Console.WriteLine(id);
        string base64id = HttpUtility.UrlEncode(Convert.ToBase64String(BitConverter.GetBytes(id)));
        Console.WriteLine(base64id);
        var idBytes = Convert.FromBase64String(HttpUtility.UrlDecode(base64id));
        int iddecoded = BitConverter.ToInt32(idBytes, 0);
        Console.WriteLine(iddecoded);
        return RedirectToAction("Counter", new {id = base64id});
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}