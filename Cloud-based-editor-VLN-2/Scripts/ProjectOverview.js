var EditProjectName = function (ProjectID) {
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

var deleteProject = function (projectID) {

    $.ajax({
        type: "GET",
        url: "/Project/DeleteProjectVal",
        data: { projectID: projectID },
        success: function (response) {
           
            if (response.success == true) {
                deleteConfirmation(projectID);
            } else {
                deleteNoPermission(projectID);
            }

        } 
    });
    
}

var deleteConfirmation = function (projectID) {

    var url = "/Project/DeleteProjectConfirm?ProjectID=" + projectID;

    $("#myModal1").load(url, function () {
        $("#myModal1").modal("show");

    })
}

var deleteNoPermission =  function (projectID) {

    var url = "/Project/DeleteNoPermission?ProjectID=" + projectID;

    $("#myModal1").load(url, function () {
        $("#myModal1").modal("show");

    })
}

var deleteProjectAjax = function (projectID) {
    $.ajax({

        type: "POST",
        url: "/Project/DeleteProject",
        data: { id: projectID },
        success: function (response) {
            var linkString = "link\\" + projectID;
            var divString = "div\\" + projectID;
            var linkToDelete = document.getElementById(linkString);
            var divToDelete = document.getElementById(divString);
            linkToDelete.parentNode.removeChild(linkToDelete);
            divToDelete.parentNode.removeChild(divToDelete);
        }
    });

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