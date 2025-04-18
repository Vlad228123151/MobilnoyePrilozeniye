using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Models
{
    public class TaxDatabase
    {
        public List<User> Users { get; set; } = new();
        public List<Deduction> Deductions { get; set; } = new();
        public List<TaxResult> TaxResults { get; set; } = new();
        public List<TaxRate> TaxRates { get; set; } = new();
    }
}
