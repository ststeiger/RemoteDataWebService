
using System.Windows.Forms;


namespace DemoSqlClient
{


    public partial class frmMain : Form
    {


        public frmMain()
        {
            InitializeComponent();
        }


        private void btnGetSchema_Click(object sender, System.EventArgs e)
        {

            this.dgvBasicData.DataSource = SQL.GetDataTable(@"
SELECT isc.* 
FROM INFORMATION_SCHEMA.COLUMNS AS isc 

INNER JOIN INFORMATION_SCHEMA.TABLES AS ist 
	ON ist.TABLE_NAME = isc.TABLE_NAME 
	AND ist.TABLE_SCHEMA = isc.TABLE_SCHEMA 
	AND TABLE_TYPE = 'BASE TABLE' 
	
ORDER BY TABLE_SCHEMA, TABLE_NAME, ORDINAL_POSITION 
");
        }


        public void Test()
        {
            object obj = SQL.ExecuteScalar("SELECT COUNT(*) FROM information_schema.tables");
            System.Console.WriteLine(obj);

            SQL.ExecuteNonQuery("SELECT * FROM T_Benutzer");


            using (System.Data.DataTable dt = SQL.GetDataTable("SELECT * FROM information_schema.tables"))
            {
                System.Console.WriteLine(dt.Rows.Count);
            } // End Using dt



            using (System.Data.IDbCommand cmd = SQL.ProviderFactory.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM T_Benutzer";

                var para = cmd.CreateParameter();
                para.DbType = System.Data.DbType.AnsiString;
                para.ParameterName = "lala";
                para.Value = "123";
                cmd.Parameters.Add(para);

                SQL.ExecuteNonQuery(cmd);

                using (System.Data.DataTable dt2 = SQL.GetDataTable(cmd))
                {
                    System.Console.WriteLine(dt2.Rows.Count);
                } // End Using dt2

            }
        }


    }


}
