using System.Data;
using System.Data.SqlClient;

namespace DataRepository.Hibernate
{
	internal class MsSqlDataBase : AbstractDataBase
	{
		public MsSqlDataBase(string sectionName)
			: base(sectionName)
		{
		}

		public override DataTable GetDataTableBySql(string sql)
		{
			var dataTable = new DataTable();
			var session = Session;
			IDbCommand command = session.Connection.CreateCommand();
			command.CommandText = sql;
			SqlDataAdapter da = new SqlDataAdapter(command as SqlCommand);
			da.Fill(dataTable);
			session.Close();
			return dataTable;
		}
	}
}