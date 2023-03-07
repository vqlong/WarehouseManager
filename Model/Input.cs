using System;
using System.Collections.Generic;

namespace Model
{
    public class Input : Model
    { 
        public Guid SerialNumber { get; set; }
        public DateTime Date { get; set; }
        public string? MoreInfo { get; set; }
        public virtual ICollection<InputInfo> InputInfos { get; set; } = new HashSet<InputInfo>();
    }
}
