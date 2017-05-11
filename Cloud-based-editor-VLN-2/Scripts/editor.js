(function ($) {
    $(document).ready(function () {
    $("#saveFileBtn").on("click", function() {
    
        saveEditorContent($);
     
        });
    });

})(jQuery);

function saveEditorContent($) {
    var updateDocumentID = document.getElementById("activeDocID").value;
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

$(function () {
    window.hubReady = $.connection.hub.start();
});

(function ($) {
    'use strict';
    $(function () {
        var invhub = $.connection.invitationHub;

        invhub.client.sendInvite = function (fromID, toID, projectID) {
            
            var notifyCount = document.getElementById("NotifyCount");

            if (notifyCount.innerHTML == "") {
                notifyCount.innerHTML = "1";
            }
            else {
                notifyCount.innerHTML = (parseInt(notifyCount.innerHTML) + 1).toString();
                
            }
        }
 
        window.hubReady.done(function () {
            invhub.server.joinUserGroup(userID);
            $(document).on('click', '#InviteUserSubmitBtn', function () {
                var projectID = $('#CurrentProjectToInvite').val();
                var userName = $('#userListInput').val();
                console.log(projectID);
                console.log(userName);
                $.ajax({
                    type: "POST",
                    url: "/Project/Invite",
                    data: { projectID: projectID, userName: userName },
                    success: function (response) {
                        if (response.success === "hasProject") {
                            //document.getElementById("userListInput").classList.toggle("hideInput");
                            $("#InviteUserError").empty();
                            document.getElementById("InviteUserError").innerHTML = "   User is already a collaborator in this project";
                            document.getElementById("InviteUserError").style.color = "red";
                            $('#InviteUserError').fadeIn().delay(3500).fadeOut();
                            /*document.getElementById("InviteUserSubmitBtn").innerHTML = "Try again";
                            var functionName = "InviteToProject(" + response.projectID + " )";
                            $("#InviteUserSubmitBtn").attr("onclick", functionName);*/
                        }
                        else if (response.success === "hasInvite") {
                            /*document.getElementById("userListInput").classList.toggle("hideInput");
                            document.getElementById("myHeaderMessage").innerHTML = "User <strong> " + response.name + " </strong> already has an invite";
                            document.getElementById("InviteUserSubmitBtn").innerHTML = "Try again";
                            var functionName = "InviteToProject(" + response.projectID + " )";
                            $("#InviteUserSubmitBtn").attr("onclick", functionName);*/
                            $("#InviteUserError").empty();
                            document.getElementById("InviteUserError").innerHTML = "   User already has an invtie to this project";
                            document.getElementById("InviteUserError").style.color = "red";
                            $('#InviteUserError').fadeIn().delay(3500).fadeOut();
                        }
                        else if (response.success === "userNotFound") {
                            /*document.getElementById("userListInput").classList.toggle("hideInput");
                            document.getElementById("myHeaderMessage").innerHTML = "User <strong> " + response.name + " </strong> not found";
                            document.getElementById("InviteUserSubmitBtn").innerHTML = "Try again";
                            var functionName = "InviteToProject(" + response.projectID + " )";
                            $("#InviteUserSubmitBtn").attr("onclick", functionName);*/
                            $("#InviteUserError").empty();
                            document.getElementById("InviteUserError").innerHTML = "   User not found";
                            document.getElementById("InviteUserError").style.color = "red";
                            $('#InviteUserError').fadeIn().delay(3500).fadeOut();
                        }
                        else {
                            /*document.getElementById("userListInput").classList.toggle("hideInput");
                            document.getElementById("myHeaderMessage").innerHTML = "User <strong> " + response.name + " </strong> has received an invite";
                            
                            document.getElementById("InviteUserSubmitBtn").innerHTML = "Invite another user";
                            var functionName = "InviteToProject(" + response.projectID + " )";
                            $("#InviteUserSubmitBtn").attr("onclick", functionName);*/
                            $("#InviteUserError").empty();
                            document.getElementById("InviteUserError").innerHTML = "   Invite sent";
                            document.getElementById("InviteUserError").style.color = "green";
                            $('#InviteUserError').fadeIn().delay(3500).fadeOut();

                            invhub.server.sendInvitation(userID, userName, parseInt(projectID));
                        }
                    }
                });

                //invhub.server.sendInvitation(userID, toUserName, parseInt(projectID));
            });
        });
    });
})(jQuery);


(function ($) {
    'use strict';
    $(function () {
        var dochub = $.connection.documentHub,
            $editor = ace.edit("editorID"),
            changed = false,
            markerID = null;

        dochub.client.updateText = function (obj, cursorScreenPos) {

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
            message.innerHTML = "User";
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
            dochub.server.joinUserGroup(userID);
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

    
