using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UnityVentures.Models;

namespace UnityVentures.Data
{
    public class UnityVenturesDbContext : IdentityDbContext
    {
        public UnityVenturesDbContext(DbContextOptions<UnityVenturesDbContext> options): base(options)
        {
        }

        public DbSet<Business> Businesses { get; set; }
        public DbSet<Transaction> transactions { get; set; }
        public DbSet<Loan> Loans { get; set; }
    }
}
