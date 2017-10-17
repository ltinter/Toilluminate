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
        getGroupTreeData: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var data = {
                success: function (result) {
                    options.success(result);
                },
                format: 'json',
                method: 'get',
                role: "ams",
                key: "grouptreedata",
                denied: function () {
                    // Just do it again and we should land in the success callback next time
                    $.insmFramework('getUsers', options);
                }
            };
            $.insmFramework('ajax', {
                url: options.apiUrl,
                data: data
            });
            return $this;
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

        $.ajax({
            url: 'http://' + '/Login.aspx',
            data: data,
            dataType: 'jsonp',
            timeout: '1000',
            success: function (data) {
                //_plugin.settings.session = data.Session;

                // TODO
                // Switch which should handle OK, Error, Warning, Denied, etc.
                switch (data.Status.toLowerCase()) {
                    case 'ok':
                        if (data.Result) {
                            _plugin.settings.user = {
                                name: data.Result.Username
                            };
                        }
                        else {
                            _plugin.settings.user = {
                                name: data.User
                            };
                        }


                        if (_plugin.data.previousUsername && _plugin.data.previousUsername !== _plugin.settings.user.name) {

                            // Fix the hashtag to only "#" and 
                            $.insmHashChange('updateHash', {});
                            window.location.reload();

                            return;
                        }
                        if (!!window.localStorage && typeof (Storage) !== "undefined") {
                            localStorage.insmFrameworkSession = data.Session;
                        }

                        $.insmFramework('downloadCurrentUser', {
                            success: function (user) {
                                _plugin.settings.user = user;

                                $.insmFramework('regionTree', {
                                    success: function (regionTree) {
                                        _plugin.settings.user.regionTree = regionTree;

                                        if (options && typeof options.success == 'function') {
                                            options.success(data);
                                        }
                                    }
                                });
                            }
                        });
                        break;
                    case 'denied':
                        if (options && typeof options.denied == 'function') {
                            options.denied();
                        }
                        break;
                    default:
                        //throw new Error('Login response status "' + data.Status + '" not implemented.');
                        //$.insmNotification({
                        //    type: "warning",
                        //    message: 'Login response status "' + data.Status + '" not implemented.',
                        //    text: 'Login response status "' + data.Status + '" not implemented.'
                        //});
                        break;
                }
            },
            error: function (message) {
                if (typeof options.error === "function") {
                    options.error();
                }
                //throw new Error(message);
                $.insmNotification({
                    type: "warning",
                    message: message,
                    text: message
                });
            }
        });

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