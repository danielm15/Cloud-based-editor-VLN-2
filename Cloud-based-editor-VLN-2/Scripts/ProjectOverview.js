var EditProjectName = function (ProjectID) {
    var url = "/Project/_RenameProject?ProjectID=" + ProjectID;

    $("#myModal1").load(url, function () {
        $("#myModal1").modal("show");
    })
}


    var submitUpdatedName = function () {
        var test = document.getElementById("MyId").value;
        if (test != "") {
            var myformdata = $("#myForm").serialize();
            $.ajax({

                type: "POST",
                url: "/Project/_RenameProject",
                data: myformdata,
                success: function () {
                    $("#myModal").modal("hide");
                    window.location.href = "/Project/";
                }
            })
        }
        else {
            var div = document.getElementById("RenameProjectErrorDiv");
            div.innerHTML = "You must enter a name";
            div.style.display = "block";
        }
    }
    
var deleteProject = function (projectID) {
    deleteProjectAjax(projectID);
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