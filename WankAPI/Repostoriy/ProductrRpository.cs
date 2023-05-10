using H1_ERP.DataBase;
using H1_ERP.DomainModel;
using H1_ERP.Interfaces_s;

namespace W.A.N.K_API.Repostoriy
{
    public class ProductrRpository : IRepos<Product>
    {
        private DataBase _dataBase = new DataBase();
        public void Delete(int id)
        {
            try
            {
                _dataBase.DeleteProduct(id);
            }
            catch(Exception ex)
            {

            }
        }

        public List<Product> GetAll()
        {
            try
            {
                return _dataBase.GetAllProduct();
            }catch(Exception ex)
            {
                return null; 
            }
        }

        public Product GetFromID(int id)
        {
            try
            {
                return _dataBase.GetProductFromID(id);
            }
             catch(Exception ex)
            {
                return null; 

            }
        
        }

        public void Insert(Product entity)
        {
            try
            {
                _dataBase.InputProduct(entity);
            }catch(Exception ex)
            {

            }
        }

        public void Update(Product enity)
        {
            try
            {
                _dataBase.UpdataProduct(enity);
            }catch(Exception ex)
            {

            }
        
        }
    }
}
