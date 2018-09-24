using Bussiness;
using System;
using System.Web;

namespace WebClasses
{
    public class WebUtilities
    {
        private static string MIDDLE_TIER_KEY = ".ylh";
        public static MiddleTier GetMiddleTier(HttpContextBase context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            MiddleTier mt = (MiddleTier)context.Application[MIDDLE_TIER_KEY];
            if (mt == null)
            {
                context.Application.Lock();
                mt = (MiddleTier)context.Application[MIDDLE_TIER_KEY];
                if (mt == null)
                {
                    mt = new MiddleTier();
                    context.Application.Add(MIDDLE_TIER_KEY, mt);
                }
                context.Application.UnLock();
            }
            return mt;
        }
    }
}
