using DataRepository.Hibernate;

namespace DataRepository
{
    public class DataRepositoryFactory
    {
        public static IDataBase CreateDataBaseHibernate()
        {
            return new MsSqlDataBase("mssql_config_path");
        }
    }
}
