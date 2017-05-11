using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Cloud_based_editor_VLN_2.Models.Entities {
    public class Document {
        [Key]
        [JsonIgnore]
        public int ID { get; set; }

        [JsonIgnore]
        [Index("NameProject", 1, IsUnique = true)]
        [StringLength(400)]
        public string Name { get; set; }

        [JsonIgnore]
        public string Type { get; set; }

        [JsonIgnore]
        public string CreatedBy { get; set; }

        [JsonIgnore]
        public DateTime DateCreated { get; set; }
        
        [JsonProperty("content")]
        [Column(TypeName = "ntext")]
        [AllowHtml]
        public string Content { get; set; }

        [JsonIgnore]
        public string LastUpdatedBy { get; set; }

        [JsonIgnore]
        [Index("NameProject", 2, IsUnique = true)]
        public int ProjectID { get; set; }

        [JsonIgnore]
        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }
    }
}