function userDropDownFunc() {
    document.getElementById("dropdownId").classList.toggle("show");
}

// Close the dropdown menu if the user clicks outside of it
window.onclick = function (event) {

    if (!event.target.matches('.dropdownBtn')) {

        var dropdowns = document.getElementsByClassName("dropdownLinks");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}