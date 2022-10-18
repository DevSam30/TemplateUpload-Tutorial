using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Runtime.Versioning;

namespace TemplateUpload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateUploadController : ControllerBase
    {
        [HttpPost]
        //[SupportedOSPlatform("windows")]
        public void UploadTemplate()
        {
            string path = @"C:\Projects\TemplateUpload\TemplateUpload\Excel Files\Student_Details.xlsx";
            string excel_conn_str = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+path+";Extended Properties=\"Excel 12.0 Xml;HDR=YES\";";

            DataSet ds;
            OleDbConnection oleDbConnection = new OleDbConnection(excel_conn_str);
            OleDbCommand cmd = new OleDbCommand("SELECT first_name, last_name, age FROM [Sheet1$]", oleDbConnection);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            ds = new DataSet();
            da.Fill(ds);

            oleDbConnection.Open();
            DbDataReader dr = cmd.ExecuteReader();
            string server_conn_str = @"Data Source=(localdb)\Template_Upload;Initial Catalog=Students;Integrated Security=True";
            SqlBulkCopy bulkInsert = new SqlBulkCopy(server_conn_str);
            bulkInsert.DestinationTableName = "Student_Details";
            bulkInsert.WriteToServer(dr);
            oleDbConnection.Close();
        }
    
    }
}
