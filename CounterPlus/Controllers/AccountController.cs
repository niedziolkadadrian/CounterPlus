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

    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInModel user)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var res = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, false, false);
        if (!res.Succeeded)
        {
            TempData["error_msg"] = "Nie udało się zalogować!";
            return View(user);
        }
        return RedirectToAction("Index", "Counters");
    }

    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel user)
    {
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
                if (err.Code == "DuplicateUserName")
                    ModelState.AddModelError("UserName","Nazwa użytkownika zajęta!");
                if (err.Code == "PasswordTooShort")
                    ModelState.AddModelError("Password", "Hasło za krótkie!");
                if (err.Code == "PasswordRequiresDigit")
                    ModelState.AddModelError("Password", "Hasło musi zawierać przynajmniej jedną cyfrę.");
                if (err.Code == "PasswordRequiresLower")
                    ModelState.AddModelError("Password", "Hasło musi zawierać przynajmniej jedną małą literę.");
                if (err.Code == "PasswordRequiresUpper")
                    ModelState.AddModelError("Password", "Hasło musi zawierać przynajmniej jedną dużą literę.");
                if (err.Code == "PasswordRequiresNonAlphanumeric")
                    ModelState.AddModelError("Password", "Hasło musi zawierać przynajmniej jeden nie alfanumeryczny znak.");
            }
            TempData["error_msg"] = "Wystąpił błąd podczas rejestracji użytkownika!";
            return View(user);
        }

        TempData["msg"] = "Pomyślnie zarejestrowano!";
        return RedirectToAction("SignIn", "Account");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("SignIn", "Account");
    }
}