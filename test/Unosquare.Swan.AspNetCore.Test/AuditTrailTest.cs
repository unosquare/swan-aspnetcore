namespace Unosquare.Swan.AspNetCore.Test
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Unosquare.Swan.AspNetCore.Test.Mocks;

    [TestFixture]
    class AuditTrailTest
    {
        private readonly DbContextOptions<BusinessDbContextMock> options;
        private HttpContextAccessor _httpAccessor = new HttpContextAccessor();
        private int count;
        private ProductMock product;

        public AuditTrailTest()
        {
            var builder = new DbContextOptionsBuilder<BusinessDbContextMock>()
                .UseInMemoryDatabase("AuditTestDb");
            options = builder.Options;

            var Identity = new ApplicationUserMock().GetAdmin();
            _httpAccessor.HttpContext = new DefaultHttpContext();
            _httpAccessor.HttpContext.User.AddIdentity(Identity);
        }

        [SetUp]
        public void SetUp()
        {
            product = new ProductMock().GetProduct();
        }
    
        [Test]
        public async Task SaveChangesAsyncEntityTest()
        {
            using (var context = new BusinessDbContextMock(options, _httpAccessor))
            {
                context.Products.Add(product);

                await context.SaveChangesAsync();
                count++;

                Assert.IsNotEmpty(context.AuditTrailEntries);

                Assert.AreEqual(count, context.AuditTrailEntries.Local.Count);

            }
        }

        [Test]
        public void SaveChangesEntityTest()
        {
            using (var context = new BusinessDbContextMock(options, _httpAccessor))
            {
                context.Products.Add(product);

                context.SaveChanges();
                count++;

                Assert.IsNotEmpty(context.AuditTrailEntries);

                Assert.AreEqual(count, context.AuditTrailEntries.Local.Count);

            }
        }
    }
}
