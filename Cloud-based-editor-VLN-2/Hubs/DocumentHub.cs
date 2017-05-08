using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Cloud_based_editor_VLN_2.Models.Entities;

namespace Cloud_based_editor_VLN_2.Hubs {
    public class DocumentHub : Hub {

        public void UpdateDocument(object changedData, int documentID) {
            //documentModel.LastUpdatedBy = Context.ConnectionId;
            //Clients.AllExcept(Context.ConnectionId).updateText(changedData);
            Clients.Group(Convert.ToString(documentID), Context.ConnectionId).updateText(changedData);
        }

        public void JoinDocument(int documentID) {
            Groups.Add(Context.ConnectionId, Convert.ToString(documentID));

        }
    }
}