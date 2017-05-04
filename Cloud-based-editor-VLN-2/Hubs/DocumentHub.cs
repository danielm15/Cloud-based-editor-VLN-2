using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Cloud_based_editor_VLN_2.Models.Entities;

namespace Cloud_based_editor_VLN_2.Hubs {
    public class DocumentHub : Hub {

        public void UpdateDocument(Document documentModel) {
            documentModel.LastUpdatedBy = Context.ConnectionId;
            Clients.AllExcept(documentModel.LastUpdatedBy).updateText(documentModel);
        }
    }
}