using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace internalTools.Model
{
    public class TaskDetails
    {
        public int taskID { get; set; }
        public string taskTitle { get; set; }
        public string taskDescription { get; set; }
        public DateTime createdON { get; set; }
        public DateTime modifiedON { get; set; }
        public int clientID { get; set; }
        public string clientName { get; set; }
    }

    public class FinalObjects
    {
        public string clientName { get; set; }
        public int clientID { get; set; }
        public List<TaskDetails> tasksList { get; set; }
    }
}
