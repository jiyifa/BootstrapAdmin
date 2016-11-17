﻿$(function () {
    var url = '../api/Exceptions/';
    var $dialog = $('#dialogNew');
    var $dataForm = $('#dataForm');
    var $dataFormDetail = $('#dataFormDetail');
    var $errorList = $('#errorList');
    var $errorDetail = $('#errorDetail');
    var $errorDetailTitle = $('#myDetailModalLabel');

    var bsa = new BootstrapAdmin({
        url: url,
        bootstrapTable: null,
        validateForm: null
    });

    $('table').smartTable({
        url: url,
        sortName: 'LogTime',
        queryParams: function (params) { return $.extend(params, { StartTime: $("#txt_operate_start").val(), EndTime: $("#txt_operate_end").val() }); },
        columns: [{ checkbox: true },
            { title: "请求网址", field: "ErrorPage", sortable: true },
            { title: "用户名", field: "UserID", sortable: true },
            { title: "IP", field: "UserIp", sortable: true },
            { title: "错误", field: "Message", sortable: false },
            { title: "记录时间", field: "LogTime", sortable: true }
        ]
    });

    $('input[type="datetime"]').parent().datetimepicker({
        locale: "zh-cn",
        format: "YYYY-MM-DD"
    });

    $('#btn_view').on('click', function (row) {
        Exceptions.getFiles(function (data) {
            $dataForm.children('div').html(data);
        });
        $dialog.modal('show');
    });

    $dialog.on('click', 'a', function () {
        var fileName = $(this).find('span').text();
        $errorDetailTitle.text(fileName);
        $errorList.hide();
        $errorDetail.show();
        $dataFormDetail.html('<div class="text-center"><i class="fa fa-spinner fa-pulse fa-3x fa-fw"></i></div>');
        Exceptions.getFileByName(fileName, function (data) {
            $dataFormDetail.html(data);
        });
    });

    $errorDetail.on('click', 'button', function () {
        $errorDetail.hide();
        $errorList.show();
    });
});