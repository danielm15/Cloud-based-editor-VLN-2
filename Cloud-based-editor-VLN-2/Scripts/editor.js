﻿(function ($) {
    $(document).ready(function () {
    $("#saveFileBtn").on("click", function () {
      
        var updateDocumentID = document.getElementById("activeDocID").value
        var editor = ace.edit("editorID");
        var contentData = editor.getValue();

        $.ajax({
            type: "POST",
            url: "/Editor/SaveFile",
            data: {updateDocumentID, contentData: contentData},
            success: function (response) {
                alert("changes saved.");
            }
        });
    });
    });

})(jQuery);

function bodyMargin() {
    document.getElementById("bodyId").classList.toggle("addToBody");
    document.getElementById("containerBodyId").classList.toggle("changeWidth");
    document.getElementById("containerHeaderId").classList.toggle("changeWidth");
    document.getElementById("containerBodyId").style.paddingLeft = "0";
    
}

function hideFooter() {
    document.getElementById("footerId").classList.toggle("hideFooter");
}

function showNavBar() {
    document.getElementById("sideNav").classList.toggle("activeNav");
    if (document.getElementById("editorContentId").style.marginLeft == "200px") {
        document.getElementById("editorContentId").style.marginLeft = "0px";
        document.getElementById("verticalTextID").style.borderBottom = "1px solid darkgray";
    } else {
        document.getElementById("editorContentId").style.marginLeft = "200px";
        document.getElementById("verticalTextID").style.borderBottom = "1px solid white";
    }
    
}

function showHeader(id) {
    var classElementsNav = document.getElementsByClassName("navigationLink");

    for (var i = 0; i < classElementsNav.length; i++) {
        var element = classElementsNav[i];
        element.classList.remove("selectedNav");
    }
    document.getElementById(id).classList.toggle("selectedNav");

    var id = "doc" + id.substring(3);
    var classElements = document.getElementsByClassName("hideHeader");
    
    for (var i = 0; i < classElements.length; i++) {
        var element = classElements[i];
        element.classList.remove("showHeader");
    }
    document.getElementById(id).classList.toggle("showHeader");

    var id = id.substring(3);

    document.getElementById("activeDocID").value = id;
}
