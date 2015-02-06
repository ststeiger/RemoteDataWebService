
namespace DemoSqlClient
{


    // http://localhost:3830/COR-Basic/SoapApi/SqlService.asmx
    internal class SQL
    {

        public static System.Data.Common.DbProviderFactory ProviderFactory = System.Data.Common.DbProviderFactories.GetFactory("System.Data.SqlClient");
        private static SqlService.SqlService service = SetupService();





        public static SqlService.SqlService SetupService()
        {
            SqlServiceFormsAuthEnabled service = new SqlServiceFormsAuthEnabled();
            service.Url = "http://localhost:3830/COR-Basic/SoapApi/SqlService.asmx";
            // service.Url = "https://www6.cor-asp.ch/cor_basic/demo/SoapApi/SqlService.asmx";
            // service.Credentials = new System.Net.NetworkCredential("username", "password", "domain");
            // service.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;

            service.Login("username", "password");
            string loginStatus = service.GetLoginStatus();
            System.Console.WriteLine(loginStatus);
            // service.Logout();
            // string rofl = service.GetLoginStatus();

            return service;
        }


        public static System.Data.IDbCommand CreateCommand()
        {
            return CreateCommand("");
        }


        public static System.Data.IDbCommand CreateCommand(string strSQL)
        { 
            return new System.Data.SqlClient.SqlCommand(strSQL);
        }


        public static System.Data.DataTable GetDataTable(string strSQL)
        {
            string str = service.GetDataTable(strSQL);
            System.Data.DataTable dt = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(str);

            return dt;
        }


        public static System.Data.DataTable GetDataTable(System.Data.IDbCommand cmd)
        {
            SqlService.CommandWrapper cwcmd = (new CommandWrapper(cmd)).Workaround;
            string str = service.GetDataTableCommand(cwcmd);
            System.Data.DataTable dt = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(str);
            return dt;
        }


        public static object ExecuteScalar(string strSQL)
        {
            return service.ExecuteScalar(strSQL);
        }


        public static object ExecuteScalar(System.Data.IDbCommand cmd)
        {
            SqlService.CommandWrapper cwcmd = (new CommandWrapper(cmd)).Workaround;
            return service.ExecuteScalarCommand(cwcmd);
        }


        public static void ExecuteNonQuery(string strSQL)
        {
            SqlService.ExceptionWrapper ex2 = service.ExecuteNonQuery(strSQL);
            if (ex2 != null)
            {
                RemoteDbException ex = new RemoteDbException(ex2);
                throw ex;
            }
        }


        public static void ExecuteNonQuery(System.Data.IDbCommand cmd)
        {
            SqlService.CommandWrapper cwcmd = (new CommandWrapper(cmd)).Workaround;
            SqlService.ExceptionWrapper ex2 = service.ExecuteNonQueryCommand(cwcmd);
            if (ex2 != null)
            {
                RemoteDbException ex = new RemoteDbException(ex2);
                throw ex;
            }
        }


        public static void ExecuteNonQuery(string strSQL, System.Data.IDbDataParameter[] args)
        {
            System.Collections.Generic.List<SqlService.ParameterWrapper> ls = new System.Collections.Generic.List<SqlService.ParameterWrapper>();

            foreach (System.Data.IDbDataParameter parm in args)
            {
                SqlService.ParameterWrapper sqparw = new ParameterWrapper(parm).Workaround;
                ls.Add(sqparw);
            } // Next parm 

            SqlService.ExceptionWrapper ex2 = service.ExecuteNonQueryWithParameters(strSQL, ls.ToArray());
            ls.Clear();
            ls = null;

            if (ex2 != null)
            {
                RemoteDbException ex = new RemoteDbException(ex2);
                throw ex;
            }
        }


        public class RemoteDbException : System.Data.Common.DbException
        {


            protected static System.Exception exConvert(SqlService.ExceptionWrapper ex)
            { 
                System.Exception ex2 = null;

                if(ex != null && ex.InnerException != null)
                    ex2 = exConvert(ex.InnerException);

                if(ex != null)
                    new System.Exception(ex.Message, ex2);

                return ex2;
            }


            public RemoteDbException() :base()
            { }

            public RemoteDbException(System.Exception ex)
                : base(ex.Message, ex)
            { }

            public RemoteDbException(SqlService.ExceptionWrapper ex)
                : base(ex.Message, exConvert(ex.InnerException))
            { }

        } // End Class RemoteDbException 


        public static System.Data.Common.DbProviderFactory GetFactory(System.Type tFactoryType)
        {
            if (tFactoryType != null && tFactoryType.IsSubclassOf(typeof(System.Data.Common.DbProviderFactory)))
            {
                // Provider factories are singletons with Instance field having the sole instance
                System.Reflection.FieldInfo field = tFactoryType.GetField("Instance", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (field != null)
                {
                    //return field.GetValue(null) as DbProviderFactory;
                    return (System.Data.Common.DbProviderFactory)field.GetValue(null);
                } // End if (field != null)
            }

            throw new System.Exception("DataProvider is missing!");
        } // End Function GetFactory




        public static System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue)
        {
            return AddParameter(command, strParameterName, objValue, System.Data.ParameterDirection.Input);
        } // End Function AddParameter


        public static System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue, System.Data.ParameterDirection pad)
        {
            if (objValue == null)
            {
                //throw new ArgumentNullException("objValue");
                objValue = System.DBNull.Value;
            }
            // End if (objValue == null)
            System.Type tDataType = objValue.GetType();
            System.Data.DbType dbType = GetDbType(tDataType);

            return AddParameter(command, strParameterName, objValue, pad, dbType);
        } // End Function AddParameter


        public static System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue, System.Data.ParameterDirection pad, System.Data.DbType dbType)
        {
            System.Data.IDbDataParameter parameter = command.CreateParameter();

            if (!strParameterName.StartsWith("@"))
            {
                strParameterName = "@" + strParameterName;
            }

            if (command.Parameters.Contains(strParameterName)) command.Parameters.RemoveAt(strParameterName);


            if (!command.Parameters.Contains(strParameterName))
            {

                // End if (!strParameterName.StartsWith("@"))
                parameter.ParameterName = strParameterName;
                parameter.DbType = dbType;
                parameter.Direction = pad;

                if (objValue == null)
                {
                    parameter.Value = System.DBNull.Value;
                }
                else
                {
                    parameter.Value = objValue;
                }

                command.Parameters.Add(parameter);
                return parameter;
            }

            return null;
        } // End Function AddParameter


        protected static string SqlTypeFromDbType(System.Data.DbType type)
        {
            string strRetVal = null;

            // http://msdn.microsoft.com/en-us/library/cc716729.aspx
            switch (type)
            {
                case System.Data.DbType.Guid:
                    strRetVal = "uniqueidentifier";
                    break;
                case System.Data.DbType.Date:
                    strRetVal = "date";
                    break;
                case System.Data.DbType.Time:
                    strRetVal = "time(7)";
                    break;
                case System.Data.DbType.DateTime:
                    strRetVal = "datetime";
                    break;
                case System.Data.DbType.DateTime2:
                    strRetVal = "datetime2";
                    break;
                case System.Data.DbType.DateTimeOffset:
                    strRetVal = "datetimeoffset(7)";
                    break;

                case System.Data.DbType.StringFixedLength:
                    strRetVal = "nchar(MAX)";
                    break;
                case System.Data.DbType.String:
                    strRetVal = "nvarchar(MAX)";
                    break;

                case System.Data.DbType.AnsiStringFixedLength:
                    strRetVal = "char(MAX)";
                    break;
                case System.Data.DbType.AnsiString:
                    strRetVal = "varchar(MAX)";
                    break;

                case System.Data.DbType.Single:
                    strRetVal = "real";
                    break;
                case System.Data.DbType.Double:
                    strRetVal = "float";
                    break;
                case System.Data.DbType.Decimal:
                    strRetVal = "decimal(19, 5)";
                    break;
                case System.Data.DbType.VarNumeric:
                    strRetVal = "numeric(19, 5)";
                    break;

                case System.Data.DbType.Boolean:
                    strRetVal = "bit";
                    break;
                case System.Data.DbType.SByte:
                case System.Data.DbType.Byte:
                    strRetVal = "tinyint";
                    break;
                case System.Data.DbType.Int16:
                    strRetVal = "smallint";
                    break;
                case System.Data.DbType.Int32:
                    strRetVal = "int";
                    break;
                case System.Data.DbType.Int64:
                    strRetVal = "bigint";
                    break;
                case System.Data.DbType.Xml:
                    strRetVal = "xml";
                    break;
                case System.Data.DbType.Binary:
                    strRetVal = "varbinary(MAX)"; // or image
                    break;
                case System.Data.DbType.Currency:
                    strRetVal = "money";
                    break;
                case System.Data.DbType.Object:
                    strRetVal = "sql_variant";
                    break;

                case System.Data.DbType.UInt16:
                case System.Data.DbType.UInt32:
                case System.Data.DbType.UInt64:
                    throw new System.NotImplementedException("Uints not mapped - MySQL only");
            } // End switch (type)

            return strRetVal;
        } // End Function SqlTypeFromDbType


        protected static System.Data.DbType GetDbType(System.Type type)
        {
            // http://social.msdn.microsoft.com/Forums/en/winforms/thread/c6f3ab91-2198-402a-9a18-66ce442333a6
            string strTypeName = type.Name;
            System.Data.DbType DBtype = System.Data.DbType.String;
            // default value

            try
            {
                if (object.ReferenceEquals(type, typeof(System.DBNull)))
                {
                    return DBtype;
                }

                if (object.ReferenceEquals(type, typeof(System.Byte[])))
                {
                    return System.Data.DbType.Binary;
                }


                DBtype = (System.Data.DbType)System.Enum.Parse(typeof(System.Data.DbType), strTypeName, true);
                // add error handling to suit your taste
            }
            catch (System.Exception exNoMappingPresent)
            {
                // LogError("claSQL.cs ==> SQL.GetDbType", exNoMappingPresent, null);
                throw new System.NotSupportedException("SQL.GetDbType for type \"" + type.ToString() + "\"");
            }

            return DBtype;
        } // End Function GetDbType


    } // End Class SQL


} // End Namespace DemoSqlClient
