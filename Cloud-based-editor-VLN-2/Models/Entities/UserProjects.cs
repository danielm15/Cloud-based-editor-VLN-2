using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Models.Entities {
    public class UserProjects {
        [Key]
        public int ID { get; set; }

        public int AppUserID { get; set; }

        public int ProjectID { get; set; }

        //public virtual List<Project> Projects { get; set; }

        [ForeignKey("AppUserID")]
        public virtual AppUser AppUser { get; set; }

        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }

    }
}