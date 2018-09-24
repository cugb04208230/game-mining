using System;
using System.Linq.Expressions;
using System.Reflection;
using BaseClasses.Attributes;
using Bussiness;
using Hangfire;
using Microsoft.Owin;
using Owin;
using WebGrease.Css.Extensions;

//[assembly: OwinStartup(typeof(WebAdmin.Startup))]
namespace WebAdmin
{
	/// <summary>
	/// 启动
	/// </summary>
	public partial class Startup
    {
		/// <summary>
		/// 任务
		/// </summary>
		/// <param name="app"></param>
        public void Configuration(IAppBuilder app)
		{
		}

		/// <summary>
		/// 注册jobmanager中的
		/// </summary>
	    public void HangfireRegist()
		{
			try
			{
				foreach (var method in typeof(JobManager).GetMethods())
				{
					if (method == null|| method.DeclaringType==null)
					{
						continue;
					}
					var attr =
						Attribute.GetCustomAttribute(method, typeof(JobAttribute), false) as
							JobAttribute;
					if (attr == null)
					{
						continue;
					}
					HangfireRegist($"{method.DeclaringType.Name}.{method.Name}", method, attr.Cron,TimeZoneInfo.Local, "jobs");
				}
			}
			catch (Exception error)
			{
			}
		}

		/// <summary>
		/// Register RecurringJob via <see cref="MethodInfo"/>.
		/// </summary>
		/// <param name="recurringJobId">The identifier of the RecurringJob</param>
		/// <param name="method">the specified method</param>
		/// <param name="cron">Cron expressions</param>
		/// <param name="timeZone"><see cref="TimeZoneInfo"/></param>
		/// <param name="queue">Queue name</param>
		public void HangfireRegist(string recurringJobId, MethodInfo method, string cron, TimeZoneInfo timeZone, string queue)
		{
			if (recurringJobId == null) throw new ArgumentNullException(nameof(recurringJobId));
			if (method == null) throw new ArgumentNullException(nameof(method));
			if (cron == null) throw new ArgumentNullException(nameof(cron));
			if (timeZone == null) throw new ArgumentNullException(nameof(timeZone));
			if (queue == null) throw new ArgumentNullException(nameof(queue));

			var parameters = method.GetParameters();

			Expression[] args = new Expression[parameters.Length];

			for (int i = 0; i < parameters.Length; i++)
			{
				args[i] = Expression.Default(parameters[i].ParameterType);
			}

			var x = Expression.Parameter(method.DeclaringType, "x");

			var methodCall = method.IsStatic ? Expression.Call(method, args) : Expression.Call(x, method, args);

			var addOrUpdate = Expression.Call(
				typeof(RecurringJob),
				nameof(RecurringJob.AddOrUpdate),
				new Type[] { method.DeclaringType },
				new Expression[]
				{
					Expression.Constant(recurringJobId),
					Expression.Lambda(methodCall, x),
					Expression.Constant(cron),
					Expression.Constant(timeZone),
					Expression.Constant(queue)
				});

			Expression.Lambda(addOrUpdate).Compile().DynamicInvoke();
		}
	}
	
}
