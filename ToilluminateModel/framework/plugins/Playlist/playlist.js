(function ($) {

    var methods = {
        init: function (options) {
            // Global vars
            var $this = $('html').eq(0);
            var _plugin = $this.data('playlist');

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
                    htmlElements: {
                        playelist: {
                            container: $('<div />').addClass('row').attr('id', 'div_Mainplaylist').css('overflow-y', 'auto').css('height', '810px'),
                            playelistGroup: {
                                container: $('<div />').addClass('col-xl-3'),
                                playelistGroupbody: $('<div />').addClass('m-portlet m-portlet--success m-portlet--head-solid-bg m-portlet--bordered'),
                                headcontainer: $('<div />').addClass('m-portlet__head'),
                                headcaption: $('<div />').addClass('m-portlet__head-caption'),
                                headtitle: $('<div />').addClass('m-portlet__head-title'),
                                headtitletext: $('<h3 />').addClass('m-portlet__head-text'),

                                headtools: $('<div />').addClass('m-portlet__head-tools'),
                                headtoolstitle: $('<div />').addClass('m-btn-group m-btn-group--pill btn-group mr-2').attr('role', 'group').attr('aria-label', '...'),
                                headButtonCollapseAll: $('<button type="button"/>').addClass('m-btn btn btn-outline-dark btn-secondary').attr('id', 'playlist_collapseAll'),
                                headButtonCollapseAlli: $('<i />').addClass('fa fa-angle-double-up'),
                                headButtonExpandAll: $('<button type="button"/>').addClass('m-btn btn btn-outline-dark btn-secondary').attr('id', 'playlist_expandAll'),
                                headButtonExpandAlli: $('<i />').addClass('fa fa-angle-double-down'),
                                playelistGrouptreeContainer: $('<div />').addClass('m-portlet__body'),
                                playelistGrouptree: $('<div />').addClass('tree-demo  groupTree').attr('id', 'groupTreeForPlaylistEditor'),
                            },
                            playelistmenu: {
                                menu: $('<div />').addClass('col-xl-9'),
                                menucontainer: $('<div />').addClass('m-portlet m-portlet--brand m-portlet--head-solid-bg m-portlet--bordered'),
                                menubody: $('<div />').addClass('m-form m-form--label-align-right m--margin-top-20 m--margin-bottom-30 m--margin-left-50 m--margin-right-50'),
                                menuItems: $('<div />').addClass('row align-items-center'),
                                search:{
                                    menusearch: $('<div />').addClass('col-xl-4 order-2 order-xl-1'),
                                    searchcontainer: $('<div />').addClass('form-group m-form__group row align-items-center'),
                                    searchcol: $('<div />').addClass('col-md-10'),
                                    searchinput: $('<div />').addClass('m-input-icon m-input-icon--left'),
                                    input: $('<input type="text" />').addClass('form-control m-input').attr('id', 'm_form_search'),
                                    span: $('<span />').addClass('m-input-icon__icon m-input-icon__icon--left'),
                                    searchspan: $('<span />'),
                                    searchi: $('<i />').addClass('la la-search'),
                                },
                                buttons: {
                                    buttonscontainer: $('<div />').addClass('col-xl-8 order-1 order-xl-2 m--align-right'),
                                    container: $('<div />').addClass('m-btn-group m-btn-group--pill btn-group mr-2').attr('role', 'group').attr('aria-label', '...'),
                                    addplaylist: $('<button type="button"/>').addClass('m-btn btn btn-outline-dark btn-secondary').attr('id', 'add_playlist'),
                                    addplaylisti: $('<i />').addClass('fa fa-file-text'),

                                    sortascplaylist: $('<button type="button"/>').addClass('m-btn btn btn-outline-dark btn-secondary').attr('id', 'sort-alpha-asc'),
                                    sortascplaylisti: $('<i />').addClass('fa fa-sort-alpha-asc'),

                                    sortdescplaylist: $('<button type="button"/>').addClass('m-btn btn btn-outline-dark btn-secondary').attr('id', 'sort-alpha-desc'),
                                    sortdescplaylisti: $('<i />').addClass('fa fa-sort-alpha-desc'),

                                    sortnumericasc: $('<button type="button"/>').addClass('m-btn btn btn-outline-dark btn-secondary').attr('id', 'sort-numeric-asc'),
                                    sortnumericasci: $('<i />').addClass('fa fa-sort-numeric-asc'),

                                    sortnumericdesc: $('<button type="button"/>').addClass('m-btn btn btn-outline-dark btn-secondary').attr('id', 'sort-numeric-desc'),
                                    sortnumericdesci: $('<i />').addClass('fa fa-sort-numeric-desc'),
                                }
                                
                            },
                            playlists: {
                                container: $('<div />').addClass('m-portlet__body'),
                                playlistsContainer: $('<div />').addClass('row'),
                                playlistsBody: $('<div />').addClass('col-xl-12').attr('id', 'div_PlaylistEditorContent'),
                                //playlists_list: $('<div />').addClass('col-xl-12'),
                            }
                        }
                    },
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
                

                $this.find('#PlaylistEditorContentDiv').append(_plugin.htmlElements.playelist.container
                    .append(_plugin.htmlElements.playelist.playelistGroup.container
                        .append(_plugin.htmlElements.playelist.playelistGroup.playelistGroupbody
                            .append(_plugin.htmlElements.playelist.playelistGroup.headcontainer
                                .append(_plugin.htmlElements.playelist.playelistGroup.headcaption
                                    .append(_plugin.htmlElements.playelist.playelistGroup.headtitle
                                        .append(_plugin.htmlElements.playelist.playelistGroup.headtitletext.append($.localize('translate', 'Groups'))
                                        )
                                    )
                                )
                                .append(_plugin.htmlElements.playelist.playelistGroup.headtools
                                    .append(_plugin.htmlElements.playelist.playelistGroup.headtoolstitle
                                        .append(_plugin.htmlElements.playelist.playelistGroup.headButtonCollapseAll
                                            .append(_plugin.htmlElements.playelist.playelistGroup.headButtonCollapseAlli)
                                        )
                                        .append(_plugin.htmlElements.playelist.playelistGroup.headButtonExpandAll
                                            .append(_plugin.htmlElements.playelist.playelistGroup.headButtonExpandAlli)
                                        )
                                    )
                                )
                                
                            )
                            .append(_plugin.htmlElements.playelist.playelistGroup.playelistGrouptreeContainer
                                    .append(_plugin.htmlElements.playelist.playelistGroup.playelistGrouptree)
                                )
                        )
                    )

                    .append(_plugin.htmlElements.playelist.playelistmenu.menu.
                        append(_plugin.htmlElements.playelist.playelistmenu.menucontainer.
                            append(_plugin.htmlElements.playelist.playelistmenu.menubody.
                                append(_plugin.htmlElements.playelist.playelistmenu.menuItems.
                                    append(_plugin.htmlElements.playelist.playelistmenu.search.menusearch.
                                        append(_plugin.htmlElements.playelist.playelistmenu.search.searchcontainer.
                                            append(_plugin.htmlElements.playelist.playelistmenu.search.searchcol.
                                                append(_plugin.htmlElements.playelist.playelistmenu.search.searchinput.
                                                    append(_plugin.htmlElements.playelist.playelistmenu.search.input)
                                                    .append(_plugin.htmlElements.playelist.playelistmenu.search.span.
                                                        append(_plugin.htmlElements.playelist.playelistmenu.search.searchspan.
                                                            append(_plugin.htmlElements.playelist.playelistmenu.search.searchi))))))
                                    )
                                    .append(_plugin.htmlElements.playelist.playelistmenu.buttons.buttonscontainer
                                        .append(_plugin.htmlElements.playelist.playelistmenu.buttons.container
                                            .append(_plugin.htmlElements.playelist.playelistmenu.buttons.addplaylist
                                                .append(_plugin.htmlElements.playelist.playelistmenu.buttons.addplaylisti)
                                            )
                                            .append(_plugin.htmlElements.playelist.playelistmenu.buttons.sortascplaylist
                                                .append(_plugin.htmlElements.playelist.playelistmenu.buttons.sortascplaylisti)
                                            )
                                            .append(_plugin.htmlElements.playelist.playelistmenu.buttons.sortdescplaylist
                                                .append(_plugin.htmlElements.playelist.playelistmenu.buttons.sortdescplaylisti)
                                            )
                                            .append(_plugin.htmlElements.playelist.playelistmenu.buttons.sortnumericasc
                                                .append(_plugin.htmlElements.playelist.playelistmenu.buttons.sortnumericasci)
                                            )
                                            .append(_plugin.htmlElements.playelist.playelistmenu.buttons.sortnumericdesc
                                                .append(_plugin.htmlElements.playelist.playelistmenu.buttons.sortnumericdesci)
                                            )
                                        )
                                    )
                                )
                            )
                            .append(_plugin.htmlElements.playelist.playlists.container
                                .append(_plugin.htmlElements.playelist.playlists.playlistsContainer
                                    .append(_plugin.htmlElements.playelist.playlists.playlistsBody)
                                )
                            )
                        )
                    )
                )
                var loginuser = $.insmFramework('user');;
                var groupJstreeData = {
                    "core": {
                        "multiple": false,
                        "themes": {
                            "responsive": true
                        },
                        // so that create works
                        "check_callback": true,
                        'data': {
                            url: 'api/GroupMasters/GetGroupJSTreeDataWithChildByGroupID/' + loginuser.GroupID,
                            //+ options.userGroupId,
                            dataFilter: function (data) {
                                _plugin.data.temp_GroupTreeData = JSON.parse(data);
                                return data;
                            }
                        }
                    },
                    "types": {
                        "default": {
                            "icon": "fa fa-sitemap m--font-success"
                        },
                        "file": {
                            "icon": "fa fa-sitemap m--font-success"
                        }
                    },
                    "state": {
                        "key": "demo2"
                    },
                    "plugins": ["dnd", "types"]

                };

                //var tree = $('.tree-demo.groupTree');
                //tree.jstree(groupJstreeData);
                _plugin.htmlElements.playelist.playelistGroup.playelistGrouptree.jstree(groupJstreeData);

                _plugin.htmlElements.playelist.playelistGroup.playelistGrouptree.bind("refresh.jstree", function (e, data) {
                    var loginUser = $.insmFramework('user');
                    _plugin.htmlElements.grouptree.jstree(true).select_node(loginUser.GroupID);
                });

                _plugin.htmlElements.playelist.playelistGroup.playelistGrouptree.on("changed.jstree", function (e, data) {
                    //存储当前选中的区域的名称
                    if (data.node) {
                        $.playlistEditor('init', {
                            selectedGroupID: data.node.id
                        });
                    }
                });

                $this.data('playlist', _plugin);
            }

            return $this;
        },
}


    $.playlist = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on $.Playlist');
            return null;
        }
    };
})(jQuery);