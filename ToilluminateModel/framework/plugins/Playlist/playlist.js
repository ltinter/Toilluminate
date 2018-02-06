(function ($) {

    var methods = {
        init: function (options) {
            // Global vars
            var $this = $('body').eq(0);
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
                        retryFlag: false,
                        selectedGroupID: null,
                        div_playlist: $('#div_playlist'),
                        select_palylistItem: null,
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
                _plugin.htmlElements.playelist.playelistGroup.playelistGrouptree.jstree(groupJstreeData);

                _plugin.htmlElements.playelist.playelistGroup.playelistGrouptree.bind("refresh.jstree", function (e, data) {
                    var loginUser = $.insmFramework('user');
                    _plugin.htmlElements.grouptree.jstree(true).select_node(loginUser.GroupID);
                });

                _plugin.htmlElements.playelist.playelistGroup.playelistGrouptree.on("changed.jstree", function (e, data) {
                    //存储当前选中的区域的名称
                    if (data.node) {
                        _plugin.data.selectedGroupID = data.node.id;
                        $.playlist('getPlaylistByGroupID', { selectedGroupID: data.node.id });
                        $.playlistEditor();
                    }
                });

                //add New playlist
                _plugin.htmlElements.playelist.playelistmenu.buttons.addplaylist.click(function () {
                    if (_plugin.data.selectedGroupID == null) {
                        toastr.warning("Please select playlist's group!");
                        return;
                    }
                    if (_plugin.htmlElements.playelist.playelistGroup.playelistGrouptree.jstree(true).get_selected().length == 0) {
                        toastr.warning("Please select playlist's group!");
                        return;
                    }

                    $.playlistEditor('setfolder', { selectedGroupID: _plugin.data.selectedGroupID });
                    $.playlistEditor('playlistDefaultvalue', { editflg:false});
                    _plugin.data.div_playlist.show();
                    _plugin.htmlElements.playelist.container.hide();
                });
                //a-->z
                _plugin.htmlElements.playelist.playelistmenu.buttons.sortascplaylist.click(function () {
                    var playlistDivs = $.makeArray(_plugin.htmlElements.playelist.playlists.playlistsBody.find(".m-portlet.m-portlet--warning.m-portlet--head-sm"));
                    playlistDivs.sort(function (a, b) {
                        var aPlaylistName = $(a).find("h3").text().trim().toLowerCase();
                        var bPlaylistName = $(b).find("h3").text().trim().toLowerCase();
                        if (aPlaylistName == bPlaylistName) return 0;
                        return (aPlaylistName < bPlaylistName) ? 1 : -1;
                    })
                    $.each(playlistDivs, function (playlistIndex, playlistItem) {
                        _plugin.htmlElements.playelist.playlists.playlistsBody.append($(playlistItem));
                    })
                });
                //z-->a
                _plugin.htmlElements.playelist.playelistmenu.buttons.sortdescplaylist.click(function () {
                    var playlistDivs = $.makeArray($("#div_PlaylistEditorContent").find(".m-portlet.m-portlet--warning.m-portlet--head-sm"));
                    playlistDivs.sort(function (a, b) {
                        var aPlaylistName = $(a).find("h3").text().trim().toLowerCase();
                        var bPlaylistName = $(b).find("h3").text().trim().toLowerCase();
                        if (aPlaylistName == bPlaylistName) return 0;
                        return (aPlaylistName > bPlaylistName) ? 1 : -1;
                    })
                    $.each(playlistDivs, function (playlistIndex, playlistItem) {
                        $("#div_PlaylistEditorContent").append($(playlistItem));
                    })
                });
                //1-->9
                _plugin.htmlElements.playelist.playelistmenu.buttons.sortnumericasc.click(function () {
                    var playlistDivs = $.makeArray($("#div_PlaylistEditorContent").find(".m-portlet.m-portlet--warning.m-portlet--head-sm"));
                    playlistDivs.sort(function (a, b) {
                        var aPlaylistName = new Date($(a).find("i.fa.fa-calendar").text().trim().toLowerCase());
                        var bPlaylistName = new Date($(b).find("i.fa.fa-calendar").text().trim().toLowerCase());
                        if (aPlaylistName == bPlaylistName) return 0;
                        return (aPlaylistName < bPlaylistName) ? 1 : -1;
                    })
                    $.each(playlistDivs, function (playlistIndex, playlistItem) {
                        $("#div_PlaylistEditorContent").append($(playlistItem));
                    })
                });
                //9-->1
                _plugin.htmlElements.playelist.playelistmenu.buttons.sortnumericdesc.click(function () {
                    var playlistDivs = $.makeArray($("#div_PlaylistEditorContent").find(".m-portlet.m-portlet--warning.m-portlet--head-sm"));
                    playlistDivs.sort(function (a, b) {
                        var aPlaylistName = new Date($(a).find("i.fa.fa-calendar").text().trim().toLowerCase());
                        var bPlaylistName = new Date($(b).find("i.fa.fa-calendar").text().trim().toLowerCase());
                        if (aPlaylistName == bPlaylistName) return 0;
                        return (aPlaylistName > bPlaylistName) ? 1 : -1;
                    })
                    $.each(playlistDivs, function (playlistIndex, playlistItem) {
                        $("#div_PlaylistEditorContent").append($(playlistItem));
                    })
                });

                _plugin.htmlElements.playelist.playelistGroup.headButtonExpandAll.click(function () {
                    _plugin.htmlElements.playelist.playelistGroup.playelistGrouptree.jstree('open_all');
                });
                _plugin.htmlElements.playelist.playelistGroup.headButtonCollapseAll.click(function () {
                    _plugin.htmlElements.playelist.playelistGroup.playelistGrouptree.jstree('close_all');
                });

                _plugin.htmlElements.playelist.playelistmenu.search.input.keyup(function () {
                    $.each(_plugin.htmlElements.playelist.playlists.playlistsBody.find(".m-portlet.m-portlet--warning.m-portlet--head-sm"), function (index, item) {
                        if ($(item).find("h3").text().trim().toLowerCase().indexOf($("#m_form_search").val().toLowerCase()) > -1) {
                            $(item).show();
                        }
                        else {
                            $(item).hide();
                        }
                    })
                });

                $this.data('playlist', _plugin);
            }

            return $this;
        },
        getPlaylistByGroupID: function (options) {
            var $this = $('body').eq(0);
            var _plugin = $this.data('playlist');
            _plugin.htmlElements.playelist.playlists.playlistsBody.empty();
            //div_PlaylistEditorContent = $this.find('#div_PlaylistEditorContent');

            //div_PlaylistEditorContent
            $.insmFramework('getPlaylistByGroupID', {
                GroupID: options.selectedGroupID,
                success: function (playlistData) {
                    if (playlistData) {
                        $.each(playlistData, function (index, item) {
                            var div_PlaylistEditor = $('<div/>').addClass('m-portlet m-portlet--warning m-portlet--head-sm');
                            var div_head = $('<div/>').addClass('m-portlet__head');
                            var div_head_caption = $('<div/>').addClass('m-portlet__head-caption');
                            var div_head_title = $('<div/>').addClass("m-portlet__head-title");
                            var spantitle = $("<span />").addClass('m-portlet__head-icon');
                            var span_i = '<i class="fa fa-file-text"></i>';
                            var head_text = $('<h3 />').addClass('m-portlet__head-text').text(item.PlayListName);

                            var div_head_tools = $('<div/>').addClass('m-portlet__head-tools');
                            var div_portlet_nav = $('<ul>').addClass("m-portlet__nav");
                            var div_li = $('<li />').addClass('m-portlet__nav-item');
                            var href = $('<a />').addClass("m-portlet__nav-link m-portlet__nav-link--icon");
                            var href_i = $('<i />').addClass("fa fa-calendar");

                            var div_li_list = $('<li />').addClass("m-portlet__nav-item m-dropdown m-dropdown--inline m-dropdown--arrow m-dropdown--align-right m-dropdown--align-push")
                            div_li_list.attr('data-dropdown-toggle', 'hover').attr('aria-expanded', 'true');

                            var div_li_a_toggle = $('<a href="#"/>').addClass('m-portlet__nav-link m-portlet__nav-link--icon m-dropdown__toggle');
                            var div_li_i = $('<i />').addClass('la la-ellipsis-v');
                            var div_m_dropdown_wrapper = $('<div/>').addClass('m-dropdown__wrapper');

                            var wrappe_spantitle = $("<span style='left: auto; right: 18.5px;'/>").addClass('m-dropdown__arrow m-dropdown__arrow--right m-dropdown__arrow--adjust');
                            var div_m_dropdown_inner = $('<div/>').addClass("m-dropdown__inner");
                            var div_m_dropdown_bodyr = $('<div/>').addClass("m-dropdown__body");
                            var div_m_dropdown_content = $('<div/>').addClass("m-dropdown__content");

                            spantitle.append(span_i);
                            div_head_title.append(spantitle);
                            div_head_title.append(head_text);
                            div_head_caption.append(div_head_title);
                            div_head.append(div_head_caption)

                            var ul = $('<ul>').addClass("m-nav");
                            //Item
                            if (item.Settings) {
                                var playlistSetting = JSON.parse(item.Settings);
                                $.each(playlistSetting.PlaylistItems, function (index, PlaylistItem) {
                                    var ul_li = $('<li />').addClass('m-nav__item');
                                    var ul_li_href = $('<a />').addClass("m-nav__link");
                                    var ul_li_a = $('<i />').addClass("m-nav__link-icon flaticon-share");
                                    var ul_li_href_span = $("<span />").addClass('m-nav__link-text');

                                    ul_li_href_span.text(PlaylistItem.PlaylistItemName);
                                    ul_li_href.append(ul_li_a, ul_li_href_span);
                                    ul_li.append(ul_li_href);
                                    ul.append(ul_li);
                                })
                            }

                            div_m_dropdown_content.append(ul);
                            div_m_dropdown_bodyr.append(div_m_dropdown_content);
                            div_m_dropdown_inner.append(div_m_dropdown_bodyr);

                            div_m_dropdown_wrapper.append(wrappe_spantitle);
                            div_m_dropdown_wrapper.append(div_m_dropdown_inner);
                            div_li_a_toggle.append(div_li_i);
                            div_li_list.append(div_li_a_toggle, div_m_dropdown_wrapper)

                            var datetime = new Date(item.UpdateDate).toLocaleDateString()
                            href.append(href_i.text(datetime));
                            div_li.append(href);

                            var div_li_edit = $('<li />').addClass('m-portlet__nav-item');
                            var edit_href = $('<a />').addClass("btn btn-outline-success m-btn m-btn--pill m-btn--wide btn-sm").text($.localize('translate', 'Edit'));

                            edit_href.click(function () {
                                editplaylistID = item.PlayListID;
                                _plugin.data.select_palylistItem = item;
                                _plugin.htmlElements.playelist.container.hide();
                                $.playlistEditor('setfolder', { selectedGroupID: _plugin.data.selectedGroupID });
                                $.playlistEditor('editPlaylist', { playlistID: item.PlayListID, playlistDate: item });
                            });

                            div_li_edit.append(edit_href);
                            div_portlet_nav.append(div_li);
                            div_portlet_nav.append(div_li_edit);

                            div_portlet_nav.append(div_li_list);
                            div_head_tools.append(div_portlet_nav);

                            div_head.append(div_head_tools);
                            div_PlaylistEditor.append(div_head);
                            _plugin.htmlElements.playelist.playlists.playlistsBody.append(div_PlaylistEditor);
                        })
                    }
                }
            })
        },
        getselectGroupId: function () {
            var $this = $('body').eq(0);
            var _plugin = $this.data('playlist');
            
            return _plugin.data.selectedGroupID;
        },
        getselect_palylistItem: function () {
            var $this = $('body').eq(0);
            var _plugin = $this.data('playlist');

            return _plugin.data.select_palylistItem;
        }
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