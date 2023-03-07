using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Customer : Model
    { 
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? MoreInfo { get; set; }
        public DateTime ContractDate { get; set; }
        public virtual ICollection<Output> Outputs { get; set; } = new HashSet<Output>();
    }
}
