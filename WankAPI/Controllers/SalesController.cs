using H1_ERP.DataBase;
using H1_ERP.DomainModel;
using Microsoft.AspNetCore.Mvc;

namespace WankAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : Controller
    {
        private readonly DataBase _dataBase = new DataBase();

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
                var Order = _dataBase.GetSalesOrderHeaderFromID(id);
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
                _dataBase.DeleteSalesOrderHeaderFromID(id);
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
                _dataBase.InsertSalesOrderHeader(Order);
                return Ok(Order.OrderID);
            }

        }
        [HttpPost(Name = "UpdateOrder")]
        public ActionResult<SalesOrderHeader> Update([FromBody] SalesOrderHeader Order)
        {
            try
            {
                _dataBase.UpdateSalesorderHeader((int)Order.OrderID,Order);
                return Ok(Order.OrderID);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
