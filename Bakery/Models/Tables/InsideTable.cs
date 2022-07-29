using System;
using System.Collections.Generic;
using System.Text;

namespace Bakery.Models.Tables
{
    public class InsideTable : Table
    {
        private const decimal pricePerPersonInsideTable = 2.50M;

        public InsideTable(int tableNumber, int capacity) 
            : base(tableNumber, capacity, pricePerPersonInsideTable)
        {
        }
    }
}
