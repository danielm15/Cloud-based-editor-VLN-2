$(function () {
    $("#createFileForm").on("submit", function () {
        $('#noFilesListItem').empty();
        var form = $(this);
        $.ajax({
            method: "post",
            url: "Document/Create",
            data: form.serialize(),
            success: function (response) {
                var html;
                var error;
                console.log(response);
                if (response.success === false) {
                    alert('her');
                    $('#duplicateErrorMsg').empty();
                    html = "  Duplicate file name";
                    $('#duplicateErrorMsg').append(html);
                    error = document.getElementById('duplicateErrorMsg');
                    error.style.color = "red";
                    $('#duplicateErrorMsg').fadeIn().delay(2000).fadeOut();
   
                }
                else if (response.success === "bothempty") {
                    $('#duplicateErrorMsg').empty();
                    html = "  File name empty";
                    $('#duplicateErrorMsg').append(html);
                    error = document.getElementById('duplicateErrorMsg');
                    error.style.color = "red";
                    $('#duplicateErrorMsg').fadeIn().delay(2000).fadeOut();

                    $('#typeErrorMsg').empty();
                    html = "  File type empty";
                    $('#typeErrorMsg').append(html);
                    error = document.getElementById('typeErrorMsg');
                    error.style.color = "red";
                    $('#typeErrorMsg').fadeIn().delay(2000).fadeOut();
                }
                else if (response.success === "nameempty") {
                    $('#duplicateErrorMsg').empty();
                    html = "  File name empty";
                    $('#duplicateErrorMsg').append(html);
                    error = document.getElementById('duplicateErrorMsg');
                    error.style.color = "red";
                    $('#duplicateErrorMsg').fadeIn().delay(2000).fadeOut();
                }
                else if (response.success === "filetypeempty") {
                    $('#typeErrorMsg').empty();
                    html = "  File type empty";
                    $('#typeErrorMsg').append(html);
                    error = document.getElementById('typeErrorMsg');
                    error.style.color = "red";
                    $('#typeErrorMsg').fadeIn().delay(2000).fadeOut();

                }
                else {
                    $("#documentUlListID").append(response.html);
                    document.getElementById("createFileForm").reset();
                    document.getElementById("createFileBtn").classList.toggle("open");

                    $("#documentMsg").empty();
                    html = "File: <strong>" + response.Name + response.Type + "</strong> Created";
                    $("#documentMsg").append(html);
                    document.getElementById("documentMsg").style.color = "gray";
                    $('#documentMsg').fadeIn().delay(3500).fadeOut();
                }
            }
        });
        return false;
    });
});

var deleteDocument = function (documentID) {

    $.ajax({
        method: "post",
        url: "Document/Delete",
        data: { documentID: documentID },
        success: function (response) {
            if (response.success === true) {
                var itemID = "listItem" + response.documentID;
                $("#" + itemID).remove();
                $("#documentMsg").empty();
                var html = "File: <strong>" + response.name + response.type + "</strong> deleted";

                $("#documentMsg").append(html);
                document.getElementById("documentMsg").style.color = "red";
                $('#documentMsg').fadeIn().delay(3500).fadeOut();

                var liList = document.getElementById("documentUlListID").getElementsByTagName("li");
                var listLength = liList.length - 1;
                if (listLength < 2) {
                    $('#noFilesListItem').empty();
                    var html = "<li id=\"noFilesListItem\"><h4 class=\"noFiles\">No files to display</h4></li>";
                    $("#documentUlListID").append(html);
                }

            } else {
                if (response.message === "noPermission") {
                    $("#documentMsg").empty();
                    var html = "You don't have permission to delete file: <strong>" + response.name + response.type + "</strong>";

                    $("#documentMsg").append(html);
                    document.getElementById("documentMsg").style.color = "red";
                    $('#documentMsg').fadeIn().delay(3500).fadeOut();
                } else {
                    $("#documentMsg").empty();
                    var html = "Failed to delete <strong>" + response.name + response.type + "</strong>";

                    $("#documentMsg").append(html);
                    document.getElementById("documentMsg").style.color = "red";
                    $('#documentMsg').fadeIn().delay(3500).fadeOut();
                }
            }
        }

    });
}

