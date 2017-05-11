$(document).on('click', '#notifyButton', function () {
//var listNotifications = $(function () {
    $.ajax({
        url:'/Project/GetInvites',
        type: 'GET',
        //contentType: "application/json; charset=utf-8",
        dataType: 'JSON',
        success: function (response) {
            //console.log(response);
            $('#inviteDropDown').empty();
            var html;
            var arr = $.parseJSON('[' + response + ']');
            console.log(arr);

            for (i = 0; i < arr[0].length; i++) {
                html = '<li id="inviteItem' + arr[0][i].ID + '">Invitaion to project: ' + arr[0][i].Name
                     + '<button class="btn btn-primary" onclick="acceptProject(' + arr[0][i].ID + ')">Accept</button>'
                     + '<button class="btn btn-default" onclick="declineProject(' + arr[0][i].ID + ')">Decline</button>'
                     + '</li>';
                console.log(html);
                $('#inviteDropDown').append(html);
                //console.log(i);
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
        }
    });
};

var declineProject = function (projectID) {
    $.ajax({
        url: '/Project/DeclineProject',
        type: 'POST',
        data: { projectID: projectID },
        success: function (response) {
            alert('decline');
            var elemID = '#inviteItem' + projectID;
            $(elemID).empty();
        }
    });
};