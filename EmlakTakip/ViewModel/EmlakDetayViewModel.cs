using EmlakTakip.Models;

namespace EmlakTakip.ViewModel;

public class EmlakDetayViewModel
{
    public Emlak Emlak { get; set; }
    public List<Emlak> BenzerEmlaklar { get; set; }
}
