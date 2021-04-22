namespace GroceryShop.Web.Controllers
{
    using GroceryShop.Services.Data;
    using GroceryShop.Web.ViewModels.Deals;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class DealsController : ControllerBase
    {
        private readonly IDealsService dealsService;

        public DealsController(IDealsService dealsService)
        {
            this.dealsService = dealsService;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DealViewModel>>> GetDeals(int count = 5)
        {
            var dealViews = await this.dealsService.GetAllAsync<DealViewModel>(count);

            return this.Ok(dealViews);
        }

        // GET api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DealViewModel>> GetDeal(int id)
        {
            var dealView = await this.dealsService.GetByIdAsync<DealViewModel>(id);

            return this.Ok(dealView);
        }

        // POST api/products
        [HttpPost]
        public async Task<ActionResult<DealViewModel>> PostDealProducts(int id, string[] productNames)
        {
            var dealView = await this.dealsService.AddProductsToDealAsync<DealViewModel>(id, productNames);
            
            return this.CreatedAtAction("GetDeal", new { id = dealView.Id }, dealView);
        }
    }
}
