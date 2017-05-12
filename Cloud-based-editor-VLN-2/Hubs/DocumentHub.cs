using System;
using Microsoft.AspNet.SignalR;

namespace Cloud_based_editor_VLN_2.Hubs {
    public class DocumentHub : Hub {

        public void UpdateDocument(object changedData, int documentID, object cursorScreenPos) {
            var user = System.Web.HttpContext.Current.User;
            Clients.Group(Convert.ToString(documentID), Context.ConnectionId).updateText(changedData, cursorScreenPos, user.Identity.Name);
        }

        public void JoinDocument(int documentID) {
            Groups.Add(Context.ConnectionId, Convert.ToString(documentID));
        }

    }
}