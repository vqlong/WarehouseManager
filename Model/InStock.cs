using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class InStock
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int TotalInput { get; set; }
        public int TotalOutput { get; set; }
    }
}
