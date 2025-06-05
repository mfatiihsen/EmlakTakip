

using EmlakTakip.Models;

namespace EmlakTakip.ViewModel;

public class DashboardViewModel
{
    public int ToplamEmlak { get; set; }
    public int SatilikIlanSayisi { get; set; }
    public int KiralikIlanSayisi { get; set; }
    public int KullaniciSayisi { get; set; }
    public List<Emlak> SonEklenenIlanlar { get; set; }
    public Dictionary<string, int> EmlakTipleri { get; set; }
    public Dictionary<string, int> IlanTipleri { get; set; }

}
