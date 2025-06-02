



using System.ComponentModel.DataAnnotations;
namespace EmlakTakip.ViewModel;

public class LoginViewModel
{
  [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Sifre { get; set; }
}
