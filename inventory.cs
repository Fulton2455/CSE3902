using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3902sprint0{
   public class Inventory {
        public List<Iitem> items = new List<Iitem>();

        public void AddItem(Iitem item)
        {
            if ((items.Count<8))
            {
                items.Add(item);
            }
           
        }
        public void ResetItems()
        {
            foreach (Iitem item in items)
            {
                item.cooldownReset();
            }
        }
    }
}

