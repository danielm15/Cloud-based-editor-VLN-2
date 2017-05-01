using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Models.Entities {
    public class AppUser {
        [Key]
        public int ID { get; set; }

        public string Email { get; set; }

        //public virtual Project Projects[] { get; set; }
    }
}