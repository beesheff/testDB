using MySqlConnector;

namespace testDB
{
	internal class Database
	{
		public MySqlConnection cn;
		public MySqlCommand command;
		public void Connect()
		{
			cn = new MySqlConnection("Datasource=localhost; port=3306; username = root; password=;database=practice3_2");
		}
	}
}