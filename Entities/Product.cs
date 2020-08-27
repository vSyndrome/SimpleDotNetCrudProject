using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Product
    {
        public int ProductId { get; set; } = 0;
        public String ProductName { get; set; } = "Default";
        public String SupplierID { get; set; } = "Default";
        public String CategoryID { get; set; } = "Default";
        public String QuantityPerUnit { get; set; } = "Default";
        public String UnitPrice { get; set; } = "Default";
        public String UnitsInStock { get; set; } = "Default";
        public String UnitsOnOrder { get; set; } = "Default";
        public String ReorderLevel { get; set; } = "Default";
        public String Discontinued { get; set; } = "Default";
    }
}
