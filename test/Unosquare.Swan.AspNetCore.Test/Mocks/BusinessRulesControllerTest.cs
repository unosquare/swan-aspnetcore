namespace Unosquare.Swan.AspNetCore.Test.Mocks
{
    using System;

    class BusinessRulesControllerTest : BusinessRulesController<BusinessDbContextMock>
    {
        public BusinessRulesControllerTest(BusinessDbContextMock instance)
            : base(instance)
        {
        }

        [BusinessRule(typeof(ProductMock), ActionFlags.Create)]
        public void UpdateProduct(ProductMock product)
        {
            product.Name += " (VALID)";
        }
    }
}
