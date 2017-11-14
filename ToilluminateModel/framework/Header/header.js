/*
* Header
* This file contain the Header function.
*/

(function ($) {
    var methods = {
        init: function (options) {
            var $this = $(this);
            var _plugin = $this.data('insmGenerateHeader');

            if (!_plugin) {
                _plugin = {
                    settings: $.extend({
                        apiUrl: '',
                        applicationName: 'Header',
                        language: "en",
                        user: null,
                    }, options),
                    htmlElements: {
                        connect: $('<div/>').addClass('m-grid__item    m-header '),
                        mcontainer: $('<div/>').addClass('m-container m-container--fluid m-container--full-height'),
                        desktop: $('<div/>').addClass('m-stack m-stack--ver m-stack--desktop'),
                        item: $('<div/>').addClass('m-stack__item m-brand  m-brand--skin-dark '),
                        general: $('<div/>').addClass('m-stack m-stack--ver m-stack--general'),
                        logo: $('<div/>').addClass('m-stack__item m-stack__item--middle m-brand__logo'),
                        tools: $('<div/>').addClass('m-stack__item m-stack__item--middle m-brand__tools')
                    },
                    data: {

                    }
                };
                $this.data('insmGenerateHeader', _plugin);
            }

            //Init html
            _plugin.htmlElements.mcontainer.append(
                _plugin.htmlElements.desktop.append(
                    _plugin.htmlElements.item.append(
                        _plugin.htmlElements.general.append(
                            _plugin.htmlElements.logo.append('<a href="index.html" class="m-link m--font-boldest"> Toilluminate </a>'),
                            //BEGIN: Left Aside Minimize Toggle
                            _plugin.htmlElements.tools.append('<a href="javascript:;" id="m_aside_left_minimize_toggle" class="m-brand__icon m-brand__toggler m-brand__toggler--left m--visible-desktop-inline-block"> <span></span> </a>'),
                            //Responsive Aside Left Menu Toggler
                            _plugin.htmlElements.tools.append('<a href="javascript:;" id="m_aside_left_offcanvas_toggle" class="m-brand__icon m-brand__toggler m-brand__toggler--left m--visible-tablet-and-mobile-inline-block"> <span></span> </a>')
                            )
                        )
                    )
                );

            return $this;
        }
    };

    $.fn.insmGenerateHeader = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on $.insmGenerateHeader');
        }
    };

})(jQuery);