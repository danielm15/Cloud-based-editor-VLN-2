/*
$(document).ready(function {
    var element = document.getElementById("editorID");

    var editor = ace.edit("editorID");
    editor.setTheme("ace/theme/twilight");
    editor.session.setMode("ace/mode/csharp");
    editor.getSession().setValue(content);

});
*/


    /*
    (function ($) {
        'use strict';
    
        $(function () {
            var documentHub = $.connection.documentHub,
            $editor = $("#editorID"),
            documentModel = {
                content: null
            };
            documentHub.client.updateText = function (model) {
                documentModel.content = model.content;
                //$editor
            };
            $.connection.hub.start().done(function () {
                $editor.contents
                documentHub.server.updateDocument(documentModel);
                documentModel.content;
            });
        });
    
    })(jQuery);
    */