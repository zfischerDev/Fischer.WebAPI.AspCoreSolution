using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#region Added NuGet Entity Framework libraries
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
#endregion

namespace Fischer.WebAPI.AspCoreSolution.PolarisLibraries.Objects
{
    public class PolarisEFContext : DbContext
    {
        private readonly string _connectionString;
        public PolarisEFContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        public DbSet<PolarisAccountHolder> PolarisAccounts { get; set; }
        public DbSet<PolarisTransaction> PolarisTransactions { get; set; }
    }
}
