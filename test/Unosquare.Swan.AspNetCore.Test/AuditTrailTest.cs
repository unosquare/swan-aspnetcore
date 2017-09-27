namespace Unosquare.Swan.AspNetCore.Test
{
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Linq;
    using System.Threading.Tasks;
    using Unosquare.Swan.AspNetCore.Test.Mocks;

    [TestFixture]
    class AuditTrailTest
    {
        private DbContextOptions<BusinessDbContextMock> options;
        private ProductMock product;
        int count;

        [SetUp]
        public void SetUp()
        {
            var builder = new DbContextOptionsBuilder<BusinessDbContextMock>()
                .UseInMemoryDatabase("AuditTestDb");
            options = builder.Options;
            product = new ProductMock().GetProduct();
        }
    
        [Test]
        public async Task SaveChangesAsyncEntityTest()
        {
            using (var context = new BusinessDbContextMock(options))
            {
                context.Products.Add(product);
                await context.SaveChangesAsync();
                count++;

                var auditTrail = context.AuditTrailEntries.Where(x => x.UserId == "Admin");
                var action = auditTrail.FirstOrDefault(x => x.AuditId == count);

                Assert.IsTrue(context.AuditTrailEntries.Any());
                Assert.AreEqual(count, auditTrail.Count());
                Assert.AreEqual(1, action.Action, "Where 1 means Create");
            }
        }

        [Test]
        public void SaveChangesEntityTest()
        {
            using (var context = new BusinessDbContextMock(options))
            {
                context.Products.Add(product);
                context.SaveChanges();
                count++;

                var auditTrail = context.AuditTrailEntries.Where(x => x.UserId == "Admin");
                var action = auditTrail.FirstOrDefault(x => x.AuditId == count);

                Assert.IsTrue(context.AuditTrailEntries.Any());
                Assert.AreEqual(count, auditTrail.Count());
                Assert.AreEqual(1, action.Action, "Where 1 means Create");
            }
        }

        [Test]
        public void UpdatedChangesEntityTest()
        {
            using (var context = new BusinessDbContextMock(options))
            {
                context.Products.Add(product);
                context.SaveChanges();
                count++;

                context.Update(product);
                context.SaveChanges();
                count++;

                var auditTrail = context.AuditTrailEntries.Where(x => x.UserId == "Admin");
                var action = auditTrail.FirstOrDefault(x => x.AuditId == count);

                Assert.IsTrue(context.AuditTrailEntries.Any());
                Assert.AreEqual(count, auditTrail.Count());
                Assert.AreEqual(2, action.Action, "Where 2 means Update");
            }
        }

        [Test]
        public void DeleteChangesEntityTest()
        {
            using (var context = new BusinessDbContextMock(options))
            {
                context.Products.Add(product);
                context.SaveChanges();
                count++;

                context.Remove(product);
                context.SaveChanges();
                count++;

                var auditTrail = context.AuditTrailEntries.Where(x => x.UserId == "Admin");
                var action = auditTrail.FirstOrDefault(x => x.AuditId == count);

                Assert.IsTrue(context.AuditTrailEntries.Any());
                Assert.AreEqual(count, auditTrail.Count());
                Assert.AreEqual(3, action.Action, "Where 3 means Delete");
            }
        }
    }
}
