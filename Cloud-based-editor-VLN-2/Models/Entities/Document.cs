using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Models.Entities {
    public class Document {
        [Key]
        [JsonIgnore]
        public int ID { get; set; }

        [JsonIgnore]
        public string Name { get; set; }

        [JsonIgnore]
        public string Type { get; set; }

        [JsonIgnore]
        public string CreatedBy { get; set; }

        [JsonIgnore]
        public DateTime DateCreated { get; set; }
        
        [JsonProperty("content")]
        [Column(TypeName = "ntext")]
        public string Content { get; set; }

        [JsonIgnore]
        public string LastUpdatedBy { get; set; }

        [JsonIgnore]
        public int ProjectID { get; set; }

        [JsonIgnore]
        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }

        //public virtual List<User> Collaborators { get; set; }
    }
}