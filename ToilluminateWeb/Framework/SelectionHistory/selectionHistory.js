/*
* Header
* This file contain the Header function.
*/

(function ($) {
    var methods = {
        init: function (options) {
            var $this = $(this);
            var _plugin = $this.data('insmSelectionHistory');

            if (!_plugin) {
                _plugin = {
                    settings: $.extend({
                        apiUrl: '',
                        applicationName: 'SelectionHistory',
                        language: "en",
                        user: null,
                    }, options),
                    htmlElements: {
                        connect: $('<div/>').addClass('m-header-menu m-menu__submenu m-menu__submenu--classic m-menu__submenu--left'),
                        ul: $('<ul/>').addClass('class="m-menu__nav  m-menu__nav--submenu-arrow "')
                    },
                    data: {

                    }
                };
                $this.data('insmSelectionHistory', _plugin);
            }

            var selectionHistory = [];
            var selectionHistory1 = {};
            selectionHistory1.number = 1;
            selectionHistory1.text = 'selectionHistory1';
            selectionHistory.push(selectionHistory1);

            var selectionHistory2 = {};
            selectionHistory2.number = 2;
            selectionHistory2.text = 'selectionHistory2';
            selectionHistory.push(selectionHistory2);

            var selectionHistory3 = {};
            selectionHistory3.number = 3;
            selectionHistory3.text = 'selectionHistory3';
            selectionHistory.push(selectionHistory3);

            var selectionHistorydata = selectionHistory;
            //Init html
    
    
            $.each(selectionHistorydata, function (index, selectionHistory) {
                var arrowadjust = $("<span />").addClass("m-menu__arrow m-menu__arrow--adjust");
                var ul = $('<ul style="overflow-x: hidden;">').addClass("m-menu__subnav");
                var root = $('<li data-redirect="true" aria-haspopup="true"/>').addClass("m-menu__item");
                var href = $('<a href="header/actions.html" />').addClass("m-menu__link ");
                var linktitle = $("<span />").addClass('m-menu__link-title');
                var linkwrap = $("<span />").addClass('m-menu__link-wrap');
                var linkbadge = $("<span />").addClass("m-menu__link-badge");
                var badgesuccess = $("<span />").addClass("m-badge m-badge--success");
                var linktext = $("<span />").addClass("m-menu__link-text");

                linkbadge.append(badgesuccess.text(selectionHistory.number));
                linktext.text(selectionHistory.text);
                ul.append(root.append(href.append(linktitle.append(linkwrap.append(linkbadge).append(linktext)))));
                _plugin.htmlElements.connect.append(_plugin.htmlElements.ul.append(ul));
            });
            $this.append(_plugin.htmlElements.connect);


            //var systemData = $.insmFramework('login', {
            //    username: 'aaa',
            //    password: 'bbb',
            //    success: function () {
            //        document.write('ok');
            //        // TODO: Fetch everything in an success array and while it until empty.
            //        //while (_plugin.data.successCallbacks.length > 0) {
            //        //    _plugin.data.successCallbacks[0]();
            //        //    _plugin.data.successCallbacks.shift();
            //        //}
            //        //_plugin.htmlElements.usernameInput.val('');
            //        //_plugin.htmlElements.passwordInput.val('');
            //        //_plugin.htmlElements.loginButton.removeAttr('disabled');
            //        //_plugin.htmlElements.usernameInput.removeAttr('disabled');
            //        //_plugin.htmlElements.passwordInput.removeAttr('disabled');
            //        //_plugin.data.loginInProgress = false;
            //    },
            //    denied: function () {
            //        // Global vars
            //        document.write('denied');
            //    }, error: function (message) {
            //        // Global vars
            //        document.write('error');
            //    }
            //});
            return $this;
        }    
    };

    $.fn.insmSelectionHistory = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on $.insmSelectionHistory');
        }
    };

})(jQuery);

