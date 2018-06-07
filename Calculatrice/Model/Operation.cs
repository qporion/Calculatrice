using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculatrice.Model
{
    class Operation
    {
        public Operation valueLeft { get; set; }
        public Operation valueRight { get; set; }
        public String operande { get; set; }
        
        public double value { get; set; }
    }
}
