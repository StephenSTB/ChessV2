using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessV2
{
    public static class ChessDB
    {
        public static SqlConnection GetDBConnection()
        {
            // Get DataBase Connection
            string cn_String = Properties.Settings.Default.ChessDatabase;
            // Create Sql connection from the ChessDataBaseConnectionString.
            SqlConnection cn_connection = new SqlConnection(cn_String);
            
            // Condition to test that the ConnectionState is open (Open if it is not).
            if (cn_connection.State != ConnectionState.Open) cn_connection.Open();

            // Return the connection to the Chess Database.
            return cn_connection;
        }

        // Used for SELECT Queries.
        public static DataTable GetDataTable(string SQL_Text)
        {
            // Initialize a Chess Database connection;
            SqlConnection cn_connection = GetDBConnection();

            // Declare a DataTable to hold the sql query results.
            DataTable table = new DataTable();

            // Declare SqlDataAdapter to query the database.
            SqlDataAdapter adapter = new SqlDataAdapter(SQL_Text, cn_connection);

            // Use adapter to file the table based on the query.
            adapter.Fill(table);

            // Return the resulting table.
            return table;
        }

        public static void ExecuteSQL(string SQL_Text)
        {
            // Initialize a Chess Database connection;
            SqlConnection cn_connection = GetDBConnection();

            // Declare and initialize SqlCommand from the SQL_Text and database connection.
            SqlCommand cmd_Command = new SqlCommand(SQL_Text, cn_connection);

            // Execute the command.
            cmd_Command.ExecuteNonQuery();
        }

    }
}
