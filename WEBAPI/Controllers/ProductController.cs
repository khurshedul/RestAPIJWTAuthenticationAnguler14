using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.EMS.Entities;
using Core.EMS.Interfaces;
using Infra.EMS.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace WEBAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("ProductsList")]
        public async Task<ActionResult> Getproducts()
        {
            return Ok(await _productService.ListAsync());
        }

        [HttpGet("ProductDetail")]
        public async Task<IActionResult> Getproduct(int? id)
        {
            var product = await _productService.FirstOrDefaultAsync(s => s.ID == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> Putproduct(int? id, Product product)
        {
            if (id != product.ID)
            {
                return BadRequest();
            }


            var res = await _productService.UpdateProduct(product);



            return Ok(res);
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> Postproduct(Product product)
        {

            var res = await _productService.Add(product);
            return Ok(res);


        }

        [HttpPost("DeleteProduct")]
        public async Task<IActionResult> Deleteproduct(int id)
        {
            var product = await _productService.GetProductByIDAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.PermanentDelete(product);
            return Ok(product);
        }

        private async Task<bool> productExists(int? id)
        {
            var res = await _productService.ListAsync(s => s.ID == id);
            return res.Any();
        }
    }
}
