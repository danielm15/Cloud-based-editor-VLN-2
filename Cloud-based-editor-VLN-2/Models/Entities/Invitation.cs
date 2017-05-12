using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cloud_based_editor_VLN_2.Models.Entities {
    public class Invitation {
        [Key]
        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("fromUserName")]
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