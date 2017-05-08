(function ($) {
    $(document).ready(function () {
    $("#saveFileBtn").on("click", function() {
    
        saveEditorContent($);
     
        });
    });

})(jQuery);

function saveEditorContent($) {
    var updateDocumentID = document.getElementById("activeDocID").value
    var editor = ace.edit("editorID");
    var contentData = editor.getValue();

    $.ajax({
        type: "POST",
        url: "/Editor/SaveFile",
        data: { updateDocumentID, contentData: contentData },
        success: function (response) {
        }
    });
}

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
    if (document.getElementById("editorContentId").style.marginLeft == "200px") {
        document.getElementById("editorContentId").style.marginLeft = "0px";
        document.getElementById("verticalTextID").style.borderBottom = "1px solid darkgray";
    } else {
        document.getElementById("editorContentId").style.marginLeft = "200px";
        document.getElementById("verticalTextID").style.borderBottom = "1px solid white";
    }
    
}

function showHeader(id) {
    saveEditorContent($);
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



(function ($) {
    'use strict';
    $(function () {
        var dochub = $.connection.documentHub,
            $editor = ace.edit("editorID"),
            changed = false,
            markerID = null;
        
        dochub.client.updateText = function (obj, cursorScreenPos) {
            var message = document.getElementById("currentUser");
            $('#currentUser').clearQueue();

            var Range = ace.require('ace/range').Range;
            var myRange = new Range(obj.start.row, obj.start.column, obj.end.row, obj.end.column);
            
            if (markerID != null) {
                $editor.getSession().removeMarker(markerID);
            }

            changed = true;
            $editor.getSession().getDocument().applyDelta(obj);
            markerID = $editor.getSession().addMarker(myRange, 'mymarker', 'text', false);

            var message = document.getElementById("currentUser");
            message.innerHTML = "User";
            message.style.position = "absolute";
            cursorScreenPos.row *= 30;
            cursorScreenPos.column *= 20;
            message.style.top = cursorScreenPos.row + "px";
            message.style.left = cursorScreenPos.column + "px";
            message.style.zIndex = 100;
            $('#currentUser').fadeIn().delay(1000).fadeOut();

            changed = false;
        };
        $.connection.hub.start().done(function () {
            dochub.server.joinDocument(documentID);
            $editor.on('change',
                function (obj) {
                    saveEditorContent($);
                    if (changed) {
                        return;
                    }
                    var cursorScreenPos = $editor.getCursorPositionScreen();
                    console.log(cursorScreenPos)
                    dochub.server.updateDocument(obj, documentID, cursorScreenPos);
                }
            );
                
        });
    });
})(jQuery);

    
