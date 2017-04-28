using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Models.Entities {
    public class ProjectList {
        public int ProjectListID { get; set; }

        public int UserID { get; set; }

        public virtual List<Project> Projects { get; set; }

    }
}