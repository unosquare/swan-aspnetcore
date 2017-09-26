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
        private HttpContextAccessor _httpAccessor = new HttpContextAccessor();

        public BusinessDbTest()
        {
            var builder = new DbContextOptionsBuilder<BusinessDbContextMock>()
                .UseInMemoryDatabase("TestDb");
            options = builder.Options;
        }

        [Test]
        public async Task AddControllerTest()
        {         
            using (var context = new BusinessDbContextMock(options, _httpAccessor))
            {
                var businessController = new BusinessRulesControllerTest(context);

                var controller = context.ContainsController(businessController);
                Assert.IsFalse(controller);

                context.AddController(businessController);

                controller = context.ContainsController(businessController);
                Assert.IsTrue(controller);

                await context.SaveChangesAsync();
            }
        }

        [Test]
        public async Task RemoveControllerTest()
        {
            var businessController = new BusinessRulesControllerTest();

            using (var context = new BusinessDbContextMock(options, _httpAccessor))
            {
                var controller = context.ContainsController(businessController);
                Assert.IsFalse(controller);

                context.AddController(businessController);

                controller = context.ContainsController(businessController);
                Assert.IsTrue(controller);

                context.RemoveController(businessController);

                controller = context.ContainsController(businessController);
                Assert.IsFalse(controller);

                await context.SaveChangesAsync();
            }
        }

        [Test]
        public async Task RunBussinesRulesControllerTest()
        {
            var product = new ProductMock();

            using (var context = new BusinessDbContextMock(options, _httpAccessor))
            {
                var productCount = await context.Products.CountAsync();

                Assert.AreNotEqual(0, productCount, "Zero user entities");

                var businessController = new BusinessRulesControllerTest(context);
                businessController.RunBusinessRules();

                productCount = await context.Products.CountAsync();

                Assert.AreEqual(1, productCount, "One user entity");

                await context.SaveChangesAsync();
            }
        }
    }
}
