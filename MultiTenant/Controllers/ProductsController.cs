using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiTenant.Services;

namespace MultiTenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> AllProudcts()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ProductById(int id)
        {
            var product = _productService.GetByIdAsync(id);
            if (product is null) return BadRequest("Invalid Data!");
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Product product)
        {
            await _productService.CreateAsync(product);
            return Ok(product);
        }

    }
}
