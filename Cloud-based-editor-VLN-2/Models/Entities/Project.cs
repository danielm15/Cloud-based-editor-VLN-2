using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cloud_based_editor_VLN_2.Models.Entities {
    public class Project {
        [Key]
        public int ID { get; set; }

        public int OwnerID { get; set; }

        [Required(ErrorMessage = "Name is Required.")]
        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        //public int? StartUpFileID { get; set; }
        [JsonIgnore]
        public virtual List<Document> Documents { get; set; }

        [ForeignKey("OwnerID")]
        [JsonIgnore]
        public virtual AppUser AppUser { get; set; }

        //[ForeignKey("StartUpFileID")]
        //public virtual Document Document { get; set; }

        [Required(ErrorMessage = "Type of project is Required")]
        public string ProjectType { get; set; }
    }
}