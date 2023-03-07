using System;

namespace Model
{
    public class OutputInfo : Model
    { 
        public int ItemId { get; set; }
        public int OutputId { get; set; } 
        public int Count { get; set; }
        public double OutputPrice { get; set; }
        public string Status { get; set; }
        public virtual Output Output { get; set; }
        public virtual Item Item { get; set; }
    }
}
