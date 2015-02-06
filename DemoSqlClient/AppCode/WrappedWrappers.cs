
namespace DemoSqlClient
{


    [System.Serializable()]
    public class ParameterWrapper 
    {
        public SqlService.ParameterWrapper Workaround;


        public ParameterWrapper()
        {
            this.Workaround = new SqlService.ParameterWrapper();
        } // End Constructor 

        public ParameterWrapper(System.Data.IDbDataParameter prm)
        {
            this.Workaround = new SqlService.ParameterWrapper();

            this.Workaround.ParameterName = prm.ParameterName;
            this.Workaround.Value = prm.Value;
            // this.Workaround.DbType = (SqlService.DbType)prm.DbType;
            this.Workaround.DbType = (SqlService.DbType)System.Enum.Parse(typeof(SqlService.DbType), prm.DbType.ToString(), true);


            this.Workaround.Size = prm.Size;
            this.Workaround.Precision = prm.Precision;
            this.Workaround.Scale = prm.Scale;
            this.Workaround.Direction = (SqlService.ParameterDirection)System.Enum.Parse(typeof(SqlService.ParameterDirection), prm.Direction.ToString(), true);


            this.Workaround.SourceColumn = prm.SourceColumn;
            // this.Workaround.SourceVersion = (SqlService.DataRowVersion)prm.SourceVersion; // Not a valid value ? WTF ??? 
            this.Workaround.SourceVersion = (SqlService.DataRowVersion)System.Enum.Parse(typeof(SqlService.DataRowVersion), prm.SourceVersion.ToString(), true);
        } // End Constructor 


    } // End Class ParameterWrapper 



    [System.Serializable()]
    public class CommandWrapper
    {

        public SqlService.CommandWrapper Workaround;

        public CommandWrapper()
        { } // End Constructor 


        public CommandWrapper(string strSQL)
        {
            Workaround = new SqlService.CommandWrapper();
            Workaround.SQL = strSQL;
        } // End Constructor 


        public CommandWrapper(System.Data.IDbCommand cmd) : this(null, cmd) 
        { } // End Constructor 


        public CommandWrapper(string strSQL, System.Data.IDbCommand cmd)
        {
            Workaround = new SqlService.CommandWrapper();

            System.Collections.Generic.List<SqlService.ParameterWrapper> ls = new System.Collections.Generic.List<SqlService.ParameterWrapper>();
            if (strSQL != null)
                Workaround.SQL = strSQL;
            else
                Workaround.SQL = cmd.CommandText;

            foreach (System.Data.IDbDataParameter prm in cmd.Parameters)
            {
                ls.Add((new ParameterWrapper(prm)).Workaround);
            } // Next prm 

            Workaround.Parameters = ls.ToArray();
            ls.Clear();
            ls = null;
        } // End Constructor 


    } // End Class CommandWrapper


}
