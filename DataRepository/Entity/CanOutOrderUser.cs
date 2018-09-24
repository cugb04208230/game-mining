using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Entity
{
    public class CanOutOrderUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string GPNO { get; set; }
        public DateTime? OrderTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
