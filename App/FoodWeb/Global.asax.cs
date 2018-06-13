using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Support;
using sqlData;
using System.Configuration;

namespace FoodWeb
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

        private void Application_BeginRequest(Object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            // Attempt to peform first request initialization
            var rURL = context.Request.Url.ToString();
            if (rURL.Contains("ErrorPageServer") || rURL.Contains("ensilogo.png"))
                return;

            Logger.Initialize();

            try
            {
                if (!rURL.Contains(".js") && !rURL.Contains(".css") && !rURL.Contains(".png") && !rURL.Contains(".jpg"))
                {
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
                    command.CommandTimeout = 15;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "select getdate()";
                    XstarConnection xstarConnection = new XstarConnection(new System.Data.SqlClient.SqlConnection(ProjectSettings.GetValue("MainConnectionString"))); //GetConnection();
                    command.Connection = xstarConnection.Connection;
                    if (command.Connection.State != System.Data.ConnectionState.Open) command.Connection.Open();
                    command.ExecuteNonQuery();
                    //Global.ENSIDataProvider.ExecuteNonQuery("select getdate()", System.Data.CommandType.Text);
                    if (command.Connection.State == System.Data.ConnectionState.Open) command.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                //throw ex; 
                HttpContext.Current.Server.ClearError();
                HttpContext.Current.ClearError();
                string redirectPage = ConfigurationManager.AppSettings["ErrorPageServer"];
                Logger.WriteError("!!!ERROR!!! select getdate() Redirect to :" + redirectPage);
                HttpContext.Current.Response.Redirect(redirectPage, true);
                return;
            }
        }

    }
}
