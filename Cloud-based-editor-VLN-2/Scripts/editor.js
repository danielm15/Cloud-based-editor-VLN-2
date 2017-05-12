// Saves the file when the save button is clicked
(function ($) {
    $(document).ready(function () {
    $("#saveFileBtn").on("click", function() {
        saveEditorContent($);
        });
    });

})(jQuery);

// Ajax post request that saves the changes to the document
function saveEditorContent($) {
    var updateDocumentID = parseInt(document.getElementById("activeDocID").value);
    var editor = ace.edit("editorID");
    var contentData = editor.getValue().toString();

    $.ajax({
        type: "POST",
        url: "/Editor/SaveFile",
        data: { updateDocumentID: updateDocumentID, contentData: contentData },
        success: function (response) {
        }
    })
}

// Ajax get request that gets the content of a specific file and displays it in the editor
function getContent($, ID) {
    $.ajax({
        type: "GET",
        url: "/Editor/GetContent",
        data: {documentID: ID},
        success: function (response) {
            var editor = ace.edit("editorID");
            editor.getSession().setValue(response);
            
        }
    });
}

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
    if (document.getElementById("editorContentId").style.marginLeft === "200px") {
        document.getElementById("editorContentId").style.marginLeft = "0";
        document.getElementById("verticalTextID").style.borderBottom = "1px solid darkgray";
        document.getElementById("editorHeaderContainer").style.marginLeft = "3%";
    } else {
        document.getElementById("editorContentId").style.marginLeft = "200px";
        document.getElementById("editorHeaderContainer").style.marginLeft = "220px";
        document.getElementById("verticalTextID").style.borderBottom = "1px solid white";
    }
    
}

function showHeader(id) {

    if ($("#" + id).hasClass("selectedNav")) {
        return;
    }

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
    getContent($, id);
}

// Funtion that starts a connection for SignalR
$(function () {
    window.hubReady = $.connection.hub.start();
});

// Changes the syntax highlighting based on the file being edited in the editor
function changeHighlighting(type) {
    if (type === ".html") {
        return "html";
    }
    else if (type === ".js") {
        return "javascript";
    }
    else if (type === ".cs") {
        return "csharp";
    }
    else if (type === ".css") {
        return "css";
    }
    else if (type === ".c" || type === ".cpp") {
        return "c_cpp";
    }
    else if (type === ".py") {
        return "python";
    }
    else {
        return "txt";
    }
}