﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DataBase
{
    internal partial class Database
    {
        private SqlConnection getConnection()
        {
            SqlConnectionStringBuilder sb = new();
            sb.DataSource = "docker.data.techcollege.dk";
            sb.InitialCatalog = "H1PD021123_Gruppe1";
            sb.UserID = "H1PD021123_Gruppe1";
            sb.Password = "H1PD021123_Gruppe1";
            string connectionString = sb.ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }


    }
}
