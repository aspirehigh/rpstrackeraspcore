using RPS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RPS.Web.Models.ViewModels
{
    public class PtItemTasksVm
    {
        public int ItemId { get; set; }
        public string NewTaskTitle { get; set; }

        public List<PtTask> Tasks { get; set; }

        public PtItemTasksVm()
        {
            Tasks = new List<PtTask>();
        }

        public PtItemTasksVm(PtItem item)
        {
            ItemId = item.Id;
            Tasks = item.Tasks;
        }
    }
}