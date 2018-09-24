using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using BaseClasses.Data;
using BaseClasses.Extensions;
using NHibernate.Linq;

namespace DataRepository.Hibernate
{
    internal abstract class AbstractDataBase : IDataBase
    {
        private static readonly Dictionary<string, ISessionFactory> SessionFactoryDict = new Dictionary<string, ISessionFactory>();
        private static readonly object MutexSesson = new object();
        private readonly string _sectionName;
        /// <summary>
        /// 配置节点的名称
        /// </summary>
        /// <param name="sectionName"></param>
        protected AbstractDataBase(string sectionName)
        {
            _sectionName = sectionName;
        }

        public ISessionFactory SessionFactory
        {
            get
            {
                ISessionFactory sessionFactory = null;
                if (!SessionFactoryDict.ContainsKey(_sectionName))
                {
                    lock (MutexSesson)
                    {
                        if (!SessionFactoryDict.ContainsKey(_sectionName))
                        {
                            NHibernate.Cfg.Configuration mConfiguration = new NHibernate.Cfg.Configuration().Configure($"{AppDomain.CurrentDomain.BaseDirectory}/Config/hibernate.cfg.config");
                            sessionFactory = mConfiguration.BuildSessionFactory();
                            SessionFactoryDict.Add(_sectionName, sessionFactory);
                        }
                    }
                }
                else
                {
                    sessionFactory = SessionFactoryDict[_sectionName];
                }
                return sessionFactory;
            }
        }


        public ISession Session => SessionFactory.OpenSession();


	    public IStatelessSession StatelessSession => SessionFactory.OpenStatelessSession();

	    public T Get<T>(object id)
        {
            ISession session = Session;
            T obj = session.Get<T>(id);
            session.Close();
            return obj;
        }

        public T Get<T>(Expression<Func<T, bool>> predicate)
        {
            ISession session = Session;
            T obj = session.Query<T>().Where(predicate).FirstOrDefault();
            session.Close();
            return obj;
        }


	    public T Get<T>(Expression<Func<T, bool>> predicate,ISession session)
	    {
		    T obj = session.Query<T>().Where(predicate).FirstOrDefault();
		    session.Flush();
		    return obj;
	    }

		public T GetByHql<T>(string hqlString)
        {
            ISession session = Session;
            T obj = session.CreateQuery(hqlString).UniqueResult<T>();
            session.Close();
            return obj;
        }

        public T GetBySql<T>(string sqlString)
        {
            ISession session = Session;
            T obj = session.CreateSQLQuery(sqlString).AddEntity(typeof(T)).UniqueResult<T>();
            session.Close();
            return obj;
        }

        public void Save<T>(T entity)
        {
            ISession session = Session;
			entity.SetPropertyValue("CreatedAt",DateTime.Now);
	        entity.SetPropertyValue("LastModifiedAt", DateTime.Now);
	        entity.SetPropertyValue("IsEnabled", true);
			session.Save(entity);
            session.Flush();
            session.Close();
        }

        public void Save<T>(T entity, ISession session)
		{
			entity.SetPropertyValue("CreatedAt", DateTime.Now);
			entity.SetPropertyValue("LastModifiedAt", DateTime.Now);
			entity.SetPropertyValue("IsEnabled", true);
			session.Save(entity);
            session.Flush();
        }

        public void Update<T>(T entity)
        {
            ISession session = Session;
	        entity.SetPropertyValue("LastModifiedAt", DateTime.Now);
			session.Update(entity);
            session.Flush();
            session.Close();
        }

        public void Update<T>(T entity, ISession session)
		{
			entity.SetPropertyValue("LastModifiedAt", DateTime.Now);
			session.Update(entity);
            session.Flush();
        }

        public void Delete<T>(T entity)
        {
            ISession session = Session;
            session.Delete(entity);
            session.Flush();
            session.Close();
        }

        public void Delete<T>(T entity, ISession session)
        {
            session.Delete(entity);
            session.Flush();
        }

        public void Delete<T>(Expression<Func<T, bool>> predicate)
		{
			ISession session = Session;
			Delete(predicate,session);
			session.Close();
		}

        public void Delete<T>(Expression<Func<T, bool>> predicate, ISession session)
        {
	        var entity = Get(predicate);
			session.Delete(entity);
			session.Flush();
		}

        public void SaveOrUpdate<T>(T entity)
        {
            ISession session = Session;
            session.SaveOrUpdate(entity);
            session.Flush();
            session.Close();
        }

        public void SaveOrUpdate<T>(T entity, ISession session)
        {
            session.SaveOrUpdate(entity);
            session.Flush();
        }

        public int ExecuteBySql(string sqlString)
        {
            ISession session = Session;
            int result = session.CreateSQLQuery(sqlString).ExecuteUpdate();
            session.Flush();
            session.Close();
            return result;
        }

        public int ExecuteBySql(string sqlString, ISession session)
        {
            int result = session.CreateSQLQuery(sqlString).ExecuteUpdate();
            session.Flush();
            return result;
        }

        public int ExecuteByHql(string hqlString)
        {
            ISession session = Session;
            int result = session.CreateQuery(hqlString).ExecuteUpdate();
            session.Flush();
            session.Close();
            return result;
        }

        public int ExecuteByHql(string hqlString, ISession session)
        {
            int result = session.CreateQuery(hqlString).ExecuteUpdate();
            session.Flush();
            return result;
        }

        public int Count<T>(Expression<Func<T, bool>> predicate)
        {
            ISession session = Session;
            int count = session.Query<T>().Where(predicate).Count();
            session.Close();
            return count;
        }

	    public int Count<T>(Expression<Func<T, bool>> predicate, ISession session)
	    {
		    int count = session.Query<T>().Where(predicate).Count();
		    session.Flush();
		    return count;
	    }
		public int CountBySql(string sql)
        {
            ISession session = Session;
	        var count = int.Parse(session.CreateSQLQuery(sql).UniqueResult().ToString());
            session.Close();
            return count;
        }

        public int CountByHql(string hql)
        {
            ISession session = Session;
	        var count = int.Parse(session.CreateQuery(hql).UniqueResult().ToString());
            session.Close();
            return count;
        }

        public abstract DataTable GetDataTableBySql(string sql);


        public List<T> GetList<T>(Expression<Func<T, bool>> predicate)
        {
            ISession session = Session;
            var query = session.Query<T>();
            if (predicate != null)
                query = query.Where(predicate);
            List<T> result = query.ToList();
            session.Close();
            return result;
        }


	    public List<T> GetList<T>(Expression<Func<T, bool>> predicate,ISession session)
	    {
		    var query = session.Query<T>();
		    if (predicate != null)
			    query = query.Where(predicate);
		    List<T> result = query.ToList();
		    session.Flush();
		    return result;
	    }

		public IList<T> GetList<T>(String sql)
        {
            ISession session = Session;
            IList<T> result = session.CreateSQLQuery(sql).AddEntity(typeof(T)).List<T>();
            session.Close();
            return result;
        }

        public List<T> GetList<T>(Expression<Func<T, bool>> predicate, int pageSize, int pageIndex)
        {
            int skipCount = (pageIndex - 1) * pageSize;
            ISession session = Session;
            var query = session.Query<T>();
            if (predicate != null)
                query = query.Where(predicate);
            List<T> result = query.Skip(skipCount).Take(pageSize).ToList();
            session.Close();
            return result;
        }

        public List<T> GetSortedList<T, TSortColumn1>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSortColumn1>> orderBy1, bool asc)
        {
            ISession session = Session;
            var query = session.Query<T>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy1 != null)
            {
	            query = asc ? query.OrderBy(orderBy1) : query.OrderByDescending(orderBy1);
            }
            List<T> result = query.ToList();
            session.Close();
            return result;
        }

        public List<T> GetSortedList<T, TSortColumn1>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSortColumn1>> orderBy1, bool asc, int pageSize, int pageIndex)
        {
            int skipCount = (pageIndex - 1) * pageSize;
            ISession session = Session;
            var query = session.Query<T>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy1 != null)
            {
	            query = asc ? query.OrderBy(orderBy1) : query.OrderByDescending(orderBy1);
            }
            List<T> result = query.Skip(skipCount).Take(pageSize).ToList();
            session.Close();
            return result;
        }

        public List<T> GetSortedList<T, TSortColumn1, TSortColumn2>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSortColumn1>> orderBy1, bool asc1, Expression<Func<T, TSortColumn2>> orderBy2, bool asc2)
        {
            ISession session = Session;
            var query = session.Query<T>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy1 != null)
            {
	            query = asc1 ? query.OrderBy(orderBy1) : query.OrderByDescending(orderBy1);
            }
            if (orderBy2 != null)
            {
	            query = asc2 ? query.OrderBy(orderBy2) : query.OrderByDescending(orderBy2);
            }
            List<T> result = query.ToList();
            session.Close();
            return result;
        }

        public List<T> GetSortedList<T, TSortColumn1, TSortColumn2>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSortColumn1>> orderBy1, bool asc1, Expression<Func<T, TSortColumn1>> orderBy2, bool asc2, int pageSize, int pageIndex)
        {
            int skipCount = (pageIndex - 1) * pageSize;
            ISession session = Session;
            var query = session.Query<T>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy1 != null)
            {
	            query = asc1 ? query.OrderBy(orderBy1) : query.OrderByDescending(orderBy1);
            }
            if (orderBy2 != null)
            {
	            query = asc2 ? query.OrderBy(orderBy2) : query.OrderByDescending(orderBy2);
            }
            List<T> result = query.Skip(skipCount).Take(pageSize).ToList();
            session.Close();
            return result;
        }

        public List<T> GetSortedList<T, TSortColumn1, TSortColumn2, TSortColumn3>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSortColumn1>> orderBy1, bool asc1, Expression<Func<T, TSortColumn2>> orderBy2, bool asc2, Expression<Func<T, TSortColumn3>> orderBy3, bool asc3)
        {
            ISession session = Session;
            var query = session.Query<T>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy1 != null)
            {
	            query = asc1 ? query.OrderBy(orderBy1) : query.OrderByDescending(orderBy1);
            }
            if (orderBy2 != null)
            {
	            query = asc2 ? query.OrderBy(orderBy2) : query.OrderByDescending(orderBy2);
            }
            if (orderBy3 != null)
            {
	            query = asc3 ? query.OrderBy(orderBy3) : query.OrderByDescending(orderBy3);
            }
            List<T> result = query.ToList();
            session.Close();
            return result;
        }

        public List<T> GetSortedList<T, TSortColumn1, TSortColumn2, TSortColumn3>(Expression<Func<T, bool>> predicate, Expression<Func<T, TSortColumn1>> orderBy1, bool asc1, Expression<Func<T, TSortColumn2>> orderBy2, bool asc2, Expression<Func<T, TSortColumn3>> orderBy3, bool asc3, int pageSize, int pageIndex)
        {
            int skipCount = (pageIndex - 1) * pageSize;
            ISession session = Session;
            var query = session.Query<T>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy1 != null)
            {
	            query = asc1 ? query.OrderBy(orderBy1) : query.OrderByDescending(orderBy1);
            }
            if (orderBy2 != null)
            {
	            query = asc2 ? query.OrderBy(orderBy2) : query.OrderByDescending(orderBy2);
            }
            if (orderBy3 != null)
            {
	            query = asc3 ? query.OrderBy(orderBy3) : query.OrderByDescending(orderBy3);
            }
            List<T> result = query.Skip(skipCount).Take(pageSize).ToList();
            session.Close();
            return result;
        }

	    /// <inheritdoc />
	    /// <summary>
	    /// 查询
	    /// </summary>
	    /// <param name="query"></param>
	    /// <returns></returns>
	    public QueryResult<TEntity> Query<TEntity>(BaseQuery<TEntity> query)
		{
			ISession session = Session;
			var result = new QueryResult<TEntity> { Query = query };
		    var queryable = Select(query, session).Where(Where(query));
		    queryable = OrderBy(queryable, query);
		    queryable = queryable.Skip(query.Skip).Take(query.PageSize ?? 10);
		    result.Count = Count(query);
		    result.List = queryable.ToList();
			session.Close();
		    return result;
	    }

	    /// <inheritdoc />
	    /// <summary>
	    /// 数量
	    /// </summary>
	    /// <param name="query"></param>
	    /// <returns></returns>
	    public int Count<TEntity>(BaseQuery<TEntity> query)
		{
			ISession session = Session;
			var count =  Select(query, session).Where(Where(query)).Count();
			session.Close();
			return count;
		}

		/// <inheritdoc />
		/// <summary>
		/// 模型
		/// </summary>
		/// <returns></returns>
		public IQueryable<TEntity> Select<TEntity>(BaseQuery<TEntity> query, ISession session) 
		{
			return session.Query<TEntity>();
	    }

		/// <inheritdoc />
		/// <summary>
		/// 条件
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public Expression<Func<TEntity, bool>> Where<TEntity>(BaseQuery<TEntity> query) 
		{
		    return query.Where();
	    }

	    /// <inheritdoc />
	    /// <summary>
	    /// 排序
	    /// </summary>
	    /// <param name="queryable"></param>
	    /// <param name="query"></param>
	    /// <returns></returns>
	    public IQueryable<TEntity> OrderBy<TEntity>(IQueryable<TEntity> queryable, BaseQuery<TEntity> query)
		{
		    return queryable.InternalOrderBy(query.OrderBys);
	    }
	}
}
