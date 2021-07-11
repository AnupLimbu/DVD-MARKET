// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// remove stock from user stock list

var dvdIndex;
var categoryIndex;
var memberIndex, memberTrIndex;
var producerIndex;
var castIndex;

function setDvdIndex(index) {
    this.dvdIndex = index;
}

function setCategoryIndex(index) {
    this.categoryIndex = index;
}

function setMemberIndex(index, indextr) {
    this.memberIndex = index;
    this.memberTrIndex = indextr;
}

function setProducerIndex(index) {
    this.producerIndex = index;
}

function setCastIndex(index) {
    this.castIndex = index;
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

function removeCategory() {
    $.ajax({
        type: "POST",
        url: "/CategoryModels/Delete/" + categoryIndex,
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (res) {
            if (res) {
                $("#category_" + categoryIndex).remove();
                alert("Delete Success!")
            } else {
                alert("Delete Failed!");
            }
        }
    });
}

function removeMember() {
    $.ajax({
        type: "POST",
        url: "/DVDCleaner/Delete/" + memberIndex,
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (res) {
            if (res) {
                $("#month_" + memberTrIndex).remove();
                alert("Delete Success!")
            } else {
                alert("Delete Failed!");
            }
        }
    });
}

function removeProducer() {
    $.ajax({
        type: "POST",
        url: "/ProducerModels/Delete/" + producerIndex,
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (res) {
            if (res) {
                $("#producer_" + producerIndex).remove();
                alert("Delete Success!")
            } else {
                alert("Delete Failed!");
            }
        }
    });
}

function removeCast() {
    $.ajax({
        type: "POST",
        url: "/CastModels/Delete/" + castIndex,
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (res) {
            if (res) {
                $("#cast_" + castIndex).remove();
                alert("Delete Success!")
            } else {
                alert("Delete Failed!");
            }
        }
    });
}

function deleteAll365Days(count) {
    $.ajax({
        type: "POST",
        url: "/DVDCleaner/DeleteAllYear/",
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (res) {
            if (res) {
                for (var index = 1; index < count + 1; index++) {
                    $("#year_" + index).remove();
                }
                alert("Delete All Success!");
            } else {
                alert("Delete Failed!");
            }
        }
    });
}

(function ($) {
    /*--/ Star Typed /--*/
    if ($('.text-slider').length == 1) {
        var typed_strings = $('.text-slider-items').text();
        var typed = new Typed('.text-slider', {
            strings: typed_strings.split(','),
            typeSpeed: 80,
            loop: true,
            backDelay: 1100,
            backSpeed: 30
        });
    }
})(jQuery);
