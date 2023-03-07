using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public abstract class Model
    {
        public int Id { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime? DateUpdated { get; set; } 
        public bool IsDeleted { get; set; }
        public T GetClone<T>() where T : Model
        {
            if (GetType() == typeof(T))
                return (T)MemberwiseClone();
            else
                throw new Exception($"T phải là {GetType()}");
        }
        
    }
}
