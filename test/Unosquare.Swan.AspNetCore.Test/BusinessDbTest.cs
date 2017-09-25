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
            var user = new SampleUser();

            using (var context = new BusinessDbContextMock(options, _httpAccessor))
            {
                var userCount = await context.Users.CountAsync();
                var managerCount = await context.Manager.CountAsync();

                Assert.AreNotEqual(0, userCount, "Zero user entities");
                Assert.AreNotEqual(0, managerCount, "Zero manager entities");

                var businessController = new BusinessRulesControllerTest(context);
                businessController.RunBusinessRules();

                userCount = await context.Users.CountAsync();
                managerCount = await context.Manager.CountAsync();

                Assert.AreEqual(1, userCount, "One user entity");
                Assert.AreEqual(1, managerCount, "One manager entity");

                await context.SaveChangesAsync();
            }
        }
    }
}
