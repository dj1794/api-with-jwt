using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;
using internalTools.Model;

namespace internalTools.wwwroot
{
    
    public class dbHelper
    {
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=XFM_TASK_PORTAL;Integrated Security=True");
        SqlCommand cmd = new SqlCommand();
        DataTable globalTable = new DataTable();
        public List<FinalObjects> getTasks()
        {
            List<FinalObjects> list = new List<FinalObjects>();
            List<TaskDetails> tempTaskList = new List<TaskDetails>();

            con.Open();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "USP_EXPORT_TASK";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tempTaskList.Add(new TaskDetails() {taskID =Convert.ToInt32(reader["TDPK"]), taskTitle = reader["TASK_TITLE"].ToString(), taskDescription = reader["task_Description"].ToString(), createdON = Convert.ToDateTime(reader["created_date"])
                    ,modifiedON = Convert.ToDateTime( reader["modified_ON"]), clientID = Convert.ToInt32(reader["CLPK"]), clientName = reader["CLIENT_NAME"].ToString()
                });
            }
            var tempList = tempTaskList.GroupBy(m => new { m.clientID, m.clientName }).Select(x => x.First()).OrderBy(m => m.clientID);
            foreach(var item in tempList)
            {
                list.Add(new FinalObjects() { clientID = item.clientID, clientName = item.clientName, tasksList = tempTaskList.Where(x => x.clientID == item.clientID).ToList() });
            }
            con.Close();
            return list;

        }
        public string addTask(TaskDetails taskDetails)
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "USP_ADD_TASK";
            cmd.Parameters.AddWithValue("@taskTitle", taskDetails.taskTitle);
            cmd.Parameters.AddWithValue("@taskDescription", taskDetails.taskDescription);
            cmd.Parameters.AddWithValue("@createdDate", taskDetails.createdON);
            cmd.Parameters.AddWithValue("@modifiedOn", taskDetails.modifiedON);
            cmd.Parameters.AddWithValue("@clientID", taskDetails.clientID);
            cmd.ExecuteNonQuery();
            con.Close();
            return "success";
        }
    }
}
