using H1_ERP.DataBase;
using H1_ERP.DomainModel;
using H1_ERP.Interfaces_s;

namespace W.A.N.K_API.Repostoriy
{
    public class SalesRepository : IRepos<SalesOrderHeader>
    {
        private DataBase _dataBase = new DataBase();
        public void Delete(int id)
        {
            try
            {
                _dataBase.DeleteSalesOrderHeaderFromID(id);
            }
            catch (Exception ex)
            {

            }
        }

        public List<SalesOrderHeader> GetAll()
        {
            try
            {
                return _dataBase.GetAll();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public SalesOrderHeader GetFromID(int id)
        {
            try
            {
                return _dataBase.GetSalesOrderHeaderFromID(id);
            }
            catch (Exception ex)
            {
                return null;

            }

        }

        public void Insert(SalesOrderHeader entity)
        {
            try
            {
                _dataBase.InsertSalesOrderHeader(entity);
            }
            catch (Exception ex)
            {

            }
        }

        public void Update(SalesOrderHeader enity)
        {
            try
            {
                _dataBase.UpdateSalesorderHeader((int)enity.OrderID,enity);
            }
            catch (Exception ex)
            {

            }

        }
    }
}
