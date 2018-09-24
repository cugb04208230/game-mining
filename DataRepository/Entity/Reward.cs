using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Entity
{
    public class Reward
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 0-提现  1-推荐人奖励 2-出彩奖励(团购奖励) 3-直通车奖励 4-分期奖励 5激活积分扣除
        /// </summary>
        public int RewardType { get; set; }
        /// <summary>
        /// 0-提现中 1-提现成功 2-提现失败
        /// </summary>
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
