using System.Collections.Generic;

namespace Model
{
    public class InputInfo : Model
    { 
        public int ItemId { get; set; }
        public int InputId { get; set; }
        public int Count { get; set; }
        public double InputPrice { get; set; }
        public string Status { get; set; }
        public virtual Input Input { get; set; }
        public virtual Item Item { get; set; } 
    }
}
