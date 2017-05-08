$(function () {
    $("#createFileForm").on("submit", function () {
        document.getElementById("noFilesListItme").style.display = "none";
        var form = $(this);
        $.ajax({
            method: "post",
            url: "Document/Create",
            data: form.serialize(),
            success: function (response) {
                var jsonDate = String(response.DateCreated);
                var re = /-?\d+/; 
                var m = re.exec(jsonDate); 
                var date = new Date(parseInt(m[0]));
                var time = date.toLocaleTimeString('en-GB'  );
                var day = date.getDate();
                var month = date.getMonth() + 1;
                var year = date.getFullYear();
                var html = "<li>"
                   + "<div>"
                       + "<div class=\"row documentListItem\">"
                        + "<a href=\"/Editor?projectID=" + response.ProjectID + "&documentID=" + response.ID + "\" class=\"clickableDiv\">"
                               + "<div class=\"col-md-3 listText\">"
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
                               +  "<div class=\"col-md-2 right-float\">"
                               +    "<div class=\"dropdown right-float\">"
                               +        "<a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">"
                               +             "<span class=\"glyphicon glyphicon-option-vertical optionsButton\"></span>"
                               +        "</a>"
                               +        "<ul class=\"dropdown-menu\">"
                               +            "<li><a href=\"/Document/Rename?projectID=" + response.ProjectID + "\" tabindex=\"-1\" type=\"a\">Rename</a></li>"
                               +            "<li><a href=\"/Document/DownloadFile?documentID=" + response.ID + "\" tabindex=\"-1\" type=\"a\">Download</a></li>"
                               +            "<li><a href=\"/Document/Delete?projectID=" + response.ProjectID + "\" tabindex=\"-1\" type=\"a\">Delete</a></li>"
                               +        "</ul>"
                               +    "</div>"
                               + "</div>"
                    + "</div>"
                + "</div>"
            + "</li>";
                $("#documentUlListID").append(html);
            }
        });
        document.getElementById("createFileForm").reset();
        document.getElementById("createFileBtn").classList.toggle("open");
        return false;
    });
});

$(function createFile() {

});
