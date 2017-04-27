using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Models
{
    public class ProjectList
    {
        public int ProjectListID { get; set; }

        public int UserID { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

    }
}