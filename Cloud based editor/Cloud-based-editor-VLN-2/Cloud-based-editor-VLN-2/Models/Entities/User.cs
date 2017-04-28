using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Models.Entities {
    public class User {
        public int UserID { get; set; }

        public string Email { get; set; }

        public ProjectList ProjectList { get; set; }
    }
}