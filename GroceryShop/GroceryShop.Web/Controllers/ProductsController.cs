namespace GroceryShop.Web.Controllers
{
    using GroceryShop.Services.Data;
    using GroceryShop.Web.ViewModels.Products;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProducts(int count = 10)
        {
            var productViews = await this.productsService.GetAllAsync<ProductViewModel>(count);

            return this.Ok(productViews);
        }

        // GET api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductViewModel>> GetProduct(int id)
        {
            var productView = await this.productsService.GetByIdAsync<ProductViewModel>(id);

            return this.Ok(productView);
        }

        // POST api/products
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> PostProducts(ProductCreateInputModel[] inputModels)
        {
            var productViews = await this.productsService.CreateAsync(inputModels);

            return this.CreatedAtAction("GetProducts", productViews);
        }

        // PUT api/products/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, ProductUpdateModel updateModel)
        {
            await this.productsService.UpdateAsync(id, updateModel.Name, updateModel.Price);

            return this.NoContent();
        }

        // DELETE api/products/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await this.productsService.DeleteAsync(id);

            return this.NoContent();
        }
    }
}
