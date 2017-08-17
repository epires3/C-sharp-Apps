using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SuperStore
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Dept> superStore = new List<Dept>();
            ReadDept(out superStore);
            // Stores total of the whole transaction, must not exceed $100
            double totalCost = 0;
            List<Item> receipt = Shop(superStore, ref totalCost);
            PrintReceipt(receipt, totalCost);
        }

        // PARSES ITEM LIST AND ORGANIZES STORE INTO SEPERATE DEPTS
        // PROVIDED BY PROFESSOR VIALL
        static void ReadDept(out List<Dept> s)
        {
            string[] tokens;
            string deptName;
            int deptQnt;
            s = new List<Dept>();

            try
            {
                using (StreamReader sr = new StreamReader(@"..\..\inv.txt"))
                    while (sr.Peek() >= 0)
                    {
                        List<Item> myItemList = new List<Item>();
                        // Parses Dept name and quanitity of items
                        SplitLine(sr, ',', out tokens);
                        deptName = tokens[0];
                        deptQnt = Convert.ToInt32(tokens[1]);
                        // Parses item name, item price, and item quantity inside the current Dept
                        // Parsed information is stored into Item List as class Item
                        for (int i = 0; i < deptQnt; i++)
                        {
                            SplitLine(sr, ',', out tokens);
                            Item myItem = new Item(tokens[0], Convert.ToInt32(tokens[1]), Convert.ToDouble(tokens[2]));
                            myItemList.Add(myItem);
                        }
                        // Creates Dept class with fields: Name, Qnt, ItemList
                        s.Add(new Dept(deptName, deptQnt, myItemList));
                    }
            }
            catch (Exception e) { Console.WriteLine("Error: {0}", e.Message); }
        }

        // TAKES STREAMED DATA AND SPLITS TO TOKENS ARRAY BASED ON SPECIFIED DELIM
        // PROVIDED BY PROFESSOR VIALL
        static void SplitLine(StreamReader sr, char delim, out string[] tokens)
        {
            string line = sr.ReadLine();
            tokens = line.Split(delim);
        }

        // ROOT MENU THAT HANDLES ALL STORE ROUTINES, RETURNS RECEIPT
        static List<Item> Shop(List<Dept> s, ref double finalTotal)
        {
            string deptName;
            string myItem;
            List<Item> receipt = new List<Item>(); 
            /******************************************************************************************/
            while (SelectDept(s, out deptName))
            {
                if (deptName == "INV") PrintInv(s);
                else // User wishes to purchase items
                {    // Stores index of dept in List s 
                    int deptIdx = GetDeptIndx(s, deptName);
                    // Loops until user can afford current purchase (Budget = $100)
                    // If user cant afford, they are redirected to item selection
                    bool validItem = SelectItem(s, deptIdx, out myItem);
                    bool OK;
                    if (validItem)
                        do { OK = Checkout(s, deptIdx, myItem, 100, ref finalTotal, ref receipt); } while (finalTotal > 100 | !OK);
                    else break;
                }
            }
            return receipt;
        }

        // DETERMINES DEPARTMENT NAVIGATION. RETURNS TRUE FOR DEPT INPUT, FALSE FOR EXIT INPUT
        static bool SelectDept(List<Dept> s, out string dept)
        {
            string[] valid = { "BOOKS", "FOOD", "VIDEO", "SPORTS", "STATIONARY", "TOOLS", "EXIT", "INV" };
            // Greet User
            Console.WriteLine("Welcome to the ECE-264 Superstore!!!");
            Console.WriteLine("Which department would you like to got to? (Type Exit to quit program)");
            // Print Dept Names     
            for (int i = 0; i < valid.Length-2; i++)
                Console.WriteLine("\t{0,-20}", valid[i]);
            // Parse user input, if user enters "Exit", will return false
            dept = GetString("=> ", valid);
            if (dept == "EXIT") return false;
            return true;
        }

        // PRINTS CURRENT DEPARTMENT INFO FOR USER TO BROWSE SELECTION
        // SOURCE: https://msdn.microsoft.com/en-us/library/x0b5b5bc(v=vs.110).aspx
        static bool SelectItem(List<Dept> s, int deptIdx, out string item)
        {
            // Prints items of current dept, price, and quantity
            Console.WriteLine("What would you like to buy? (Type Leave to return to Dept Menu)");
            for (int i = 0; i < s[deptIdx].NumItems; i++)
                Console.WriteLine("\t{0,-15} {1,7:$,##0.00} ({2,2} left in stock)",
                    s[deptIdx].GetItem(i).Name, s[deptIdx].GetItem(i).PriceEach, s[deptIdx].GetItem(i).Qnt);

            // Loads valid item names to array for GetString
            string[] valid = new string[s[deptIdx].NumItems+1];
            for (int i = 0; i < s[deptIdx].NumItems; i++)
                valid[i] = (s[deptIdx].GetItem(i).Name);
            // Adds "Leave" as an input at the end of the valid array
            valid[s[deptIdx].NumItems] = "Leave";
            item = GetString("=> ", valid);
            if (item.ToUpper().Equals("LEAVE")) return false;
            return true;
        }

        // HANDLES STORE TRANSACTION. RETURNS TRUE IF TRANSACTION SUCCEEDS, FALSE IF NOT SUCCESSFUL
        static bool Checkout(List<Dept> s, int deptIdx, string myItem, double maxCash, ref double finalTotal, ref List<Item> receipt)
        {
            int myQnt;
            int itemIdx = GetItemIndex(s, deptIdx, myItem);
            double thisTotal;

            // Stores quantity user wishes to buy (Max parameter equal to item quantity)
            GetInt("How many:", out myQnt, s[deptIdx].GetItem(itemIdx).Qnt, 0,
                "Sorry, that exceeds our current stock, please try again");
            // thisTotal stores total of current item transaction
            thisTotal = (s[deptIdx].GetItem(itemIdx).PriceEach) * (double)myQnt;
            // Sums to total cost
            finalTotal += thisTotal;
            // Returns back to Shop if user cant afford current transaction
            if (finalTotal > maxCash)
            {
                Console.WriteLine("WARNING: You have exceeded your maximum balance, please budget accordingly!");
                finalTotal -= thisTotal;
                return false;
            }
            // Transaction is under budget, therfore inform price of purchased item(s) and update inventory 
            Console.WriteLine("That will be {0:$,##0.00}", thisTotal);
            s[deptIdx].RemoveItem(itemIdx, myQnt);
            // Adds current purchase to Receipt list
            Item purchase = new Item(myItem, myQnt, thisTotal);
            receipt.Add(purchase);
            return true;
        }

        // PRINTS RECEIPT IN RECEIPT.TXT
        //
        static void PrintReceipt(List<Item> receipt, double total)
        {
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter("receipt.txt"))
            {   
                file.WriteLine("--------------------------");
                foreach (Item i in receipt)
                    if (i.Qnt != 0) file.WriteLine("{0,2} {1,15} {2,7:$,##0.00}", i.Qnt, i.Name, i.PriceEach);
                file.WriteLine("--------------------------");
                file.WriteLine("Total {0,20:$,##0.00}", total);
            }
            // DELETE AFTER DEBUG
            Console.WriteLine("--------------------------");
            foreach(Item i in receipt)
                if (i.Qnt != 0) Console.WriteLine("{0,2} {1,15} {2,7:$,##0.00}", i.Qnt, i.Name, i.PriceEach);
            Console.WriteLine("--------------------------");
            Console.WriteLine("Total {0,20:$,##0.00}", total);
        }

        // STORES INDEX OF DEPARTMENT IN SUPERSTORE LIST
        static int GetDeptIndx(List<Dept> s, string deptName) { return s.FindIndex(i => i.Name.ToUpper().Equals(deptName)); }

        // STORES INDEX OF ITEM IN DEPARTMENT CLASS
        static int GetItemIndex(List<Dept>s, int deptIdx, string myItem)
        {
            int itemIdx;
            for ( itemIdx = 0; itemIdx < s[deptIdx].NumItems; itemIdx++)
                if (s[deptIdx].GetItem(itemIdx).Name.ToUpper().Equals(myItem))
                    break;
            return itemIdx;
        }

        // PARSES USER INPUT TO VALIDATE AS STRING
        // PROVIDED BY PROFESSOR VIALL
        static string GetString(string prompt, string[] valid)
        {
            /* prompt=user prompt, valid=array of valid responses, error=msg to display 
               upon invalid response
               ALL STRINGS RETURNED UPPER CASE. ALL valid[] ENTRIES MUST BE UPPER CASE */
            bool OK = false;
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine().ToUpper();
                foreach (string s in valid) if (input == s.ToUpper()) OK = true;
                if (!OK) Console.WriteLine("Sorry, thats not a valid input. Please try again.");
            } while (!OK);
            return input;
        }

        // PARSES INTEGER INPUT AND CHECKS TO SEE IF IT FALLS IN SET RANGE. 
        // PROVIDED BY PROFESSOR VIALL
        static bool GetInt(string prompt, out int val, int max, int min, string maxError)
        {
            bool OK = false;
            do
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                OK = Int32.TryParse(input, out val);
                if (!OK || val < min) Console.WriteLine("Please enter a valid number");
                else if (val > max) Console.WriteLine(maxError);
            } while (!OK || val < min || val > max);
            return true;
        }

        // PRINTS FULL INVENTORY LIST
        // PROVIDED BY PROFESSOR VIALL
        static void PrintInv(List<Dept> s)
        {
            foreach (Dept d in s)
            {
                Console.WriteLine("Dept: {0,-20} [{1} items]", d.Name, d.NumItems);
                for (int i = 0; i < d.NumItems; i++)
                    Console.WriteLine("      {0,-15} {1,4} {2,7:$,##0.00}", d.GetItem(i).Name,
                        d.GetItem(i).Qnt, d.GetItem(i).PriceEach);
                Console.WriteLine();
            }
        }
    }
}
 