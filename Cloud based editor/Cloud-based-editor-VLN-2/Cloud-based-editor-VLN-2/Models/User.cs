using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Models
{
    public class User
    {
        public int UserID { get; set; }

        public string email { get; set; }

        public int ProjectListID { get; set; }
    }
}