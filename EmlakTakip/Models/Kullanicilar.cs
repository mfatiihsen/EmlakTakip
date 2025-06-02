

using System.ComponentModel.DataAnnotations;

namespace EmlakTakip.Models
{
    public class Kullanicilar
    {
        public int Id { get; set; }
        
    [Required(ErrorMessage = "Ad alanı zorunludur.")]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "Kullanıcı Adı alanı zorunludur.")]       
  public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
         [Required(ErrorMessage = "Email alanı zorunludur.")]
        public string Email { get; set; }
        public DateTime KayitTarihi { get; set; } = DateTime.Now;
    }
}