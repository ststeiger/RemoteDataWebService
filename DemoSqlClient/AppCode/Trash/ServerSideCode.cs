
namespace COR_Basic
{

    // http://www.binaryintellect.net/articles/dbd724e9-78f0-4a05-adfb-190d151103b2.aspx

    // How not to do it:
    // http://dotnetslackers.com/articles/aspnet/Securing-ASP-Net-Web-Services-with-Forms-Authentication.aspx

    [System.Web.Services.WebService(Namespace = "http://tempuri.org/")]
    [System.Web.Services.WebServiceBinding(ConformsTo = System.Web.Services.WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class SqlService : System.Web.Services.WebService
    {


        //<WebMethod()> _
        //Public Function HelloWorld() As String
        //    Return "Hello World"
        //End Function


        [System.Web.Services.WebMethod()]
        public bool Login(string UserName, string Password)
        {

            // If UserName.Length > 0 And Password.Length > 0 Then
            if (System.StringComparer.InvariantCultureIgnoreCase.Equals(UserName, "foo") && System.StringComparer.InvariantCulture.Equals(Password, "bar"))
            {
                string str = System.Web.Security.FormsAuthentication.FormsCookieName;
                // FormsAuthentication.SetAuthCookie(UserName, True, "/")
                System.Web.Security.FormsAuthentication.SetAuthCookie(UserName, false);
                return true;
            }
            else
            {
                return false;
            }

        }


        [System.Web.Services.WebMethod()]
        public string GetLoginStatus()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                return "Logged In";
            }
            else
            {
                return "Not Logged In";
            }
        }


        [System.Web.Services.WebMethod()]
        public void Logout()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                System.Web.Security.FormsAuthentication.SignOut();
            }
        }



        [System.Web.Services.WebMethod(MessageName = "GetDataTable", Description = "GetDataTable a data-table by SQL-string")]
        public string GetDataTable(string strSQL)
        {
            try
            {
                if (!Context.User.Identity.IsAuthenticated)
                {
                    throw new System.UnauthorizedAccessException("You are not logged in - login is required.");
                }

                using (System.Data.DataTable dt = null) // Basic_SQL.SQL.GetDataTable(strSQL))
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                }
            }
            catch (System.UnauthorizedAccessException ex)
            {
                throw new System.Web.Services.Protocols.SoapException("Not Authenticated", new System.Xml.XmlQualifiedName("AuthFailure"), ex);
            }
            catch (System.Exception ex)
            {
                throw new System.Web.Services.Protocols.SoapException(ex.Message, new System.Xml.XmlQualifiedName("GenericError"), ex);
            }
        }



        [System.Web.Services.WebMethod(MessageName = "GetDataTableCommand", Description = "GetDataTable a data-table by SQL SQL-command")]
        public string GetDataTableCommand(CommandWrapper cmdWrapper)
        {
            try
            {
                if (!Context.User.Identity.IsAuthenticated)
                {
                    throw new System.UnauthorizedAccessException("You are not logged in - login is required.");
                }

                using (System.Data.DataTable dt = null) //Basic_SQL.SQL.GetDataTable(cmdWrapper.ToCommand(Basic_SQL.SQL.ProviderFactory)))
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                }
            }
            catch (System.UnauthorizedAccessException ex)
            {
                throw new System.Web.Services.Protocols.SoapException("Not Authenticated", new System.Xml.XmlQualifiedName("AuthFailure"), ex);
            }
            catch (System.Exception ex)
            {
                throw new System.Web.Services.Protocols.SoapException(ex.Message, new System.Xml.XmlQualifiedName("GenericError"), ex);
            }
        }


        [System.Web.Services.WebMethod(MessageName = "ExecuteScalar", Description = "Execute SQL-string scalar")]
        public object ExecuteScalar(string strSQL)
        {
            try
            {
                if (!Context.User.Identity.IsAuthenticated)
                {
                    throw new System.UnauthorizedAccessException("You are not logged in - login is required.");
                }

                return null;
                // return Basic_SQL.SQL.ExecuteScalar<object>(strSQL);
            }
            catch (System.UnauthorizedAccessException ex)
            {
                throw new System.Web.Services.Protocols.SoapException("Not Authenticated", new System.Xml.XmlQualifiedName("AuthFailure"), ex);
            }
            catch (System.Exception ex)
            {
                throw new System.Web.Services.Protocols.SoapException(ex.Message, new System.Xml.XmlQualifiedName("GenericError"), ex);
            }

        }


        [System.Web.Services.WebMethod(MessageName = "ExecuteScalarCommand", Description = "Execute SQL-command scalar")]
        public object ExecuteScalarCommand(CommandWrapper cmdWrapper)
        {
            try
            {
                if (!Context.User.Identity.IsAuthenticated)
                {
                    throw new System.UnauthorizedAccessException("You are not logged in - login is required.");
                }

                return null;
                // return Basic_SQL.SQL.ExecuteScalar<object>(cmdWrapper.ToCommand(Basic_SQL.SQL.ProviderFactory));
            }
            catch (System.UnauthorizedAccessException ex)
            {
                throw new System.Web.Services.Protocols.SoapException("Not Authenticated", new System.Xml.XmlQualifiedName("AuthFailure"), ex);
            }
            catch (System.Exception ex)
            {
                throw new System.Web.Services.Protocols.SoapException(ex.Message, new System.Xml.XmlQualifiedName("GenericError"), ex);
            }
        }



        // System.NotSupportedException: Die Schnittstelle System.Data.IDbCommand kann nicht serialisiert werden.
        // Public Function ExecuteNonQuery(cmd As System.Data.IDbCommand) As ExceptionWrapper

        [System.Web.Services.WebMethod(MessageName = "ExecuteNonQueryCommand", Description = "Execute command via CommandWrapper")]
        public ExceptionWrapper ExecuteNonQueryCommand(CommandWrapper cmdWrapper)
        {
            try
            {
                ExceptionWrapper ee = null;

                try
                {
                    // Basic_SQL.SQL.ExecuteNonQuery(cmdWrapper.ToCommand(Basic_SQL.SQL.ProviderFactory));
                }
                catch (System.Exception ex)
                {
                    ee = new ExceptionWrapper(ex);
                }

                // Return ExecuteNonQuery("bla", New System.Data.IDbDataParameter() {Basic_SQL.SQL.CreateParameter("1", 1), Basic_SQL.SQL.CreateParameter("2", 2)})
                return ee;
            }
            catch (System.UnauthorizedAccessException ex)
            {
                throw new System.Web.Services.Protocols.SoapException("Not Authenticated", new System.Xml.XmlQualifiedName("AuthFailure"), ex);
            }
            catch (System.Exception ex)
            {
                throw new System.Web.Services.Protocols.SoapException(ex.Message, new System.Xml.XmlQualifiedName("GenericError"), ex);
            }

            if (!Context.User.Identity.IsAuthenticated)
            {
                throw new System.UnauthorizedAccessException("You are not logged in - login is required.");
            }

        }


        [System.Web.Services.WebMethod(MessageName = "ExecuteNonQuery", Description = "Execute SQL-string")]
        public ExceptionWrapper ExecuteNonQuery(string strSQL)
        {
            try
            {
                if (!Context.User.Identity.IsAuthenticated)
                {
                    throw new System.UnauthorizedAccessException("You are not logged in - login is required.");
                }

                ExceptionWrapper ee = null;

                try
                {
                    // Basic_SQL.SQL.ExecuteNonQuery(strSQL);
                }
                catch (System.Exception ex)
                {
                    ee = new ExceptionWrapper(ex);
                }

                return ee;
            }
            catch (System.UnauthorizedAccessException ex)
            {
                throw new System.Web.Services.Protocols.SoapException("Not Authenticated", new System.Xml.XmlQualifiedName("AuthFailure"), ex);
            }
            catch (System.Exception ex)
            {
                throw new System.Web.Services.Protocols.SoapException(ex.Message, new System.Xml.XmlQualifiedName("GenericError"), ex);
            }
        }


        // <WebMethod()> _
        // Public Function ExecuteNonQueryaaa(strSQL As String, args As System.Data.IDbDataParameter()) As ExceptionWrapper
        [System.Web.Services.WebMethod(MessageName = "ExecuteNonQueryWithParameters", Description = "Execute string with parameters")]
        public ExceptionWrapper ExecuteNonQueryWithParameters(string strSQL, ParameterWrapper[] args)
        {
            try
            {
                if (!Context.User.Identity.IsAuthenticated)
                {
                    throw new System.UnauthorizedAccessException("You are not logged in - login is required.");
                }

                ExceptionWrapper ee = null;

                try
                {
                    using (System.Data.IDbCommand cmd = null) // Basic_SQL.SQL.CreateCommand(strSQL))
                    {
                        foreach (ParameterWrapper parm in args)
                        {
                            cmd.Parameters.Add(parm.ToParameter(cmd));
                        }

                        // Basic_SQL.SQL.ExecuteNonQuery(cmd);
                    }

                }
                catch (System.Exception ex)
                {
                    ee = new ExceptionWrapper(ex);
                }

                return ee;
            }
            catch (System.UnauthorizedAccessException ex)
            {
                throw new System.Web.Services.Protocols.SoapException("Not Authenticated", new System.Xml.XmlQualifiedName("AuthFailure"), ex);
            }
            catch (System.Exception ex)
            {
                throw new System.Web.Services.Protocols.SoapException(ex.Message, new System.Xml.XmlQualifiedName("GenericError"), ex);
            }
        }



        [System.Serializable()]
        [System.Diagnostics.DebuggerDisplay("{Message}\r\n\r\n{StackTrace}")]
        public class ExceptionWrapper
        {
            public string Message;
            public string StackTrace;
            public string Source;

            public ExceptionWrapper InnerException;

            public ExceptionWrapper()
            {
            }


            public ExceptionWrapper(System.Exception ex)
            {
                this.Message = ex.Message;
                this.StackTrace = ex.StackTrace;
                this.Source = ex.Source;
                this.InnerException = ex.InnerException == null ? null : new ExceptionWrapper(ex.InnerException);
            }

        }




        [System.Serializable()]
        public class ParameterWrapper
        {
            public string ParameterName;
            public object Value;
            public System.Data.DbType DbType;
            public int Size;
            public int Precision;
            public int Scale;
            public System.Data.ParameterDirection Direction;
            public string SourceColumn;

            public System.Data.DataRowVersion SourceVersion;

            public ParameterWrapper()
            {
            }

            public ParameterWrapper(System.Data.IDbDataParameter prm)
            {
                this.ParameterName = prm.ParameterName;
                this.Value = prm.Value;
                this.DbType = prm.DbType;

                this.Size = prm.Size;
                this.Precision = prm.Precision;
                this.Scale = prm.Scale;
                this.Direction = prm.Direction;
                this.SourceColumn = prm.SourceColumn;
                this.SourceVersion = prm.SourceVersion;
            }


            public System.Data.IDbDataParameter ToParameter(System.Data.IDbCommand cmd)
            {
                if (cmd == null)
                {
                    throw new System.ArgumentNullException("cmd is NULL");
                }

                System.Data.IDbDataParameter prm = cmd.CreateParameter();

                prm.ParameterName = this.ParameterName;
                prm.Value = this.Value;
                prm.DbType = this.DbType;
                prm.Size = this.Size;
                prm.Precision = System.Convert.ToByte(this.Precision);
                prm.Scale = System.Convert.ToByte(this.Scale);
                prm.Direction = this.Direction;
                prm.SourceColumn = this.SourceColumn;
                prm.SourceVersion = this.SourceVersion;

                return prm;
            } // ToParameter


        }



        [System.Serializable()]
        public class CommandWrapper
        {
            public string SQL;

            public ParameterWrapper[] Parameters;

            public CommandWrapper()
            {
            }


            public CommandWrapper(string strSQL)
            {
                this.SQL = strSQL;
            }


            public CommandWrapper(System.Data.IDbCommand cmd)
                : this(null, cmd)
            {
            } // Constructor 


            public CommandWrapper(string strSQL, System.Data.IDbCommand cmd)
            {
                System.Collections.Generic.List<ParameterWrapper> ls = new System.Collections.Generic.List<ParameterWrapper>();
                if (strSQL != null)
                {
                    this.SQL = strSQL;
                }
                else
                {
                    this.SQL = cmd.CommandText;
                }

                foreach (System.Data.IDbDataParameter prm in cmd.Parameters)
                {
                    ls.Add(new ParameterWrapper(prm));
                }

                this.Parameters = ls.ToArray();
                ls.Clear();
                ls = null;
            } // Constructor 


            public System.Data.IDbCommand ToCommand(System.Data.Common.DbProviderFactory factory)
            {
                System.Data.IDbCommand cmd = factory.CreateCommand();
                cmd.CommandText = this.SQL;

                foreach (ParameterWrapper prmWrapper in this.Parameters)
                {
                    cmd.Parameters.Add(prmWrapper.ToParameter(cmd));
                }

                return cmd;
            } // ToCommand


        } // CommandWrapper


    }


}
