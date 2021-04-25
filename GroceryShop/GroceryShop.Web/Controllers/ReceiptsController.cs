namespace GroceryShop.Web.Controllers
{
    using GroceryShop.Services.Data;
    using GroceryShop.Web.ViewModels.Receipts;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class ReceiptsController : ControllerBase
    {
        private readonly IReceiptsService receiptsService;

        public ReceiptsController(IReceiptsService receiptsService)
        {
            this.receiptsService = receiptsService;
        }

        // GET: api/receipts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptViewModel>>> GetReceipts(int count = 5)
        {
            var receiptViews = await this.receiptsService.GetAllAsync<ReceiptViewModel>(count);

            return this.Ok(receiptViews);
        }

        // GET api/receipts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptViewModel>> GetReceipt(int id)
        {
            var receiptView = await this.receiptsService.GetByIdAsync<ReceiptViewModel>(id);

            return this.Ok(receiptView);
        }

        // POST api/receipts
        [HttpPost]
        public async Task<ActionResult<ReceiptViewModel>> PostReceipt(string[] scannedProducts)
        {
            var receiptView = await this.receiptsService.CreateReceiptAsync(scannedProducts);

            return this.CreatedAtAction("GetReceipt", new { id = receiptView.Id }, receiptView);
        }
    }
}
