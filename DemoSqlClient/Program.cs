
using System.Windows.Forms;


namespace DemoSqlClient
{


    static class Program
    {


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [System.STAThread]
        static void Main()
        {
            bool bShowForm = true;

            if (bShowForm)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }


            if (!bShowForm)
            {
                System.Console.WriteLine(System.Environment.NewLine);
                System.Console.WriteLine(" --- Press any key to continue --- ");
                System.Console.ReadKey();
            }
            
        } // End Sub Main 


    } // End Class Program 


}
