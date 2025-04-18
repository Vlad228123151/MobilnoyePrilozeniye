using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Models
{
    public class TaxResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Income { get; set; }
        public double Tax { get; set; }
        public string UserType { get; set; }
    }
}
