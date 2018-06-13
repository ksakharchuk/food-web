using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Support;

namespace sqlData
{
    public class SqlDataProvider
    {

        public static int ParseInt(string value, int defaultValue)
        {
            int res;
            if (!int.TryParse(value, out res))
                res = defaultValue;
            return res;
        }

        // TODO:RT> use appropriate StringExtensions methods instead
        public static DateTime ParseDate(string value, DateTime defaultValue)
        {
            DateTime res;
            if (!DateTime.TryParse(value, out res))
                res = defaultValue;
            return res;
        }

        private static ThrowExceptionWay m_ThrowExceptionWay = ThrowExceptionWay.Log;

        public static int DefaultInt
        {
            get
            {
                return int.MinValue;
            }
        }
        public static byte DefaultByte
        {
            get
            {
                return byte.MinValue;
            }
        }
        public static Char DefaultChar
        {
            get
            {
                return Char.MinValue;
            }
        }
        public static DateTime DefaultDateTime
        {
            get
            {
                return DateTime.MinValue;
            }
        }
        public static string DefaultString
        {
            get
            {
                return string.Empty;
            }
        }
        public static decimal DefaultDecimal
        {
            get
            {
                return decimal.MinValue;
            }
        }
        public static double DefaultDouble
        {
            get
            {
                return double.MinValue;
            }
        }

        public static Guid DefaultGuid
        {
            get
            {
                return Guid.Empty;
            }
        }

        public static byte[] DefaultByteArray
        {
            get
            {
                return new byte[0];
            }
        }

        /// <summary>
        /// Returns DBNull if the parameter value equal to DefaultInt otherwise returns a value of the parameter 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static object DefaultToDBNull(int val)
        {
            return val != DefaultInt ? (object)val : DBNull.Value;
        }
        /// <summary>
        /// Returns DBNull if the parameter value equal to DefaultString otherwise returns a value of the parameter 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static object DefaultToDBNull(string val)
        {
            return val != string.Empty && val != null ? (object)val : DBNull.Value;
        }

        /// <summary>
        /// Returns DBNull if the parameter value equal to DefaultDateTime otherwise returns a value of the parameter 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static object DefaultToDBNull(DateTime val)
        {
            return val != DefaultDateTime ? (object)val : DBNull.Value;
        }


        public static bool IsDefValue(int val)
        {
            return (val == DefaultInt);
        }
        public static bool IsDefValue(DateTime val)
        {
            return (val == DefaultDateTime);
        }
        public static bool IsDefValue(decimal val)
        {
            return (val == DefaultDecimal);
        }
        public static bool IsDefValue(double val)
        {
            return (val == DefaultDouble);
        }
        public static bool IsDefValue(byte val)
        {
            return (val == DefaultByte);
        }

        protected string _ConnectionString = ProjectSettings.GetValue("MainConnectionString");

        public string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
        }

        private int timeOut = 30;
        private int defaultCommandTimeOut
        {
            get
            {
                return timeOut;
            }
            set
            {
                timeOut = value;
            }
        }

        public int CommandTimeOut
        {
            get
            {
                return defaultCommandTimeOut;
            }
            set
            {
                defaultCommandTimeOut = value > 0 ? value : defaultCommandTimeOut;
            }
        }

        public object GetScalar(string query)
        {
            using (SqlCommand command = GetCommand(query))
            {
                return GetScalar(command);
            }
        }
        public object GetScalar(string query, CommandType typeOf)
        {
            using (SqlCommand command = GetCommand(query, typeOf))
            {
                return GetScalar(command);
            }
        }
        public object GetScalar(string query, SqlParameter[] parameters)
        {
            using (SqlCommand command = GetCommand(query, parameters))
            {
                return GetScalar(command);
            }
        }
        public object GetScalar(string query, CommandType typeOf, SqlParameter[] parameters)
        {
            using (SqlCommand command = GetCommand(query, typeOf, parameters))
            {
                return GetScalar(command);
            }
        }

        public bool ExecuteNonQuery(string query)
        {
            using (SqlCommand command = GetCommand(query))
            {
                return ExecuteNonQuery(command);
            }
        }
        public bool ExecuteNonQuery(string query, CommandType typeOf)
        {
            using (SqlCommand command = GetCommand(query, typeOf))
            {
                return ExecuteNonQuery(command);
            }
        }
        public bool ExecuteNonQuery(string query, SqlParameter[] parameters)
        {
            using (SqlCommand command = GetCommand(query, parameters))
            {
                return ExecuteNonQuery(command);
            }
        }
        public bool ExecuteNonQuery(string query, CommandType typeOf, SqlParameter[] parameters)
        {
            return ExecuteNonQuery(query, typeOf, parameters, m_ThrowExceptionWay);
        }
        public bool ExecuteNonQuery(string query, CommandType typeOf, SqlParameter[] parameters, ThrowExceptionWay exceptionWay)
        {
            using (SqlCommand command = GetCommand(query, typeOf, parameters))
            {
                return ExecuteNonQuery(command, exceptionWay);
            }
        }

        public DataTable GetDataTable(string query)
        {
            using (SqlCommand command = GetCommand(query))
            {
                return GetDataTable(command);
            }
        }
        public DataTable GetDataTable(string query, CommandType typeOf)
        {
            using (SqlCommand command = GetCommand(query, typeOf))
            {
                return GetDataTable(command);
            }
        }
        public DataTable GetDataTable(string query, SqlParameter[] parameters)
        {
            using (SqlCommand command = GetCommand(query, parameters))
            {
                return GetDataTable(command);
            }
        }
        public DataTable GetDataTable(string query, CommandType typeOf, SqlParameter[] parameters)
        {
            using (SqlCommand command = GetCommand(query, typeOf, parameters))
            {
                return GetDataTable(command);
            }
        }

        public DataSet GetDataSet(string query)
        {
            using (SqlCommand command = GetCommand(query))
            {
                return GetDataSet(command);
            }
        }
        public DataSet GetDataSet(string query, CommandType typeOf)
        {
            using (SqlCommand command = GetCommand(query, typeOf))
            {
                return GetDataSet(command);
            }
        }
        public DataSet GetDataSet(string query, SqlParameter[] parameters)
        {
            using (SqlCommand command = GetCommand(query, parameters))
            {
                return GetDataSet(command);
            }
        }
        public DataSet GetDataSet(string query, CommandType typeOf, SqlParameter[] parameters)
        {
            using (SqlCommand command = GetCommand(query, typeOf, parameters))
            {
                return GetDataSet(command);
            }
        }

        private object GetScalar(SqlCommand command)
        {
            return GetScalar(command, m_ThrowExceptionWay);
        }

        private object GetScalar(SqlCommand command, ThrowExceptionWay exceptionWay)
        {
            if (command == null)
                return null;

            XstarConnection xstarConnection = GetConnection();
            command.Connection = xstarConnection.Connection;

            try
            {
                if (command.Connection.State != ConnectionState.Connecting)
                    command.Connection.Open();

                object res = command.ExecuteScalar();
                return res != DBNull.Value ? res : null;
            }
            catch (Exception ex)
            {
                if (exceptionWay == ThrowExceptionWay.Log || exceptionWay == ThrowExceptionWay.LogAndThrow)
                {
                    Logger.WriteTrace(string.Format("|FROM SqlDataProvider 771| GetScalar | {0}", command.CommandText), Logger.LogTypes.SQL);
                    foreach (SqlParameter par in command.Parameters)
                    {
                        string text = string.Format("{0}: {1} ({2})", par.ParameterName, par.Value.ToString(), par.DbType.ToString());
                        Logger.WriteTrace(string.Format("|FROM SqlDataProvider 775| GetScalar | {0}", text), Logger.LogTypes.SQL);
                    }
                    Logger.WriteTrace(string.Format("|FROM SqlDataProvider 777| GetScalar | Exption: {0}", ex.Message.ToString()), Logger.LogTypes.Exception);

                }
                if (exceptionWay == ThrowExceptionWay.Throw || exceptionWay == ThrowExceptionWay.LogAndThrow)
                    throw ex;

                return null;
            }
            finally
            {

                if (xstarConnection.Connection.State == ConnectionState.Open)
                    xstarConnection.Connection.Close();
                //xstarConnection.IsBusy = false;
            }
        }

        private bool ExecuteNonQuery(SqlCommand command)
        {
            return ExecuteNonQuery(command, m_ThrowExceptionWay);
        }

        private bool ExecuteNonQuery(SqlCommand command, ThrowExceptionWay exceptionWay)
        {
            if (command == null) return false;

            XstarConnection xstarConnection = GetConnection();
            command.Connection = xstarConnection.Connection;

            bool result;
            try
            {
                if (command.Connection.State != ConnectionState.Open) command.Connection.Open();
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception ex)
            {
                if (exceptionWay == ThrowExceptionWay.Log || exceptionWay == ThrowExceptionWay.LogAndThrow)
                {
                    Logger.WriteTrace(string.Format("|FROM SqlDataProvider 1013| ExecuteNonQuery | {0}", command.CommandText), Logger.LogTypes.SQL);
                    foreach (SqlParameter par in command.Parameters)
                    {
                        string text = string.Format("{0}: {1} ({2})", par.ParameterName, par.Value, par.DbType);   //Logger.WriteTrace(text, Logger.LogTypes.SQL);
                        Logger.WriteTrace(string.Format("|FROM SqlDataProvider 1017| ExecuteNonQuery | {0}", text), Logger.LogTypes.SQL);
                    }
                    Logger.WriteTrace(string.Format("|FROM SqlDataProvider 1019| ExecuteNonQuery | Exption: {0}", ex.Message.ToString()));

                }
                if (exceptionWay == ThrowExceptionWay.Throw || exceptionWay == ThrowExceptionWay.LogAndThrow)
                    throw ex;

                result = false;
            }
            finally
            {
                if (xstarConnection.Connection.State == ConnectionState.Open)
                    xstarConnection.Connection.Close();
                //xstarConnection.IsBusy = false;
            }
            return result;
        }

        private DataTable GetDataTable(SqlCommand command)
        {
            return GetDataTable(command, m_ThrowExceptionWay);
        }
        private DataTable GetDataTable(SqlCommand command, ThrowExceptionWay exceptionWay)
        {
            if (command == null) return null;

            XstarConnection xstarConnection = GetConnection();
            command.Connection = xstarConnection.Connection;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable result = new DataTable();
            try
            {
                dataAdapter.Fill(result);
            }
            catch (SqlException ex)
            {
                if (exceptionWay == ThrowExceptionWay.Log || exceptionWay == ThrowExceptionWay.LogAndThrow)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(string.Format(" SQL Exception occured:  {0}", ex.Message));
                    sb.AppendLine(string.Format(" CommandText: {0}", command.CommandText));
                    sb.AppendLine(string.Format(" Command timeout value: {0}", command.CommandTimeout));
                    foreach (SqlParameter par in command.Parameters)
                    {
                        sb.AppendLine(string.Format("{0}: {1} ({2})", par.ParameterName, par.Value != null ? par.Value.ToString() : null, par.DbType.ToString()));
                    }
                    if (!String.IsNullOrEmpty(ex.StackTrace))
                    {
                        sb.AppendLine(string.Format("Stack Trace: {0} ", ex.StackTrace));
                    }
                    if (ex.InnerException != null)
                    {
                        var inner = ex.InnerException;
                        sb.AppendLine(string.Format("Inner Exception: {0} ", inner.Message));
                        sb.AppendLine(string.Format("Inner Stack Trace: {0} ", inner.StackTrace));
                    }
                    foreach (var sqlError in ex.Errors.Cast<SqlError>())
                    {
                        sb.AppendLine(string.Format("Sql Error Message: '{0}'; Class: {1}, Number: {2}", sqlError.Message, sqlError.Class, sqlError.Number));
                    }
                    Logger.WriteError("FROM SqlDataProvider 1082| GetDataTable | Exption:");
                    Logger.WriteError(sb.ToString());

                }
                if (exceptionWay == ThrowExceptionWay.Throw || exceptionWay == ThrowExceptionWay.LogAndThrow)
                {
                    throw ex;
                }

                return null;
            }
            catch (Exception ex)
            {
                if (exceptionWay == ThrowExceptionWay.Log || exceptionWay == ThrowExceptionWay.LogAndThrow)
                {
                    Logger.WriteTrace(string.Format("FROM SqlDataProvider 1101| GetDataTable | {0}", command.CommandText), Logger.LogTypes.SQL);
                    foreach (SqlParameter par in command.Parameters)
                    {
                        string text = string.Format("FROM SqlDataProvider 1104| GetDataTable | {0}: {1} ({2})", par.ParameterName, par.Value != null ? par.Value.ToString() : null, par.DbType.ToString());
                        Logger.WriteTrace(text, Logger.LogTypes.SQL);
                    }
                    Logger.WriteError("FROM SqlDataProvider 1107| GetDataTable | Exption:");
                    Logger.WriteTrace(ex);

                }
                if (exceptionWay == ThrowExceptionWay.Throw || exceptionWay == ThrowExceptionWay.LogAndThrow)
                    throw ex;

                return null;
            }
            finally
            {
                if (xstarConnection.Connection.State == ConnectionState.Open)
                    xstarConnection.Connection.Close();
            }
            return result;
        }

        private DataSet GetDataSet(SqlCommand command)
        {
            return GetDataSet(command, m_ThrowExceptionWay);
        }
        private DataSet GetDataSet(SqlCommand command, ThrowExceptionWay exceptionWay)
        {
            if (command == null) return null;

            XstarConnection xstarConnection = GetConnection();
            command.Connection = xstarConnection.Connection;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataSet result = new DataSet();
            try
            {
                dataAdapter.Fill(result);
            }
            catch (Exception ex)
            {
                if (exceptionWay == ThrowExceptionWay.Log || exceptionWay == ThrowExceptionWay.LogAndThrow)
                {
                    //Logger.WriteTrace(command.CommandText, Logger.LogTypes.SQL);
                    foreach (SqlParameter par in command.Parameters)
                    {
                        //string text = string.Format("{0}: {1} ({2})", par.ParameterName, (par.Value == null) ? "null" : par.Value.ToString(), par.DbType.ToString());
                        //Logger.WriteTrace(text, Logger.LogTypes.SQL);
                    }
                    Logger.WriteError("FROM SqlDataProvider 1170| GetDataSet | Exption:");
                    Logger.WriteTrace(ex);

                }
                if (exceptionWay == ThrowExceptionWay.Throw || exceptionWay == ThrowExceptionWay.LogAndThrow)
                    throw ex;

                return null;
            }
            finally
            {
                if (xstarConnection.Connection.State == ConnectionState.Open)
                    xstarConnection.Connection.Close();

                //xstarConnection.IsBusy = false;
            }
            return (result);
        }

        private SqlCommand GetCommand(string query, CommandType typeOf, SqlParameter[] parameters)
        {
            SqlCommand command = new SqlCommand();

            command.CommandTimeout = this.CommandTimeOut;

            command.CommandType = typeOf;
            command.CommandText = query;
            if (parameters == null) return command;
            foreach (SqlParameter item in parameters)
            {
                if (item == null) continue;
                command.Parameters.Add(item);
            }
            return command;
        }
        private SqlCommand GetCommand(string query, CommandType typeOf)
        {
            return GetCommand(query, typeOf, null);
        }
        private SqlCommand GetCommand(string query, SqlParameter[] parameters)
        {
            return GetCommand(query, CommandType.Text, parameters);
        }
        private SqlCommand GetCommand(string query)
        {
            return GetCommand(query, CommandType.Text, null);
        }

        private XstarConnection GetConnection()
        {
            return new XstarConnection(new SqlConnection(_ConnectionString));
        }
    }
    public class XstarConnection
    {
        public bool IsBusy = true;

        private SqlConnection _Connection = null;

        public XstarConnection(SqlConnection connection)
        {
            _Connection = connection;
        }

        public SqlConnection Connection
        {
            get
            {
                return _Connection;
            }
        }
    }

    /// <summary>
    /// The way to show an exception message
    /// </summary>
    public enum ThrowExceptionWay
    {
        /// <summary>
        /// Write exception message to log only
        /// </summary>
        Log,
        /// <summary>
        /// Throw exception message in Visual Studio during debugging
        /// </summary>
        Throw,
        /// <summary>
        /// Write exception message to log and throw in Visual Studio during debugging
        /// </summary>
        LogAndThrow
    }
}
