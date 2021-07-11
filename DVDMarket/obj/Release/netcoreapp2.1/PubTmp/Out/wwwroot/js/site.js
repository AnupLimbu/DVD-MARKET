// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// remove stock from user stock list

var dvdIndex;

function setDvdIndex(index) {
    this.dvdIndex = index;
}

function removeDvd() {
    $.ajax({
        type: "POST",
        url: "/DVDShop/Delete/" + dvdIndex,
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (res) {
            if (res) {
                $("#dvd_" + dvdIndex).remove();
                alert("Delete Success!")
            } else {
                alert("Delete Failed!");
            }
        }
    });
}
