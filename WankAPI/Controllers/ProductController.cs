using H1_ERP.DataBase;
using H1_ERP.DomainModel;
using Microsoft.AspNetCore.Mvc;
using W.A.N.K_API.Repostoriy;

namespace WankAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductrRpository _dataBase = new ProductrRpository();

        private readonly ILogger<ProductController> _logger;
        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }
        [HttpGet(Name = "GetProducts")]
        public ActionResult<IEnumerable<Product>> Get()
        {
            try
            {
                var Product = _dataBase.GetAll();
                return Ok(Product);
            }
            catch(Exception ex)
            {
                _logger.LogError(exception: ex, message: "");
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}",Name = "GetProduct")]
        public ActionResult<Product> GetFromID(int id)
        {
            try
            {
                var Product = _dataBase.GetFromID(id);
                return Ok(Product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}", Name = "DeleteProduct")]
        public ActionResult<Product> Delete(int id)
        {
            try
            {
                _dataBase.Delete(id);
                return Ok("The Product with the id of " + id + " has been deleted sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut(Name = "InsertProduct")]
        public ActionResult<Product> Post([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("SWINE !");
            }
            else
            {
                _dataBase.Insert(product);
                return Ok(product);
            }
        
        }
        [HttpPost(Name = "UpdateProduct")]
        public ActionResult<Product> Update([FromBody] Product product)
        {
            try
            {
                _dataBase.Update(product);
                return Ok(product.ProductId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
