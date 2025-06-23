using EmlakTakip.Data;
using EmlakTakip.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace sendinEmlak.Controllers
{
    public class AnaController : Controller
    {
        private readonly UygulamaDbContext _context;

        public AnaController(UygulamaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(
            string search,
            string tip,
            string emlakTipi,
            int page = 1,
            int pageSize = 10
        )
        {
            // IQueryable ile başlangıç
            var query = _context.Emlaklar.AsQueryable();

            // Arama
            if (!string.IsNullOrEmpty(search))
            {
                // Adres, Başlık veya OdaSayısı içinde arama yapılabilir
                query = query.Where(e =>
                    e.Baslik.Contains(search)
                    || e.Adres.Contains(search)
                    || e.OdaSayisi.Contains(search)
                );
            }

            // Tip filtreleme (Satılık / Kiralık)
            if (!string.IsNullOrEmpty(tip) && (tip == "Satılık" || tip == "Kiralık"))
            {
                query = query.Where(e => e.Tip == tip);
            }

            // Emlak tipi filtreleme (Daire, Villa, vb)
            if (!string.IsNullOrEmpty(emlakTipi) && emlakTipi != "Emlak Tipi...")
            {
                query = query.Where(e => e.EmlakTipi == emlakTipi);
            }

            // Toplam kayıt sayısı (sayfalama için)
            int totalCount = query.Count();

            // Sayfalama (skip, take)
            var emlaklar = query
                .OrderByDescending(e => e.IlanTarihi)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Sayfalama için ViewBag ya da ViewModel
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;

            return View(emlaklar);
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var seciliEmlak = _context
                .Emlaklar.Include(e => e.Gorseller)
                .FirstOrDefault(e => e.Id == id);

            if (seciliEmlak == null)
                return NotFound();

            var benzerler = _context
                .Emlaklar.Where(e => e.Id != id && e.Tip == seciliEmlak.Tip)
                .OrderByDescending(e => e.IlanTarihi)
                .Take(2)
                .ToList();

            var model = new EmlakDetayViewModel { Emlak = seciliEmlak, BenzerEmlaklar = benzerler };

            return View(model);
        }
    }
}
