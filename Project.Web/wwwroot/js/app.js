var Spinner = (function () {
    "use strict";
    var result = {};
    result.show = function () {
        $('#loading').show();
    }
    result.hide = function () {
        $("#loading").hide();
    }
    return result;
}());
function loadLink(url, id) {
    ResetFingerKeys();
    if (typeof (id) === 'undefined') {
        loadPartialView(url, 'mainContent');
    } else {
        loadPartialView(url, id);
    }
}
function loadPartialView(link, position) {

    $('#' + position).html('');
    Spinner.show();
    setTimeout(function () {
        var resp = $.ajax({
            url: link,
            contentType: 'application/html; charset=utf-8',
            type: 'GET',
            async: false,
            dataType: 'html',
            //headers: $('input[name="__RequestVerificationToken"]').val(),
            beforeSend: function () {
                Spinner.show();
            },
            success: function (result) {
                debugger;
                if (isJsonString(result) && JSON.parse(result).message != null) {
                    bootbox.alert(JSON.parse(result).message);
                    if (JSON.parse(result).redirectTo != null) {
                        loadLink(JSON.parse(result).redirectTo);
                    }
                } else {
                    $('#' + position).show();
                    $('#' + position).html(result);
                }
                Spinner.hide();

            },
            error: function (status) {
                bootbox.alert(status.statusText);
                Spinner.hide();
            }
        }).responseText;
    }, 10);
}
