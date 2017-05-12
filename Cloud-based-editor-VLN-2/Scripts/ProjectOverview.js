// Makes a modal window of _RenameProject view.
var EditProjectName = function (ProjectID) {
    var url = "/Project/_RenameProject?ProjectID=" + ProjectID;

    $("#myModal1").load(url, function () {
        $("#myModal1").modal("show");
    });
};

// Gets the input data from the _rename partial view
// sends it to the renameproject actionresult and gets a Json answer back
// if the answer is false there comes an errormessage and the modal does not close
// if the answer is true then we hide the modal and do some work.
// if the answer is noPermission comes an errormessage saying you dont have permission
var submitUpdatedName = function () {
    var newNameString = document.getElementById("projectTextBoxID").value;
    if (newNameString !== "") {
        var myformdata = $("#myForm").serialize();
        $.ajax({

            type: "POST",
            url: "/Project/_RenameProject",
            data: myformdata,
            success: function (response) {
                if (response.success === true) {
                    $("#myModal1").modal("hide");
                    var itemID = "projectNameHeaderID" + response.projectID;
                    document.getElementById(itemID).innerHTML = response.name;

                } else {
                    if (response.message === "noPermission") {
                        $("#myForm").remove();
                        $("#RenameProjectSubmit").remove();
                        var html = "<h5>You don't have permission to rename this project </h5>";
                        $("#RenameModalBody").append(html);
                    }
                }
            }
        });
    }
    else {
        var div = document.getElementById("RenameProjectErrorDiv");
        div.innerHTML = "You must enter a name";
        div.style.display = "block";
    }
};
    
var deleteProject = function (projectID) {
    $.ajax({
        type: "GET",
        url: "/Project/DeleteProjectVal",
        data: { projectID: projectID },
        success: function (response) {

            if (response.success === true) {
                deleteConfirmation(projectID);
            } else {
                deleteNoPermission(projectID);
            }

        }
    });

};

var deleteConfirmation = function (projectID) {

    var url = "/Project/DeleteProjectConfirm?ProjectID=" + projectID;

    $("#myModal1").load(url, function () {
        $("#myModal1").modal("show");

    });
};

var deleteNoPermission = function (projectID) {

    var url = "/Project/DeleteNoPermission?ProjectID=" + projectID;

    $("#myModal1").load(url, function () {
        $("#myModal1").modal("show");

    });
};

// We post the projectID to deleteProject in projectcontroller
// and then remove it using ajax
var deleteProjectAjax = function (projectID) {
    $.ajax({

        type: "POST",
        url: "/Project/DeleteProject",
        data: { id: projectID },
        success: function (response) {
            var linkString = "link\\" + projectID;
            var divString = "div\\" + projectID;
            var linkToDelete = document.getElementById(linkString);
            var divToDelete = document.getElementById(divString);
            linkToDelete.parentNode.removeChild(linkToDelete);
            divToDelete.parentNode.removeChild(divToDelete);
        }
    });

};

var InviteToProject = function (ProjectID) {
    var url = "/Project/InviteUser?ProjectID=" + ProjectID;
    $("#myModal1").load(url, function () {
        $("#myModal1").modal("show");

    });
};

// Compares the searchstringInput to the users in the database
var populateList = function (searchStringInput) {

    var searchString = $(searchStringInput).val().toString();
    if (searchString === "") {
        var emptyHtml = " ";
        $("#usersAutoComplete").html(emptyHtml);
        return;
    }
    $.ajax({
        type: "GET",
        url: "/Project/PopulateList",
        data: { searchString: searchString },
        success: function (response) {
            var html = "";
            for (var i=0; i < response.userList.length; i++){
                html += "<li id=\""  + response.userList[i].UserName + "\" onclick=\"chosenUser(this.id)\" ><strong>" + response.userList[i].UserName + "</strong>   Email: " + response.userList[i].Email + "</li>";
            }
            $("#usersAutoComplete").html(html);
        }
    });
}

var chosenUser = function (id) {
    document.getElementById("userListInput").value = id;
    var emptyHtml = " ";
    $("#usersAutoComplete").html(emptyHtml);
}

var AddProject = function (currUserID) {
    var url = "/Project/AddProject?ownerID=" + currUserID;
    $("#myModal1").load(url, function () {
        $("#myModal1").modal("show");

    });
};

// Get the element from the textbox and the dropdown, if the textbox value
// is null we display an error message, otherwise we send a post request to
// Addproject in projectcontroller
var AddnewProjectFunc = function () {
    var test = document.getElementById("AddProjectTextBox").value;
    var test1 = document.getElementById("AddProjectDropDown").value;
    if (test !== "") {
        var myformdata = $("#AddProjectForm").serialize();
        $.ajax({

            type: "POST",
            url: "/Project/AddProject",
            data: myformdata,
            success: function (response) {
                $("#myModal1").modal("hide");
                $("#acceptedProject").append(response);
                
            }
        });
    }
    else {
        var div = document.getElementById("AddProjectErrorDiv");
        div.innerHTML = "You must enter a name";
        div.style.display = "block";
    }
};

// We send a get request to AbandonPrj in projectcontroller and recieve a 
// Json object back, if the admin is alone in the project we offer the ability to delete the project
// if the admin is not alone he must choose someone to become admin before he abandons
// if he is not admin he can abandon without a problem
var abandonPrj = function (projectID) {
    $.ajax({
            type: "GET",
            url: "/Project/AbandonPrj",
            data: { projectID: projectID },
            success: function(response) {
                if (response.message === "Admin++") {
                    var url = "/Project/ListCollaborators?ProjectID=" + projectID;

                    $("#myModal1").load(url, function () {
                        document.getElementById("CollaboratorsID").style.display = "none";
                        document.getElementById("hiddenCollaborators").innerHTML = "You have to choose one collaberator to be admin of the project before you can abandon the project";
                        $("#myModal1").modal("show");
                    });

                }
               else if (response.message === "Admin-") {
                    var urltwo = "/Project/AbandonPrjAdmin?ProjectID=" + projectID;

                    $("#myModal1").load(urltwo,
                        function() {
                            $("#myModal1").modal("show");

                        });

                } else {
                  
                
                    var urltwo = "/Project/AbandonPrjNormal?ProjectID=" + projectID;
                    $("#myModal1").load(urltwo,
                        function() {
                            $("#myModal1").modal("show");

                        });
                }

            }
        }
    );
}

var abandonProjectAdmin = function (projectID) {

    var url = "/Project/AbandonPrjAdmin?ProjectID=" + projectID;

    $("#myModal1").load(url, function () {
        $("#myModal1").modal("show");

    });
};

// post to AbandonPrj in projectcontroller and on success we remove the project on constant time
var AbandonProjectAjax = function (projectID, UserID) {
    $.ajax({
        type: "POST",
        url: "/Project/AbandonPrj",
        data: { id: projectID, userID: UserID },
        success: function() {
            var linkString = "link\\" + projectID;
            var divString = "div\\" + projectID;
            var linkToDelete = document.getElementById(linkString);
            var divToDelete = document.getElementById(divString);
            linkToDelete.parentNode.removeChild(linkToDelete);
            divToDelete.parentNode.removeChild(divToDelete);
        }
    });
}

var listCollaboratorsFunc = function (ProjectID) {
    var url = "/Project/ListCollaborators?ProjectID=" + ProjectID;

    $("#myModal1").load(url, function () {
        $("#myModal1").modal("show");  
    });
}

var deleteUserFromProject = function (projectID, UserID) {
    $.ajax({
        type: "POST",
        url: "/Project/AbandonPrj",
        data: { id: projectID, userID: UserID },
        success: function(){
            var removedUserID = "collaboratorListID" + UserID;
            $("#" + removedUserID).remove();
        }
    })
}


var makeAdminOfProject = function (projectID, UserID) {
    $.ajax({
        type: "POST",
        url: "/Project/MakeAdmin",
        data: { id: projectID, userID: UserID },
        success: function (response) {
            listCollaboratorsFunc(projectID);
        }
    })
}
