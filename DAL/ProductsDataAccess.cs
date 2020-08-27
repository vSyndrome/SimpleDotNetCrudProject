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
    public class ProductsDataAccess : DataAccessClass
    {
        public List<String> GetList()
        {
            List<String> ProductsList = new List<string>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    if(sqlConnection.State == ConnectionState.Closed)
                        sqlConnection.Open();
                    string queryString = "SELECT ProductName FROM Products";
                    SqlCommand cmd = new SqlCommand(queryString, sqlConnection);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ProductsList.Add(rdr["ProductName"].ToString());
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ProductsList;
        }
    }
}
