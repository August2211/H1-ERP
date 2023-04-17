using H1_ERP.DataBase;
using H1_ERP.DomainModel;
using Microsoft.AspNetCore.Mvc;

namespace WankAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly DataBase _dataBase = new DataBase();

        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        [HttpGet (Name ="GetCustomers")]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            var customer = _dataBase.GetAllCustomers(); 
            return Ok(customer); 
        }
    }
}