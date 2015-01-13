
namespace DemoSqlClient.Original 
{


    [System.Serializable()]
    [System.Diagnostics.DebuggerDisplay("{Message}\r\n\r\n{StackTrace}")]
    public class ExceptionWrapper
    {
        public string Message;
        public string StackTrace;
        public string Source;
        public ExceptionWrapper InnerException;


        public ExceptionWrapper()
        { }


        public ExceptionWrapper(System.Exception ex)
        {
            this.Message = ex.Message;
            this.StackTrace = ex.StackTrace;
            this.Source = ex.Source;
            this.InnerException = ex.InnerException == null ? null : new ExceptionWrapper(ex.InnerException);
        }

    } // End Class ExceptionWrapper


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
        { } // End Constructor 


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
        } // End Constructor 


        public System.Data.IDbDataParameter ToParameter(System.Data.IDbCommand cmd)
        {
            if (cmd == null)
                throw new System.ArgumentNullException("cmd is NULL");

            System.Data.IDbDataParameter prm = cmd.CreateParameter();

            prm.ParameterName = this.ParameterName;
            prm.Value = this.Value;
            prm.DbType = this.DbType;
            prm.Size = this.Size;
            prm.Precision = (byte)this.Precision;
            prm.Scale = (byte)this.Scale;
            prm.Direction = this.Direction;
            prm.SourceColumn = this.SourceColumn;
            prm.SourceVersion = this.SourceVersion;

            return prm;
        } // End Function ToParameter 


    } // End Class ParameterWrapper 



    [System.Serializable()]
    public class CommandWrapper
    {
        public string SQL;
        public ParameterWrapper[] Parameters;


        public CommandWrapper()
        { } // End Constructor 


        public CommandWrapper(string strSQL)
        {
            this.SQL = strSQL;
        } // End Constructor 


        public CommandWrapper(System.Data.IDbCommand cmd) : this(null, cmd)
        { } // End Constructor 


        public CommandWrapper(string strSQL, System.Data.IDbCommand cmd)
        {
            System.Collections.Generic.List<ParameterWrapper> ls = new System.Collections.Generic.List<ParameterWrapper>();
            if (strSQL != null)
                this.SQL = strSQL;
            else
                this.SQL = cmd.CommandText; 

            foreach (System.Data.IDbDataParameter prm in cmd.Parameters)
            {
                ls.Add(new ParameterWrapper(prm));
            } // Next prm 

            this.Parameters = ls.ToArray();
            ls.Clear();
            ls = null;
        } // End Constructor 


        public System.Data.IDbCommand ToCommand(System.Data.Common.DbProviderFactory factory)
        {
            System.Data.IDbCommand cmd = factory.CreateCommand();
            cmd.CommandText = this.SQL;

            foreach (ParameterWrapper prmWrapper in this.Parameters)
            {
                cmd.Parameters.Add(prmWrapper.ToParameter(cmd));
            } // Next prmWrapper

            return cmd;
        } // End Function ToCommand


    } // End Class CommandWrapper


}
