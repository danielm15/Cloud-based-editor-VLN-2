var EditFileName = function (fileID) {
    //var url = "~/Document/_RenameDocument?documentID=" + fileID;

    $("#myModal1file").load("/Document/_RenameDocument?documentID=" + fileID, function () {
        $("#myModal1file").modal("show");
        //$("#myModal1file").style.display = "block";

    })
    //$("#myModal1file").modal("show");
}

var submitUpdatedFileName = function (projectID) {
    var test = document.getElementById("RenameDocumentTextBox").value;
    if (test != "") {
        var myformdata = $("#myFormFile").serialize();
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
    else {
        var div = document.getElementById("RenameDocumentErrorDiv");
        div.innerHTML = "You must enter a name";
        div.style.display = "block";
    }
}