$(function () {
    $("#createFileForm").on("submit", function () {
        var form = $(this);
        $.ajax({
            method: "post",
            url: "Document/Create",
            data: form.serialize(),
            success: function (respnse) {

            }
        });
        return false;
    });
});