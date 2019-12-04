namespace Swan.AspNetCore.Test.Mocks
{
    internal class BusinessRulesControllerTest : BusinessRulesController<BusinessDbContextMock>
    {
        public BusinessRulesControllerTest(BusinessDbContextMock instance)
            : base(instance)
        {
        }

        [BusinessRule(typeof(ProductMock), ActionFlags.Create)]
        public void CreateProduct(ProductMock product)
        {
            product.ActionFlag = "Create";
        }

        [BusinessRule(typeof(ProductMock), ActionFlags.Update)]
        public void UpdateProduct(ProductMock product)
        {
            product.ActionFlag = "Update";
        }
        [BusinessRule(typeof(ProductMock), ActionFlags.Delete)]
        public void DeleteProduct(ProductMock product)
        {
            product.ActionFlag = "Delete";
        }

    }
}
