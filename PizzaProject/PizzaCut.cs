using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaProject
{
    class PizzaCut
    {
        public double probability;
        public bool horizontal;
        public int i;

        public PizzaCut(int i, bool horizontal, double probability)
        {
            this.i = i;
            this.horizontal = horizontal;
            this.probability = probability;
        }

        public override String  ToString()
        {
            return "" + i + " " + horizontal + " " + probability;
        }
    }
}
