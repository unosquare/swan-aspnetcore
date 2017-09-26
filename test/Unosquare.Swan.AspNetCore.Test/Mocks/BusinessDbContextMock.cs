namespace Unosquare.Swan.AspNetCore.Test.Mocks
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Unosquare.Swan.AspNetCore.Models;

    class BusinessDbContextMock : BusinessDbContext
    {
        public BusinessDbContextMock(DbContextOptions<BusinessDbContextMock> options)
            : base(options)
        {
            var auditController = new AuditTrailController<BusinessDbContextMock, AuditTrailMock>(this,
                "Admin");
            auditController.AddTypes(ActionFlags.Create, new[] { typeof(ProductMock) });

            AddController(auditController);
        }

        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<AuditTrailMock> AuditTrailEntries { get; set; }
        public DbSet<ProductMock> Products { get; set; }
    }
}
