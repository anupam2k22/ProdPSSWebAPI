using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
    public class StatusPieController : ApiController
    {
        [HttpPost]
        public string getFileStatus()
        {
            //HttpContext.Current.Request.InputStream.Position = 0;
            //var body = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
            string returnVal = "";
            try
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PSSConnectionString"].ConnectionString);
                conn.Open();

                String query = "select Job_Status.Status_descriptions, count(PSS_Job_Level.Status_Id) as FileCount from Job_Status LEFT OUTER JOIN PSS_Job_Level on PSS_Job_Level.Status_Id = Job_Status.Status_Id group by Job_Status.Status_descriptions";
                SqlCommand com = new SqlCommand(query, conn);
                com.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter sda = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                conn.Close();
                returnVal = JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                returnVal = "";
            }
            return returnVal;
        }

    }
}
