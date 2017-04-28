using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Models.Entities {
    public class ProjectFile {
        public int ProjectFileID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int OwnerID { get; set; }

        //public virtual List<User> Collaborators { get; set; }

    }
}