using Dapper;
using H1_ERP.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DapperDB
{
    public partial class DatabaseDapper
    {
        public Company GetCompanyFromID(int ID)
        {
            using (var conn = getConnection())
            {
                var query = conn.QuerySingle<Company>($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Company] WHERE CompanyID = {ID}");

                return query;
            }
        }
    }
}
