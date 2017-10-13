/*
* Header
* This file contain the Header function.
*/

(function ($) {
    var methods = {
        init: function (options) {
            var $this = $(this);
            var _plugin = $this.data('insmHeaderTopBar');

            if (!_plugin) {
                _plugin = {
                    settings: $.extend({
                        apiUrl: '',
                        applicationName: 'Header',
                        language: "en",
                        user: null,
                    }, options),
                    htmlElements: {
                        container: $('<div/>').addClass('m-topbar  m-stack m-stack--ver m-stack--general'),
                        wrapper: $('<div/>').addClass('m-stack__item m-topbar__nav-wrapper')
                    },
                    data: {

                    }
                };
                $this.data('insmHeaderTopBar', _plugin);
            }

            //Init html
            _plugin.htmlElements.container.append(
                _plugin.htmlElements.wrapper.append('<ul class="m-topbar__nav m-nav m-nav--inline">'
                    ).append()
                );

            return $this;
        }
    };

    $.fn.insmHeaderTopBar = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on $.insmHeaderTopBar');
        }
    };

})(jQuery);