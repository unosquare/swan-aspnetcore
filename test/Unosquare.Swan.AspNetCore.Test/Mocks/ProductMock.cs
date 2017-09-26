namespace Unosquare.Swan.AspNetCore.Test.Mocks
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ProductMock
    {
        [Key]
        public int ProductID { get; set; }

        public string Name { get; set; }

        private readonly Random rnd = new Random();

        public ProductMock GetProduct()
        {
            var r = rnd.Next(0,100);
            return new ProductMock()
            {
                ProductID = r,
                Name = "Product" + r
            };
        }
    }
}
