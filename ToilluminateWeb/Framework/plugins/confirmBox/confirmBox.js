/*

*/
var rootUI;

(function ($) {
    var methods = {
        init: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('confirmBox');

            if (!_plugin) {
                _plugin = {
                    settings: $.extend({
                        message: "",
                        onOk: function () {
                            return true;
                        },
                        onCancel: function () { return true; }
                    }, options),

                    htmlElements: {
                        container: $('<div />').addClass('insm-confirmbox'),
                        title: $('<div />').addClass('insm-confirmbox-header'),
                        message: $('<p />').addClass('insm-confirmbox-message'),
                        buttonContainer: $('<div />').addClass('insm-confirmbox-button-container'),
                        okButton: $('<button id="confirm-ok" />').addClass('button').text($.insmLocalize('translate', 'OK')),
                        cancelButton: $('<button id="confirm-close" />').addClass('button').text($.insmLocalize('translate', 'Cancel')),
                        backdropView: $('<div id="confirm-backdrop"/>').addClass('insm-lightbox-backdrop')
                    }
                };
            }
            $this.data('confirmBox', _plugin);

            $this.
				append(_plugin.htmlElements.container.
					append(_plugin.htmlElements.title.text(_plugin.settings.title)).
					append(_plugin.htmlElements.message.append(_plugin.settings.message)).
					append(_plugin.htmlElements.buttonContainer.
						append(_plugin.htmlElements.okButton).
						append(_plugin.htmlElements.cancelButton)
					)
				);

            $this
				.append(
					_plugin.htmlElements.backdropView.append(_plugin.htmlElements.backdropView));


            $('.insm-lightbox-backdrop, insm-lightbox-box').animate({ 'opacity': '.5' }, 300, 'linear');
            $('.insm-lightbox-box').animate({ 'opacity': '1' }, 300, 'linear');
            $('.insm-lightbox-backdrop, .insm-lightbox-box').css('display', 'block');

            _plugin.htmlElements.okButton.on("click", function () {
                clearLightBox();
                _plugin.settings.onOk();
            });

            _plugin.htmlElements.cancelButton.on("click", function () {
                clearLightBox();
                _plugin.settings.onCancel();
            });

            function clearLightBox() {
                _plugin.htmlElements.backdropView.detach();
                _plugin.htmlElements.container.detach();
                $this.data('confirmBox', null);
            }
        }
    };

    $.confirmBox = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on $.confirmBox');
            return null;
        }
    };
})(jQuery);
