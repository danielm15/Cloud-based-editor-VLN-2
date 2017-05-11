using Cloud_based_editor_VLN_2.Models.Entities;
using System.Collections.Generic;


namespace Cloud_based_editor_VLN_2.Models.ViewModels {
    public class ProjectViewModel {
        public int CurrUserID { get; set; }
        public IEnumerable<Project> Projects { get; set; }
    }
}