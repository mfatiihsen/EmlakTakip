

using System.ComponentModel.DataAnnotations;

namespace EmlakTakip.ViewModel;
public class SifreDegistirModel
{
    [Required(ErrorMessage = "Mevcut şifre gereklidir")]
    [DataType(DataType.Password)]
    [Display(Name = "Mevcut Şifre")]
    public string MevcutSifre { get; set; }

    [Required(ErrorMessage = "Yeni şifre gereklidir")]
    [StringLength(100, ErrorMessage = "Şifre en az {2} karakter olmalıdır.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Yeni Şifre")]
    public string YeniSifre { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Yeni Şifre Tekrar")]
    [Compare("YeniSifre", ErrorMessage = "Şifreler uyuşmuyor.")]
    public string YeniSifreTekrar { get; set; }
}