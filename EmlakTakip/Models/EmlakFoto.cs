


// Models/EmlakFoto.cs
using EmlakTakip.Models;

namespace EmlakTakip.Models; 

public class EmlakFoto
{
    public int Id { get; set; }
    public string DosyaYolu { get; set; }

    public int EmlakId { get; set; }
    public Emlak Emlak { get; set; }
}
