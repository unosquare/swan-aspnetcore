namespace Unosquare.Swan.AspNetCore.Test.Mocks
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Unosquare.Swan.AspNetCore.Models;

    class BusinessDbContextMock : BusinessDbContext
    {
        public BusinessDbContextMock(DbContextOptions<BusinessDbContextMock> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            var auditController = new AuditTrailController<BusinessDbContextMock, AuditTrailMock>(this,
                httpContextAccessor?.HttpContext?.User?.Identity?.Name);
            auditController.AddTypes(ActionFlags.Create, new[] { typeof(SampleUser) });

            AddController(auditController);
        }

        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<AuditTrailMock> AuditTrailEntries { get; set; }
        public DbSet<SampleUser> Users { get; set; }
        public DbSet<SampleManager> Manager { get; set; }
    }
}
