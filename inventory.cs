using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _3902sprint0.Environment;

namespace _3902sprint0
{
    public class Inventory : IKeyHolder
    {
        public List<Iitem> items = new List<Iitem>();
        private readonly Dictionary<LockType, int> keys = new Dictionary<LockType, int>();

        public void AddItem(Iitem item)
        {
            if ((items.Count < 8))
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

        // Call this when the player picks up a keyhole or diamond key.
        public void AddKey(LockType lockType)
        {
            keys.TryGetValue(lockType, out int count);
            keys[lockType] = count + 1;
        }

        public bool HasKey(LockType lockType)
        {
            return keys.TryGetValue(lockType, out int count) && count > 0;
        }

        public void ConsumeKey(LockType lockType)
        {
            if (keys.TryGetValue(lockType, out int count) && count > 0)
                keys[lockType] = count - 1;
        }
    }
}