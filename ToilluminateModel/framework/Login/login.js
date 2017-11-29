(function ($) {
    var selectedFileData = {};
    var selectedFolderID = null;
    var actionEnum = { "cut": "cutFile", "copy": "copyFile" };
    var action = "";
    var methods = {
        init: function (options) {
            selectedFolderID = options.selectedFolderID;
            $('#datatable_file').prop("outerHTML", "<div class='m_datatable' id='datatable_file'></div>");
            $.insmFramework('getFilesByFolder', {
                FolderID: selectedFolderID,
                success: function (fileData) {
                    tableData.data.source.data = fileData;
                    $("#datatable_file").data("datatable", $('#datatable_file').mDatatable(tableData));
                },
                error: function () {
                    //options.error();
                },
            });
        },
        uploadFile: function () {
            if (!selectedFolderID) {
                return false;
            }
        },




    };

    $("#m_login_signin_submit").click(function (e) {
        
        e.preventDefault();
        var btn = $(this);
        var form = $(this).closest('form');

        form.validate({
            rules: {
                email: {
                    required: true
                },
                password: {
                    required: true
                }
            }
        });

        if (!form.valid()) {
            return;
        }

        btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);

        form.ajaxSubmit({
            url: '',
            success: function (response, status, xhr, $form) {
                // similate 2s delay
                setTimeout(function () {
                    btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                    showErrorMsg(form, 'danger', 'Incorrect username or password. Please try again.');
                }, 2000);
                $("#mainDiv").show();
                $("#divLogin").hide();
            }
        });
    });


    $.login = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on $.insmGroup');
        }
        return null;
    };
})(jQuery);