using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Entity
{
    public class SystemLog
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Action { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
