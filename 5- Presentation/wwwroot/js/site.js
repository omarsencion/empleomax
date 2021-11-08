// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//$(function () {
//    var placeHolderHere = $('#PlaceHolderHere')
//    $('button[data-toggle="ajax-modal"]').click(function (event) {
//        var url = $(this).data('url')
//        $.get(url).done(function (data) {
//            placeHolderHere.html(data);
//            placeHolderHere.find('.modal').modal('show');
//        });

//    });

//    placeHolderHere.on('click', '[data-save="modal"]', function (event) {

//        var form = $(this).parents('.modal').find('form');
//        var actionUrl = form.attr('action');
//        var sendData = form.serialize();
//        $.post(actionUrl, sendData).done(function (data) {
//            placeHolderHere.find('.modal').modal('hide');
//        });
//    });
//})

showInPopup=(url, title) =>
{
    $.ajax({
        type: "GET",
        url: url,
        success: function (res)
        {
            $("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html(title);
            $('#form-modal').modal('show');
        }
    });
};

jqueryAjaxPost = form =>
{
    $.ajax({
        type: "POST",
        url: form.action,
        data: new FormData(form),
        contentType: false,
        processData: false,
        sucess: function (res)
        {
            if (res.isValid)
            {
                debugger;
                window.location.href = "/Plans/Edit/" + res.data;
                $('#form-modal').modal('hide');
            }
        },
        error: function (err) { }
        
    });
}

$('.select2').select2({
    dropdownAutoWidth: 'true',
    width: '100%'
})

//Date picker
$('.datepicker').datepicker({
    autoclose: true
})