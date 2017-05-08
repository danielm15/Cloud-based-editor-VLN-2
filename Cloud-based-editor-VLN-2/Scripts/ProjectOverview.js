var EditProjectName = function (ProjectID) {
    var url = "/Project/_RenameProject?ProjectID=" + ProjectID;

    $("#myModal1").load(url, function () {
        $("#myModal1").modal("show");

    })

}

var submitUpdatedName = function () {
        var myformdata = $("#myForm").serialize();
        $.ajax({

            type: "POST",
            url: "/Project/_RenameProject",
            data: myformdata,
            success: function () {
                $("#loaderDiv").hide();
                $("#myModal").modal("hide");
                window.location.href = "/Project/";
            },
        })
}