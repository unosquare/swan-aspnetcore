namespace Swan.AspNetCore.Test.Mocks
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ProductMock
    {
        [Key]
        public int ProductId { get; set; }

        public string Name { get; set; }
        public string ActionFlag { get; set; }
        
        public static ProductMock GetProduct()
        {
            var r = new Random().Next(0,100);
            return new ProductMock()
            {
                ProductId = r,
                Name = "Product" + r,
                ActionFlag = ""
            };
        }
    }
}
