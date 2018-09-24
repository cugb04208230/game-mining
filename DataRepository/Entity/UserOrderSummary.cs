using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Entity
{
    public class UserOrderSummary
    {
        public int Id { get; set; }
        public int TotalConfirmOrderUserCount { get; set; }
        /// <summary>
        /// 实际出彩人数
        /// </summary>
        public int OutOrderUserCount { get; set; }
        /// <summary>
        /// 确认出彩人数
        /// </summary>
        public int OutOrderUserCount1 { get; set; }
    }
}
