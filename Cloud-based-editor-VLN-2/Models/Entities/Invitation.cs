using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Models.Entities {
    public class Invitation {
        [Key]
        public int ID { get; set; }

        [ForeignKey("AppUser")]
        public int FromID { get; set; }

        [ForeignKey("AppUser")]
        public int ToID { get; set; }

        public int ProjectID { get; set; }

        public virtual AppUser AppUser { get; set; }

        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }
    }
}