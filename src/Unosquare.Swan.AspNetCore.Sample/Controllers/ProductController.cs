﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swan.AspNetCore.Sample.Database;

namespace Swan.AspNetCore.Sample.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly SampleDbContext _context;

        public ProductController(SampleDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get() =>
            Ok(new List<Product>()
            {
                new Product { Name = "Gatorade" },
                new Product { Name = "Red Bull"},
                new Product { Name = "Powerade"},
                new Product { Name = "Electrolit" }
            });

        [HttpPost]
        public IActionResult Post([FromBody] Product[] values)
        {
            _context.Products.AddRange(values);

            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductID == id);
            
            if (product == null)
                return NotFound();

            product.Name = "Coca";

            _context.SaveChanges();
            return  Ok(product);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductID == id);

            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();
            return Ok(product);
        }
    }
}
