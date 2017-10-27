(function ($) {
    var div_PlaylistEditorContent = $('#div_PlaylistEditorContent');
    var div_PlaylistEditor = $('<div/>').addClass('m-portlet m-portlet--warning m-portlet--head-sm');
    var div_head = $('<div/>').addClass('m-portlet__head');
    var div_head_caption = $('<div/>').addClass('m-portlet__head-caption');
    var div_head_title = $('<div/>').addClass("m-portlet__head-title");
    var spantitle = $("<span />").addClass('m-portlet__head-icon');
    var span_i = '<i class="fa fa-file-text"></i>';
    var head_text = $('<h3 />').addClass('m-portlet__head-text').text('asdad');

    var div_head_tools = $('<div/>').addClass('m-portlet__head-tools');
    var div_portlet_nav = $('<ul>').addClass("m-portlet__nav");
    var div_li = $('<li />').addClass('m-portlet__nav-item');
    var href = $('<a />').addClass("m-portlet__nav-link m-portlet__nav-link--icon");
    var href_i = $('<i />').addClass("fa fa-calendar").text('2017-10-19 13:35');

    var div_li_list = $('<li />').addClass("m-portlet__nav-item m-dropdown m-dropdown--inline m-dropdown--arrow m-dropdown--align-right m-dropdown--align-push");
    div_li_list.attr('data-dropdown-toggle', 'hover').attr('aria-expanded','true');
    var div_li_a_toggle = $('<a href="#"/>').addClass('m-portlet__nav-link m-portlet__nav-link--icon m-dropdown__toggle');
    var div_li_i = $('<i />').addClass('la la-ellipsis-v');
    var div_m_dropdown_wrapper = $('<div/>').addClass('m-dropdown__wrapper');

    var wrappe_spantitle = $("<span style='left: auto; right: 18.5px;'/>").addClass('m-dropdown__arrow m-dropdown__arrow--right m-dropdown__arrow--adjust');
    var div_m_dropdown_inner = $('<div/>').addClass("m-dropdown__inner");
    var div_m_dropdown_bodyr = $('<div/>').addClass("m-dropdown__body");
    var div_m_dropdown_content= $('<div/>').addClass("m-dropdown__content");

    var ul = $('<ul>').addClass("m-nav");
    var ul_li = $('<li />').addClass('m-nav__item');
    var ul_li_href = $('<a />').addClass("m-nav__link");
    var ul_li_a = $('<i />').addClass("m-nav__link-icon flaticon-share");
    var ul_li_href_span = $("<span />").addClass('m-nav__link-text');


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
                $this.data('playlistEditor', _plugin);
            }
            spantitle.append(span_i);
            div_head_title.append(spantitle);
            div_head_title.append(head_text);
            div_head_caption.append(div_head_title);
            div_head.append(div_head_caption)
            

            ul_li_href_span.text('tset111');
            ul_li_href.append(ul_li_a, ul_li_href_span);
            ul_li.append(ul_li_href);
            ul.append(ul_li);
            div_m_dropdown_content.append(ul);
            div_m_dropdown_bodyr.append(div_m_dropdown_content);
            div_m_dropdown_inner.append(div_m_dropdown_bodyr);

            div_m_dropdown_wrapper.append(wrappe_spantitle);
            div_m_dropdown_wrapper.append(div_m_dropdown_inner);
            div_li_a_toggle.append(div_li_i);
            div_li_list.append(div_li_a_toggle, div_m_dropdown_wrapper)

            href.append(href_i);
            div_li.append(href);
            div_portlet_nav.append(div_li);
            div_portlet_nav.append(div_li_list);
            div_head_tools.append(div_portlet_nav);

            div_head.append(div_head_tools);
            div_PlaylistEditorContent.append(div_PlaylistEditor.append(div_head));
            return $this;
        },
        short: function (options) {
        }
    }
    $.playlistEditor = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on $.reports');
        }
        return null;
    };

    $("#playlist_expandAll").click(function () {
        $('#groupTreeForPlaylistEditor').jstree('open_all');
    });
    $("#playlist_collapseAll").click(function () {
        $('#groupTreeForPlaylistEditor').jstree('close_all');
    });
})(jQuery);