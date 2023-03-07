using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Item : Model
    { 
        public string Name { get; set; }
        public int UnitId { get; set; }
        public int SuplierId { get; set; }
        public string? QRCode { get; set; }
        public string? Barcode { get; set; }
        public virtual Suplier Suplier { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ICollection<InputInfo> InputInfos { get;set; } = new HashSet<InputInfo>();
        public virtual ICollection<OutputInfo> OutputInfos { get; set; } = new HashSet<OutputInfo>();
    }
}
