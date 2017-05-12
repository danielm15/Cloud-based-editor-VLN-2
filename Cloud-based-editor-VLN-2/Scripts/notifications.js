$(document).on('click', '#notifyButton', function () {
    $.ajax({
        url:'/Project/GetInvites',
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            if (response.success === false) {
                $('#inviteDropDown').empty();
                var html = '<div id="noNotifactions">You have no new notifications</div>';
                $('#inviteDropDown').append(html);
            }
            else {
                $('#inviteDropDown').empty();
                var html;
                var arr = $.parseJSON('[' + response.projectsResult + ']');
                var fromUserNames = $.parseJSON('[' + response.invitesResult + ']');

                for (var i = 0; i < arr[0].length; i++) {
                    html = '<li id="inviteItem' + arr[0][i].ID + '"> <div class="notfiyListitem">'
                         + '<p>Invitation to project: <strong> ' + arr[0][i].Name + '</strong> </p>'
                         + '<p>From user: <strong> ' + fromUserNames[0][i].fromUserName + '</strong> </p>'
                         + '<button type="button" class="btn btn-primary btn-sm" onclick="acceptProject(' + arr[0][i].ID + ')">Accept</button>'
                         + '<button type="button" class="btn btn-default btn-sm" onclick="declineProject(' + arr[0][i].ID + ')">Decline</button>'
                         + '</div></li>';
                    $('#inviteDropDown').append(html);
                }
            }
        }
    });
    return false;
});

var acceptProject = function (projectID) {
    $.ajax({
        url: '/Project/AcceptProject',
        type: 'POST',
        data: { projectID: projectID },
        success: function (response) {
            var elemID = '#inviteItem' + projectID;
            $(elemID).empty();
            var notifyCount = document.getElementById("NotifyCount");

            if (notifyCount.innerHTML === "" || notifyCount.innerHTML === "1") {
                notifyCount.innerHTML = "";
            }
            else {
                notifyCount.innerHTML = (parseInt(notifyCount.innerHTML) - 1).toString();
            }
            reloadProjectList(projectID);
        }
    });
};

var declineProject = function (projectID) {
    $.ajax({
        url: '/Project/DeclineProject',
        type: 'POST',
        data: { projectID: projectID },
        success: function (response) {
            var elemID = '#inviteItem' + projectID;
            $(elemID).empty();

            var notifyCount = document.getElementById("NotifyCount");

            if (notifyCount.innerHTML === "" || notifyCount.innerHTML === "1") {
                notifyCount.innerHTML = "";
            }
            else {
                notifyCount.innerHTML = (parseInt(notifyCount.innerHTML) - 1).toString();
            }
        }
    });
};

var reloadProjectList = function (projectID) {
    $.ajax({
        type: "GET",
        url: "/Project/AddInvitedProject",
        data: { projectID: projectID },
        success: function (response) {
            $("#acceptedProject").append(response);
        }
    });
};