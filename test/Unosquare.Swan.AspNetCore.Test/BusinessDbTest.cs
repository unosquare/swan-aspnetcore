namespace Unosquare.Swan.AspNetCore.Test
{
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Mocks;

    [TestFixture]
    public class BusinessDbTest
    {
        private static BusinessDbContextMock SetupDatabase(string name)
        {
            var builder = new DbContextOptionsBuilder<BusinessDbContextMock>()
               .UseInMemoryDatabase(name);
            var options = builder.Options;
            return new BusinessDbContextMock(options);
        }

        [Test]
        public void ContainsControllerTest()
        {
            using (var context = SetupDatabase(nameof(ContainsControllerTest)))
            {
                var businessController = new BusinessRulesControllerTest(context);

                var controller = context.ContainsController(businessController);
                Assert.IsFalse(controller);
            }
        }

        [Test]
        public void AddControllerTest()
        {
            using (var context = SetupDatabase(nameof(AddControllerTest)))
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
            using (var context = SetupDatabase(nameof(RemoveControllerTest)))
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
            var product = ProductMock.GetProduct();

            using (var context = SetupDatabase(nameof(CreateEntityControllerTest)))
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
            var product = ProductMock.GetProduct();

            using (var context = SetupDatabase(nameof(UpdateEntityControllerTest)))
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
            var product = ProductMock.GetProduct();

            using (var context = SetupDatabase(nameof(DeleteEntityControllerTest)))
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
