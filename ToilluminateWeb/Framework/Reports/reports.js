(function ($) {
    var _guid = 0;
    var methods = {
        init: function (options) {
            // Global vars
            var $this = $('html').eq(0);
            var _plugin = $this.data('reports');

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
                $this.data('reports', _plugin);
            }


            return $this;
        },
        getsuccess: function (options) {
            var a = '';
        },
        
    }
    $.reports = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on $.reports');
        }
        return null;
    };
})(jQuery);