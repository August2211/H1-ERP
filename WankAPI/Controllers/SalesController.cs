using H1_ERP.DataBase;
using H1_ERP.DomainModel;
using Microsoft.AspNetCore.Mvc;
using W.A.N.K_API.Repostoriy;

namespace WankAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : Controller
    {
        private readonly SalesRepository _dataBase = new SalesRepository();

        private readonly ILogger<SalesController> _logger;
        public SalesController(ILogger<SalesController> logger)
        {
            _logger = logger;
        }
        [HttpGet(Name = "GetOrders")]
        public ActionResult<IEnumerable<SalesOrderHeader>> Get()
        {
            try
            {
                var Sales = _dataBase.GetAll();
                return Ok(Sales);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public ActionResult<SalesOrderHeader> GetFromID(int id)
        {
            try
            {
                var Order = _dataBase.GetFromID(id);
                return Ok(Order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}", Name = "DeleteOrder")]
        public ActionResult<SalesOrderHeader> Delete(int id)
        {
            try
            {
                _dataBase.Delete(id);
                return Ok("The Order with the id of " + id + " has been deleted sucessfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut(Name = "InsertOrder")]
        public ActionResult<SalesOrderHeader> Post([FromBody] SalesOrderHeader Order)
        {
            if (Order == null)
            {
                return BadRequest("SWINE !");
            }
            else
            {
                _dataBase.Insert(Order);
                return Ok(Order.OrderID);
            }

        }
        [HttpPost(Name = "UpdateOrder")]
        public ActionResult<SalesOrderHeader> Update([FromBody] SalesOrderHeader Order)
        {
            try
            {
                _dataBase.Update(Order);
                return Ok(Order.OrderID);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
