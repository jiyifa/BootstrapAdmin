﻿$(function () {
    $('form').autoValidate({
        userName: {
            required: true,
            maxlength: 50
        },
        displayName: {
            required: true,
            maxlength: 50
        },
        password: {
            required: true,
            maxlength: 50
        },
        description: {
            required: true,
            maxlength: 500
    }
    });

    $('#btnAccount').click(function () {
        var valid = $('form').valid();
        if (valid) {
            $('.setup-main').hide();
            $('.steps li').toggleClass('current');
            $('#loginID').text($('#userName').val());
            $('#loginName').text($('#displayName').val());
            $('#loginDesc').text($('#description').val());
            $('.setup-confirm').show();
        }
    });
});