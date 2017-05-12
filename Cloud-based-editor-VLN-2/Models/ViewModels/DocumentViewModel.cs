using Cloud_based_editor_VLN_2.Models.Entities;
using System.Collections.Generic;


namespace Cloud_based_editor_VLN_2.Models.ViewModels {
    public class DocumentViewModel {
        public int CurrProjectID { get; set; }

        public Document Doc { get; set; }

        public IEnumerable<Document> Documents { get; set; }
    }
}