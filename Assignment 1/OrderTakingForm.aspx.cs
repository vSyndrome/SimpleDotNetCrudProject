using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.WebSockets;
using Assignment_1.ServiceRef;


namespace Assignment_1.PresentationLayer
{
    public partial class OrderTakingForm : System.Web.UI.Page
    {
        private ServiceRef.WebService1 TestService = new ServiceRef.WebService1();

        public void LoadCustomerData(String customerID)
        {
            try
            {
                string[] CustomerData = TestService.GetCustomerData(customerID);
                int length = 0;
                tbCustomerID.Text = CustomerData[length];
                tbCompanyName.Text = CustomerData[++length];
                tbName.Text = CustomerData[++length];
                tbAddress.Text = CustomerData[++length];
                tbContactTitle.Text = CustomerData[++length];
                tbCity.Text = CustomerData[++length];
                tbRegion.Text = CustomerData[++length];
                tbPostalCode.Text = CustomerData[++length];
                tbCountry.Text = CustomerData[++length];
                tbPhone.Text = CustomerData[++length];
                tbFax.Text = CustomerData[++length];
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        public void AddOrUpdateCustomer(List<String> CustomerData)
        {
            statusLabel.Text = TestService.CreateOrUpdate(CustomerData.ToArray());
        }

        public void ClearTextFields()
        {
            tbCustomerID.Text = tbCompanyName.Text = tbName.Text = tbContactTitle.Text = "";
            tbAddress.Text = tbCity.Text = tbRegion.Text = tbPostalCode.Text = "";
            tbCountry.Text = tbPhone.Text = tbFax.Text = "";
        }

        public String CurrentCustomerID()
        {
            return tbCustomerID.Text;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentCustomerID() == "")
            {
                btnNext.Enabled = false;
                btnPrev.Enabled = false;
            }
            else
            {
                btnNext.Enabled = true;
                btnPrev.Enabled = true;
            }
        }

        protected void FillProductsList()
        {
            try
            {
                ddlProducts.DataSource = TestService.GetProductsList();
                ddlProducts.DataBind();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void FillGridView()
        {
            if (CurrentCustomerID() == "")
            {
                gvOrders.DataSource = null;
                gvOrders.DataBind();
                return;
            }
            else
            {
                string[] orders = TestService.GetOrders(CurrentCustomerID());
                gvOrders.DataSource = orders;
                gvOrders.DataBind();
                gvOrders.Caption = "Order Details";
                gvOrders.CellPadding = 5;
                gvOrders.CellSpacing = 5;

            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            UndoReadOnlyTextFields();

            statusLabel.Text = "";
            ClearTextFields();
            btnSave.Enabled = true;
            btnSave.Text = "Add Customer";

            gvOrders.DataSource = null;
            gvOrders.DataBind();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            UndoReadOnlyTextFields();
            tbCustomerID.ReadOnly = true;
            btnSave.Text = "Update Customer";
        }
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (CurrentCustomerID() == "")
            {
                statusLabel.Text = "Cant perform any operation without customerID";
                btnSave.Text = "Save";
                ClearTextFields();
            }
            else
            {
                List<String> inputCustomerData = new List<string>();
                inputCustomerData.Add(CurrentCustomerID());
                inputCustomerData.Add(tbCompanyName.Text);
                inputCustomerData.Add(tbName.Text);
                inputCustomerData.Add(tbContactTitle.Text);
                inputCustomerData.Add(tbAddress.Text);
                inputCustomerData.Add(tbCity.Text);
                inputCustomerData.Add(tbRegion.Text);
                inputCustomerData.Add(tbPostalCode.Text);
                inputCustomerData.Add(tbCountry.Text);
                inputCustomerData.Add(tbPhone.Text);
                inputCustomerData.Add(tbFax.Text);

                AddOrUpdateCustomer(inputCustomerData);
                FillProductsList();
                btnSave.Text = "Save";
            }
            MakeTextFieldsReadOnly();

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (CurrentCustomerID() == "")
            {
                statusLabel.Text = "There is nothing to delete!";
            }
            else
            {
                try
                {
                    TestService.DeleteCustomer(CurrentCustomerID());
                    statusLabel.Text = "Deleted!!!!!";
                    ClearTextFields();
                    FillGridView();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        protected void btnLast_Click(object sender, EventArgs e)
        {
            btnNext.Enabled = false;
            btnPrev.Enabled = true;
            ClearTextFields();
            try
            {
                string[] CustomerData = TestService.Last();
                int length = 0;
                tbCustomerID.Text = CustomerData[length];
                tbCompanyName.Text = CustomerData[++length];
                tbName.Text = CustomerData[++length];
                tbAddress.Text = CustomerData[++length];
                tbContactTitle.Text = CustomerData[++length];
                tbCity.Text = CustomerData[++length];
                tbRegion.Text = CustomerData[++length];
                tbPostalCode.Text = CustomerData[++length];
                tbCountry.Text = CustomerData[++length];
                tbPhone.Text = CustomerData[++length];
                tbFax.Text = CustomerData[++length];

                FillProductsList();
                FillGridView();
                MakeTextFieldsReadOnly();
                statusLabel.Text = "End of the list";
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            btnPrev.Enabled = true;
            statusLabel.Text = "";
            if (CurrentCustomerID() == "")
            {
                statusLabel.Text = "Can not move next from a NULL ID";
                return;
            }

            bool IsIdValid = false;
            string[] customerIDs = TestService.CustomerIDList();
            for (int i = 0; i < customerIDs.Length; i++)
            {
                if (CurrentCustomerID() == customerIDs[i])
                {
                    IsIdValid = true;
                }
            }

            if (IsIdValid)
            {
                try
                {
                    string[] CustomerData = TestService.Next(CurrentCustomerID());
                    int length = 0;
                    tbCustomerID.Text = CustomerData[length];
                    tbCompanyName.Text = CustomerData[++length];
                    tbName.Text = CustomerData[++length];
                    tbAddress.Text = CustomerData[++length];
                    tbContactTitle.Text = CustomerData[++length];
                    tbCity.Text = CustomerData[++length];
                    tbRegion.Text = CustomerData[++length];
                    tbPostalCode.Text = CustomerData[++length];
                    tbCountry.Text = CustomerData[++length];
                    tbPhone.Text = CustomerData[++length];
                    tbFax.Text = CustomerData[++length];

                    FillProductsList();
                    FillGridView();
                    MakeTextFieldsReadOnly();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                ClearTextFields();
                statusLabel.Text = "can not move to NEXT when current ID is non existent";
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (CurrentCustomerID() == "")
            {
                statusLabel.Text = "Can not move next from a NULL ID";
                return;
            }
            btnNext.Enabled = true;
            statusLabel.Text = "";

            bool IsIdValid = false;
            string[] customerIDs = TestService.CustomerIDList();
            for (int i = 0; i < customerIDs.Length; i++)
            {
                if (CurrentCustomerID() == customerIDs[i])
                {
                    IsIdValid = true;
                }
            }

            if (IsIdValid)
            {
                try
                {
                    string[] CustomerData = TestService.Prev(CurrentCustomerID());
                    int length = 0;
                    tbCustomerID.Text = CustomerData[length];
                    tbCompanyName.Text = CustomerData[++length];
                    tbName.Text = CustomerData[++length];
                    tbAddress.Text = CustomerData[++length];
                    tbContactTitle.Text = CustomerData[++length];
                    tbCity.Text = CustomerData[++length];
                    tbRegion.Text = CustomerData[++length];
                    tbPostalCode.Text = CustomerData[++length];
                    tbCountry.Text = CustomerData[++length];
                    tbPhone.Text = CustomerData[++length];
                    tbFax.Text = CustomerData[++length];

                    FillProductsList();
                    FillGridView();
                    MakeTextFieldsReadOnly();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                ClearTextFields();
                statusLabel.Text = "can not move to NEXT when current ID is non existent";
            }
        }

        protected void btnFirst_Click(object sender, EventArgs e)
        {
            btnPrev.Enabled = false;
            btnNext.Enabled = true;
            ClearTextFields();

            try
            {
                string[] CustomerData = TestService.First();
                int length = 0;
                tbCustomerID.Text = CustomerData[length];
                tbCompanyName.Text = CustomerData[++length];
                tbName.Text = CustomerData[++length];
                tbAddress.Text = CustomerData[++length];
                tbContactTitle.Text = CustomerData[++length];
                tbCity.Text = CustomerData[++length];
                tbRegion.Text = CustomerData[++length];
                tbPostalCode.Text = CustomerData[++length];
                tbCountry.Text = CustomerData[++length];
                tbPhone.Text = CustomerData[++length];
                tbFax.Text = CustomerData[++length];

                FillProductsList();
                FillGridView();
                MakeTextFieldsReadOnly();
                statusLabel.Text = "Start of the list";
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {

            if (CurrentCustomerID() == "")
            {
                statusLabel.Text = "Can not place an order without customer details.";
            }
            else
            {
                if (TestService.CustomerIDList().Contains(CurrentCustomerID()))
                {
                    try
                    {
                        string ProductName = ddlProducts.SelectedItem.Text;
                        statusLabel.Text = "ORDER PLACED FOR ITEM: " + ProductName;
                        TestService.PlaceOrder(CurrentCustomerID());
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                else
                {
                    statusLabel.Text = "The Customer you're trying to place order for does not exist in the database.";
                }
            }
        }

        public void UndoReadOnlyTextFields()
        {
            tbCustomerID.ReadOnly = false;
            tbCompanyName.ReadOnly = false;
            tbName.ReadOnly = false;
            tbAddress.ReadOnly = false;
            tbContactTitle.ReadOnly = false;
            tbCity.ReadOnly = false;
            tbRegion.ReadOnly = false;
            tbPostalCode.ReadOnly = false;
            tbCountry.ReadOnly = false;
            tbPhone.ReadOnly = false;
            tbFax.ReadOnly = false;
        }
        public void MakeTextFieldsReadOnly()
        {
            tbCustomerID.ReadOnly = true;
            tbCompanyName.ReadOnly = true;
            tbName.ReadOnly = true;
            tbAddress.ReadOnly = true;
            tbContactTitle.ReadOnly = true;
            tbCity.ReadOnly = true;
            tbRegion.ReadOnly = true;
            tbPostalCode.ReadOnly = true;
            tbCountry.ReadOnly = true;
            tbPhone.ReadOnly = true;
            tbFax.ReadOnly = true;
        }
        protected void btnGetDetails_Click(object sender, EventArgs e)
        {
            if (CurrentCustomerID() == "")
            {
                statusLabel.Text = "Please Enter CustomerID First";
                return;
            }

            bool CustomerIDExists = false;
            string[] CustomersIDList = null;
            
            CustomersIDList = TestService.CustomerIDList();

            for (int i = 0; i < CustomersIDList.Length; i++)
            {
                if (CustomersIDList[i] == CurrentCustomerID())
                {
                    CustomerIDExists = true;
                }
            }
            
            if (CustomerIDExists == true)
            {
                LoadCustomerData(CurrentCustomerID());

                FillGridView();
                FillProductsList();
                statusLabel.Text = "Welcome" + CurrentCustomerID();
            }
            else
            {
                statusLabel.Text = "Customer Doesnt Exist";
            }
            MakeTextFieldsReadOnly();
            btnSave.Text = "Save";
        }
    }
}