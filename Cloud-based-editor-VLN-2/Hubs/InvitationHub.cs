using System;
using Microsoft.AspNet.SignalR;
using Cloud_based_editor_VLN_2.Services;

namespace Cloud_based_editor_VLN_2.Hubs {
    public class InvitationHub : Hub {
        ProjectService _service = new ProjectService(null);

        public void SendInvitation(int fromID, string userName, int projectID) {
            int toID = _service.getUserID(userName);

            Clients.Group(Convert.ToString(toID)).sendInvite(fromID, toID, projectID);
        }

        public void JoinUserGroup(int userID) {
            Groups.Add(Context.ConnectionId, Convert.ToString(userID));
        }
    }
}