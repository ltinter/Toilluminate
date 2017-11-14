(function ($) {
    var _guid = 0;
    var GroupTreedata = [];
    
    var GroupData;
    var selectPlayerdata;
    var playListgroup=[];
    var editGroupID;
    var selectedGroupID = null;
    var groupTreeForPlayerEditID = null;
    var div_main = $("#div_main");
    var div_edit = $("#div_edit");
    var div_groupTree = $("#groupTree");
    var div_groupTreeForPlayerEdit = $("#groupTreeForPlayerEdit");
    var div_groupTreeForFileManager = $("#groupTreeForFileManager");
    var div_groupTreeForPlaylistEditor = $("#groupTreeForPlaylistEditor");
    var player_Alldata;

    var ActivechangeFlg = false;
    var OnlinechangeFlg = false;
    var DisplayNamechangeFlg = false;
    var NotechangeFlg = false;

    var editPlayerFlg = false;
    var editGroupFlg = false;
    var datatable = null;

    
    var groupJstreeData = {
        "core": {
            "themes": {
                "responsive": true
            },
            // so that create works
            "check_callback": true,
            'data': GroupTreedata
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
        "plugins": ["dnd", "state", "types"]

    };
    var methods = {
        init: function (options) {
            // Global vars
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmGroup');

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
                $this.data('insmGroup', _plugin);
            }
            $.insmGroup('initGroupTree');
            div_edit.hide();
            $.insmGroup('defaultDataSet');
            $("#PlayerDetail").css('display', 'none');

            $("#button_save").text($.localize('translate', "Save"));
            return $this;
        },
        editgroup: function (options) {
            $.insmFramework('creatGroup', {
                groupID: options.groupID,
                newGroupName: options.newGroupName,
                active: options.ActiveFlag,
                onlineUnits: options.OnlineFlag,
                note: options.Comments,
                newGroupNameParentID: options.newGroupNameParentID,
                success: function (data) {
                    $.insmGroup('initGroupTree');
                    editGroupID = undefined;
                }
            })
        },
        initGroupTree: function () {
            $.insmFramework('getGroupTreeData', {
                success: function (tempdataGroupTreeData) {
                    if (tempdataGroupTreeData) {

                        var tree = $('.tree-demo.groupTree');
                        tree.jstree(groupJstreeData);
                        $.each(tree, function (key, item) {
                            $(item).jstree(true).settings.core.data = tempdataGroupTreeData;
                            $(item).jstree(true).refresh();
                        });

                        div_groupTree.on("changed.jstree", function (e, data) {
                            //存储当前选中的区域的名称
                            if (data.node) {
                                selectedGroupID = data.node.id;
                                $.insmGroup('showPlayerDetail', { GroupID: selectedGroupID });
                            }
                            $("#radio_All").click();
                        });

                        $(div_groupTreeForPlayerEdit).on("changed.jstree", function (e, data) {
                            //存储当前选中的区域的名称
                            if (data.node) {
                                groupTreeForPlayerEditID = data.node.id;
                            }
                        });

                        $(div_groupTreeForFileManager).on("changed.jstree", function (e, data) {
                            //存储当前选中的区域的名称
                            if (data.node) {
                                $.folder('init', {
                                    selectedGroupID: data.node.id
                                });
                            }
                        });

                        $(div_groupTreeForPlaylistEditor).on("changed.jstree", function (e, data) {
                            //存储当前选中的区域的名称
                            if (data.node) {
                                $.playlistEditor('init', {
                                    selectedGroupID: data.node.id
                                });
                            }
                        });

                        tree.on("move_node.jstree", function (e, data) {
                            var node = data.node;
                            if (node) {
                                $.insmGroup('editgroup', {
                                    groupID: node.id,
                                    newGroupNameParentID: node.parent,
                                    newGroupName: node.text,
                                    ActiveFlag: node.li_attr.ActiveFlag,
                                    OnlineFlag: node.li_attr.OnlineFlag,
                                    Comments: node.li_attr.Comments
                                });
                            }
                        });
                    }
                },
                invalid: function () {
                    invalid = true;
                },
                error: function () {
                    options.error();
                },
            });
            $("#PlayerDetail").css('display', 'none');
        },
        defaultDataSet: function () {
            $("#label_Active_null").click();
            $("#label_Online_null").click();
            $("#groupname").val('');
            $("#text_note").val('');
        },
        showPlayerDetail: function (options) {
            $("#PlayerDetail").css('display', 'block');
            $.insmFramework('getGroupPlayers', {
                GroupID: options.GroupID,
                newGroupName: $("#groupname").val(),
                active: $("input[name='radio_Active']:checked").val(),
                onlineUnits: $("input[name='radio_Online']:checked").val(),
                //resolution:$("#select_resolution").find("option:selected").text(),
                note: $("#text_note").val(),
                newGroupNameParentID: groupTreeForPlayerEditID,
                success: function (data) {
                    div_main.show();
                    div_edit.hide();
                    player_Alldata = data;
                    $.insmGroup('DatatableResponsiveColumnsDemo', { PlayersData: data });
                }
            })
        },
        DatatableResponsiveColumnsDemo: function (options) {
            $('#base_responsive_columns').prop("outerHTML", "<div class='m_datatable' id='base_responsive_columns'></div>");
            datatable = $('#base_responsive_columns').mDatatable({
                // datasource definition
                data: {
                    type: 'local',
                    source: {
                        "meta": {
                            "page": 1,
                            "pages": 1,
                            "perpage": -1,
                            "total": 350,
                            "sort": "asc",
                            "field": "PlayerID"
                        },
                        "data": options.PlayersData
                    },
                    //pageSize: 10,
                    saveState: {
                        cookie: false,
                        webstorage: false
                    },
                    serverPaging: false,
                    serverFiltering: false,
                    serverSorting: false
                },

                // layout definition
                layout: {
                    theme: 'default', // datatable theme
                    class: '', // custom wrapper class
                    scroll: false, // enable/disable datatable scroll both horizontal and vertical when needed.
                    height: 550, // datatable's body's fixed height
                    footer: false // display/hide footer
                },

                // column sorting
                sortable: true,

                // column based filtering
                filterable: true,

                pagination: false,

                // columns definition
                columns: [{
                    field: "RecordID",
                    title: "#",
                    sortable: false, // disable sort for this column
                    width: 40,
                    textAlign: 'center',
                    filterable: false,
                    selector: { class: 'm-checkbox--solid m-checkbox--brand' },
                }, {
                    field: "PlayerID",
                    title: "モニターＩＤ",
                    filterable: false, // disable or enable filtering
                    width: 85
                }, {
                    field: "PlayerName",
                    title: "モニター名",
                    responsive: { visible: 'lg' }
                }, {
                    field: "GroupName",
                    title: "グループ名",
                    width: 150,
                    filterable: false,
                    responsive: { visible: 'lg' }
                }, {
                    field: "GreatDate",
                    title: "登録日時",
                    field: "UpdateDate",
                    title: "更新日時",
                    filterable: false,
                    textAlign: 'center',
                    datetype: "yyyy-MM-dd HH:mm",
                    responsive: { visible: 'lg' }
                }, {
                    field: "Online",
                    title: "オンライン",
                    filterable: false,
                    responsive: { visible: 'lg' }
                }]
            });
            datatable.on('m-datatable--on-check', function (e, args) {
                var selected = datatable.setSelectedRecords().getSelectedRecords();
                selectPlayerdata = selected;
            })


            $('#m_form_search_Player').on('keyup', function (e) {
                datatable.search($(this).val().toLowerCase(), "PlayerName");
            });

            $('#m_form_status, #m_form_type').selectpicker();
        },
        addNewGroup: function () {
            if ($.trim($("#groupname").val()) == '') {
                toastr.warning("Group name is empty!");
                return;
            };
            if (groupTreeForPlayerEditID == null) {
                toastr.warning("Please select new group's Parent Group !");
                return;
            };
            if (editGroupID == groupTreeForPlayerEditID) {
                toastr.warning("Group ID have same ID!");
                return;
            };
            $.insmFramework('creatGroup', {
                groupID: editGroupID,
                newGroupName: $("#groupname").val(),
                active: $("input[name='radio_Active']:checked").val(),
                onlineUnits: $("input[name='radio_Online']:checked").val(),
                //resolution:$("#select_resolution").find("option:selected").text(),
                note: $("#text_note").val(),
                newGroupNameParentID: groupTreeForPlayerEditID,
                success: function (data) {
                    //div_main.show();
                    //div_edit.hide();
                    //$.insmGroup('initGroupTree');
                    editGroupID = undefined;
                    $.insmFramework('GroupPlayListLinkTables', {
                        groupID: editGroupID,
                        PlayListID: playListgroup,
                        success: function (data) {
                            div_main.show();
                            div_edit.hide();
                            $.insmGroup('initGroupTree');
                            editGroupID = undefined;
                        },
                        error: function () {
                        }
                    })
                },
                error: function () {
                }
            })
        },
        activechange: function () {
            ActivechangeFlg = true;
            if (selectPlayerdata) {
                $.each(selectPlayerdata, function (index, item) {
                    $(selectPlayerdata[index]).data().obj.ActiveFlag = $("input[name='radio_Active']:checked").val()
                });
            }
        },
        onlinechange: function () {
            OnlinechangeFlg = true;
            if (selectPlayerdata) {
                $.each(selectPlayerdata, function (index, item) {
                    $(selectPlayerdata[index]).data().obj.OnlineFlag = $("input[name='radio_Online']:checked").val()
                });
            }
        },
        getPlaylistByGroup: function (options) {
            $.insmFramework('getPlaylistByGroup', {
                groupID: options.groupID,
                success: function (GroupPlayLists) {
                    $.insmGroup('showPlaylist', { Playlists: GroupPlayLists, isGroup: options.isGroup });
                },
                error: function () {
                },
            });
        },
        showPlaylist: function (options) {
            var div_PlaylistEditorContent = $('#group_player_playlist');
            div_PlaylistEditorContent.empty();

            var div_forcedplaylists = $('#forcedplaylists');
            div_forcedplaylists.empty();

            if (options.Playlists) {
                $.each(options.Playlists, function (index, Playlist) {
                    var div_Playlist = $('<div/>').addClass('m-portlet m-portlet--warning m-portlet--head-sm');
                    var div_Playlisthead = $('<div/>').addClass('m-portlet__head');
                    var div_head_caption = $('<div/>').addClass('m-portlet__head-caption');
                    var div_title = $('<div/>').addClass('m-portlet__head-title');
                    var span_head_icon = $("<span />").addClass('m-portlet__head-icon');
                    var span_i = '<i class="fa fa-file-text"></i>';
                    var head_text = $('<h3 />').addClass('m-portlet__head-text').text(Playlist.PlayListName);
                    span_head_icon.append(span_i);
                    div_title.append(span_head_icon);
                    div_title.append(head_text);
                    div_head_caption.append(div_title);
                    div_Playlisthead.append(div_head_caption);

                    var div_head_tools = $('<div/>').addClass('m-portlet__head-tools');
                    var div_portlet_nav = $('<ul>').addClass("m-portlet__nav");
                    var div_li = $('<li />').addClass('m-portlet__nav-item');
                    var href = $('<a />').addClass("m-portlet__nav-link m-portlet__nav-link--icon");
                    var href_i = $('<i />').addClass("fa fa-toggle-right");
                    href.append(href_i);
                    div_li.append(href);
                    div_portlet_nav.append(div_li);
                    var div_li_list = $('<li />').addClass("m-portlet__nav-item m-dropdown m-dropdown--inline m-dropdown--arrow m-dropdown--align-right m-dropdown--align-push");
                    div_li_list.attr('data-dropdown-toggle', 'hover').attr('aria-expanded', 'true');
                    var div_li_a_toggle = $('<a href="#"/>').addClass('m-portlet__nav-link m-portlet__nav-link--icon m-dropdown__toggle');
                    var div_li_i = $('<i />').addClass('la la-ellipsis-v');
                    div_li_a_toggle.append(div_li_i);
                    div_li_list.append(div_li_a_toggle);

                    var div_m_dropdown_wrapper = $('<div/>').addClass('m-dropdown__wrapper');
                    var wrappe_spantitle = $("<span style='left: auto; right: 18.5px;'/>").addClass('m-dropdown__arrow m-dropdown__arrow--right m-dropdown__arrow--adjust');
                    var div_m_dropdown_inner = $('<div/>').addClass("m-dropdown__inner");
                    var div_m_dropdown_bodyr = $('<div/>').addClass("m-dropdown__body");
                    var div_m_dropdown_content = $('<div/>').addClass("m-dropdown__content");

                    var ul = $('<ul>').addClass("m-nav");
                    //Item
                    if (Playlist.Settings) {
                        var playlistSetting = JSON.parse(Playlist.Settings);
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
                    div_head_tools.append(div_portlet_nav);
                    div_m_dropdown_wrapper.append(wrappe_spantitle);
                    div_m_dropdown_wrapper.append(div_m_dropdown_inner);
                    div_li_list.append(div_m_dropdown_wrapper)
                    div_portlet_nav.append(div_li_list);
                    div_Playlisthead.append(div_head_tools);
                    div_Playlist.append(div_Playlisthead);
                    div_li.click(function () {
                        var playlistclone = div_Playlist.clone(true);
                        div_forcedplaylists.append(playlistclone);
                        playListgroup.push(Playlist.PlayListID);
                    });
                    div_PlaylistEditorContent.append(div_Playlist);
                });
            };
            var tempForcedPlayList = null;
            if (options.isGroup) {
                $.insmFramework('getForcedPlaylistByGroup', {
                    groupID: editGroupID,
                    success: function (forcedPlayList) {
                        tempForcedPlayList = forcedPlayList
                    },
                    error: function () {
                    }
                })
            } else {
                $.insmFramework('getForcedPlaylistByPlayer', {
                    groupID: editGroupID,
                    success: function (forcedPlayList) {
                        tempForcedPlayList = forcedPlayList;
                    },
                    error: function () {
                    }
                })
            }

            if (tempForcedPlayList) {
                $.each(options.Playlists, function (index, Playlist) {
                    var div_Playlist = $('<div/>').addClass('m-portlet m-portlet--warning m-portlet--head-sm');
                    var div_Playlisthead = $('<div/>').addClass('m-portlet__head');
                    var div_head_caption = $('<div/>').addClass('m-portlet__head-caption');
                    var div_title = $('<div/>').addClass('m-portlet__head-title');
                    var span_head_icon = $("<span />").addClass('m-portlet__head-icon');
                    var span_i = '<i class="fa fa-file-text"></i>';
                    var head_text = $('<h3 />').addClass('m-portlet__head-text').text(Playlist.PlayListName);
                    span_head_icon.append(span_i);
                    div_title.append(span_head_icon);
                    div_title.append(head_text);
                    div_head_caption.append(div_title);
                    div_Playlisthead.append(div_head_caption);

                    var div_head_tools = $('<div/>').addClass('m-portlet__head-tools');
                    var div_portlet_nav = $('<ul>').addClass("m-portlet__nav");
                    var div_li = $('<li />').addClass('m-portlet__nav-item');
                    var href = $('<a />').addClass("m-portlet__nav-link m-portlet__nav-link--icon");
                    var href_i = $('<i />').addClass("fa fa-toggle-right");
                    href.append(href_i);
                    div_li.append(href);
                    div_portlet_nav.append(div_li);
                    var div_li_list = $('<li />').addClass("m-portlet__nav-item m-dropdown m-dropdown--inline m-dropdown--arrow m-dropdown--align-right m-dropdown--align-push");
                    div_li_list.attr('data-dropdown-toggle', 'hover').attr('aria-expanded', 'true');
                    var div_li_a_toggle = $('<a href="#"/>').addClass('m-portlet__nav-link m-portlet__nav-link--icon m-dropdown__toggle');
                    var div_li_i = $('<i />').addClass('la la-ellipsis-v');
                    div_li_a_toggle.append(div_li_i);
                    div_li_list.append(div_li_a_toggle);

                    var div_m_dropdown_wrapper = $('<div/>').addClass('m-dropdown__wrapper');
                    var wrappe_spantitle = $("<span style='left: auto; right: 18.5px;'/>").addClass('m-dropdown__arrow m-dropdown__arrow--right m-dropdown__arrow--adjust');
                    var div_m_dropdown_inner = $('<div/>').addClass("m-dropdown__inner");
                    var div_m_dropdown_bodyr = $('<div/>').addClass("m-dropdown__body");
                    var div_m_dropdown_content = $('<div/>').addClass("m-dropdown__content");

                    var ul = $('<ul>').addClass("m-nav");
                    //Item
                    if (Playlist.Settings) {
                        var playlistSetting = JSON.parse(Playlist.Settings);
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
                    div_head_tools.append(div_portlet_nav);
                    div_m_dropdown_wrapper.append(wrappe_spantitle);
                    div_m_dropdown_wrapper.append(div_m_dropdown_inner);
                    div_li_list.append(div_m_dropdown_wrapper)
                    div_portlet_nav.append(div_li_list);
                    div_Playlisthead.append(div_head_tools);
                    div_Playlist.append(div_Playlisthead);
                    div_forcedplaylists.append(div_Playlist);
                });
            }
        },  
    }
    $.insmGroup = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on $.insmGroup'); 
        }
        return null;
    };
    $("#newgroup").click(function (e) {
        div_main.hide();
        div_edit.show();
        $("#button_save").css('display', 'block').removeClass('m-dropdown__toggle');
        $("#button_save_Player").css('display', 'none');
        $.insmGroup('defaultDataSet');
        editGroupID = undefined;
        var div_PlaylistEditorContent = $('#group_player_playlist');
        div_PlaylistEditorContent.empty();
        $("#div_edit .m-portlet__head-caption:first").find("h3:first").text(localize_jap["New Group"]);
        //$.insmFramework('getPlaylistByGroupID', {
        //    GroupID: selectedGroupID,
        //    success: function (data) {
        //        if (data) {
        //            $.insmGroup('showPlaylist', { Playlists: data });
        //        }
        //    },
        //    error: function () {

        //    },
        //})
    })
    $("#deletegroup").click(function (e) {
        var newGroupdata = $.insmFramework('deleteGroup', {
            deleteGroupId: selectedGroupID,
            success: function (resultdata) {
                div_main.show();
                div_edit.hide();
                $.insmGroup('initGroupTree');
            }
        })
    })
    $("#expandAll").click(function () {
        $('#groupTree').jstree('open_all');
    });
    $("#collapseAll").click(function () {
        $('#groupTree').jstree('close_all');
    });
    $("#editgroup").click(function () {
        if (!selectedGroupID) {
            toastr.warning("Select Group first!");
            return;
        }
        div_main.hide();
        div_edit.show();


        editGroupFlg = true;
        $.insmGroup('defaultDataSet');
        $("#button_save").css('display', 'block').removeClass('m-dropdown__toggle');
        $("#button_save_Player").css('display', 'none');
        $.insmFramework('editGroup', {
            groupID: selectedGroupID,
            success: function (userGroupData) {
                if (userGroupData) {
                    $("#label_Active_" + userGroupData.ActiveFlag).click();
                    $("#label_Online_" + userGroupData.OnlineFlag).click();
                    $("#groupname").val(userGroupData.GroupName);
                    $("#text_note").val(userGroupData.Comments);
                    editGroupID = selectedGroupID;
                    $("#div_edit .m-portlet__head-caption:first").find("h3:first").text(userGroupData.GroupName);
                };
                $.insmGroup('getPlaylistByGroup', { groupID: editGroupID, isGroup:true });
            },
            error: function () {

            },
        })
    });
    $("#button_save").click(function (e) {
        $.insmGroup('addNewGroup');
        editGroupID = undefined;
    })
    $("#button_back").click(function () {
        div_main.show();
        div_edit.hide();
    });

    $("#add_player").click(function () {
        div_main.hide();
        div_edit.show();
        $.insmGroup('defaultDataSet');
        $("#button_save_Player").css('display', 'block').removeClass('m-dropdown__toggle');
        $("#button_save").css('display', 'none');
        $("#div_edit .m-portlet__head-caption:first").find("h3:first").text(localize_jap["Add"]);

        $.insmFramework('getPlaylistByGroupID', {
            GroupID: selectedGroupID,
            success: function (data) {
                if (data) {
                    $.insmGroup('showPlaylist', {Playlists:data});
                }
            },
            error: function () {
               
            },
        })
    });
    $("#button_save_Player").click(function () {
        div_main.show();
        div_edit.hide();

        if (!editPlayerFlg) {
            if ($.trim($("#groupname").val()) == '' || groupTreeForPlayerEditID == null) {
                toastr.warning("Player name is empty!");
                return;
            }
            $.insmFramework('creatPlayer', {
                GroupID: groupTreeForPlayerEditID,
                PlayerName: $("#groupname").val(),
                active: $("input[name='radio_Active']:checked").val(),
                onlineUnits: $("input[name='radio_Online']:checked").val(),
                //resolution:$("#select_resolution").find("option:selected").text(),
                note: $("#text_note").val(),
                ActivechangeFlg: ActivechangeFlg,
                OnlinechangeFlg: OnlinechangeFlg,
                DisplayNamechangeFlg: DisplayNamechangeFlg,
                NotechangeFlg: DisplayNamechangeFlg,
                success: function (data) {
                    div_main.show();
                    div_edit.hide();
                    $.insmGroup('initGroupTree');
                    editGroupID = undefined;
                    $.insmFramework('PlayerPlayListLinkTables', {
                        Index: '',
                        PlayerID: data.PlayerID,
                        PlayListID: '25',
                        success: function (data) {
                            div_main.show();
                            div_edit.hide();
                            $.insmGroup('initGroupTree');
                            editGroupID = undefined;
                        }
                    })
                }
            })

            
        } else {
            $.insmFramework('editGroupPlayers', {
                Playerdata: selectPlayerdata,
                ActivechangeFlg: ActivechangeFlg,
                OnlinechangeFlg: OnlinechangeFlg,
                DisplayNamechangeFlg: DisplayNamechangeFlg,
                NotechangeFlg: DisplayNamechangeFlg,
                newGroupID: groupTreeForPlayerEditID,
                success: function (data) {
                    div_main.show();
                    div_edit.hide();
                    $.insmGroup('initGroupTree');
                    editGroupID = undefined;
                }
            })
        }
        $("#PlayerDetail").css('display', 'none');
        
    });
    $("#radio_All").click(function () {
        $('#m_form_search_Player').val('');
        $.insmGroup('DatatableResponsiveColumnsDemo', { PlayersData: player_Alldata });
    })
    $("#radio_Current").click(function () {
        $('#m_form_search_Player').val('');
        var Current_data = [];
        $.each(player_Alldata, function (key, item) {
            if (item.GroupID == selectedGroupID) {
                Current_data.push(item)
            }
        });
        $.insmGroup('DatatableResponsiveColumnsDemo', { PlayersData: Current_data });
    })

    var edit_player_click = function (selectPlayer, allPlayerID) {
        div_main.hide();
        div_edit.show();
        var allPlayerNames = "";
        $.each(selectPlayer, function (playerIndex, playerItem) {
            allPlayerNames += ", " + $(playerItem).data().obj.PlayerName;
        })
        allPlayerNames = allPlayerNames.substr(2);
        div_edit.find("H3:first").text(selectPlayer.length + " Display Units / (" + allPlayerNames + ")");
        editPlayerFlg = true;
        $.insmGroup('defaultDataSet');
        $("#button_save_Player").css('display', 'block').removeClass('m-dropdown__toggle');
        $("#button_save").css('display', 'none');
        $.each(selectPlayer, function (index, item) {
            if (index != 0) {
                if ($("#groupname").val() != $(selectPlayer[index]).data().obj.PlayerName) {
                    $("#groupname").val('');
                }
                if ($("#groupname").val() != $(selectPlayer[index]).data().obj.PlayerName) {
                    $("#text_note").val('');
                }
            } else {
                $("#groupname").val($(selectPlayer[index]).data().obj.PlayerName);
                $("#text_note").val($(selectPlayer[index]).data().obj.Comments);

                $("#label_Active_" + $(selectPlayer[index]).data().obj.ActiveFlag).click();
                $("#label_Online_" + $(selectPlayer[index]).data().obj.OnlineFlag).click();
            }
        });
        if (allPlayerID != '') {
            $.insmFramework('getPlaylistByPlayerID', {
                playerID: allPlayerID,
                success: function (playlistData) {
                    if (playlistData) {
                        $.insmGroup('showPlaylist', { Playlists: playlistData });
                    }
                    
                }
            })
        }

        $.insmGroup('getPlaylistByGroup', { groupID: editGroupID });
    }
    $("#edit_player").click(function () {
        var selected = datatable.setSelectedRecords().getSelectedRecords();
        selectPlayerdata = selected;
        if (!selectPlayerdata) { return; }
        var allPlayerNames = "";
        $.each(selectPlayerdata, function (playerIndex, playerItem) {
            allPlayerNames += ", " + $(playerItem).data().obj.PlayerName;
        });

        var allPlayerID = "";
        $.each(selectPlayerdata, function (playerIndex, playerItem) {
            allPlayerID += ", " + $(playerItem).data().obj.PlayerID;
        })

        allPlayerNames = allPlayerNames.substr(2);
        var playerSelectionLi = $('<li class="m-menu__item " data-redirect="true" aria-haspopup="true">\
                                                <a class="m-menu__link" title="'+ allPlayerNames +'">\
                                                    <span class="m-menu__link-title" style="text-overflow:ellipsis;white-space: nowrap;overflow: hidden;">\
                                                        <span class="m-menu__link-wrap">\
                                                            <span class="m-menu__link-badge">\
                                                                <span class="m-badge m-badge--success">'
                                                                    + selectPlayerdata.length +
                                                                '</span>\
                                                            </span>\
                                                            <span class="m-menu__link-text">'
                                                                + allPlayerNames +
                                                            '</span>\
                                                        </span>\
                                                    </span>\
                                                </a>\
                                            </li>');
        playerSelectionLi.find("a").data("playersData", $.extend(true, {}, selectPlayerdata)).click(function () {
            selectPlayerdata = $(this).data("playersData");
            edit_player_click(selectPlayerdata);
        });
        $("#playerSelectionHistroyUl").prepend(playerSelectionLi);
        edit_player_click(selectPlayerdata, allPlayerID);
    })



    $("#label_Active_null").click(function () {
        if (editPlayerFlg) { $.insmGroup('activechange'); }
    })
    $("#label_Active_1").click(function () {
        if (editPlayerFlg) { $.insmGroup('activechange'); }
    })
    $("#label_Active_0").click(function () {
        if (editPlayerFlg) { $.insmGroup('activechange'); }
    })

    $("#label_Onlinenull").click(function () {
        if (editPlayerFlg) { $.insmGroup('onlinechange'); }
    })
    $("#label_Online_1").click(function () {
        if (editPlayerFlg) { $.insmGroup('onlinechange'); }
    })
    $("#label_Online_0").click(function () {
        if (editPlayerFlg) { $.insmGroup('onlinechange'); }
    })

    $("#groupname").change(function () {
        DisplayNamechangeFlg = true;
        $.each(selectPlayerdata, function (index, item) {
            $(selectPlayerdata[index]).data().obj.PlayerName = $("#groupname").val();
        });
    })
    $("#text_note").change(function () {
        NotechangeFlg = true;
        $.each(selectPlayerdata, function (index, item) {
            $(selectPlayerdata[index]).data().obj.Comments = $("#text_note").val();
        });
    })

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-center",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
})(jQuery);