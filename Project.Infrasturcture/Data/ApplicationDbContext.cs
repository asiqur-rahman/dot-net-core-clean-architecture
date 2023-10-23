using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project.Core.Entities.Common.Role;
using Project.Core.Entities.Common.User;

namespace Project.Infrasturcture.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        private readonly ILoggerFactory _loggerFactory;
        protected ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            this._loggerFactory = loggerFactory;
        }

        //#region COMMON DBSETS
        //public DbSet<Point> Branches { get; set; }
        //public DbSet<Country> Countries { get; set; }
        

        //#endregion

        //#region AGENT DBSETS
        //public DbSet<CustomerInformation> CustomerInformation { get; set; }
        
        //#endregion

        

        //#region DBQUERY
        //public DbQuery<AgentPointViewModel> AgentPointQuery { get; set; }
        //public DbQuery<BranchAndAccountTagging> BranchAndAccountTagging { get; set; }
        //public DbQuery<VmCustomerInformation> VmCustomerInformation { get; set; }
        //public DbQuery<VmAccountInformation> VmAccountInformation { get; set; }
        //#endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
