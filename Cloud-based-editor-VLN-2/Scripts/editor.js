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
            changed = false;
        
        dochub.client.updateText = function (obj) {
            
            //$editor.getSession().addMarker(range, "ace_active-line", "fullLine");
            //var Anchor = ace.require('ace/anchor').Anchor;
            //var myAnchor = new Anchor(editor.getSession().getDocument(), obj.start.row, obj.start.col);
            /*var Range = ace.require('ace/range').Range;
            var myRange = new Range(obj.start.row, obj.start.col, obj.start.row, obj.start.col + 1);
            var currentMarker;
            $editor.getSession().removeMarker(currentMarker);
            $editor.getSession().addMarker(myRange, 'ace_marker-layer', 'text', false);*/

            changed = true;
            $editor.getSession().getDocument().applyDelta(obj);
            
            changed = false;
        };
        $.connection.hub.start().done(function () {
            console.log(documentID);
            dochub.server.joinDocument(documentID);
            $editor.on('change',
                function (obj) {
                    saveEditorContent($);
                    //console.log(obj);
                    if (changed) {
                        return;
                    }
                    dochub.server.updateDocument(obj, documentID);
                }
            );
                
        });
    });
})(jQuery);

    
