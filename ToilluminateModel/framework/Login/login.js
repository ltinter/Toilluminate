(function ($) {
    var selectedFileData = {};
    var selectedFolderID = null;
    var actionEnum = { "cut": "cutFile", "copy": "copyFile" };
    var action = "";
    var loginUser = null;
    var usergroupJstreeData = {
        "core": {
            "multiple": false,
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
            var $this = $('body').eq(0);
            var _plugin = $this.data('login');

            if (!_plugin) {
                _plugin = {
                    settings: $.extend({
                        apiUrl: '',
                        version: '',
                        links: {},
                        session: '',
                        timeout: 20000,
                        username: '',
                        user: {}
                    }, options),

                    htmlElements: {
                        container: $('<div />').addClass('m-grid m-grid--hor m-grid--root m-page').attr('id', 'divLogin'),
                        background_image: '',
                        loginbody: $('<div />').addClass('m-grid__item m-grid__item--fluid m-grid m-grid--hor m-login m-login--singin m-login--2 m-login-2--skin-1').css('background-image', 'url(/Framework/assets/app/media/img/bg/bg-1.jpg'),
                        loginmain: $('<div />').addClass('m-grid__item m-grid__item--fluid	m-login__wrapper'),
                        logincontainer: {
                            container: $('<div />').addClass('m-login__container'),
                            signin: {
                                signincontainer:$('<div />').addClass('m-login__signin'),
                                signinhead: $('<div />').addClass('m-login__head'),
                                title: $('<h3 />').addClass('m-login__title').text('Toilluminate').css('font-size', '4.0rem'),
                                titlelable: $('<h3 />').addClass('m-login__title').text('Display Contents Management System').css('font-size', '1.0rem'),
                            },
                            form: {
                                formcontainer: $('<form />').addClass('m-login__form m-form').css('width', '65%').attr('action', ''),
                                groupusername: $('<div />').addClass('form-group m-form__group'),
                                inputusername: $('<input type="text"/>').addClass('form-control m-input').attr('placeholder', 'UserName').attr('name', 'username').attr('autocomplete', 'off'),
                                grouppassword: $('<div />').addClass('form-group m-form__group'),
                                inputpassword: $('<input type="password"/>').addClass('form-control m-input m-login__form-input--last').attr('placeholder', 'Password').attr('name', 'password').attr('autocomplete', 'off'),
                            },
                            form_action: {
                                formcontainer: $('<div />').addClass('m-login__form-action'),
                                button_SignIn: $('<button />').addClass('btn btn-focus m-btn m-btn--pill m-btn--custom m-btn--air  m-login__btn m-login__btn--primary').text('Sign In').attr('id', 'm_login_signin_submit'),
                                appdownload: $('<div />').css('text-align', 'center').css('margin-top', '1.5rem'),
                                href: $('<a href="ToilluminateClient.zip"/>').css("color", '#faffbd').text('ダウンロード: Toilluminate')
                            },
                            signup: {
                                signupcontainer: $('<div />').addClass('m-login__signup'),
                                signuphead: $('<div />').addClass('m-login__head'),
                                title: $('<h3 />').addClass('m-login__title').text('Sign Up').css('font-size', '4.0rem'),
                                lable: $('<div />').addClass('m-login__desc').text('Enter your details to create your account:'),
                            },
                            signupform: {
                                formcontainer: $('<form />').addClass('m-login__form m-form').attr('action', ''),
                                groupusername: $('<div />').addClass('form-group m-form__group'),
                                inputusername: $('<input type="text"/>').addClass('form-control m-input').attr('placeholder', 'UserName').attr('name', 'username').attr('autocomplete', 'off').attr('id', 'new_username'),
                                grouppassword: $('<div />').addClass('form-group m-form__group'),
                                inputpassword: $('<input type="password"/>').addClass('form-control m-input').attr('placeholder', 'Password').attr('name', 'password').attr('autocomplete', 'off').attr('id', 'new_password'),
                                groupconfirmpassword: $('<div />').addClass('form-group m-form__group'),
                                inputconfirmpassword: $('<input type="password"/>').addClass('form-control m-input m-login__form-input--last').attr('placeholder', 'Confirm Password').attr('name', 'rpassword').attr('autocomplete', 'off').attr('id', 'new_confirmpassword'),
                                groupGroupTree: $('<div />').addClass('m-portlet__body').css('height', '250px').css('overflow', 'auto').css('background-color', 'navajowhite').css('margin-top', '1.5rem'),
                                loginGroupTree: $('<div />').addClass('tree-demo groupTree'),
                                form_SignUp: {
                                    container: $('<div />').addClass('m-login__form-action'),
                                    buttonSignUp: $('<button />').addClass('btn m-btn m-btn--pill m-btn--custom m-btn--air m-login__btn m-login__btn--primary').attr('id', 'm_login_signup_submit').text('Sign Up'),
                                    buttonCancel: $('<button />').addClass('btn m-btn m-btn--pill m-btn--custom m-btn--air m-login__btn').attr('id', 'm_login_signup_cancel').text('Cancel'),
                                },
                            },
                            buttonsignUp: {
                                container: $('<div />').addClass('m-login__account').css('display','none'),
                                href: $('<a href="javascript:;"/>').text('Sign Up').attr('id', 'm_login_signup').addClass('m-link m-link--light m-login__account-link')
                            },
                        }
                    }
                };
                $this.
				append(_plugin.htmlElements.container.
                    append(_plugin.htmlElements.loginbody
                        .append(_plugin.htmlElements.loginmain
                            .append(_plugin.htmlElements.logincontainer.container
                                .append(_plugin.htmlElements.logincontainer.signin.signincontainer.append(_plugin.htmlElements.logincontainer.signin.signinhead
                                    .append(
                                    _plugin.htmlElements.logincontainer.signin.title,
                                    _plugin.htmlElements.logincontainer.signin.titlelable
                                 )
                              )
                              .append(_plugin.htmlElements.logincontainer.form.formcontainer
                                .append(_plugin.htmlElements.logincontainer.form.groupusername.append(_plugin.htmlElements.logincontainer.form.inputusername))
                                .append(_plugin.htmlElements.logincontainer.form.grouppassword.append(_plugin.htmlElements.logincontainer.form.inputpassword))
                                .append(_plugin.htmlElements.logincontainer.form_action.formcontainer.append(_plugin.htmlElements.logincontainer.form_action.button_SignIn))
                                .append(_plugin.htmlElements.logincontainer.form_action.appdownload.append(_plugin.htmlElements.logincontainer.form_action.href))
                              )
                            )
                            .append(_plugin.htmlElements.logincontainer.signup.signupcontainer
                                .append(_plugin.htmlElements.logincontainer.signup.signuphead
                                    .append(
                                    _plugin.htmlElements.logincontainer.signup.title,
                                    _plugin.htmlElements.logincontainer.signup.lable
                                 ))
                                 .append(_plugin.htmlElements.logincontainer.signupform.formcontainer
                                .append(_plugin.htmlElements.logincontainer.signupform.groupusername.append(_plugin.htmlElements.logincontainer.signupform.inputusername))
                                .append(_plugin.htmlElements.logincontainer.signupform.grouppassword.append(_plugin.htmlElements.logincontainer.signupform.inputpassword))
                                .append(_plugin.htmlElements.logincontainer.signupform.groupconfirmpassword.append(_plugin.htmlElements.logincontainer.signupform.inputconfirmpassword))
                                .append(_plugin.htmlElements.logincontainer.signupform.groupGroupTree.append(_plugin.htmlElements.logincontainer.signupform.loginGroupTree))
                                .append(_plugin.htmlElements.logincontainer.signupform.form_SignUp.container.append(_plugin.htmlElements.logincontainer.signupform.form_SignUp.buttonSignUp)
                                .append(_plugin.htmlElements.logincontainer.signupform.form_SignUp.buttonCancel))
                              )
                            )

                            .append(_plugin.htmlElements.logincontainer.buttonsignUp.container
                                .append(_plugin.htmlElements.logincontainer.buttonsignUp.href
                                    )

                            )
                          )
                        )
                      )
					)
                ;
                _plugin.htmlElements.logincontainer.form_action.button_SignIn.click(function (e) {
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
                    //btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                    _plugin.htmlElements.logincontainer.form_action.button_SignIn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                    $.insmFramework('userlogin', {
                        userName: _plugin.htmlElements.logincontainer.form.inputusername.val(),
                        password: _plugin.htmlElements.logincontainer.form.inputpassword.val(),
                        success: function (response, status, xhr, $form, data) {
                            $.cookie('userticket', response.Ticket);
                            loginUser = response.UserMaster;
                            // similate 2s delay
                            setTimeout(function () {
                                //btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                                _plugin.htmlElements.logincontainer.form_action.button_SignIn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                                //showErrorMsg(form, 'danger', 'Incorrect username or password. Please try again.');
                            }, 2000);
                            //btn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                            _plugin.htmlElements.logincontainer.form_action.button_SignIn.addClass('m-loader m-loader--right m-loader--light').attr('disabled', true);
                            _plugin.htmlElements.logincontainer.form.inputusername.val('');
                            _plugin.htmlElements.logincontainer.form.inputpassword.val('');
                            $.insmGroup({});
                            $.insmGroup('initGroupTree', { userGroupId: response.UserMaster.GroupID });
                            $("#mainDiv").show();
                            $("#divLogin").hide();
                        },
                        error: function () {
                            toastr.warning("Incorrect username or password. Please try again.");
                            //btn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                            _plugin.htmlElements.logincontainer.form_action.button_SignIn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
                        }
                    })
                });

                _plugin.htmlElements.logincontainer.buttonsignUp.href.click(function (e) {
                    _plugin.htmlElements.logincontainer.signin.signincontainer.hide();
                    _plugin.htmlElements.logincontainer.signup.signupcontainer.show();
                    var tree = _plugin.htmlElements.logincontainer.signupform.loginGroupTree;
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

                _plugin.htmlElements.logincontainer.signupform.form_SignUp.buttonSignUp.click(function (e) {
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
                            }
                            //agree: {
                            //    required: true
                            //}
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
                                toastr.success("Thank you. To complete your registration please check your email.");
                                //showErrorMsg(signInForm, 'success', 'Thank you. To complete your registration please check your email.');
                            }, 2000);
                        }
                    })
                });
            }
            $this.data('login', _plugin);
            if (_plugin.htmlElements.logincontainer.form.inputusername.val() !='') {
                toastr.warning("Incorrect username or password. Please try again.");
                _plugin.htmlElements.logincontainer.form_action.button_SignIn.removeClass('m-loader m-loader--right m-loader--light').attr('disabled', false);
            }
            if (options.checkLogout) {
                $("#mainDiv").hide();
                $("#divLogin").show();
            }
        },
        uploadFile: function () {
            if (!selectedFolderID) {
                return false;
            }
        },
        logout : function () {
            $.insmFramework('userLogout', {
                loginUser: loginUser,
                success: function (response, status, xhr, $form) {
                    $("#mainDiv").hide();
                    $("#divLogin").show();
                },
                error: function () {
                    $("#mainDiv").hide();
                    $("#divLogin").show();
                }
            })
        }
    };
    var login = $('#m_login');

    var displaySignUpForm = function () {
        login.removeClass('m-login--forget-password');
        login.removeClass('m-login--signin');

        login.addClass('m-login--signup');
        login.find('.m-login__signup').animateClass('flipInX animated');
    }
    $('#Logout').click(function (e) {
        $.login('logout');
    });
    
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
            $.error('Method ' + method + ' does not exist on $.login');
        }
        return null;
    };
})(jQuery);