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
    var login = $('#m_login');
    $("#m_login_signin_submit").click(function (e) {
        
        e.preventDefault();
        var btn = $(this);
        var form = $(this).closest('form');

        form.validate({
            rules: {
                username: {
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


        $.insmFramework('userlogin', {
            userName: 'test',
            password: 'admin',
            success: function (response, status, xhr, $form) {
                // similate 2s delay
                setTimeout(function () {
                    btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                    showErrorMsg(form, 'danger', 'Incorrect username or password. Please try again.');
                }, 2000);
                $("#mainDiv").show();
                $("#divLogin").hide();
            }
        })

        //form.ajaxSubmit({
        //    url: '',
        //    success: function (response, status, xhr, $form) {
        //        // similate 2s delay
        //        setTimeout(function () {
        //            btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
        //            showErrorMsg(form, 'danger', 'Incorrect username or password. Please try again.');
        //        }, 2000);
        //        $("#mainDiv").show();
        //        $("#divLogin").hide();
        //    }
        //});
    });

    $('#m_login_signup').click(function (e) {
        e.preventDefault();
        displaySignUpForm();
    });
    var displaySignUpForm = function () {
        login.removeClass('m-login--forget-password');
        login.removeClass('m-login--signin');

        login.addClass('m-login--signup');
        login.find('.m-login__signup').animateClass('flipInX animated');
    }
    //var handleSignUpFormSubmit = function () {
        $('#m_login_signup_submit').click(function (e) {
            e.preventDefault();

            var btn = $(this);
            var form = $(this).closest('form');

            form.validate({
                rules: {
                    username: {
                        required: true
                    },
                    password: {
                        required: true
                    },
                    rpassword: {
                        required: true
                    },
                    agree: {
                        required: true
                    }
                }
            });

            if (!form.valid()) {
                return;
            }

            btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);


            $.insmFramework('creatUser', {
                userName: 'test',
                groupID: 1,
                password: 'admin',
                emailAddress: '',
                comments: '',
                settings: '',
                success: function (response, status, xhr, $form) {
                    // similate 2s delay
                    setTimeout(function () {
                        btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                        form.clearForm();
                        form.validate().resetForm();

                        // display signup form
                        displaySignInForm();
                        var signInForm = login.find('.m-login__signin form');
                        signInForm.clearForm();
                        signInForm.validate().resetForm();

                        showErrorMsg(signInForm, 'success', 'Thank you. To complete your registration please check your email.');
                    }, 2000);
                }
            })

            //form.ajaxSubmit({
            //    url: '',
                
            //});
        });
    //}

    var displaySignInForm = function () {
        login.removeClass('m-login--forget-password');
        login.removeClass('m-login--signup');

        login.addClass('m-login--signin');
        login.find('.m-login__signin').animateClass('flipInX animated');
    }
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