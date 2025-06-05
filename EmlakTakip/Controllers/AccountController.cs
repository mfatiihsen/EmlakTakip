




using EmlakTakip.Data;
using EmlakTakip.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmlakTakip.Controllers
{
    public class AccountController : Controller
    {


    private readonly UygulamaDbContext _context;
    public AccountController(UygulamaDbContext context)
    {
        _context = context;
    }
        
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }


    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var admin = _context.Kullanicilars
            .FirstOrDefault(a => a.KullaniciAdi == model.KullaniciAdi && a.Sifre == model.Sifre);

        if (admin == null)
        {
            ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış.");
            return View(model);
        }

        // Giriş başarılı, oturum başlat
        HttpContext.Session.SetInt32("AdminId", admin.Id);
        HttpContext.Session.SetString("AdminKullaniciAdi", admin.KullaniciAdi);

        return RedirectToAction("Index", "Home");
    }

    // Çıkış işlemi
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }

    }
}