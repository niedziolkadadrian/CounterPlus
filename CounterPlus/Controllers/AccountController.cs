﻿using Microsoft.AspNetCore.Mvc;

namespace CounterPlus.Controllers;

public class AccountController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}