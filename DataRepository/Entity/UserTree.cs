using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Entity
{
    public class UserTree
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserChain { get; set; }
        public string PreviousUserId { get; set; }
        public bool IsLeaf { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
