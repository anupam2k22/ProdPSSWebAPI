using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ProdPSSWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ApproveController : ApiController
    {
        [HttpPost]
        public bool ApproveFile()
        {
            HttpContext.Current.Request.InputStream.Position = 0;
            var body = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
            var fileID = body.Remove(body.Length - 1, 1);
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PSSConnectionString"].ConnectionString);
                conn.Open();
                string updateQuery = "update [PSS_Job_Level] set status_ID=11 where File_Id in ("+ fileID + ")";
                SqlCommand com = new SqlCommand(updateQuery, conn);
                com.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex.ToString());
                return false;
            }

        }
    }
}
