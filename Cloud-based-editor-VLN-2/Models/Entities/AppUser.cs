using System.ComponentModel.DataAnnotations;


namespace Cloud_based_editor_VLN_2.Models.Entities {
    public class AppUser {
        [Key]
        public int ID { get; set; }
        
        public string UserName { get; set; }

        public string Email { get; set; }
    }
}