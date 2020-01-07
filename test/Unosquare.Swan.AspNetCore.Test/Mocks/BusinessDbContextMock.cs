using Swan.AspNetCore.Models;

namespace Swan.AspNetCore.Test.Mocks
{
    using Microsoft.EntityFrameworkCore;

    internal class BusinessDbContextMock : BusinessDbContext
    {
        public BusinessDbContextMock(DbContextOptions<BusinessDbContextMock> options)
            : base(options)
        {
            var auditController = new CustomAuditTrailController(this, "Admin");
            auditController.AddTypes(ActionFlags.Create, new[] { typeof(ProductMock) });

            AddController(auditController);
        }

        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<AuditTrailMock> AuditTrailEntries { get; set; }
        public DbSet<ProductMock> Products { get; set; }
    }
}
