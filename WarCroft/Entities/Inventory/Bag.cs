using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarCroft.Constants;
using WarCroft.Entities.Items;

namespace WarCroft.Entities.Inventory
{
    public abstract class Bag : IBag
    {
        private readonly List<Item> items;

        public Bag(int capacity = 100)
        {
            Capacity = capacity;
            items = new List<Item>();
        }

        public int Capacity { get; set; }
        

        public int Load => Items.Select(w => w.Weight).Sum();

        public IReadOnlyCollection<Item> Items => items;

        public void AddItem(Item item)
        {
            if (Load + item.Weight > Capacity)
            {
                throw new InvalidOperationException(ExceptionMessages.ExceedMaximumBagCapacity);
            }

            items.Add(item);
        }

        public Item GetItem(string name)
        {

            Item item = items.FirstOrDefault(x => x.GetType().Name == name);

            if (items.Count == 0)
            {
                throw new InvalidOperationException(ExceptionMessages.EmptyBag);
            }

            if (item == null)
            {
                throw new ArgumentException(ExceptionMessages.ItemNotFoundInBag, name);
            }

            items.Remove(item);
            return item; // eventualno
        }
    }
}
