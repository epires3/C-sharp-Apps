using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperStore
{
    class Receipt
    {
        private string name;
        private int qnt;
        private double price;

        public string Name { get { return name; } set { name = value; } }
        public int Quantity { get { return qnt; } set { qnt = value; } }
        public double Price { get { return price; } set { price = value; } }

        public Receipt() { Name = ""; Quantity = 0; Price = 0.0; }
        public Receipt(string nam, int qnt, double price) { Name = nam; Quantity = qnt; Price = price; }
    }
}
