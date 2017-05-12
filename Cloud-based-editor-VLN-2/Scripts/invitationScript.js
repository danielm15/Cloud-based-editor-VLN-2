
(function ($) {
    'use strict';
    $(function () {
        var invhub = $.connection.invitationHub;
        invhub.client.sendInvite = function (fromID, toID, projectID) {

            var notifyCount = document.getElementById("NotifyCount");

            if (notifyCount.innerHTML === "") {
                notifyCount.innerHTML = "1";
            }
            else {
                notifyCount.innerHTML = (parseInt(notifyCount.innerHTML) + 1).toString();
            }
        };

        window.hubReady.done(function () {
            invhub.server.joinUserGroup(userID);
            $(document).on('click', '#InviteUserSubmitBtn', function () {
                var projectID = $('#CurrentProjectToInvite').val();
                var userName = $('#userListInput').val();
                $.ajax({
                    type: "POST",
                    url: "/Project/Invite",
                    data: { projectID: projectID, userName: userName },
                    success: function (response) {
                        if (response.success === "hasProject") {
                            $("#InviteUserError").empty();
                            document.getElementById("InviteUserError").innerHTML = "   User is already a collaborator in this project";
                            document.getElementById("InviteUserError").style.color = "red";
                            $('#InviteUserError').fadeIn().delay(3500).fadeOut();
                        }
                        else if (response.success === "hasInvite") {
                            $("#InviteUserError").empty();
                            document.getElementById("InviteUserError").innerHTML = "   User already has an invtie to this project";
                            document.getElementById("InviteUserError").style.color = "red";
                            $('#InviteUserError').fadeIn().delay(3500).fadeOut();
                        }
                        else if (response.success === "userNotFound") {
                            $("#InviteUserError").empty();
                            document.getElementById("InviteUserError").innerHTML = "   User not found";
                            document.getElementById("InviteUserError").style.color = "red";
                            $('#InviteUserError').fadeIn().delay(3500).fadeOut();
                        }
                        else {
                            $("#InviteUserError").empty();
                            document.getElementById("InviteUserError").innerHTML = "   Invite sent";
                            document.getElementById("InviteUserError").style.color = "green";
                            $('#InviteUserError').fadeIn().delay(3500).fadeOut();

                            invhub.server.sendInvitation(userID, userName, parseInt(projectID));
                        }
                    }
                });

            });
        });
    });
})(jQuery);
