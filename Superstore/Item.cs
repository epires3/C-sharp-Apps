using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperStore
{
    class Item
    {
        private string name;
        private int qnt;
        private double priceEach;

        public string Name { get { return name; } set { name = value; } }
        public int Qnt { get { return qnt; } set { qnt = value; } }
        public double PriceEach { get { return priceEach; } set { priceEach = value; } }

        public Item() { Name = ""; Qnt = 0;PriceEach = 0.0; }
        public Item(string n,int q,double p) { Name = n; Qnt = q; PriceEach = p; }
    }
}
