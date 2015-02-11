
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


        private void btnExecuteScalar_Click(object sender, System.EventArgs e)
        {

            string strSQL = @"
SELECT 
	 --uuid,
	Value
FROM ____File
WHERE uuid = @uuid 
";

            using (System.Data.IDbCommand cmd = SQL.CreateCommand(strSQL))
            {
                cmd.CommandText = "SELECT TOP 1 BE_ID FROM T_Benutzer WHERE BE_User = @be";
                //SQL.AddParameter(cmd, "@be", 12435);
                SQL.AddParameter(cmd, "@be", "administrator");
                
                SQL.AddParameter(cmd, "@uuid", "3FF28752-C436-47F4-94F7-2471B06D30D9");

                

                object obj = SQL.ExecuteScalar(cmd);
                // byte[] content = (byte[])obj;
                // System.IO.File.WriteAllBytes(@"d:\bbb.docx", content);

                MsgBox(obj);
            }

        }


        public void MsgBox(object obj)
        {
            if (obj != null)
                System.Windows.Forms.MessageBox.Show(obj.ToString());
            else
                System.Windows.Forms.MessageBox.Show("obj is NULL");
        }


        private void btnExecuteNonQuery_Click(object sender, System.EventArgs e)
        {
            string strSQL = @"
INSERT INTO ____File(uuid, Value)
VALUES
( 
	 NEWID() --  uuid, uniqueidentifier 
	,@file-- Value, varbinary(max)
)
;
";


            using (System.Data.IDbCommand cmd = SQL.CreateCommand(strSQL))
            {

                byte[] file = System.IO.File.ReadAllBytes(@"D:\stefan.steiger\Downloads\aaaaaaa.docx");

                SQL.AddParameter(cmd, "@file", file);

                SQL.ExecuteNonQuery(cmd);
            }
        }


    }


}
