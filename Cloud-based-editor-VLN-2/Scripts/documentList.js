$(function () {
    $("#createFileForm").on("submit", function () {
        /*var noFilesHeader = document.getElementById("noFilesListItem");
        if (noFilesHeader != null) {
            document.getElementById("noFilesListItme").style.display = "none";
        }*/
        $('#noFilesListItem').empty();
        var form = $(this);
        $.ajax({
            method: "post",
            url: "Document/Create",
            data: form.serialize(),
            success: function (response) {
                
                if (response.success == false) {
                    $('#duplicateErrorMsg').empty();
                    var html = "  Duplicate file name"
                    $('#duplicateErrorMsg').append(html);
                    var error = document.getElementById('duplicateErrorMsg');
 
                    error.style.color = "red";
                    $('#duplicateErrorMsg').fadeIn().delay(2000).fadeOut();
   
                }
                else if (response.success == "bothempty") {
                    $('#duplicateErrorMsg').empty();
                    var html = "  File name empty"
                    $('#duplicateErrorMsg').append(html);
                    var error = document.getElementById('duplicateErrorMsg');

                    error.style.color = "red";
                    $('#duplicateErrorMsg').fadeIn().delay(2000).fadeOut();

                    $('#typeErrorMsg').empty();
                    var html = "  File type empty"
                    $('#typeErrorMsg').append(html);
                    var error = document.getElementById('typeErrorMsg');

                    error.style.color = "red";
                    $('#typeErrorMsg').fadeIn().delay(2000).fadeOut();
                }
                else if (response.success == "nameempty") {
                    $('#duplicateErrorMsg').empty();
                    var html = "  File name empty"
                    $('#duplicateErrorMsg').append(html);
                    var error = document.getElementById('duplicateErrorMsg');

                    error.style.color = "red";
                    $('#duplicateErrorMsg').fadeIn().delay(2000).fadeOut();
                }
                else if (response.success == "filetypeempty") {
                    $('#typeErrorMsg').empty();
                    var html = "  File type empty"
                    $('#typeErrorMsg').append(html);
                    var error = document.getElementById('typeErrorMsg');

                    error.style.color = "red";
                    $('#typeErrorMsg').fadeIn().delay(2000).fadeOut();

                }
                else {
                    var jsonDate = String(response.DateCreated);
                    var re = /-?\d+/;
                    var m = re.exec(jsonDate);
                    var date = new Date(parseInt(m[0]));
                    var time = date.toLocaleTimeString('en-GB');
                    var day = date.getDate();
                    var month = date.getMonth() + 1;
                    var year = date.getFullYear();
                    var html = "<li id=\"listItem" + response.ID + "\">"
                       + "<div>"
                           + "<div class=\"row documentListItem\">"
                            + "<a href=\"/Editor?projectID=" + response.ProjectID + "&documentID=" + response.ID + "\" class=\"clickableDiv\">"
                                   + "<div class=\"col-md-3 listText\" id=\"nameID" + response.ID + "\">"
                                        + response.Name
                                   + "</div>"
                                   + "<div class=\"col-md-2 listText\">"
                                        + response.CreatedBy
                                   + "</div>"
                                   + "<div class=\"col-md-3 listText\">"
                                        + day + "." + month + "." + year + " " + time
                                   + "</div>"
                                   + "<div class=\"col-md-2 listText\">"
                                        + response.Type
                                   + "</div>"
                                   + "</a>"
                                   + "<div class=\"col-md-2 right-float\">"
                                   + "<div class=\"dropdown right-float\">"
                                   + "<a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">"
                                   + "<span class=\"glyphicon glyphicon-option-vertical optionsButton\"></span>"
                                   + "</a>"
                                   + "<ul class=\"dropdown-menu\">"
                                   + "<li><a href=\"/Document/Rename?projectID=" + response.ProjectID + "\" tabindex=\"-1\" type=\"a\">Rename</a></li>"
                                   + "<li><a href=\"/Document/DownloadFile?documentID=" + response.ID + "\" tabindex=\"-1\" type=\"a\">Download</a></li>"
                                   + "<li><a href=\"#\" onclick=\"deleteDocument(" + response.ID + ")\">Delete</a></li>"
                                   + "</ul>"
                                   + "</div>"
                                   + "</div>"
                        + "</div>"
                    + "</div>"
                + "</li>";
                    $("#documentUlListID").append(html);
                    document.getElementById("createFileForm").reset();
                    document.getElementById("createFileBtn").classList.toggle("open");

                    $("#documentMsg").empty();
                    var html = "File: <strong>" + response.Name + response.Type + "</strong> Created";

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
            if (response.success == true) {
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
                if (response.message == "noPermission") {
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

