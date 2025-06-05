using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmlakTakip.Models;
using EmlakTakip.ViewModel;
using EmlakTakip.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace EmlakTakip.Controllers;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly UygulamaDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public HomeController(ILogger<HomeController> logger, UygulamaDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        var emlakTipleri = _context.Emlaklar
            .GroupBy(e => e.EmlakTipi) // Örneğin: "Daire", "Villa"
            .ToDictionary(g => g.Key, g => g.Count());

        var ilanTipleri = _context.Emlaklar
            .GroupBy(e => e.Tip) // Örneğin: "Satılık", "Kiralık"
            .ToDictionary(g => g.Key, g => g.Count());

        var model = new DashboardViewModel
        {
            ToplamEmlak = _context.Emlaklar.Count(),
            SatilikIlanSayisi = _context.Emlaklar.Count(e => e.Tip == "Satılık"),
            KiralikIlanSayisi = _context.Emlaklar.Count(e => e.Tip == "Kiralık"),
            KullaniciSayisi = _context.Kullanicilars.Count(),
            SonEklenenIlanlar = _context.Emlaklar
                .OrderByDescending(e => e.IlanTarihi)
                .Take(5)
                .ToList(),
            EmlakTipleri = emlakTipleri,
            IlanTipleri = ilanTipleri
        };

        return View(model);
    }


    public IActionResult EmlakListeleme(string? tip)
    {
        var emlaklar = _context.Emlaklar.AsQueryable();

        if (!string.IsNullOrEmpty(tip))
        {
            emlaklar = emlaklar.Where(e => e.Tip == tip);
        }

        return View(emlaklar.ToList());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmlakViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var emlak = new Emlak
        {
            Baslik = model.Baslik,
            Tip = model.Tip,
            Fiyat = model.Fiyat,
            Adres = model.Adres,
            Aciklama = model.Aciklama,
            Metrekare = model.Metrekare,
            OdaSayisi = model.OdaSayisi,
            BinaYasi = model.BinaYasi,
            IsitmaTipi = model.IsitmaTipi,
            BulunduguKat = model.BulunduguKat,
            KatSayisi = model.KatSayisi,
            EmlakTipi = model.EmlakTipi,
            Balkon = model.Balkon,
            Gorseller = new List<EmlakFoto>()
        };

        var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
        Directory.CreateDirectory(uploads);

        foreach (var file in model.EmlakFoto)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                emlak.Gorseller.Add(new EmlakFoto { DosyaYolu = "/uploads/" + fileName });
            }
        }

        _context.Emlaklar.Add(emlak);
        await _context.SaveChangesAsync();

        return RedirectToAction("EmlakListeleme");
    }


    public async Task<IActionResult> DetayJson(int id)
    {
        var emlak = await _context.Emlaklar.FindAsync(id);
        if (emlak == null)
            return NotFound();

        return Json(new
        {
            emlak.Baslik,
            emlak.Tip,
            Fiyat = emlak.Fiyat.ToString("N0") + " ₺",
            emlak.Adres,
            emlak.Aciklama,
            emlak.Metrekare,
            emlak.OdaSayisi,
            emlak.IsitmaTipi,
            Kat = emlak.BulunduguKat,
            IlanTarihi = emlak.IlanTarihi.ToString("dd.MM.yyyy")
        });
    }

    public IActionResult Details(int id)
    {
        var emlak = _context.Emlaklar
            .Include(e => e.Gorseller)  // Bu satırı ekle
            .FirstOrDefault(e => e.Id == id);

        if (emlak == null)
            return NotFound();

        return View(emlak);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var emlak = _context.Emlaklar.FirstOrDefault(e => e.Id == id);
        if (emlak == null)
        {
            return NotFound();
        }
        return View(emlak);
    }

    // POST: Emlak/Sil/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteOnay(int id)
    {
        var emlak = _context.Emlaklar.Find(id);
        if (emlak == null)
        {
            return NotFound();
        }

        _context.Emlaklar.Remove(emlak);
        _context.SaveChanges();

        TempData["SuccessMessage"] = "Emlak başarıyla silindi.";
        return RedirectToAction("Index");
    }

    public IActionResult Kullanicilar()
    {
        var kullanicilar = _context.Kullanicilars.ToList();
        return View(kullanicilar);

    }

    [HttpGet]
    public IActionResult KullaniciEkle()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult KullaniciEkle(Kullanicilar model)
    {
        if (!ModelState.IsValid)
        {
            // Model doğrulaması başarısızsa, aynı sayfayı hata mesajları ile tekrar göster
            return View(model);
        }

        try
        {
            model.KayitTarihi = DateTime.Now;
            _context.Kullanicilars.Add(model);
            _context.SaveChanges();

            // Başarılıysa başka sayfaya yönlendirebiliriz veya mesaj gösterebiliriz
            TempData["SuccessMessage"] = "Kullanıcı başarıyla eklendi.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            // Hata varsa ModelState'a ekleyip formda göster
            ModelState.AddModelError("", "Kayıt işlemi sırasında bir hata oluştu: " + ex.Message);
            return View(model);
        }
    }


    public IActionResult KullaniciDuzenle()
    {
        return View();
    }




    [ValidateAntiForgeryToken]
    public async Task<IActionResult> KullaniciSil(int id)
    {
        var kullanici = await _context.Kullanicilars.FindAsync(id);
        if (kullanici == null)
        {
            return NotFound();
        }

        _context.Kullanicilars.Remove(kullanici);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public IActionResult SifreDegistir()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SifreDegistir(SifreDegistirModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var kullaniciAdi = HttpContext.Session.GetString("AdminKullaniciAdi");
        var id = HttpContext.Session.GetString("AdminId");
        if (string.IsNullOrEmpty(kullaniciAdi))
        {
            return RedirectToAction("Login", "Home");
        }

        var user = await _context.Kullanicilars
            .FirstOrDefaultAsync(u => u.KullaniciAdi == kullaniciAdi);

        if (user == null)
        {
            return NotFound("Kullanıcı bulunamadı.");
        }

        if (user.Sifre != model.MevcutSifre)
        {
            ModelState.AddModelError(string.Empty, "Mevcut şifreniz yanlış.");
            return View(model);
        }

        user.Sifre = model.YeniSifre; // Gerçekte burada hash'lenmeli
        _context.Kullanicilars.Update(user);
        await _context.SaveChangesAsync();

        return RedirectToAction("SifreDegistirBasarili");
    }

    [HttpGet]
    public IActionResult SifreDegistirBasarili()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult KullaniciDuzenle(Kullanicilar kullanici)
    {
        if (!ModelState.IsValid)
        {
            return View(kullanici);
        }

        var mevcut = _context.Kullanicilars.Find(kullanici.Id);
        if (mevcut == null) return NotFound();

        mevcut.AdSoyad = kullanici.AdSoyad;
        mevcut.KullaniciAdi = kullanici.KullaniciAdi;
        mevcut.Email = kullanici.Email;
        // Şifre alanını güncellemiyoruz

        _context.SaveChanges();
        return RedirectToAction("Kullanicilar", "Home");
    }

    [HttpGet]
    public IActionResult KullaniciDuzenle(int id)
    {
        var kullanici = _context.Kullanicilars.Find(id);
        if (kullanici == null)
        {
            return NotFound();
        }
        return View(kullanici);
    }


    [HttpGet]
    public IActionResult Edit(int id)
    {
        var emlak = _context.Emlaklar.Find(id);
        if (emlak == null)
        {
            return NotFound();
        }
        return View(emlak);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Emlak model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var existingEmlak = _context.Emlaklar.Find(model.Id);
        if (existingEmlak == null)
        {
            return NotFound();
        }

        // Güncellenecek alanlar
        existingEmlak.Baslik = model.Baslik;
        existingEmlak.Tip = model.Tip;
        existingEmlak.Fiyat = model.Fiyat;
        existingEmlak.Adres = model.Adres;
        existingEmlak.Metrekare = model.Metrekare;
        existingEmlak.OdaSayisi = model.OdaSayisi;
        existingEmlak.IsitmaTipi = model.IsitmaTipi;
        existingEmlak.KatSayisi = model.KatSayisi;
        existingEmlak.Aciklama = model.Aciklama;

        _context.SaveChanges();
        return RedirectToAction("Index");
    }

}
