using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Order
    {
        public String OrderID { get; set; } = null;
        public String CustomerID { get; set; }
        public String EmployeeID { get; set; }
        public String OrderDate { get; set; } = "Default";
        public String RequiredDate { get; set; } = "Default";
        public String ShippedDate { get; set; } = "Default";
        public String ShipVia { get; set; } = "Default";
        public String Freight { get; set; } = "Default";
        public String ShipName { get; set; } = "Default";
        public String ShipAddress { get; set; } = "Default";
        public String ShipCity { get;  set; } = "Default";
        public String ShipRegion { get; set; } = "Default";
        public String ShipPostalCode { get; set; } = "Default";
        public String ShipCountry { get; set; } = "Default";
        public int RowNum { get; set; } = 0;
    }
}
