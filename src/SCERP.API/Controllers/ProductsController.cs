using SCERP.API.Models;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Model.Production;
using System.Web.Http;

namespace SCERP.API.Controllers
{
    [RoutePrefix(GTexRoutePrefix.Products)]
    public class ProductsController : BaseApiController
    {
        private readonly ICuttingBatchManager cuttingBatchManager;
        public ProductsController(ICuttingBatchManager cuttingBatchManager)
        {
            this.cuttingBatchManager = cuttingBatchManager;
        }
        ProductModel[] products = new ProductModel[]
        {
            new ProductModel { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
            new ProductModel { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
            new ProductModel { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
        };
        [HttpGet]
        [Route("get-products")]
        public IHttpActionResult GetProducts()
        {
            var compId = base.CompId;
            var userName = base.UserName;
            var userId = base.UserId;
            return Ok(new ResponseMessage<ProductModel[]>
            {
                Result = products
            });
           
        }
        [HttpGet]
        [Route("get-product/{id}")]
        public IHttpActionResult GetProduct(long id)
        {
            var product = cuttingBatchManager.GetCuttingBatchById(id);
            return Ok(new ResponseMessage<PROD_CuttingBatch>
            {
                Result = product
            });
        }
    }
}
