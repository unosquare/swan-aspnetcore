namespace Unosquare.Swan.AspNetCore.Test
{
    using Microsoft.AspNetCore.Http;
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

                Assert.IsTrue(context.AuditTrailEntries.Any());
            }
        }

        [Test]
        public void SaveChangesEntityTest()
        {
            using (var context = new BusinessDbContextMock(options))
            {
                context.Products.Add(product);

                context.SaveChanges();

                Assert.IsNotEmpty(context.AuditTrailEntries);
                Assert.Greater(context.AuditTrailEntries.Local.Count, 0);
            }
        }

        // TODO: Retrieve AuditTrail entity and check state

        [Test]
        public void UpdatedChangesEntityTest()
        {
            var newProductName = "New Product";
            using (var context = new BusinessDbContextMock(options))
            {
                context.Products.Add(product);

                context.SaveChanges();

                var findProduct = context.Products.Find(product.ProductID);

                Assert.IsNotEmpty(context.AuditTrailEntries);
                Assert.AreEqual(product, findProduct);
                Assert.Greater(context.AuditTrailEntries.Local.Count, 0);

                product.Name = newProductName;
                context.Update(product);

                context.SaveChanges();

                findProduct = context.Products.Find(product.ProductID);

                Assert.IsNotEmpty(context.AuditTrailEntries);
                Assert.AreEqual(newProductName, findProduct.Name);
                Assert.Greater(context.AuditTrailEntries.Local.Count, 0);
            }
        }

        [Test]
        public void DeleteChangesEntityTest()
        {
            using (var context = new BusinessDbContextMock(options))
            {
                context.Products.Add(product);

                context.SaveChanges();

                var findProduct = context.Products.Find(product.ProductID);

                Assert.IsNotEmpty(context.AuditTrailEntries);
                Assert.AreEqual(product, findProduct);
                Assert.Greater(context.AuditTrailEntries.Local.Count, 0);

                context.Remove(product);

                context.SaveChanges();

                findProduct = context.Products.Find(product.ProductID);

                Assert.IsNotEmpty(context.AuditTrailEntries);
                Assert.IsNull(findProduct);
                Assert.Greater(context.AuditTrailEntries.Local.Count, 0);
            }
        }
    }
}
