﻿var EditProjectName = function (ProjectID) {

    var url = "/Project/_RenameProject?ProjectID=" + ProjectID;

    $("#myModal1").load(url, function () {
        $("#myModal1").modal("show");

    })

}

var submitUpdatedName = function () {
    var myformdata = $("#myForm").serialize();
        $.ajax({

            type: "POST",
            url: "/Project/_RenameProject",
            data: myformdata,
            success: function () {
                $("#myModal").modal("hide");
                window.location.href = "/Project/";
            },
        })
}

var InviteToProject = function (ProjectID) {
    var url = "/Project/InviteUser?ProjectID=" + ProjectID;
    $("#myModal1").load(url, function () {
        $("#myModal1").modal("show");

    })
}

var submitInviteName = function () {
    var myformdata = $("#InviteUserForm").serialize();
    console.log(myformdata);
    $.ajax({
        type: "POST",
        url: "/Project/InviteUser",
        data: myformdata,
        success: function() {
            $("#myModal").modal("hide");
            window.location.href = "/Project";
        }
    })
}