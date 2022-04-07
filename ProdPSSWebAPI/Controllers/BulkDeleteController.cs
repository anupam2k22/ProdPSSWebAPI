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
    public class BulkDeleteController : ApiController
    {
        [HttpPost]
        public string deleteBulk()
        {
            HttpContext.Current.Request.InputStream.Position = 0;
            var body = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();
            string returnVal = "";
            try
            {
                System.Data.SqlClient.SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["PSSConnectionString"].ConnectionString);
                conn.Open();
                SqlCommand sqlCommand = new SqlCommand()
                {
                    Connection = conn,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "usp_deleteBulk"
                };
                SqlParameter sqlParameter = new SqlParameter("@TCN", body);
                sqlParameter.Direction = ParameterDirection.Input;
                sqlParameter.DbType = DbType.String;
                sqlCommand.Parameters.Add(sqlParameter);
                SqlDataAdapter sda = new SqlDataAdapter(sqlCommand);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                conn.Close();
                returnVal = JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                returnVal = "Error " + ex.Message;
            }
            return returnVal;
        }

    }
}
