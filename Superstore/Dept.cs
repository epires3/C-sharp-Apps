using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperStore
{
    class Dept
    {
        private string name;
        private int numItems;
        private List<Item> items;

        public string Name { get { return name; } set { name = value; } }
        public int NumItems { get { return numItems; } set { numItems = value; } }

        public Item GetItem(int i) { return items.ElementAt(i); }
        public void RemoveItem(int i, int amount) { while (amount-- > 0) items.ElementAt(i).Qnt--; }
        
        public Dept() { Name = ""; NumItems = 0; items = null; }
        public Dept(string nam, int num, List<Item> itm) { Name = nam; NumItems = num; items = itm; }
    }
}
