var EditFileName = function (fileID) {

    $("#myModal1file").load("/Document/_RenameDocument?documentID=" + fileID,

        function () {

            $("#myModal1file").modal("show");



        });

}



var submitUpdatedFileName = function (projectID) {

    var nameField = document.getElementById("RenameDocumentTextBox").value;

    if (nameField !== "") {

        var myformdata = $("#myFormFile").serialize();

        $.ajax({

            type: "POST",

            url: "/Document/_RenameDocument",

            data: myformdata,

            success: function (response) {

                if (response.success === true) {

                    $("#myModal1file").modal("hide");

                    var itemID = "nameID" + response.docID;

                    document.getElementById(itemID).innerHTML = response.name;

                    $("#documentMsg").empty();

                    var html = "File name change to: <strong>" + response.name + response.type + "</strong>";

                    $("#documentMsg").append(html);

                    document.getElementById("documentMsg").style.color = "gray";

                    $('#documentMsg').fadeIn().delay(3500).fadeOut();

                } else {

                    if (response.message === "noPermission") {

                        $("#myModal1file").modal("hide");

                        $("#documentMsg").empty();

                        var html = "You don't have permission to rename this file";

                        $("#documentMsg").append(html);

                        document.getElementById("documentMsg").style.color = "red";

                        $('#documentMsg').fadeIn().delay(3500).fadeOut();

                    } else if (response.message === "duplicateFileName") {

                        $('#RenameDocumentErrorDiv').empty();



                        var html = "Duplicate file name";

                        $('#RenameDocumentErrorDiv').append(html);



                        var error = document.getElementById('RenameDocumentErrorDiv');

                        error.style.color = "red";



                        $('#RenameDocumentErrorDiv').fadeIn().delay(2000).fadeOut();

                    }

                }

            }

        });

    } else {

        var div = document.getElementById("RenameDocumentErrorDiv");

        div.innerHTML = "You must enter a name";

        div.style.display = "block";

    }

}

