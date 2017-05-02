using Cloud_based_editor_VLN_2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud_based_editor_VLN_2.Models.ViewModels {
    public class ProjectViewModel {
        public IEnumerable<Project> Projects { get; set; }
    }
}