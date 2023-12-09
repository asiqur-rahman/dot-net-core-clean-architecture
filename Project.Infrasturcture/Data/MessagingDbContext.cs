using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project.Core.Entities.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrasturcture.Data
{

    public class MessagingDbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        protected MessagingDbContext(DbContextOptions options) : base(options)
        {
        }

        public MessagingDbContext(DbContextOptions<MessagingDbContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            this._loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Notification> Notifications { get; set; }
    }
}
