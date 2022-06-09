using CounterPlus.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CounterPlus.Controllers;

public class AccountController : Controller
{
    private readonly CPdbContext _db;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(CPdbContext db, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _db = db;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult LogIn()
    {
        if (User.Identity is {IsAuthenticated: true})
        {
            return RedirectToAction("Index", "Counters");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LogIn(SignInModel user)
    {
        if (User.Identity is {IsAuthenticated: true})
        {
            return RedirectToAction("Index", "Counters");
        }
        if (!ModelState.IsValid)
        {
            return View();
        }
        
        var res = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, false, false);
        if (res.Succeeded) return RedirectToAction("Index", "Counters");
        
        TempData["error_msg"] = "Nieprawidłowa nazwa użytkownika lub hasło!";
        return View(user);
    }

    public IActionResult Register()
    {
        if (User.Identity is {IsAuthenticated: true})
        {
            return RedirectToAction("Index", "Counters");
        }
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel user)
    {
        if (User.Identity is {IsAuthenticated: true})
        {
            return RedirectToAction("Index", "Counters");
        }
        if (!ModelState.IsValid) return View(user);
        
        var newUser = new User
        {
            UserName = user.UserName,
            Email = user.Email
        };
        var res = await _userManager.CreateAsync(newUser, user.Password);
        if (!res.Succeeded)
        {
            foreach (var err in res.Errors)
            {
                switch (err.Code)
                {
                    case "DuplicateUserName":
                        ModelState.AddModelError("UserName","Nazwa użytkownika zajęta!");
                        break;
                    case "PasswordTooShort":
                        ModelState.AddModelError("Password", "Hasło za krótkie!");
                        break;
                    case "PasswordRequiresDigit":
                        ModelState.AddModelError("Password", "Hasło musi zawierać przynajmniej jedną cyfrę.");
                        break;
                    case "PasswordRequiresLower":
                        ModelState.AddModelError("Password", "Hasło musi zawierać przynajmniej jedną małą literę.");
                        break;
                    case "PasswordRequiresUpper":
                        ModelState.AddModelError("Password", "Hasło musi zawierać przynajmniej jedną dużą literę.");
                        break;
                    case "PasswordRequiresNonAlphanumeric":
                        ModelState.AddModelError("Password", "Hasło musi zawierać przynajmniej jeden nie alfanumeryczny znak.");
                        break;
                }
            }
            TempData["error_msg"] = "Wystąpił błąd podczas rejestracji użytkownika!";
            return View(user);
        }

        TempData["msg"] = "Pomyślnie zarejestrowano!";
        return RedirectToAction("LogIn", "Account");
    }

    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("LogIn", "Account");
    }
}