


using EmlakTakip.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmlakTakip.Data;


public class UygulamaDbContext : IdentityDbContext
{
    public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options) { }

    public DbSet<Emlak> Emlaklar { get; set; }
    public DbSet<EmlakFoto> EmlakFotolar { get; set; }
    public DbSet<Kullanicilar> Kullanicilars { get; set; }
}
