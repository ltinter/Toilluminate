
((function ($) {
    var methods = {
        init: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('localize');

            if (!_plugin) {
                _plugin = {
                    settings: $.extend({
                        language: "jap"
                    }, options)
                };

                if ($.localize.en) {
                    _plugin["en"] = $.localize.en();
                }
                if ($.localize.jap) {
                    _plugin["jap"] = $.localize.jap();
                }
                if ($.localize.jap) {
                    _plugin["chn"] = $.localize.chn();
                }

                $this.data('localize', _plugin);
            }

            return $this;
        },
        translate: function (word, options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('localize');
            if (options == 'Upload File') {
            }
            if (options && options.language) {
                if (_plugin[options.language][word]) {
                    return _plugin[options.language][word];
                }
            }
            else {
                if (_plugin[_plugin.settings.language][word]) {
                    return _plugin[_plugin.settings.language][word];
                }
            }
            return word;
        },
        translatePredefined: function (stringToTrans) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('localize');
            var displayName;

            if (stringToTrans.indexOf("|") > -1) {
                var englishDisplayNameArray = stringToTrans.split("|");
                $.each(englishDisplayNameArray, function (ind, lau) {
                    var nameArray = lau.split(";");
                    if (nameArray[1].replace('"', '') == _plugin.settings.language) {
                        displayName = nameArray[0];
                    }


                });
            }
            if (!displayName) {
                displayName = stringToTrans;
            }
            return displayName;
        }
    };

    $.fn.localize = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on $.localize');
        }
    };

    $.localize = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on $.localize');
        }
    };
})(jQuery));