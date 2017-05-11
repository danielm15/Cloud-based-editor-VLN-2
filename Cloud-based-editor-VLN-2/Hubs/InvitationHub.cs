using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Cloud_based_editor_VLN_2.Services;
using Cloud_based_editor_VLN_2.Models.Entities;

namespace Cloud_based_editor_VLN_2.Hubs {
    public class InvitationHub : Hub {
        ProjectService _service = new ProjectService(null);
        public void SendInvitation(int fromID, string toUserName, int projectID) {
            int toID = _service.getUserID(toUserName);
            //Invitation inv = new Invitation();
            //inv.AppUserID = toID;
            //inv.ProjectID = projectID;

            //_service.AddInvitation(inv);
            //Clients.Group(Convert.ToString(toID), Context.ConnectionId).sendInvite(fromID, toID, projectID);
            Clients.Group(Convert.ToString(toID)).sendInvite(fromID, toID, projectID);
            //Clients.All.sendInvite(fromID, toID, projectID);
        }

        public void JoinUserGroup(int userID) {
            Groups.Add(Context.ConnectionId, Convert.ToString(userID));
        }
    }
}