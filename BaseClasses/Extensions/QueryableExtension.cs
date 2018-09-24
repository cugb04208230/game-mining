using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BaseClasses.Data;

namespace BaseClasses.Extensions
{
	/// <summary>
	/// Queryable=>Expression
	/// Enumerable=>Func
	/// </summary>
	public static class QueryableExtension
	{
		#region Order

		private static IOrderedQueryable<T> Order<T>(this IQueryable<T> source, String propertyName, String methodName)
		{
			Type type = typeof(T);
			ParameterExpression arg = Expression.Parameter(type, "p");
			PropertyInfo property = type.GetProperty(propertyName);
			Expression expr = Expression.Property(arg, property);
			Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), property.PropertyType);
			LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
			return (IOrderedQueryable<T>) typeof(Queryable).GetMethods().Single(
					p => String.Equals(p.Name, methodName, StringComparison.Ordinal)
					     && p.IsGenericMethodDefinition
					     && p.GetGenericArguments().Length == 2
					     && p.GetParameters().Length == 2)
				.MakeGenericMethod(typeof(T), property.PropertyType)
				.Invoke(null, new Object[] {source, lambda});
		}

		/// <summary>
		/// 排序
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="orders"></param>
		/// <returns></returns>
		public static IQueryable<T> Order<T>(this IQueryable<T> source, List<OrderField> orders)
		{
			if (orders != null)
			{
				for (int i = 0; i < orders.Count; i++)
				{
					var order = orders[i];
					var methodName = (i == 0 ? "OrderBy" : "ThenBy") +
					                 (order.OrderDirection == OrderDirection.Desc ? "Descending" : "");
					source = source.Order(order.OrderBy, methodName);
				}
			}
			return source;
		}

		/// <summary>
		/// 排序
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="orders"></param>
		/// <returns></returns>
		public static IQueryable<T> OrderDefault<T>(this IQueryable<T> queryable, List<OrderField> orders)
		{
			if (orders == null || orders.Count == 0)
			{
				orders = new List<OrderField> {new OrderField ()};
			}
			return queryable.Order(orders);
		}

        /// <summary>
        /// 正序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, String propertyName)
        {
            return InternalOrder<T>(source, propertyName, "OrderBy");
        }

        /// <summary>
        /// 倒序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, String propertyName)
        {
            return InternalOrder<T>(source, propertyName, "OrderByDescending");
        }

        /// <summary>
        /// ThenBy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, String propertyName)
        {
            return InternalOrder<T>(source, propertyName, "ThenBy");
        }
        /// <summary>
        /// ThenByDescending
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, String propertyName)
        {
            return InternalOrder<T>(source, propertyName, "ThenByDescending");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static IOrderedQueryable<T> InternalOrder<T>(this IQueryable<T> source, String propertyName, String methodName)
        {
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "p");
            PropertyInfo property = type.GetProperty(propertyName);
            Expression expr = Expression.Property(arg, property);
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), property.PropertyType);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            return ((IOrderedQueryable<T>)(typeof(Queryable).GetMethods().Single(
                    p => String.Equals(p.Name, methodName, StringComparison.Ordinal)
                         && p.IsGenericMethodDefinition
                         && p.GetGenericArguments().Length == 2
                         && p.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.PropertyType)
                .Invoke(null, new Object[] { source, lambda })));
        }

        /// <summary>
        /// 排序表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public static IQueryable<T> InternalOrder<T>(this IQueryable<T> source, List<OrderField> orders)
        {
            for (int i = 0; i < orders.Count; i++)
            {
                var order = orders[i];
                var methodName = (i == 0 ? "OrderBy" : "ThenBy") +
                    (order.OrderDirection == OrderDirection.Desc ? "Descending" : "");
                source = source.InternalOrder(order.OrderBy, methodName);
            }
            return source;
        }


        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public static IQueryable<T> InternalOrderBy<T>(this IQueryable<T> queryable, List<OrderField> orders)
        {
            if (orders == null || orders.Count == 0)
            {
                orders = new List<OrderField> { new OrderField () };
            }
            return queryable.InternalOrder(orders);
        }

        #endregion

        #region Where

        /// <summary>
        /// Query to Where Expression
        /// </summary>
        /// <typeparam name="TEntity">Query Data Type</typeparam>
        /// <param name="query">
        /// query can be divided into two categories
        /// 1.Nullable =>{long,int,Enum,DateTime} ExpressionType:GreaterThanOrEqual,LessThanOrEqual,Equal; Name can use Regular Name or use Suffix like (From or To)
        /// 2.NotNullable=>{String,Array} Method Contains
        /// </param>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> Where<TEntity>(this BaseQuery<TEntity> query)
		{
			var queryTypeProperties = query.GetType().GetProperties(); //查询字段的所有属性
			var defaultExpr = InitDefaultLambdaBody(query); //生成默认的Lambda表达式
			foreach (var queryTypeProperty in queryTypeProperties)
			{
				var queryTypePropertyTypeName = queryTypeProperty.PropertyType.Name;
				var queryTypePropertyValue = queryTypeProperty.GetValue(query);
				if (queryTypePropertyValue != null) //字段属性是否为空
				{
					if (queryTypePropertyTypeName.Contains("Nullable")|| queryTypeProperty.Name.EndsWith("Equal") || queryTypeProperty.Name.EndsWith("Not"))
					{
						defaultExpr = defaultExpr.NullablePropertyExpression(queryTypeProperty, queryTypePropertyValue, query.EntityType);
						continue;
					}
					defaultExpr =
						defaultExpr.NotNullablePropertyExpression(queryTypeProperty, queryTypePropertyValue, query.EntityType);
				}
			}
			return defaultExpr;
		}

		/// <summary>
		/// 可为空对象的表达式生成
		/// {long,int,Enum,DateTime} ExpressionType:GreaterThanOrEqual,LessThanOrEqual,Equal; Name can use Regular Name or use Suffix like (From or To)
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="defaultExpr"></param>
		/// <param name="queryTypeProperty"></param>
		/// <param name="queryTypePropertyValue"></param>
		/// <param name="entityType"></param>
		/// <returns></returns>
		private static Expression<Func<TEntity, bool>> NullablePropertyExpression<TEntity>(
			this Expression<Func<TEntity, bool>> defaultExpr, PropertyInfo queryTypeProperty, object queryTypePropertyValue,
			Type entityType)
		{
			var lambdaParam = Expression.Parameter(entityType);
			var entityTypeProperties = entityType.GetProperties();
			var entityTypePropertyName = queryTypeProperty.Name;
			var expressionType = ExpressionType.Equal;
			if (entityTypePropertyName.EndsWith("From"))
			{
				entityTypePropertyName = entityTypePropertyName.Remove(entityTypePropertyName.Length - 4);
				expressionType = ExpressionType.GreaterThanOrEqual;
			}
			if (entityTypePropertyName.EndsWith("To"))
			{
				entityTypePropertyName = entityTypePropertyName.Remove(entityTypePropertyName.Length - 2);
				expressionType = ExpressionType.LessThanOrEqual;
			}
			if (entityTypePropertyName.EndsWith("Equal"))
			{
				entityTypePropertyName = entityTypePropertyName.Remove(entityTypePropertyName.Length - 5);
				expressionType = ExpressionType.Equal;
			}
			if (entityTypePropertyName.EndsWith("Not"))
			{
				entityTypePropertyName = entityTypePropertyName.Remove(entityTypePropertyName.Length - 3);
				expressionType = ExpressionType.NotEqual;
			}
			var entityTypeProperty = entityTypeProperties.FirstOrDefault(e => e.Name == entityTypePropertyName);
			if (entityTypeProperty != null)
			{
				return defaultExpr.And(Expression.Lambda<Func<TEntity, bool>>(
					BuildLambdaBody(expressionType, Expression.PropertyOrField(lambdaParam, entityTypePropertyName),
						Expression.Constant(queryTypePropertyValue, entityTypeProperty.PropertyType)), lambdaParam));
			}
			return defaultExpr;
		}

		/// <summary>
		/// NotNullable=>{String,Array} Method Contains
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="defaultExpr"></param>
		/// <param name="queryTypeProperty"></param>
		/// <param name="queryTypePropertyValue"></param>
		/// <param name="entityType"></param>
		/// <returns></returns>
		private static Expression<Func<TEntity, bool>> NotNullablePropertyExpression<TEntity>(
			this Expression<Func<TEntity, bool>> defaultExpr, PropertyInfo queryTypeProperty, object queryTypePropertyValue,
			Type entityType)
		{
			var entityTypePropertyName = queryTypeProperty.Name.ToSingular();
			var entityTypeProperty = entityType.GetProperties().FirstOrDefault(e => e.Name == entityTypePropertyName);
			if (entityTypeProperty != null)
			{
				var p1 = Expression.Parameter(entityType, entityType.Name);
				var constant = Expression.Constant(queryTypePropertyValue);
				if (queryTypeProperty.PropertyType.IsArray) //数组Contains
				{
					var property = Expression.Property(p1, entityType, entityTypeProperty.Name);
					var searchValuesAsConstant = Expression.Constant(queryTypePropertyValue, queryTypePropertyValue.GetType());
					var containsBody = Expression.Call(typeof(Enumerable), "Contains", new[] {entityTypeProperty.PropertyType},
						searchValuesAsConstant, property);
					defaultExpr = defaultExpr.And(Expression.Lambda<Func<TEntity, bool>>(containsBody, p1));
				}
				else //字符串 Contains or Equal
				{
					var method = queryTypeProperty.PropertyType.GetMethod("Contains", new[] {queryTypeProperty.PropertyType});
					defaultExpr =
						defaultExpr.And(
							Expression.Lambda<Func<TEntity, bool>>(
								Expression.Call(Expression.Property(p1, entityTypeProperty), method, constant), p1));
				}
			}
			return defaultExpr;
		}

		private static Expression<Func<TEntity, bool>> InitDefaultLambdaBody<TEntity>(this BaseQuery<TEntity> query)
		{
			var lambdaParam = Expression.Parameter(query.EntityType);
			var defaultLambdaBody = BuildLambdaBody(ExpressionType.GreaterThanOrEqual,
				Expression.PropertyOrField(lambdaParam, "Id"), Expression.Constant(0L, typeof(long)));
			return Expression.Lambda<Func<TEntity, bool>>(defaultLambdaBody, lambdaParam);
		}

		private static BinaryExpression BuildLambdaBody(ExpressionType expressionType, MemberExpression memberExpression,
			ConstantExpression constantExpression)
		{
			return Expression.MakeBinary(expressionType, memberExpression, constantExpression);
		}
		#endregion
	}
}