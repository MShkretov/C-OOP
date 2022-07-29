using System;
using System.Collections.Generic;
using System.Text;

namespace Bakery.Models.Drinks
{
    public class Water : Drink
    {
        private const decimal waterPrice = 1.50M;

        public Water(string name, int portion, string brand) 
            : base(name, portion, 1.50m, brand)
        {
        }
    }
}
