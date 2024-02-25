using LiveNotificationsMVC.Hubs;
using LiveNotificationsMVC.Models;
using LiveNotificationsMVC.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LiveNotificationsMVC.Controllers;

public class AccountController : Controller
{
    private readonly IUserInterface _repos;
    public AccountController(IUserInterface repos)
    {
        _repos = repos;
    }

    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(string username, string password)
    {
        if(username is not null && password is not null)
        {
            var user = await _repos.GetUserDetailsAsync(username, password);
            HttpContext.Session.SetString("Username", user.Username);
            return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError("Login", "Erro nas credenciais");
        return View();
    }
}
