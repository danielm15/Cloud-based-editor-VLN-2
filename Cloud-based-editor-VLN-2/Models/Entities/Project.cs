using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Models.Entities {
    public class Project {
        [Key]
        public int ID { get; set; }

        public int OwnerID { get; set; }

        public string Name { get; set; }

        public String DateCreated { get; set; }

        //public int? StartUpFileID { get; set; }

        public virtual List<Document> Documents { get; set; }

        [ForeignKey("OwnerID")]
        public virtual AppUser AppUser { get; set; }

        //[ForeignKey("StartUpFileID")]
        //public virtual Document Document { get; set; }
        
    }
}