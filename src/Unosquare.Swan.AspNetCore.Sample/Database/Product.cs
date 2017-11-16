namespace Unosquare.Swan.AspNetCore.Sample.Database
{
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        public string Name { get; set; }
    }
}
