using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Models.Entities {
    public class Invitation {
        [Key]
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("AppUserID")]
        public string fromUserName { get; set; }

        [JsonProperty("AppUserID")]
        public int AppUserID { get; set; }

        [JsonProperty("ProjectID")]
        public int ProjectID { get; set; }

        [JsonIgnore]
        [ForeignKey("AppUserID")]
        public virtual AppUser AppUser { get; set; }

        [JsonIgnore]
        [ForeignKey("ProjectID")]
        public virtual Project Project { get; set; }
    }
}