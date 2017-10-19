(function ($) {
    var _guid = 0;
    var methods = {
        init: function (options) {
            // Global vars
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');

            // If the plugin hasn't been initialized yet
            if (!_plugin) {
                _plugin = {
                    settings: $.extend({
                        apiUrl: '',
                        applicationName: '',
                        version: '',
                        links: {},
                        session: '',
                        timeout: 20000,
                        username: '',
                        user: {}
                    }, options),
                    cache: {
                        players: {}
                    },
                    locks: {
                        getPlayers: {
                            deferred: null,
                            callbackArray: []
                        }
                    },
                    data: {
                        type: '',
                        target: '',
                        version: '',
                        versionId: 0,
                        initialized: new $.Deferred(),
                        loginFlag: false,
                        loginDeferred: new $.Deferred(),
                        retryFlag: false
                    }
                };
                $this.data('insmFramework', _plugin);
            }

            if (!_plugin.settings.apiUrl || !_plugin.settings.applicationName || !_plugin.settings.version) {
                //throw new Error('INSM Framework not initialized correctly');
                $.insmNotification({
                    type: "warning",
                    message: 'INSM Framework not initialized correctly',
                    text: 'INSM Framework not initialized correctly'
                });
            }

            if (!_plugin.settings.apiUrl.indexOf('http://') == 0 && !_plugin.settings.apiUrl.indexOf('https://') == 0) {
                //throw new Error('Invalid configuration. API URL has to start with "http://" or "https://".');
                $.insmNotification({
                    type: "warning",
                    message: 'Invalid configuration. API URL has to start with "http://" or "https://".',
                    text: 'Invalid configuration. API URL has to start with "http://" or "https://".'
                });
            }

            _plugin.settings.apiUrl = _plugin.settings.apiUrl.replace(/\/+$/, "");
            if (_plugin.settings.session == "null") {
                _plugin.settings.session = null;
            }
            if (!!window.localStorage && typeof (Storage) !== "undefined" && !_plugin.settings.session) {
                _plugin.settings.session = localStorage.insmFrameworkSession;
                if (!_plugin.settings.session) {
                    _plugin.settings.session = '';
                }
            }


            $.insmFramework('downloadCurrentUser', {
                success: function (user) {
                    _plugin.settings.user = user;

                    $.insmFramework('regionTree', {
                        success: function (regionTree) {
                            _plugin.settings.user.regionTree = regionTree;
                            _plugin.data.initialized.resolve();
                        }
                    });
                }
            });
            return $this;
        },
        loggedIn: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');

            if ($.isEmptyObject(_plugin.settings.user)) {
                return false;
            }
            else {
                return true;
            }
        },
        getUserGroup: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var data = {
                success: function (result) {
                    options.success(result);
                },
                format: 'json',
                method: 'get',
                role: "ams",
                key: "userGroup",
                denied: function () {
                    // Just do it again and we should land in the success callback next time
                    $.insmFramework('getUsers', options);
                }
            };
            $.insmFramework('ajax', {
                url: _plugin.settings.apiUrl + '/AppSettings.aspx',
                data: data
            });
            return $this;
        },
        ajax: function (options) {
            // Global vars
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');

            var restartOptions = $.extend(true, {}, options);
            //if (_plugin.settings.password) {
            //    $.extend(options.data, {
            //        username: _plugin.settings.username,
            //        password: _plugin.settings.password,
            //        format: 'json'
            //    });
            //} else {
            //    $.extend(options.data, {
            //        session: _plugin.settings.session,
            //        format: 'json'
            //    });
            //}
            //if (options.data.session == null || options.data.session == 'null' || options.data.session == '') {
            //    delete options.data.session;
            //}

            var callbacks = {
                success: options.data.success,
                denied: options.data.denied,
                invalid: options.data.invalid,
                warning: options.data.warning,
                unauthorized: options.data.unauthorized,
                error: options.data.error
            };

            if (!callbacks.success || !callbacks.denied) {
                //throw new Error('Required callbacks not defined');
                $.insmNotification({
                    type: "warning",
                    message: 'Required callbacks not defined',
                    text: 'Required callbacks not defined'
                });
            }

            // Retry callback
            callbacks.retry = function (message) {
                if (!_plugin.data.retryFlag) {
                    _plugin.data.retryFlag = true;
                    //$.insmNotification({
                    //	type: 'warning',
                    //	message: 'Communication with API was timed out: ' + message
                    //});
                    setTimeout(function () {
                        _plugin.data.retryFlag = false;
                    }, 5000);
                }
                setTimeout(function () {
                    $.insmFramework('ajax', restartOptions);
                }, 1000);
            };

            delete options.data.success;
            delete options.data.denied;
            delete options.data.invalid;
            delete options.data.warning;
            delete options.data.unauthorized;
            delete options.data.error;

            var urlLength = JSON.stringify(options.data).length + options.url.length;
            //if (urlLength > 1000) {
            //    var trackingId = new Date().getTime();
            //    $.extend(options.data, {
            //        trackingId: trackingId
            //    });
            //    //the server will support cross domain ajax call in the future

            //    var iframe = $('<iframe name="guid' + _guid + '" ></iframe>').css({
            //        display: 'none'
            //    }).appendTo('body');

            //    // Removed because it did not work in IE8. Not sure if it was needed for something else.
            //    /*.append(
			//	$('<html />').append(
			//		$('<head />').append('<meta http-equiv="X-UA-Compatible" content="IE=9">')
			//	)
			//);*/

            //    var form = $(document.createElement('form')).css({
            //        display: 'none'
            //    }).appendTo('body');

            //    // TODO: Add track ID.
            //    form.attr("action", options.url);
            //    form.attr("method", "POST");
            //    form.attr("enctype", "multipart/form-data");
            //    form.attr("encoding", "application/x-www-form-urlencoded");
            //    form.attr("target", "guid" + _guid++);
            //    $.each(options.data, function (key, value) {
            //        form.append($('<input name="' + key + '" />').val(value));
            //    });

            //    form.submit();
            //    form.remove();

            //    return $.insmFramework('track', {
            //        trackId: trackingId,
            //        data: callbacks,
            //        iframe: iframe
            //    });
            //}
            //else {
                var timedOut = false;
                var timeoutHandle = setTimeout(function () {
                    timedOut = true;
                    callbacks.retry('The request took too long. Please check your connection.');
                }, 300000);

                return $.ajax({
                    type: 'GET',
                    dataType: 'jsonp',
                    url: options.url,
                    data: options.data,
                    success: function (data) {
                        _plugin.data.timestamp = data.Timestamp;
                        if (!timedOut) {
                            clearTimeout(timeoutHandle);
                            $.insmFramework('callback', {
                                result: data,
                                params: callbacks,
                                url: options.url
                            });
                        }
                    },
                    error: function (data) {
                        if (!timedOut) {
                            clearTimeout(timeoutHandle);
                            $.insmFramework('callback', {
                                result: data,
                                params: callbacks,
                                url: options.url
                            });
                        }
                    }
                });
            //}
        },
        ajax: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var restartOptions = $.extend(true, {}, options);

            var callbacks = {
                success: options.data.success,
                denied: options.data.denied,
                invalid: options.data.invalid,
                warning: options.data.warning,
                unauthorized: options.data.unauthorized,
                error: options.data.error
            };

            if (!callbacks.success || !callbacks.denied) {
                //throw new Error('Required callbacks not defined');
                $.insmNotification({
                    type: "warning",
                    message: 'Required callbacks not defined',
                    text: 'Required callbacks not defined'
                });
            }

            var timedOut = false;
            var timeoutHandle = setTimeout(function () {
                timedOut = true;
                callbacks.retry('The request took too long. Please check your connection.');
            }, 300000);

            return $.ajax({
                type: 'GET',
                //dataType: 'jsonp',
                url: options.url,
                contentType: "application/json; charset=utf-8",
                data: options.data,
                success: function (data) {
                    //_plugin.data.timestamp = data.Timestamp;
                    if (!timedOut) {
                        clearTimeout(timeoutHandle);
                        $.insmFramework('callback', {
                            result: data,
                            params: callbacks,
                            url: options.url
                        });
                    }
                },
                error: function (data) {
                    //if (!timedOut) {
                    //    clearTimeout(timeoutHandle);
                    //    $.insmFramework('callback', {
                    //        result: data,
                    //        params: callbacks,
                    //        url: options.url
                    //    });
                    //}
                }
            });
            //$.ajax({
            //    url: options.url,
            //    type: "GET",
            //    contentType:"application/json; charset=utf-8",
            //    success: function (data) {
            //        return data;
            //    },
            //    error: function (XMLHttpRequest, textStatus, errorThrown) {
            //        alert("请求失败，消息：" + textStatus + "  " + errorThrown);
            //    }
            //});
        },
        callback: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');

            var requiredCallbacks = ['success', 'denied'];
            $.each(requiredCallbacks, function (index, callback) {
                if (typeof options.params[callback] != 'function') {
                    //throw new Error('No ' + callback + ' callback defined in INSM framework');
                    $.insmNotification({
                        type: "warning",
                        message: 'No ' + callback + ' callback defined in INSM framework',
                        text: 'No ' + callback + ' callback defined in INSM framework'
                    });
                }
            });

            //if (!_plugin.data.target) {
            //    _plugin.data.type = options.result.Type;
            //    _plugin.data.target = options.result.Target;
            //    _plugin.data.version = options.result.Version;
            //    _plugin.data.database = options.result.Database;
            //}

            //if (!_plugin.settings.session || _plugin.settings.session == 'null' || _plugin.settings.session == '') {
            //    _plugin.settings.session = options.result.Session;
            //}


            if (options.result) {
                if (options.result.VersionId) {
                    //_plugin.data.versionId = options.result.VersionId;
                }
                options.params.success(options.result);
                //if (typeof options.result.StatusCode === 'number' && options.result.StatusCode % 1 == 0) {
                //    switch (options.result.StatusCode) {
                //        case 0: // Status OK
                //            options.params.success(options.result.Result);
                //            break;
                //        case 50: // Status truncated
                //            options.params.success(options.result.Result);
                //            $.insmNotification({
                //                type: 'warning',
                //                message: options.result.Message
                //            });
                //            break;
                //        case 100: // Status timeout
                //        case 101: // Status deadlock
                //        case 102: // Status communication failure
                //        case 103: // Status overload
                //            options.params.retry(options.result.Message);
                //            break;
                //        case 202:// Status unsufficient access
                //            if (typeof options.params.unauthorized === 'function') {
                //                options.params.unauthorized(options.result.Message);
                //            } else if (typeof options.params.invalid === 'function') {
                //                options.params.invalid(options.result.Message);
                //            } else {
                //                //throw new Error('Unauthorized callback missing. Message: "' + options.result.Message + '"'+" url: " + options.url);
                //                $.insmNotification({
                //                    type: "warning",
                //                    message: 'Unauthorized callback missing. Message: "' + options.result.Message + '"' + " url: " + options.url,
                //                    text: 'Unauthorized callback missing. Message: "' + options.result.Message + '"' + " url: " + options.url
                //                });
                //            }
                //            break;
                //        case 300: // Status invalid arguments
                //        case 400: // Status content error
                //            if (typeof options.params.invalid === 'function') {
                //                options.params.invalid(options.result.Message);
                //            }
                //            else {
                //                //throw new Error('Invalid callback missing. Message: "' + options.result.Message + '"');
                //                $.insmNotification({
                //                    type: "warning",
                //                    message: 'Invalid callback missing. Message: "' + options.result.Message + '"',
                //                    text: 'Invalid callback missing. Message: "' + options.result.Message + '"'
                //                });
                //            }
                //            break;
                //        case 500: // Status system failure
                //            if (typeof options.params.error === 'function') {
                //                options.params.error(options.result.Message);
                //            } else {
                //                //throw new Error(options.result.Message+" url: "+options.url);
                //                $.insmNotification({
                //                    type: "warning",
                //                    message: options.result.Message + " url: " + options.url,
                //                    text: options.result.Message + " url: " + options.url
                //                });
                //            }
                //            break;
                //        case 201: // Status unauthorized (has a windows user but has no login access to INSM)
                //            _plugin.data.previousUsername = _plugin.settings.user.name;
                //        case 200: // Status not logged in
                //            // Login and call the denied callback afterwards
                //            _plugin.data.type = options.result.Type;
                //            _plugin.data.target = options.result.Target;
                //            _plugin.data.version = options.result.Version;
                //            _plugin.data.versionId = options.result.VersionId;
                //            _plugin.data.previousUsername = _plugin.settings.user.name;
                //            $.insmLogin({
                //                success: function () {
                //                    options.params.denied();
                //                }
                //            });
                //            break;
                //        default:
                //            //throw new Error('Status code "' + options.result.StatusCode + '" (' + options.result.Status + ') callback is not implemented');
                //            $.insmNotification({
                //                type: "warning",
                //                message: 'Status code "' + options.result.StatusCode + '" (' + options.result.Status + ') callback is not implemented',
                //                text: 'Status code "' + options.result.StatusCode + '" (' + options.result.Status + ') callback is not implemented'
                //            });
                //    }
                //} else {

                //    if (typeof options.result != "undefined" && options.result.status == 404) {
                //        if (typeof options.params.invalid === 'function') {
                //            options.params.invalid(options.result.Message);
                //        }
                //    } else {
                //        $.insmNotification({
                //            type: 'warning',
                //            message: $.insmLocalize('translate', "Please check your internet connection")
                //        });
                //    }
                //    //throw new Error('Status code "' + options.result.StatusCode + '" not recognised');
                //}
            }
            else {
                //throw new Error('Result not set in API result' + 'url: ' + options.url);
                $.insmNotification({
                    type: "warning",
                    message: 'Result not set in API result' + 'url: ' + options.url,
                    text: 'Result not set in API result' + 'url: ' + options.url
                });
            }
        },
        getGroupTreeData: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var data = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/GroupMasters',
                format: 'json',
                contentType: "application/json; charset=utf-8",
                type: "GET",
                denied: function () {
                    // Just do it again and we should land in the success callback next time
                    //$.insmFramework('getUsers', options);
                }
            };
            return $.insmFramework('ajax', {
                url: data.url,
                data: data
            });
        },
        getUser: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');

            return _plugin.settings.user;
        },
        login: function (options) {
            // Global vars
        //var $this = $('html').eq(0);
        //var _plugin = $this.data('insmFramework');

        var data = {
            username: options.username,
            password: options.password
            //,
            //app: _plugin.settings.app
        };

        //if (_plugin.settings.session) {
        //    data.session = _plugin.settings.session;
        //}

        //if (data.session == null || data.session == 'null' || data.session == '') {
        //    delete data.session;
        //}

        //$.ajax({
        //    url: 'http://' + '/Login.aspx',
        //    data: data,
        //    dataType: 'jsonp',
        //    timeout: '1000',
        //    success: function (data) {
        //        //_plugin.settings.session = data.Session;

        //        // TODO
        //        // Switch which should handle OK, Error, Warning, Denied, etc.
        //        switch (data.Status.toLowerCase()) {
        //            case 'ok':
        //                if (data.Result) {
        //                    _plugin.settings.user = {
        //                        name: data.Result.Username
        //                    };
        //                }
        //                else {
        //                    _plugin.settings.user = {
        //                        name: data.User
        //                    };
        //                }


        //                if (_plugin.data.previousUsername && _plugin.data.previousUsername !== _plugin.settings.user.name) {

        //                    // Fix the hashtag to only "#" and 
        //                    $.insmHashChange('updateHash', {});
        //                    window.location.reload();

        //                    return;
        //                }
        //                if (!!window.localStorage && typeof (Storage) !== "undefined") {
        //                    localStorage.insmFrameworkSession = data.Session;
        //                }

        //                $.insmFramework('downloadCurrentUser', {
        //                    success: function (user) {
        //                        _plugin.settings.user = user;

        //                        $.insmFramework('regionTree', {
        //                            success: function (regionTree) {
        //                                _plugin.settings.user.regionTree = regionTree;

        //                                if (options && typeof options.success == 'function') {
        //                                    options.success(data);
        //                                }
        //                            }
        //                        });
        //                    }
        //                });
        //                break;
        //            case 'denied':
        //                if (options && typeof options.denied == 'function') {
        //                    options.denied();
        //                }
        //                break;
        //            default:
        //                //throw new Error('Login response status "' + data.Status + '" not implemented.');
        //                //$.insmNotification({
        //                //    type: "warning",
        //                //    message: 'Login response status "' + data.Status + '" not implemented.',
        //                //    text: 'Login response status "' + data.Status + '" not implemented.'
        //                //});
        //                break;
        //        }
        //    },
        //    error: function (message) {
        //        if (typeof options.error === "function") {
        //            options.error();
        //        }
        //        //throw new Error(message);
        //        $.insmNotification({
        //            type: "warning",
        //            message: message,
        //            text: message
        //        });
        //    }
        //});

        return _plugin.data.loginDeferred;
    },
        logout: function (options) {
        // Global vars
        var $this = $('html').eq(0);
        var _plugin = $this.data('insmFramework');
        _plugin.data.previousUsername = _plugin.settings.user.name;
        var logoutDeferred = $.insmFramework('ajax', {
            url: _plugin.settings.apiUrl + '/Logout.aspx',
            data: {
                success: function () {
                    // We never get here but if we do in the future we want to do the same as in the denied callback.
                    _plugin.data.user = {};
                    delete _plugin.settings.session;
                    if (!!window.localStorage && typeof (Storage) !== "undefined") {
                        delete localStorage.insmFrameworkSession;
                    }
                },
                denied: function () {
                    _plugin.data.user = {};
                    delete _plugin.settings.session;
                    if (!!window.localStorage && typeof (Storage) !== "undefined") {
                        delete localStorage.insmFrameworkSession;
                    }
                }
            }
        });

        return logoutDeferred;
    },
    }
    $.insmFramework = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on $.insmFramework');
        }
        return null;
    };
    })(jQuery);