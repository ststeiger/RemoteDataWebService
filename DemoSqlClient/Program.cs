
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
                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
                System.Windows.Forms.Application.Run(new frmMain());
            } // End if (bShowForm) 
            else
            {
                System.Console.WriteLine(System.Environment.NewLine);
                System.Console.WriteLine(" --- Press any key to continue --- ");
                System.Console.ReadKey();
            } // End if (!bShowForm) 
            
        } // End Sub Main 


    } // End Class Program 


} // End Namespace DemoSqlClient 
