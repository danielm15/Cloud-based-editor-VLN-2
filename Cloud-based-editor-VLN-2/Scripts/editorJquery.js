$(document).ready(function (){
    $("#saveFileBtn").on("click", function () {
        $.ajax({
            url: '@Url.Action("SaveFile", "Editor")',
            type: 'POST',
            data: { updateDocumentID: "1", documentContent: "ABC" },
            cache: false,
            success: function (response) {
                alert("Changes Saved.");
            }
        });
    });
});
