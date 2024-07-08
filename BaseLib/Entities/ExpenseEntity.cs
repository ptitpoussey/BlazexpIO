using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLib.Entities
{
    public class ExpenseEntity : CategoryEntity
    {
        public string? ExpenseName { get; set; }
        public int Price { get; set; }
    }
}
