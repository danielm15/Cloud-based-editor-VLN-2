$(document).on('click', '#notifyButton', function () {
    $.ajax({
        url:'/Project/GetInvites',
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            if (response.success === false) {
                $('#inviteDropDown').empty();
                var html = '<div>You have no new notifications</div>';
                $('#inviteDropDown').append(html);
            }
            else {
                alert('her');
                $('#inviteDropDown').empty();
                var html;
                var arr = $.parseJSON('[' + response + ']');

                for (i = 0; i < arr[0].length; i++) {
                    html = '<li id="inviteItem' + arr[0][i].ID + '">Invitaion to project: ' + arr[0][i].Name
                         + '<button class="btn btn-primary" onclick="acceptProject(' + arr[0][i].ID + ')">Accept</button>'
                         + '<button class="btn btn-default" onclick="declineProject(' + arr[0][i].ID + ')">Decline</button>'
                         + '</li>';
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

            if (notifyCount.innerHTML == "" || notifyCount.innerHTML == "1") {
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

            if (notifyCount.innerHTML == "" || notifyCount.innerHTML == "1") {
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