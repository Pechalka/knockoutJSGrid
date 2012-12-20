$.validator.setDefaults({
    highlight: function (element) {
        $(element).closest(".control-group").addClass("error");

    },
    unhighlight: function (element) {
        $(element).closest(".control-group").removeClass("error");
    }
});

$(function () {
    $('.alert-error').prepend('<button type="button" class="close" data-dismiss="alert">×</button>');
});