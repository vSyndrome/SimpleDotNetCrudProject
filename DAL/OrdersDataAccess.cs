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
    public class OrdersDataAccess : DataAccessClass
    {
        public Random GenerateRandomNumber = new Random();
        List<int> randomNumberList = new List<int>();
        public int GetUniqueOrderID()
        {
            int GenerateOrderID = GenerateRandomNumber.Next(10000, 99999);
            if (randomNumberList.Contains(GenerateOrderID))
            {
                GetUniqueOrderID();
            }
            else
            {
                randomNumberList.Add(GenerateOrderID);
            }

            return GenerateOrderID;
        }

        public int GetUniqueEmployeeID()
        {
            int GenerateEmployeeID = GenerateRandomNumber.Next(10000, 99999);
            if (randomNumberList.Contains(GenerateEmployeeID))
            {
                GetUniqueEmployeeID();
            }
            else
            {
                randomNumberList.Add(GenerateEmployeeID);
            }

            return GenerateEmployeeID;
        }
        Order order = new Order();
        public void Place(String CustomerID)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                SqlCommand sqlCommand = new SqlCommand("INSERT INTO Orders (OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry, RowNum) VALUES (@OrderID, @CustomerID, @EmployeeID, @OrderDate, @RequiredDate, @ShippedDate, @ShipVia, @Freight, @ShipName, @ShipAddress, @ShipCity, @ShipRegion, @ShipPostalCode, @ShipCountry, @RowNum)", connection); ;
                sqlCommand.Parameters.AddWithValue("@OrderID", GetUniqueOrderID());
                sqlCommand.Parameters.AddWithValue("@CustomerID", CustomerID);
                sqlCommand.Parameters.AddWithValue("@EmployeeID", GetUniqueEmployeeID());
                sqlCommand.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                sqlCommand.Parameters.AddWithValue("@RequiredDate", order.RequiredDate);
                sqlCommand.Parameters.AddWithValue("@ShippedDate", order.ShippedDate);
                sqlCommand.Parameters.AddWithValue("@ShipVia", order.ShipVia);
                sqlCommand.Parameters.AddWithValue("@Freight", order.Freight);
                sqlCommand.Parameters.AddWithValue("@ShipName", order.ShipName);
                sqlCommand.Parameters.AddWithValue("@ShipAddress", order.ShipAddress);
                sqlCommand.Parameters.AddWithValue("@ShipCity", order.ShipCity);
                sqlCommand.Parameters.AddWithValue("@ShipRegion", order.ShipRegion);
                sqlCommand.Parameters.AddWithValue("@ShipPostalCode", order.ShipPostalCode);
                sqlCommand.Parameters.AddWithValue("@ShipCountry", order.ShipCountry);
                sqlCommand.Parameters.AddWithValue("@RowNum", order.RowNum);
                sqlCommand.ExecuteNonQuery();
                connection.Close();

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<string> Get(String CustomerID)
        {
            int OrdersCount = 0;
            List<String> ListOfOrders = new List<string>();

            try
            {
                String queryString = "SELECT * FROM Orders WHERE CustomerID = @CustomerID";
                Order orderObject = new Order();

                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    if (sqlConnection.State == ConnectionState.Closed)
                        sqlConnection.Open();
                    SqlCommand cmd = new SqlCommand(queryString, sqlConnection);
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            orderObject.OrderID = rdr["OrderID"].ToString();
                            orderObject.CustomerID = rdr["CustomerID"].ToString();
                            orderObject.EmployeeID = rdr["EmployeeID"].ToString();
                            orderObject.OrderDate = rdr["OrderDate"].ToString();
                            orderObject.RequiredDate = rdr["RequiredDate"].ToString();
                            orderObject.ShippedDate = rdr["ShippedDate"].ToString();
                            orderObject.ShipVia = rdr["ShipVia"].ToString();
                            orderObject.Freight = rdr["Freight"].ToString();
                            orderObject.ShipName = rdr["ShipName"].ToString();
                            orderObject.ShipAddress = rdr["ShipAddress"].ToString();
                            orderObject.ShipCity = rdr["ShipCity"].ToString();
                            orderObject.ShipRegion = rdr["ShipRegion"].ToString();
                            orderObject.ShipPostalCode = rdr["ShipPostalCode"].ToString();
                            orderObject.ShipCountry = rdr["ShipCountry"].ToString();
                            orderObject.RowNum = Int32.Parse(rdr["RowNum"].ToString());

                            string combinedString = "OrderID: "+orderObject.OrderID+"--|-- "+ "CustomerID: " + orderObject.CustomerID + "--|-- " + "EmployeeID: " + orderObject.EmployeeID + "--|-- " + "OrderDate: " + orderObject.OrderDate + "--|-- " + "RequiredDate: " + orderObject.RequiredDate + "--|-- " + "ShippedDate: " + orderObject.ShippedDate + "--|-- " + "ShipVia: " + orderObject.ShipVia + "--|-- " + "Freight: " + orderObject.Freight + "--|-- " + "ShipName: " + orderObject.ShipName + "--|-- " + "ShipAddress: " + orderObject.ShipAddress + "--|-- " + "Shipcity: " + orderObject.ShipCity + "--|-- " + "ShipRegion: " + orderObject.ShipRegion + "--|-- " + "ShipPostalCode: " + orderObject.ShipPostalCode + "--|-- " + "ShipCountry: " + orderObject.ShipCountry + "--|-- " + "RowNum: " + orderObject.RowNum;
                            OrdersCount++;
                            ListOfOrders.Add(combinedString);
                        }
                    }
                    sqlConnection.Close();
                }
                if(OrdersCount == 0)
                {
                    ListOfOrders.Add(" No Orders Present");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ListOfOrders;
        }
    }
}
