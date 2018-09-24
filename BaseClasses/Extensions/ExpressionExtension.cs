using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BaseClasses.Extensions
{
	/// <summary>
	/// 表达式扩展函数
	/// </summary>
    public static class ExpressionExtension
	{
		/// <summary>
		/// 组合
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <param name="merge"></param>
		/// <returns></returns>
		public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
		{
			var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
			var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
			return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
		}

		/// <summary>
		/// 与
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
	    {
		    return first.Compose(second, Expression.AndAlso);
	    }

		/// <summary>
		/// 或
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
	    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
	    {
		    return first.Compose(second, Expression.Or);
	    }
	}

	/// <inheritdoc />
	/// <summary>
	/// 参数表达式遍历
	/// </summary>
	public class ParameterRebinder : ExpressionVisitor
	{
		private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

		/// <inheritdoc />
		/// <summary>
		/// ctor.
		/// </summary>
		/// <param name="map"></param>
		public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
		{
			_map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="map"></param>
		/// <param name="exp"></param>
		/// <returns></returns>
		public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
		{
			return new ParameterRebinder(map).Visit(exp);
		}

		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		protected override Expression VisitParameter(ParameterExpression p)
		{
			if (_map.TryGetValue(p, out var replacement))
			{
				p = replacement;
			}

			return base.VisitParameter(p);
		}
	}
}
