using H1_ERP.DataBase;
using H1_ERP.DomainModel;
using H1_ERP.Interfaces_s;

namespace W.A.N.K_API.Repostoriy
{
    public class CustomerRepos : IRepos<Customer>
    {
        private DataBase _data = new DataBase(); 

        public void Delete(int id)
        {
            try
            {
                _data.Delete(id);
            }catch(Exception ex)
            {

            }
        }

        public List<Customer> GetAll()
        {
            try
            {
                return _data.GetAllCustomers();
            }
            catch (Exception ex)
            {
                return null; 
            }
        }

        public Customer GetFromID(int id)
        {
            try
            {
                return _data.GetCustomerFromID(id);
            }
            catch(Exception e)
            {
                return null; 
            }
        }

        public void Insert(Customer entity)
        {
            try
            {
                _data.InsertCustomer(entity);
            }
            catch(Exception e)
            {
               
            }
        }

        public void Update(Customer enity)
        {
            try
            {
                _data.UpdateCustomer(enity);
            }catch(Exception e)
            {

            }
        }
    }
}
