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
            if (count <= 0 || count > 50)
            {
                return this.BadRequest();
            }

            var productViews = await this.productsService.GetAllAsync<ProductViewModel>(count);

            return this.Ok(productViews);
        }

        // GET api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductViewModel>> GetProduct(int id)
        {
            var productView = await this.productsService.GetByIdAsync<ProductViewModel>(id);

            if (productView == null)
            {
                return this.NotFound();
            }

            return this.Ok(productView);
        }

        // POST api/products
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> PostProducts(ProductCreateInputModel[] inputModels)
        {
            if (inputModels.Length == 0)
            {
                return this.BadRequest();
            }

            var productViewModels = new List<ProductViewModel>();

            foreach (var input in inputModels)
            {
                var productId = await this.productsService.CreateAsync(input.Name, input.Price);

                productViewModels.Add(new ProductViewModel
                {
                    Id = productId,
                    Name = input.Name,
                    Price = input.Price,
                });
            }

            return this.CreatedAtAction("GetProducts", productViewModels);
        }

        // PUT api/products/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, ProductUpdateModel updateModel)
        {
            var product = await this.productsService.GetByIdAsync<ProductViewModel>(id);

            if (product == null)
            {
                return this.NotFound();
            }

            await this.productsService.UpdateAsync(id, updateModel.Name, updateModel.Price);

            return this.NoContent();
        }

        // DELETE api/products/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await this.productsService.GetByIdAsync<ProductViewModel>(id);

            if (product == null)
            {
                return this.NotFound();
            }

            await this.productsService.DeleteAsync(id);

            return this.NoContent();
        }
    }
}
