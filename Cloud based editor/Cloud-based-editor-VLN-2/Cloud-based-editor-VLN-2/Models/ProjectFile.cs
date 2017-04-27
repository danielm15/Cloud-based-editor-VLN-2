using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Models
{
    public class ProjectFile
    {
        public int FileID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int OwnerID { get; set; }

        //public virtual ICollection<User> Collaborators { get; set; }

    }
}