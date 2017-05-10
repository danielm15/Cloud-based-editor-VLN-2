var EditFileName = function (fileID) {

    $("#myModal1file").load("/Document/_RenameDocument?documentID=" + fileID, function () {
        $("#myModal1file").modal("show");

    })
}

var submitUpdatedFileName = function (projectID) {
    var test = document.getElementById("RenameDocumentTextBox").value;
    if (test != "") {
        var myformdata = $("#myFormFile").serialize();
        $.ajax({
            type: "POST",
            url: "/Document/_RenameDocument",
            data: myformdata,
            success: function (responseData) {
                if (responseData.success == false) {
                    $('#RenameDocumentErrorDiv').empty();

                    var html = "Duplicate file name"
                    $('#RenameDocumentErrorDiv').append(html);

                    var error = document.getElementById('RenameDocumentErrorDiv');
                    error.style.color = "red";

                    $('#RenameDocumentErrorDiv').fadeIn().delay(2000).fadeOut();
                }
                else {
                    alert('her');
                    $("#myModal1file").modal("hide");

                    window.location.href = "/Document?ProjectID=" + projectID + "";
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