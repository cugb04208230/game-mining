using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Entity
{
    public class SmsVerifyCode
    {
        public int Id { get; set; }

        /// <summary>
        /// 消息类型，用来区分验证码类型
        /// </summary>
        public int CodeType { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }


        /// <summary>
        /// 当前发送的日期,如 2016-10-10
        /// </summary>
        public string CurrentDate { get; set; }

        /// <summary>
        /// 当天发送的次数
        /// </summary>
        public int SendAmount { get; set; }

        /// <summary>
        /// 验证码是否已经使用
        /// </summary>
        public bool IsUsed { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }


        public long TimeOut { get; set; }

        public long NextSendTime { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
