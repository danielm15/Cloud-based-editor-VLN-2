using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Models {
    public class Project {
        public int ProjectID { get; set; }

        public string Name { get; set; }

        // public int ownerID {get; set; }

        public int StartUpFile { get; set; }

        public virtual ICollection<ProjectFile> ProjectFiles { get; set; }
        
    }
}