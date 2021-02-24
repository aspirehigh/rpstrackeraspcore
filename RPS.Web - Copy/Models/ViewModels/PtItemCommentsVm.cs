using RPS.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RPS.Web.Models.ViewModels
{
    public class PtItemCommentsVm
    {
        public int ItemId { get; set; }

        [DataType(DataType.MultilineText)]
        public string NewCommentText { get; set; }

        public List<PtComment> Comments { get; set; }

        public PtUser CurrentUser { get; set; }

        public PtItemCommentsVm()
        {
            Comments = new List<PtComment>();
        }

        public PtItemCommentsVm(PtItem item, PtUser currentUser)
        {
            ItemId = item.Id;
            Comments = item.Comments;
            CurrentUser = currentUser;
        }
    }
}