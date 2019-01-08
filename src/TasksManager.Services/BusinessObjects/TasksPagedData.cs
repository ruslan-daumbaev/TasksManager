using System.Collections.Generic;

namespace TasksManager.Services.BusinessObjects
{
    public class TasksPagedData
    {
        public List<TaskData> Tasks { get; set; }

        public int TotalRecords { get; set; }
    }
}