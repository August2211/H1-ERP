using H1_ERP.DataBase;
using H1_ERP.DomainModel;
using Microsoft.AspNetCore.Mvc;
using W.A.N.K_API.Repostoriy;

namespace WankAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerRepos _dataBase = new CustomerRepos();

        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }
        [HttpGet (Name ="GetCustomers")]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            var customer = _dataBase.GetAll(); 
            return Ok(customer); 
        }
        [HttpGet ("{id}",Name ="GetCustomerFromID")]
        public ActionResult GetCustomerFromID(int id)
        {
            var res = _dataBase.GetFromID(id);
            return Ok(res);
        }
        [HttpPut (Name ="InsertCustomer")]
        public ActionResult<Customer> Post([FromBody] Customer customer)
        {
            if(customer == null)
            {
                return BadRequest("SWINE !");
            }
            else
            {
                _dataBase.Insert(customer);
                return Ok(customer); 
            } 
        }
        [HttpDelete ("{id}",Name ="DeleteCustomer")]
        public ActionResult<Customer> Delete(int id)
        {
            try 
            { 
                if(id == 0)
                {
                    return BadRequest("SWINE !");
                }
                _dataBase.Delete(id);
                return Ok("The customer with the id of " + id +" has been deleted sucessfully");
            }catch(Exception ex)
            {
                return BadRequest(ex); 
            }
        }
        [HttpPost(Name = "UpdateCustomer")]
        public ActionResult<Customer> Update([FromBody] Customer customer)
        {
            try
            {
                _dataBase.Update(customer);
                return Ok(customer.CustomerId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}