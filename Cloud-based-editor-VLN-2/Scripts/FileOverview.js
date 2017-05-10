var EditFileName = function (fileID) {
    $("#myModal1file").load("/Document/_RenameDocument?documentID=" + fileID, function () {
        $("#myModal1file").modal("show");

    })

}

var submitUpdatedFileName = function (projectID) {
    var nameField = document.getElementById("RenameDocumentTextBox").value;
    if (nameField != "") {
        var myformdata = $("#myFormFile").serialize();
        $.ajax({
            type: "POST",
            url: "/Document/_RenameDocument",
            data: myformdata,
            success: function (response) {
                $("#myModal1file").modal("hide");
                if (response.success == true) {
                    var itemID = "nameID" + response.docID;
                    document.getElementById(itemID).innerHTML = response.name;
                    $("#documentMsg").empty();
                    var html = "File name change to: <strong>" + response.name + response.type + "</strong>";
                    $("#documentMsg").append(html);
                    document.getElementById("documentMsg").style.color = "gray";
                    $('#documentMsg').fadeIn().delay(3500).fadeOut();
                } else {
                    if (response.message == "noPermission") {
                        $("#documentMsg").empty();
                        var html = "You don't have permission to rename this file";
                        $("#documentMsg").append(html);
                        document.getElementById("documentMsg").style.color = "red";
                        $('#documentMsg').fadeIn().delay(3500).fadeOut();
                    }
                }
            }
        });
    }

    else {
        var div = document.getElementById("RenameDocumentErrorDiv");
        div.innerHTML = "You must enter a name";
        div.style.display = "block";
    }
}