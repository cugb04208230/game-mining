using System;

namespace DataRepository.Entity
{
    public class UserInfo
    {
        public int Id { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal Balance { get; set; }
        public string RecommendUserId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string BankName { get; set; }
        public string CardId { get; set; }
        /// <summary>
        /// 1-会员，2-代理，3-报单中心
        /// </summary>
        public int Grade { get; set; }
        public string GPNo { get; set; }
        /// <summary>
        /// 排单时间
        /// </summary>
        public Nullable<DateTime> OrderTime { get; set; }
        /// <summary>
        /// 升级代理时间
        /// </summary>
        public Nullable<DateTime> UpgradeTime { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 0-未排单 1-已排单 2-已出单 3-等待发放 4-已发放 100-停用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 0-默认 1-6000现金，2-团购1.5万元产品，3-5万积分 4-7500团购金
        /// </summary>
        public int ReceiveType { get; set; }

        public int FriendCount { get; set; }

        /// <summary>
        /// 团购好友数
        /// </summary>
        public int ValidFriendCount { get; set; }

        public string QrcodeUrl { get; set; }

        public string YlUserId { get; set; }

        public string YlMobile { get; set; }

        /// <summary>
        /// 直通车数量
        /// </summary>
        public int DirectBusCount { get; set; }

        /// <summary>
        /// 直通奖励次数（25次奖励一次）
        /// </summary>
        public int DirectBusCount1 { get; set; }

        /// <summary>
        /// 是否激活用户类型
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 是否为套餐
        /// </summary>
        public bool IsPackage { get; set; }
    }
}
