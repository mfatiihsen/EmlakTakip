


using EmlakTakip.Models;
using Microsoft.EntityFrameworkCore;

namespace EmlakTakip.Data;


public class UygulamaDbContext : DbContext
{
    public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options) { }

    public DbSet<Emlak> Emlaklar { get; set; }
    public DbSet<EmlakFoto> EmlakFotolar { get; set; }
    public DbSet<Kullanicilar> Kullanicilars { get; set; }
}
