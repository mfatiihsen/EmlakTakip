
namespace EmlakTakip.Models;
public class Emlak
{
    public int Id { get; set; }

    // Genel Bilgiler
    public string Baslik { get; set; }
    public string Aciklama { get; set; }
    public string Tip { get; set; } // Satılık / Kiralık
    public decimal Fiyat { get; set; }
    public string Adres { get; set; }

    // Konum için (Google Maps)
    public double? Enlem { get; set; }
    public double? Boylam { get; set; }

    // Fiziksel Özellikler
    public int Metrekare { get; set; }
    public string OdaSayisi { get; set; } // Örn: 2+1
    public string IsitmaTipi { get; set; }
    public int BinaYasi { get; set; }
    public int BulunduguKat { get; set; }
    public int KatSayisi { get; set; }
    public bool EsyaliMi { get; set; }
    public int BanyoSayisi { get; set; }
    public bool Balkon { get; set; }
    public bool SiteIciMi { get; set; }
    public bool KrediliMi { get; set; }

    // Diğer
   public ICollection<EmlakFoto> Gorseller { get; set; }
    public DateTime IlanTarihi { get; set; } = DateTime.Now;
}
