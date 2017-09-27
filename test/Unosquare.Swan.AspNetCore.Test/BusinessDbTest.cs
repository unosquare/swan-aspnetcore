namespace Unosquare.Swan.AspNetCore.Test
{
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Linq;
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
        public async Task CreateEntityControllerTest()
        {
            var product = new ProductMock().GetProduct();

            using (var context = new BusinessDbContextMock(options))
            {
                var businessController = new BusinessRulesControllerTest(context);

                context.AddController(businessController);
                context.Add(product);

                await context.SaveChangesAsync();

                Assert.AreEqual("Create", product.ActionFlag);
            }
        }

        [Test]
        public async Task UpdateEntityControllerTest()
        {
            var product = new ProductMock().GetProduct();

            using (var context = new BusinessDbContextMock(options))
            {
                var businessController = new BusinessRulesControllerTest(context);

                context.AddController(businessController);
                context.Add(product);
                await context.SaveChangesAsync();

                context.Update(product);

                await context.SaveChangesAsync();

                Assert.AreEqual("Update", product.ActionFlag);
            }
        }

        [Test]
        public async Task DeleteEntityControllerTest()
        {
            var product = new ProductMock().GetProduct();

            using (var context = new BusinessDbContextMock(options))
            {
                var businessController = new BusinessRulesControllerTest(context);

                context.AddController(businessController);
                context.Add(product);
                await context.SaveChangesAsync();

                context.Remove(product);

                await context.SaveChangesAsync();

                Assert.AreEqual("Delete", product.ActionFlag);
            }
        }
    }
}
