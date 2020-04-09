using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Domain.Model._App
{
    public class ConnectionKeeper
    {
        public static IDbConnection Instance => new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString);
    }
}