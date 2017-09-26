namespace Unosquare.Swan.AspNetCore.Test.Mocks
{
    using System;

    class BusinessRulesControllerTest : IBusinessRulesController
    {
        public BusinessDbContextMock Context { get; protected set; }

        public BusinessRulesControllerTest()
        {
        }

        public BusinessRulesControllerTest(BusinessDbContextMock context)
        {
            Context = context;
        }

        public void RunBusinessRules()
        {
            var product = new ProductMock().GetProduct();
            Context.Add(product);
        }
    }
}
