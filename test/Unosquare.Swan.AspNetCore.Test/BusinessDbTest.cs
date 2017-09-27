namespace Unosquare.Swan.AspNetCore.Test
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Unosquare.Swan.AspNetCore.Test.Mocks;

    [TestFixture]
    class BusinessDbTest
    {
        private readonly DbContextOptions<BusinessDbContextMock> options;

        public BusinessDbTest()
        {
            var builder = new DbContextOptionsBuilder<BusinessDbContextMock>()
                .UseInMemoryDatabase("TestDb");
            options = builder.Options;
        }

        [Test]
        public void ContainsControllerTest()
        {         
            using (var context = new BusinessDbContextMock(options))
            {
                var businessController = new BusinessRulesControllerTest(context);

                var controller = context.ContainsController(businessController);
                Assert.IsFalse(controller);
            }
        }

        [Test]
        public void AddControllerTest()
        {         
            using (var context = new BusinessDbContextMock(options))
            {
                var businessController = new BusinessRulesControllerTest(context);
                context.AddController(businessController);

                var controller = context.ContainsController(businessController);
                Assert.IsTrue(controller);
            }
        }

        [Test]
        public void RemoveControllerTest()
        {
            using (var context = new BusinessDbContextMock(options))
            {
                var businessController = new BusinessRulesControllerTest(context);

                context.AddController(businessController);
                context.RemoveController(businessController);

                var controller = context.ContainsController(businessController);
                Assert.IsFalse(controller);
            }
        }

        [Test]
        public async Task RunBussinesRulesControllerTest()
        {
            // TODO: Rewrite test to verify the product new name
            var product = new ProductMock();

            using (var context = new BusinessDbContextMock(options))
            {
                var productCount = await context.Products.CountAsync();

                Assert.AreNotEqual(0, productCount, "Zero user entities");

                var businessController = new BusinessRulesControllerTest(context);
                
                productCount = await context.Products.CountAsync();

                Assert.AreEqual(1, productCount, "One user entity");

                await context.SaveChangesAsync();
            }
        }
    }
}
