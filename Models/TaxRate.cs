using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Models
{
    public class TaxRate
    {
        public int Id { get; set; }
        public string UserType { get; set; }
        public double Rate { get; set; }
    }
}
