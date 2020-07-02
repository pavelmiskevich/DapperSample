using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperExample
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public int Id { get; set; }
        public Category Category { get; set; }
    }
}
