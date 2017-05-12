(function ($) {
    'use strict';
    $(function () {
        var dochub = $.connection.documentHub,
            $editor = ace.edit("editorID"),
            changed = false,
            markerID = null;

        dochub.client.updateText = function (obj, cursorScreenPos, userName) {

            $('#currentUser').stop(true);

            var Range = ace.require('ace/range').Range;
            var myRange = new Range(obj.start.row, obj.start.column, obj.end.row, obj.end.column);

            if (markerID != null) {
                $editor.getSession().removeMarker(markerID);
            }

            changed = true;
            $editor.getSession().getDocument().applyDelta(obj);
            markerID = $editor.getSession().addMarker(myRange, 'mymarker', 'text', false);

            var message = document.getElementById("currentUser");
            message.innerHTML = userName;
            message.style.position = "absolute";

            var offset = $('#editorID').position();

            cursorScreenPos.row *= 14;
            cursorScreenPos.row += offset.top;

            cursorScreenPos.column *= 7;
            cursorScreenPos.column += 40;
            cursorScreenPos.column += (offset.left + 28);

            message.style.top = cursorScreenPos.row + "px";
            message.style.left = cursorScreenPos.column + "px";

            message.style.zIndex = 100;

            $('#currentUser').fadeIn().delay(100).fadeOut();

            changed = false;
        };

        window.hubReady.done(function () {
            dochub.server.joinDocument(documentID);
            $editor.on('change',
                function (obj) {

                    saveEditorContent($);
                    if (changed) {
                        return;
                    }

                    var cursorScreenPos = $editor.getCursorPositionScreen();
                    dochub.server.updateDocument(obj, documentID, cursorScreenPos);
                }
            );
        });
    });
})(jQuery);