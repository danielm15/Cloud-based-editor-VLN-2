(function ($) {
    'use strict';
    $(function () {
        var invhub = $.connection.invitationHub;

        invhub.client.sendInvite = function (fromID, toID, projectID) {

            var notifyCount = document.getElementById("NotifyCount");

            if (notifyCount.innerHTML == "") {
                notifyCount.innerHTML = "1";
            }
            else {
                notifyCount.innerHTML = (parseInt(notifyCount.innerHTML) + 1).toString();

            }
        }

        window.hubReady.done(function () {
            invhub.server.joinUserGroup(userID);
            $(document).on('click', '#InviteUserSubmitBtn', function () {
                var projectID = $('#CurrentProjectToInvite').val();
                var userName = $('#userListInput').val();
                console.log(projectID);
                console.log(userName);
                $.ajax({
                    type: "POST",
                    url: "/Project/Invite",
                    data: { projectID: projectID, userName: userName },
                    success: function (response) {
                        if (response.success === "hasProject") {
                            //document.getElementById("userListInput").classList.toggle("hideInput");
                            $("#InviteUserError").empty();
                            document.getElementById("InviteUserError").innerHTML = "   User is already a collaborator in this project";
                            document.getElementById("InviteUserError").style.color = "red";
                            $('#InviteUserError').fadeIn().delay(3500).fadeOut();
                            /*document.getElementById("InviteUserSubmitBtn").innerHTML = "Try again";
                            var functionName = "InviteToProject(" + response.projectID + " )";
                            $("#InviteUserSubmitBtn").attr("onclick", functionName);*/
                        }
                        else if (response.success === "hasInvite") {
                            /*document.getElementById("userListInput").classList.toggle("hideInput");
                            document.getElementById("myHeaderMessage").innerHTML = "User <strong> " + response.name + " </strong> already has an invite";
                            document.getElementById("InviteUserSubmitBtn").innerHTML = "Try again";
                            var functionName = "InviteToProject(" + response.projectID + " )";
                            $("#InviteUserSubmitBtn").attr("onclick", functionName);*/
                            $("#InviteUserError").empty();
                            document.getElementById("InviteUserError").innerHTML = "   User already has an invtie to this project";
                            document.getElementById("InviteUserError").style.color = "red";
                            $('#InviteUserError').fadeIn().delay(3500).fadeOut();
                        }
                        else if (response.success === "userNotFound") {
                            /*document.getElementById("userListInput").classList.toggle("hideInput");
                            document.getElementById("myHeaderMessage").innerHTML = "User <strong> " + response.name + " </strong> not found";
                            document.getElementById("InviteUserSubmitBtn").innerHTML = "Try again";
                            var functionName = "InviteToProject(" + response.projectID + " )";
                            $("#InviteUserSubmitBtn").attr("onclick", functionName);*/
                            $("#InviteUserError").empty();
                            document.getElementById("InviteUserError").innerHTML = "   User not found";
                            document.getElementById("InviteUserError").style.color = "red";
                            $('#InviteUserError').fadeIn().delay(3500).fadeOut();
                        }
                        else {
                            /*document.getElementById("userListInput").classList.toggle("hideInput");
                            document.getElementById("myHeaderMessage").innerHTML = "User <strong> " + response.name + " </strong> has received an invite";
                            
                            document.getElementById("InviteUserSubmitBtn").innerHTML = "Invite another user";
                            var functionName = "InviteToProject(" + response.projectID + " )";
                            $("#InviteUserSubmitBtn").attr("onclick", functionName);*/
                            $("#InviteUserError").empty();
                            document.getElementById("InviteUserError").innerHTML = "   Invite sent";
                            document.getElementById("InviteUserError").style.color = "green";
                            $('#InviteUserError').fadeIn().delay(3500).fadeOut();

                            invhub.server.sendInvitation(userID, userName, parseInt(projectID));
                        }
                    }
                });

                //invhub.server.sendInvitation(userID, toUserName, parseInt(projectID));
            });
        });
    });
})(jQuery);