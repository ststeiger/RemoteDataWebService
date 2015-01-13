
namespace DemoSqlClient
{


    internal class SQL
    {
        public static System.Data.Common.DbProviderFactory ProviderFactory = System.Data.Common.DbProviderFactories.GetFactory("System.Data.SqlClient");
        private static SqlService.SqlService service = SetupService();


        public static SqlService.SqlService SetupService()
        {
            SqlServiceFormsAuthEnabled service = new SqlServiceFormsAuthEnabled();
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


    } // End Class SQL


} // End Namespace DemoSqlClient
