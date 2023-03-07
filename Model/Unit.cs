using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Unit : Model
    { 
        public string Name { get; set; }
        public virtual ICollection<Item> Items { get; set; } = new HashSet<Item>();

    }
}
