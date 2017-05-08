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


//function update() {
    (function ($) {
        'use strict';
        $(function () {
            var dochub = $.connection.documentHub,
                $editor = ace.edit("editorID"),
                documentModel = {
                    content: null


                },
                cursorPos = {
                    row: 0,
                    col: 0
                },


                messageFrequency = 10,
                updateRate = 1000 / messageFrequency,
                changed = false;
                dochub.client.updateText = function (model, range) {
                documentModel.content = model;
                //$editor.getSession().
                //$editor.getSession().setValue(documentModel.content);
                //$editor.getSession().addMarker(range, "ace_active-line", "fullLine");

                $editor.getSession().replace(range, documentModel.content);
            };
            $.connection.hub.start().done(function () {
                $editor.on('change',
                    function () {

                        /*cursorPos = $editor.getCursorPosition();
                        documentModel.content = $editor.getSession().getLine(cursorPos.row);
                        //var range = new ace.Range(cursorPos.row, 0, cursorPos.row + 1, 0);
                        var range = {
                            start: {
                                row: cursorPos.row,
                                column: 0
                            },
                            end: {
                                row: cursorPos.row + 1,
                                column: 0
                            }
                        };*/
                        //documentModel.content = $editor.getValue();
                        //dochub.server.updateDocument(documentModel, range);

                        changed = true;
                        setInterval(updateServerModel, updateRate);
                    }
                );
                
            });
            function updateServerModel() {
                if (changed) {
                    cursorPos = $editor.getCursorPosition();
                    documentModel.content = $editor.getSession().getLine(cursorPos.row);
                    //var range = new ace.Range(cursorPos.row, 0, cursorPos.row + 1, 0);
                    
                    var range = {
                        start: {
                            row: cursorPos.row,
                            column: 0
                        },
                        end: {
                            row: cursorPos.row + 1,
                            column: 0
                        }
                    };
                    dochub.server.updateDocument(documentModel, range);
                    changed = false;
                }
            }

        });
        })(jQuery);
        //}
    
