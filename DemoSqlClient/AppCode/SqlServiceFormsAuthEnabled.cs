
namespace DemoSqlClient
{


    public class SqlServiceFormsAuthEnabled : SqlService.SqlService
    {


        private string m_authCookieName;
        private System.Net.Cookie m_authCookie;


        public SqlServiceFormsAuthEnabled()
        { } // Constructor 


        public SqlServiceFormsAuthEnabled(string strURL)
        {
            // Set the server URL. You can pull this from a config file or what ever way you want to make it dynamic.
            this.Url = strURL;

            // Calling the LogonUser method defined in the ReportService2005.asmx end point.
            // The LogonUser method authenticates the specified user to the Report Server Web Service when custom authentication has been configured.
            // This is to authenticate against the FBA code and then store the cookie for future reference.
        } // Constructor 


        public new string Url
        {
            get { return base.Url; }

            set
            {
                base.Url = value;

                //try {
                //    base.LogonUser("administrator", "pw", null);
                //} catch (System.Exception ex) {
                //    // System.Console.WriteLine(ex.Message)
                //  }
            }

        } // End Property Url 



        /// <summary>
        /// Overriding the method defined in the base class.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected override System.Net.WebRequest GetWebRequest(System.Uri uri)
        {
            System.Net.HttpWebRequest request = default(System.Net.HttpWebRequest);
            request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(uri);
            request.Timeout = base.Timeout; // Important !!!
            //request.UserAgent = base.UserAgent;
            //request.Proxy = base.Proxy;
            request.Credentials = base.Credentials;
            request.CookieContainer = new System.Net.CookieContainer();
            

            if (m_authCookie != null)
            {
                request.CookieContainer.Add(m_authCookie);
            }

            return request;
        } // GetWebRequest 


        public static string GetHeaderInfo(System.Net.WebResponse response)
        {
            string strRet = null;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (string strKey in response.Headers)
            {
                sb.AppendFormat("{0}: {1}\r\n", strKey, response.Headers[strKey]);
            } // Next strKey

            strRet = sb.ToString();
            sb.Length = 0;
            sb = null;

            return strRet;
        } // End Function GetHeaderInfo


        /// <summary>
        /// Overriding the method defined in the base class.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override System.Net.WebResponse GetWebResponse(System.Net.WebRequest request)
        {
            System.Net.WebResponse response = base.GetWebResponse(request);

            // http://social.msdn.microsoft.com/Forums/sqlserver/en-US/f68c3f2f-c498-4566-8ba4-ffd5070b8f7f/problem-with-ssrs-forms-authentication
            // string cookieName = response.Headers["RSAuthenticationHeader"];
            string cookieName = ".ASPXAUTH";
            cookieName = "SqlService";


            // string strHeaders = GetHeaderInfo(response);
            // System.Console.WriteLine(strHeaders);

            if (m_authCookie == null && cookieName != null)
            {
                m_authCookieName = cookieName;
                System.Net.HttpWebResponse webResponse = (System.Net.HttpWebResponse)response;
                System.Net.Cookie authCookie = webResponse.Cookies[cookieName];

                // Save it for future reference and use.
                m_authCookie = authCookie;
            } // End if (m_authCookie == null && cookieName != null)

            return response;
        } // GetWebResponse 


    } // SSRS_2005_Administration_WithFOA 


} // End Namespace DemoSqlClient 
