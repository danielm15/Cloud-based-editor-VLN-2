using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Models.Entities {
    public class Document {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }
        
        [Column(TypeName = "ntext")]
        public string Content { get; set; }

        public int ProjectID { get; set; }

        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }

        //public virtual List<User> Collaborators { get; set; }
    }
}