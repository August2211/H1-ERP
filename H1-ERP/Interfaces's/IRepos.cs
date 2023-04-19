using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.Interfaces_s
{
    public interface IRepos<T>
    {
        T GetFromID(int id);
        List<T> GetAll(int id);
        void Delete(int id);
        void Update(T enity);
        void Insert(T entity); 

    }
}
