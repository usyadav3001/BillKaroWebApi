using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudioWebApi.Repository
{
    public class Sql
    {
        private SqlConnection connection;
        private SqlTransaction SqlTransaction { get; set; }
        bool mustCloseConnection = true;
        public DataTable ExecuteQuery(string sqlQuery)
        {
            SqlConnection dbConnection = GetConnection();
            using SqlCommand dbCommand = new SqlCommand();


            PrepareCommand(dbCommand, dbConnection, CommandType.Text, sqlQuery, null);

            DataTable dt = new DataTable();
            using (var dataReader = dbCommand.ExecuteReader())
            {
                dt.Load(dataReader);
            }
            if (mustCloseConnection)
                CloseConnection();
            return dt;
        }

        public DataTable ExecuteSqlCommand<T>(SqlCommand sqlCommand)
        {
            SqlConnection dbConnection = GetConnection();
            SqlCommand dbCommand = new SqlCommand();
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            sqlCommand.CommandTimeout = 60;
            //PrepareCommand(dbCommand, dbConnection, CommandType.Text, "", null);

            // Execute the command & return the results
            DataTable dt = new DataTable();
            using (var dataReader = sqlCommand.ExecuteReader())
            {
                dt.Load(dataReader);
            }
            if (mustCloseConnection)
                CloseConnection();
            return dt;
        }

        public DataTable ExecuteQuery(string spName, DataTable dt, string mGUID, string dtparamname)
        {
            SqlConnection dbConnection = GetConnection();
            SqlCommand dbCommand = new SqlCommand
            {
                Connection = dbConnection,
                CommandTimeout = 60,
                CommandType = CommandType.StoredProcedure
            };

            dbCommand.Parameters.AddWithValue("@guid", mGUID);
            dbCommand.Parameters.AddWithValue(dtparamname, dt);
            dbCommand.CommandText = spName;

            // If we were provided a transaction, assign it
            if (SqlTransaction != null)
            {
                if (SqlTransaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                dbCommand.Transaction = SqlTransaction;
            }
            DataTable dt1 = new DataTable();
            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }
            using (var dataReader = dbCommand.ExecuteReader())
            {
                dt1.Load(dataReader);
            }
            if (mustCloseConnection)
                CloseConnection();
            return dt1;
        }

        public T ExecuteScalar<T>(string sqlQuery)
        {
            SqlConnection dbConnection = GetConnection();
            SqlCommand dbCommand = new SqlCommand();

            PrepareCommand(dbCommand, dbConnection, CommandType.Text, sqlQuery, null);

            // Execute the command & return the results
            object retObj = dbCommand.ExecuteScalar();
            // Detach the SqlParameters from the command object, so they can be used again
            dbCommand.Parameters.Clear();

            if (mustCloseConnection)
                CloseConnection();
            if (!(retObj is null))
                return (T)Convert.ChangeType(retObj, typeof(T));
            return default;
        }

        public T ExecuteScalar<T>(SqlCommand sqlCommand)
        {
            SqlConnection dbConnection = GetConnection();
            SqlCommand dbCommand = new SqlCommand();
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            sqlCommand.CommandTimeout = 60;
            //PrepareCommand(dbCommand, dbConnection, CommandType.Text, "", null);

            // Execute the command & return the results
            object retObj = sqlCommand.ExecuteScalar();
            // Detach the SqlParameters from the command object, so they can be used again
            dbCommand.Parameters.Clear();

            if (mustCloseConnection)
                CloseConnection();
            if (!(retObj is null))
                return (T)Convert.ChangeType(retObj, typeof(T));
            return default;
        }

        public int ExecuteNonQuery(string sqlQuery)
        {
            SqlConnection dbConnection = GetConnection();
            using SqlCommand dbCommand = new SqlCommand();

            PrepareCommand(dbCommand, dbConnection, CommandType.Text, sqlQuery, null);

            int i = dbCommand.ExecuteNonQuery();
            if (mustCloseConnection)
                CloseConnection();
            return i;
        }
        private void PrepareCommand(SqlCommand command, SqlConnection connection, CommandType commandType, string commandText, List<SqlParameter> commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            command.CommandTimeout = 60;
            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (SqlTransaction != null)
            {
                if (SqlTransaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = SqlTransaction;
            }

            // Set the command type
            command.CommandType = commandType;
            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
        }
        private void AttachParameters(SqlCommand command, List<SqlParameter> commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (DbParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }
        private SqlConnection GetConnection()
        {
            return Connection;
            
        }
        public void CloseConnection()
        {
            SqlConnection conn = GetConnection();
            if (conn.State != ConnectionState.Closed)
                conn.Close();
        }
        public SqlConnection Connection
        {
            get
            {
                if (connection is null)
                {
                    connection = new SqlConnection("Data Source=WFX-LAP-148;Initial Catalog=studio;Persist Security Info=True;User ID=sa;Password=sql@2019");
                }
                return connection;
            }
            set { connection = value; }
        }

        public string ConcatString(params string[] array) =>
       string.Concat(array);
    }
}
