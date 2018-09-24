using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using BaseClasses.Data;

namespace DataRepository
{
    public interface IDataBase
    {
        ISession Session { get; }
        IStatelessSession StatelessSession { get; }
        T Get<T>(object id);
        T Get<T>(Expression<Func<T, bool>> predicate);
	    T Get<T>(Expression<Func<T, bool>> predicate,ISession session);
		T GetByHql<T>(string hqlString);
        T GetBySql<T>(string sqlString);
        void Save<T>(T entity);
        void Save<T>(T entity, ISession session);
        void Update<T>(T entity);
        void Update<T>(T entity, ISession session);
        void Delete<T>(T entity);
        void Delete<T>(T entity, ISession session);
        void Delete<T>(Expression<Func<T, bool>> predicate);
        void Delete<T>(Expression<Func<T, bool>> predicate, ISession session);
        void SaveOrUpdate<T>(T entity);
        void SaveOrUpdate<T>(T entity, ISession session);
        int ExecuteBySql(string sqlString);
        int ExecuteBySql(string sqlString, ISession session);
        int ExecuteByHql(string hqlString);
        int ExecuteByHql(string hqlString, ISession session);
        int Count<T>(Expression<Func<T, bool>> predicate);
	    int Count<T>(Expression<Func<T, bool>> predicate, ISession session);
		int CountBySql(string sql);
        int CountByHql(string hql);
        DataTable GetDataTableBySql(string sql);
        IList<T> GetList<T>(String sql);
        List<T> GetList<T>(Expression<Func<T, bool>> predicate);
	    List<T> GetList<T>(Expression<Func<T, bool>> predicate, ISession session);

		List<T> GetList<T>(Expression<Func<T, bool>> predicate, int pageSize, int pageIndex);
        List<T> GetSortedList<T, TSortColumn1>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSortColumn1>> orderBy1, bool asc);
        List<T> GetSortedList<T, TSortColumn1>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSortColumn1>> orderBy1, bool asc, int pageSize, int pageIndex);
        List<T> GetSortedList<T, TSortColumn1, TSortColumn2>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSortColumn1>> orderBy1, bool asc1, Expression<Func<T, TSortColumn2>> orderBy2, bool asc2);
        List<T> GetSortedList<T, TSortColumn1, TSortColumn2>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSortColumn1>> orderBy1, bool asc1, Expression<Func<T, TSortColumn1>> orderBy2, bool asc2, int pageSize, int pageIndex);
        List<T> GetSortedList<T, TSortColumn1, TSortColumn2, TSortColumn3>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSortColumn1>> orderBy1, bool asc1, Expression<Func<T, TSortColumn2>> orderBy2, bool asc2, Expression<Func<T, TSortColumn3>> orderBy3, bool asc3);
        List<T> GetSortedList<T, TSortColumn1, TSortColumn2, TSortColumn3>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSortColumn1>> orderBy1, bool asc1, Expression<Func<T, TSortColumn2>> orderBy2, bool asc2, Expression<Func<T, TSortColumn3>> orderBy3, bool asc3, int pageSize, int pageIndex);


	    /// <summary>
	    /// 查询
	    /// </summary>
	    /// <param name="query"></param>
	    /// <returns></returns>
	    QueryResult<TEntity> Query<TEntity>(BaseQuery<TEntity> query);

	    /// <summary>
	    /// 数量
	    /// </summary>
	    /// <param name="query"></param>
	    /// <returns></returns>
	    int Count<TEntity>(BaseQuery<TEntity> query);

	    /// <summary>
	    /// 模型
	    /// </summary>
	    /// <returns></returns>
	    IQueryable<TEntity> Select<TEntity>(BaseQuery<TEntity> query, ISession session);

	    /// <summary>
	    /// 条件
	    /// </summary>
	    /// <param name="query"></param>
	    /// <returns></returns>
	    Expression<Func<TEntity, bool>> Where<TEntity>(BaseQuery<TEntity> query);

		/// <summary>
		/// 排序
		/// </summary>
		/// <param name="queryable"></param>
		/// <param name="query"></param>
		/// <returns></returns>
		IQueryable<TEntity> OrderBy<TEntity>(IQueryable<TEntity> queryable, BaseQuery<TEntity> query);

    }
}
