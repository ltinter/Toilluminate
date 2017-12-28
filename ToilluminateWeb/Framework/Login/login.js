login = function (options) {
    
    $.insmFramework('login', {
        username: 'aaa',
        password: 'bbb',
        success: function () {
            document.write('ok');
            // TODO: Fetch everything in an success array and while it until empty.
            //while (_plugin.data.successCallbacks.length > 0) {
            //    _plugin.data.successCallbacks[0]();
            //    _plugin.data.successCallbacks.shift();
            //}
            //_plugin.htmlElements.usernameInput.val('');
            //_plugin.htmlElements.passwordInput.val('');
            //_plugin.htmlElements.loginButton.removeAttr('disabled');
            //_plugin.htmlElements.usernameInput.removeAttr('disabled');
            //_plugin.htmlElements.passwordInput.removeAttr('disabled');
            //_plugin.data.loginInProgress = false;
        },
        denied: function () {
            // Global vars
            document.write('denied');
        }, error: function (message) {
            // Global vars
            document.write('error');
        }
    });

}

close = function () {
    var $this = $('body').eq(0);
    var _plugin = $this.data('insmLogin');
    _plugin.htmlElements.container.detach();
    return $;
}

destroy= function (options) {
    var $this = $('body').eq(0);
    var _plugin = $this.data('insmLogin');

    if (_plugin) {
        $this.data('insmLogin', null);
    }
    // $this.children().detach();
    return $;
}
