using System.Web;
using System.Web.Security;
using DataRepository;

namespace Bussiness
{
    public class BaseManager
    {
        protected MiddleTier MiddleTier;
        public BaseManager(MiddleTier middleTier)
        {
            MiddleTier = middleTier;
        }

        private IDataBase _dataBase;
        private static readonly object MutexDatabase = new object();
        protected IDataBase DataBase
        {
            get
            {
                if (_dataBase == null)
                {
                    lock (MutexDatabase)
                    {
                        if (_dataBase == null)
                        {
                            _dataBase = DataRepositoryFactory.CreateDataBaseHibernate();
                        }
                    }
                }
                return _dataBase;
            }
        }


	    protected string UserName
	    {
		    get
		    {
			    var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
			    if (cookie == null) return "";
			    var ticket = FormsAuthentication.Decrypt(cookie.Value);
			    if (ticket == null) return "";
			    return ticket.Name;
		    }
	    }

	    public void ExecuteBySql(string sql)
	    {
		    DataBase.ExecuteBySql(sql);
	    }
    }
}
