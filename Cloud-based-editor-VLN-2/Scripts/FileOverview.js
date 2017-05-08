var EditFileName = function (fileID) {
    //alert('TEST');

    //var url = "~/Document/_RenameDocument?documentID=" + fileID;

    $("#myModal1file").load("/Document/_RenameDocument?documentID=" + fileID, function () {
        $("#myModal1file").modal("show");
        //$("#myModal1file").style.display = "block";

    })
    //$("#myModal1file").modal("show");
}

var submitUpdatedFileName = function (projectID) {
    var myformdata = $("#myFormFile").serialize();
    console.log(projectID);
    $.ajax({
        type: "POST",
        url: "/Document/_RenameDocument",
        data: myformdata,
        success: function () {
            $("#myModal1file").modal("hide");

            window.location.href = "/Document?ProjectID=" + projectID + "";
        }
    });
}