(function ($) {
    var selectedFileData = {};
    var selectedFolderID = null;
    var actionEnum = { "cut": "cutFile", "copy": "copyFile" };
    var action = "";
    var usergroupJstreeData = {
        "core": {
            "themes": {
                "responsive": true
            },
            // so that create works
            "check_callback": true,
            'data': {
                url: 'api/GroupMasters/GetGroupJSTreeDataWithChildByGroupID/1',
                //+ options.userGroupId,
                dataFilter: function (data) {
                    temp_GroupTreeData = JSON.parse(data);
                    return data;
                }
            }
        },
        "types": {
            "default": {
                "icon": "fa fa-sitemap m--font-success"
            },
            "file": {
                "icon": "fa fa-sitemap m--font-success"
            }
        },
        "state": {
            "key": "demo2"
        },
        "plugins": ["dnd", "types"]

    };
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
            userName: $("#login_username").val(),
            password: $("#login_password").val(),
            success: function (response, status, xhr, $form,data) {
                // similate 2s delay
                setTimeout(function () {
                    btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                    //showErrorMsg(form, 'danger', 'Incorrect username or password. Please try again.');
                }, 2000);
                $.insmGroup('initGroupTree', { userGroupId: response.GroupID });
                btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                $("#login_username").val('');
                $("#login_password").val('');
                $("#mainDiv").show();
                $("#divLogin").hide();
            },
            error: function () {
                toastr.warning("Incorrect username or password. Please try again.");
                btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
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
        var tree = $('#newUserGroup');
        tree.jstree(usergroupJstreeData);

        tree.on('loaded.jstree', function (e, data) {
            var inst = data.instance;
            var obj = inst.get_node(e.target.firstChild.firstChild.lastChild);
            inst.select_node(obj);
            tree.jstree('close_all');
        })
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

            if ($("#new_password").val() != $("#new_confirmpassword").val()) {
                toastr.warning("password is error.");
                return;
            }
            $.insmFramework('creatUser', {
                userName: $("#new_username").val(),
                groupID: $('#newUserGroup').jstree(true).get_selected()[0],
                password: $("#new_password").val(),
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
                        toastr.warning("Thank you. To complete your registration please check your email.");
                        //showErrorMsg(signInForm, 'success', 'Thank you. To complete your registration please check your email.');
                    }, 2000);
                }
            })
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