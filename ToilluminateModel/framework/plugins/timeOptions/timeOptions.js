/*

*/
var rootUI;

(function ($) {
    var methods = {
        init: function (options) {
            var $this = $(this);
            var _plugin = $this.data('timeOptions');

            if (!_plugin) {
                _plugin = {
                    settings: $.extend({
                        message: "",
                        buttonText:'',
                        onOk: function () {
                            return true;
                        },
                        onCancel: function () { return true; }
                    }, options),

                    htmlElements: {
                        container: $('<div />').addClass('form-group m-form__group row'),
                        Button: $('<div />').addClass('btn-group col-form-label col-lg-2 col-sm-12').attr('data-toggle', 'buttons'),
                        label: $('<label />').addClass('btn m-btn--pill m-btn--air  btn-success active').attr('id', 'label_player_monday_value'),
                        input: $('<input type="checkbox" />').attr('checked', '').attr('autocomplete', 'off').attr('id', 'player_monday'),
                        hour: $('<div />').addClass('btn-group col-form-label col-lg-2 col-sm-12').attr('data-toggle', 'buttons'),
                        col: $('<div />').addClass('col-lg-8 col-md-8 col-sm-10'),
                        slider: $('<div />').addClass('m-ion-range-slider'),
                        sliderinput: $('<input type="hidden" />').addClass('irs-hidden-input player-editor-timeoptions-hidden-input').attr('tabindex', '-1').attr('readonly', '').attr('value', '0;24').attr('id', 'palyer_monday_value')
                    }
                };
            }

            $this.
				append(_plugin.htmlElements.container.
					append(_plugin.htmlElements.Button.
                        append(_plugin.htmlElements.label.
                            append(_plugin.htmlElements.input)
                            .append(_plugin.settings.buttonText)
                        )
                    )
                    .append(_plugin.htmlElements.hour)
                    .append(_plugin.htmlElements.col.
                        append(_plugin.htmlElements.slider.
                            append(_plugin.htmlElements.sliderinput)
                        )
                    )
				);

            $this.data('timeOptions', _plugin);
            return $this;
        }
    };

    $.fn.timeOptions = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on $.timeOptions');
            return null;
        }
    };
})(jQuery);
