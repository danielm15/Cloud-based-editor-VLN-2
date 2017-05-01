using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Models.Entities {
    public class ProjectList {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int ProjectID { get; set; }

        public virtual List<Project> Projects { get; set; }

    }
}