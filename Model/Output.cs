using System;
using System.Collections.Generic;

namespace Model
{
    public class Output : Model
    {
        public Guid SerialNumber { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public string? MoreInfo { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OutputInfo> OutputInfos { get; set; } = new HashSet<OutputInfo>();
    }
}
