using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DAL
{
    public class DataAccessClass
    {
        protected static string connectionString =
            @"Data Source=20.200.20.10;Initial Catalog=Northwind-QA;User ID=diyatech;Password=4Islamabad";
        public SqlConnection connection = new SqlConnection(connectionString);
    }
}
