using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Models.Entities {
    public class Project {
        [Key]
        public int ID { get; set; }
        public int ownerID { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public int StartUpFileID { get; set; }

        public virtual List<Document> ProjectFiles { get; set; }
        
    }
}