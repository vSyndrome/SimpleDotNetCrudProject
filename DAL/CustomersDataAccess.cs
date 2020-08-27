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
    public class CustomersDataAccess : DataAccessClass
    {
        public static string IsCustomerIDValid = "";

        public string CreateOrUpdate(string[] CustomerData)
        {
            int index = 0;
            string CustomerStatus = "";
            bool CustomerExists = false;
            List<String> customerIdList = new List<string>();
            customerIdList = GetIDList();

            if (CustomerData[index] == "")
            {
                CustomerStatus = "Operation failed because customer ID is null";
            }

            else
            {
                for (int i = 0; i < GetIDList().Count; i++)
                {
                    if (CustomerData[index] == customerIdList[i])
                    {
                        CustomerExists = true;
                    }
                }

                if (CustomerExists == true)
                {
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                        SqlCommand sqlCommand = new SqlCommand(
                            "UPDATE Customers SET CompanyName = @CompanyName, ContactName = @ContactName,ContactTitle = @ContactTitle,[Address] = @Address,City = @City,Region = @Region,PostalCode = @PostalCode,Country = @Country,Phone = @Phone,Fax = @Fax WHERE CustomerID = @CustomerID",
                            connection);
                        index = 0;
                        sqlCommand.Parameters.AddWithValue("@CustomerID", CustomerData[index]);
                        sqlCommand.Parameters.AddWithValue("@CompanyName", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@ContactName", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@ContactTitle", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@Address", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@City", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@Region", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@PostalCode", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@Country", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@Phone", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@Fax", CustomerData[++index]);
                        sqlCommand.ExecuteNonQuery();
                        connection.Close();
                        CustomerStatus = "Customer Updated";
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                        SqlCommand sqlCommand = new SqlCommand(
                            "INSERT INTO Customers(CustomerID,CompanyName,ContactName,ContactTitle,[Address],City,Region,PostalCode,Country,Phone,Fax) VALUES (@CustomerID,@CompanyName,@ContactName,@ContactTitle,@Address,@City,@Region,@PostalCode,@Country,@Phone,@Fax)",
                            connection);
                        index = 0;
                        sqlCommand.Parameters.AddWithValue("@CustomerID", CustomerData[index]);
                        sqlCommand.Parameters.AddWithValue("@CompanyName", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@ContactName", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@ContactTitle", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@Address", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@City", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@Region", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@PostalCode", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@Country", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@Phone", CustomerData[++index]);
                        sqlCommand.Parameters.AddWithValue("@Fax", CustomerData[++index]);
                        sqlCommand.ExecuteNonQuery();
                        connection.Close();
                        CustomerStatus = "Customer Created";
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return CustomerStatus;
        }
        public void Delete(String CustomerID)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand sqlCommand = new SqlCommand("DELETE FROM Customers WHERE CustomerID = @CustomerID DELETE FROM [Order Details] WHERE OrderID = (SELECT OrderID FROM Orders WHERE CustomerID = @CustomerID) DELETE FROM Orders WHERE CustomerID = @CustomerID", connection);
                sqlCommand.Parameters.AddWithValue("@CustomerID", CustomerID);
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
            Customer customerObject = new Customer();
            List<String> CustomerData = new List<string>();
            Boolean CustomerExists = false;

            for (int i = 0; i < GetIDList().Count; i++)
            {
                if (CustomerID == GetIDList()[i])
                {
                    CustomerExists = true;
                    IsCustomerIDValid = "Yes";
                }
            }
            if (!CustomerExists)
            {
                IsCustomerIDValid = "No";
                return null;
            }
            else
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        if (sqlConnection.State == ConnectionState.Closed)
                            sqlConnection.Open();
                        String queryString = "SELECT * FROM Customers WHERE CustomerID = @CustomerID";

                        SqlCommand cmd = new SqlCommand(queryString, sqlConnection);
                        cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                customerObject.CustomerId = rdr["CustomerID"].ToString();
                                customerObject.CompanyName = rdr["CompanyName"].ToString();
                                customerObject.ContactName = rdr["ContactName"].ToString();
                                customerObject.ContactTitle = rdr["ContactTitle"].ToString();
                                customerObject.Address = rdr["Address"].ToString();
                                customerObject.City = rdr["City"].ToString();
                                customerObject.Region = rdr["Region"].ToString();
                                customerObject.PostalCode = rdr["PostalCode"].ToString();
                                customerObject.Country = rdr["Country"].ToString();
                                customerObject.Phone = rdr["Phone"].ToString();
                                customerObject.Fax = rdr["Fax"].ToString();
                            }
                        }
                        sqlConnection.Close();
                    }
                    CustomerData.Add(customerObject.CustomerId);
                    CustomerData.Add(customerObject.CompanyName);
                    CustomerData.Add(customerObject.ContactName);
                    CustomerData.Add(customerObject.ContactTitle);
                    CustomerData.Add(customerObject.Address);
                    CustomerData.Add(customerObject.City);
                    CustomerData.Add(customerObject.Region);
                    CustomerData.Add(customerObject.PostalCode);
                    CustomerData.Add(customerObject.Country);
                    CustomerData.Add(customerObject.Phone);
                    CustomerData.Add(customerObject.Fax);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return CustomerData;
        }

        public List<String> GetIDList()
        {
            List<String> CustomersIDList = new List<string>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    if (sqlConnection.State == ConnectionState.Closed)
                        sqlConnection.Open();
                    String queryString = "Select CustomerID from Customers";

                    SqlCommand cmd = new SqlCommand(queryString, sqlConnection);
                    DataTable Table = new DataTable();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            CustomersIDList.Add(rdr["CustomerID"].ToString());
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.Write(ex.Message);
            }
            return CustomersIDList;
        }

        public String GetLastID()
        {
            int count = GetIDList().Count;
            return GetIDList()[count-1];
        }

        public String GetNextID(String CustomerID)
        {
            String NextCustomerID = null;
            int count = GetIDList().Count;
            int index = 0;

            if (CustomerID == "")
            {
                NextCustomerID = GetLastID();
            }
            else if (CustomerID == GetIDList()[count-1])
            {
                NextCustomerID = GetLastID();
            }
            else if (CustomerID == GetIDList()[0])
            {
                NextCustomerID = GetIDList()[1];
            }
            else
            {
                for (int i = 1; i < GetIDList().Count; i++)
                {
                    if (CustomerID != GetIDList()[i])
                    {
                        ++index;
                    }
                    else
                    {
                        break;
                    }
                }
                NextCustomerID = GetIDList()[index + 2];
            }

            return NextCustomerID;
        }

        public String GetPreviousID(String CustomerID)
        {
            String PrevCustomerID = null;
            int count = GetIDList().Count;
            int index = 0;

            if (CustomerID == "")
            {
                PrevCustomerID = GetIDList()[0];
            }
            else if (CustomerID == GetIDList()[count - 1])
            {
                PrevCustomerID = GetIDList()[count - 2];
            }
            else if (CustomerID == GetIDList()[0])
            {
                PrevCustomerID = GetIDList()[0];
            }
            else
            {
                for (int i = 1; i < GetIDList().Count; i++)
                {
                    if (CustomerID != GetIDList()[i])
                    {
                        ++index;
                    }
                    else
                    {
                        break;
                    }
                }
                PrevCustomerID = GetIDList()[index];
            }

            return PrevCustomerID;
        }

        public List<string> First()
        {
            Customer customerObject = new Customer();
            List<String> CustomerData = new List<string>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    if (sqlConnection.State == ConnectionState.Closed)
                        sqlConnection.Open();
                    String queryString = "select top 1 * from Customers";
                    SqlCommand cmd = new SqlCommand(queryString, sqlConnection);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            customerObject.CustomerId = rdr["CustomerID"].ToString();
                            customerObject.CompanyName = rdr["CompanyName"].ToString();
                            customerObject.ContactName = rdr["ContactName"].ToString();
                            customerObject.ContactTitle = rdr["ContactTitle"].ToString();
                            customerObject.Address = rdr["Address"].ToString();
                            customerObject.City = rdr["City"].ToString();
                            customerObject.Region = rdr["Region"].ToString();
                            customerObject.PostalCode = rdr["PostalCode"].ToString();
                            customerObject.Country = rdr["Country"].ToString();
                            customerObject.Phone = rdr["Phone"].ToString();
                            customerObject.Fax = rdr["Fax"].ToString();
                        }
                    }
                    sqlConnection.Close();
                }
                CustomerData.Add(customerObject.CustomerId);
                CustomerData.Add(customerObject.CompanyName);
                CustomerData.Add(customerObject.ContactName);
                CustomerData.Add(customerObject.ContactTitle);
                CustomerData.Add(customerObject.Address);
                CustomerData.Add(customerObject.City);
                CustomerData.Add(customerObject.Region);
                CustomerData.Add(customerObject.PostalCode);
                CustomerData.Add(customerObject.Country);
                CustomerData.Add(customerObject.Phone);
                CustomerData.Add(customerObject.Fax);
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return CustomerData;
        }

        public List<string> Last()
        { 
            Customer customerObject = new Customer();
            List<String> CustomerData = new List<string>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    if (sqlConnection.State == ConnectionState.Closed)
                        sqlConnection.Open();
                    String queryString = "select * from Customers where CustomerID = @CustomerID";
                    SqlCommand cmd = new SqlCommand(queryString, sqlConnection);
                    cmd.Parameters.AddWithValue("@CustomerID", GetLastID());
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            customerObject.CustomerId = rdr["CustomerID"].ToString();
                            customerObject.CompanyName = rdr["CompanyName"].ToString();
                            customerObject.ContactName = rdr["ContactName"].ToString();
                            customerObject.ContactTitle = rdr["ContactTitle"].ToString();
                            customerObject.Address = rdr["Address"].ToString();
                            customerObject.City = rdr["City"].ToString();
                            customerObject.Region = rdr["Region"].ToString();
                            customerObject.PostalCode = rdr["PostalCode"].ToString();
                            customerObject.Country = rdr["Country"].ToString();
                            customerObject.Phone = rdr["Phone"].ToString();
                            customerObject.Fax = rdr["Fax"].ToString();
                        }
                    }
                    sqlConnection.Close();
                }
                CustomerData.Add(customerObject.CustomerId);
                CustomerData.Add(customerObject.CompanyName);
                CustomerData.Add(customerObject.ContactName);
                CustomerData.Add(customerObject.ContactTitle);
                CustomerData.Add(customerObject.Address);
                CustomerData.Add(customerObject.City);
                CustomerData.Add(customerObject.Region);
                CustomerData.Add(customerObject.PostalCode);
                CustomerData.Add(customerObject.Country);
                CustomerData.Add(customerObject.Phone);
                CustomerData.Add(customerObject.Fax);
            }
            catch (SqlException ex) 
            {
                Console.WriteLine(ex.Message);
            }
            return CustomerData;
        }

        public List<string> Prev(String CustomerID)
        {
            Customer customerObject = new Customer();
            List<String> CustomerData = new List<string>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    if (sqlConnection.State == ConnectionState.Closed)
                        sqlConnection.Open();
                    String queryString = "select * from Customers where CustomerID = @CustomerID";
                    SqlCommand cmd = new SqlCommand(queryString, sqlConnection);
                    cmd.Parameters.AddWithValue("@CustomerID", GetPreviousID(CustomerID));
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            customerObject.CustomerId = rdr["CustomerID"].ToString();
                            customerObject.CompanyName = rdr["CompanyName"].ToString();
                            customerObject.ContactName = rdr["ContactName"].ToString();
                            customerObject.ContactTitle = rdr["ContactTitle"].ToString();
                            customerObject.Address = rdr["Address"].ToString();
                            customerObject.City = rdr["City"].ToString();
                            customerObject.Region = rdr["Region"].ToString();
                            customerObject.PostalCode = rdr["PostalCode"].ToString();
                            customerObject.Country = rdr["Country"].ToString();
                            customerObject.Phone = rdr["Phone"].ToString();
                            customerObject.Fax = rdr["Fax"].ToString();
                        }
                    }
                    sqlConnection.Close();
                }
                CustomerData.Add(customerObject.CustomerId);
                CustomerData.Add(customerObject.CompanyName);
                CustomerData.Add(customerObject.ContactName);
                CustomerData.Add(customerObject.ContactTitle);
                CustomerData.Add(customerObject.Address);
                CustomerData.Add(customerObject.City);
                CustomerData.Add(customerObject.Region);
                CustomerData.Add(customerObject.PostalCode);
                CustomerData.Add(customerObject.Country);
                CustomerData.Add(customerObject.Phone);
                CustomerData.Add(customerObject.Fax);
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return CustomerData;
        }

        public List<string> Next(String CustomerID)
        {
            Customer customerObject = new Customer();
            List<String> CustomerData = new List<string>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    if (sqlConnection.State == ConnectionState.Closed)
                        sqlConnection.Open();
                    String queryString = "select * from Customers where CustomerID = @CustomerID";
                    SqlCommand cmd = new SqlCommand(queryString, sqlConnection);
                    cmd.Parameters.AddWithValue("@CustomerID", GetNextID(CustomerID));
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            customerObject.CustomerId = rdr["CustomerID"].ToString();
                            customerObject.CompanyName = rdr["CompanyName"].ToString();
                            customerObject.ContactName = rdr["ContactName"].ToString();
                            customerObject.ContactTitle = rdr["ContactTitle"].ToString();
                            customerObject.Address = rdr["Address"].ToString();
                            customerObject.City = rdr["City"].ToString();
                            customerObject.Region = rdr["Region"].ToString();
                            customerObject.PostalCode = rdr["PostalCode"].ToString();
                            customerObject.Country = rdr["Country"].ToString();
                            customerObject.Phone = rdr["Phone"].ToString();
                            customerObject.Fax = rdr["Fax"].ToString();
                        }
                    }
                    sqlConnection.Close();
                }
                CustomerData.Add(customerObject.CustomerId);
                CustomerData.Add(customerObject.CompanyName);
                CustomerData.Add(customerObject.ContactName);
                CustomerData.Add(customerObject.ContactTitle);
                CustomerData.Add(customerObject.Address);
                CustomerData.Add(customerObject.City);
                CustomerData.Add(customerObject.Region);
                CustomerData.Add(customerObject.PostalCode);
                CustomerData.Add(customerObject.Country);
                CustomerData.Add(customerObject.Phone);
                CustomerData.Add(customerObject.Fax);
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return CustomerData;
        }


    }
}
