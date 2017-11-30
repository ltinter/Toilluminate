(function ($) {
    var GroupTreedata = [];
    var GroupData;
    var selectPlayerdata;
    var playListgroup=[];
    var editGroupID;
    var selectedGroupID = null;
    var selectedGroupIDparents;
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
    var temp_GroupTreeData;
    var firstPageload = true;
    
    
    
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
                    $.insmGroup('refreshTree');
                    editGroupID = undefined;
                }
            })
        },
        refreshTree: function () {
            var tree = $('.tree-demo.groupTree');
            $.each(tree, function (key, item) {
                $(item).jstree(true).refresh();
            });
        },
        initGroupTree: function (options) {
            var groupJstreeData = {
                "core": {
                    "themes": {
                        "responsive": true
                    },
                    // so that create works
                    "check_callback": true,
                    'data': {
                        url: 'api/GroupMasters/GetGroupJSTreeDataWithChildByGroupID/5',
                            //+ options.userGroupId,
                        dataFilter: function (data) {
                            temp_GroupTreeData = JSON.parse(data);
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
            div_edit.hide();
            $.insmGroup('defaultDataSet');
            $("#PlayerDetail").css('display', 'none');

            $("#button_save").text($.localize('translate', "Save"));

            var tree = $('.tree-demo.groupTree');
            tree.jstree(groupJstreeData);

            div_groupTree.bind("refresh.jstree", function (e, data) {
                div_groupTree.jstree(true).select_node(selectedGroupID);
            });

            tree.on('loaded.jstree', function(e, data) {
                var inst = data.instance;
                var obj = inst.get_node(e.target.firstChild.firstChild.lastChild);

                inst.select_node(obj);
                if (firstPageload) {
                    mApp.unblockPage();
                    $("#DisplayUnitsContent").show();
                    firstPageload = false;
                }
            })

            div_groupTree.on("changed.jstree", function (e, data) {
                //存储当前选中的区域的名称
                if (data.node) {
                    selectedGroupID = data.node.id;
                    selectedGroupIDparents = data.node.parents
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
            $("#PlayerDetail").css('display', 'none');
        },
        defaultDataSet: function () {
            $("#label_Active_2").click();
            $("#label_Online_2").click();
            $("#groupname").val('');
            $("#text_note").val('');
            $("#select_resolution").val('0');
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
                    title: "",
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
                    field: "OnlineFlag",
                    title: "",
                    filterable: false,
                    textAlign: 'center',
                    responsive: { visible: 'lg' },
                    template: function (row, a, b) {
                        var dropup = (row.getDatatable().getPageSize() - row.getIndex()) <= 4 ? 'dropup' : '';

                        return '\
						<span class="m-menu__link-badge">\
                             <span class="m-badge m-badge--success" id="span_success"><i class="m-menu__link-icon fa fa-check-circle"></i></span>\
                        </span>\
                        ';
                    },
                    width: 0,
                }, {
                    field: "GroupID",
                    title: "",
                    filterable: false,
                    responsive: { visible: 'lg' },
                    width: 0,
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
                if (selectedGroupIDparents && selectedGroupIDparents.length == 1) {
                    if (selectedGroupIDparents[0] != '#') {
                        toastr.warning("Group ID have same ID!");
                        return;
                    }
                } else {
                    toastr.warning("Group ID have same ID!");
                    return;
                }
                
            };
            var Settings = {};
            Settings = {
                Monday: $("#group_monday_value").val(),
                Tuesday: $("#group_tuesday_value").val(),
                Wednesday: $("#group_wednesday_value").val(),
                Thursday: $("#group_thursday_value").val(),
                Friday: $("#group_friday_value").val(),
                Saturday: $("#group_saturday_value").val(),
                Sunday: $("#group_sunday_value").val(),
                resolution: $("#select_resolution").val()
            };
            $.insmFramework('creatGroup', {
                groupID: editGroupID,
                newGroupName: $("#groupname").val(),
                active: $("input[name='radio_Active']:checked").val(),
                onlineUnits: $("input[name='radio_Online']:checked").val(),
                note: $("#text_note").val(),
                settings: JSON.stringify(Settings),
                newGroupNameParentID: groupTreeForPlayerEditID,
                success: function (data) {
                    var div_forcedplaylists = $('#forcedplaylists');
                    var forcedplaylists = div_forcedplaylists.find(".m-portlet.m-portlet--warning.m-portlet--head-sm");
                    playListgroup =[];
                    if (forcedplaylists.length > 0) {
                        $.each(forcedplaylists, function (index, forcedplaylist) {
                            var playlistItem = {};
                            var forcedplaylistID = $(forcedplaylist).attr('playlistId');
                            playListgroup.push(forcedplaylistID);
                        });
                    }
                    $.insmFramework('deleteGroupPlayListLinkTableByGroupID', {
                        groupID: selectedGroupID,
                        success: function (data) {
                            $.insmFramework('GroupPlayListLinkTables', {
                                groupID: selectedGroupID,
                                PlayListID: playListgroup,
                                success: function (data) {
                                },
                                error: function () {
                                }
                            })
                        },
                        error: function () {
                        }
                    })
                    
                    div_main.show();
                    div_edit.hide();
                    $.insmGroup('refreshTree');
                    editGroupID = undefined;
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
            var div_forcedplaylists = $('#forcedplaylists');
            div_PlaylistEditorContent.empty();
            div_forcedplaylists.empty();
            if (options.Playlists) {
                $.each(options.Playlists, function (index, Playlist) {
                    var div_Playlist = $('<div/>').addClass('m-portlet m-portlet--warning m-portlet--head-sm').attr('playlistId', Playlist.PlayListID);
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
                    var href = $('<a />').addClass("m-portlet__nav-link m-portlet__nav-link--icon").attr("href", "javascript:;");
                    var href_i = $('<i />').addClass("fa fa-toggle-right");
                    href.append(href_i);
                    div_li.append(href);
                    div_portlet_nav.append(div_li);
                    var div_li_list = $('<li />').addClass("m-portlet__nav-item m-dropdown m-dropdown--inline m-dropdown--arrow m-dropdown--align-right m-dropdown--align-push");
                    div_li_list.attr('data-dropdown-toggle', 'hover').attr('aria-expanded', 'true');
                    var div_li_a_toggle = $('<a href="javascript:;"/>').addClass('m-portlet__nav-link m-portlet__nav-link--icon m-dropdown__toggle');
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
                        playlistclone.find("li.m-portlet__nav-item:first").empty();
                        playlistclone.find("li.m-portlet__nav-item:first").append($("<a href='javascript:;' class='m-portlet__nav-link m-portlet__nav-link--icon'><i class='la la-close'></i></a>").click(function () { playlistclone.remove() }));
                        //playListgroup.push(Playlist.PlayListID);
                    });
                    div_PlaylistEditorContent.append(div_Playlist);
                });
            };
            var tempForcedPlayList = null;
            if (options.isGroup) {
                if (options.newGroup) {
                    $.insmFramework('getForcedPlaylistByGroup', {
                        groupID: options.GroupID,
                        success: function (forcedPlayList) {
                            $.insmGroup('showPlaylistForced', { tempForcedPlayList: forcedPlayList, isGroup: false, newgroup: options.newgroup });
                            div_main.hide();
                            div_edit.show();
                        },
                        error: function () {
                        }
                    })
                } else {
                    $.insmFramework('getForcedPlaylistByGroup', {
                        groupID: options.GroupID,
                        success: function (forcedPlayList) {
                            $.insmGroup('showPlaylistForced', {
                                tempForcedPlayList: forcedPlayList, isGroup: true
                            });
                            div_main.hide();
                            div_edit.show();
                        },
                        error: function () {
                        }
                    })
                }
            } else {
                if(options.newgroup){
                    $.insmFramework('getForcedPlaylistByGroup', {
                        groupID: options.GroupID,
                        success: function (forcedPlayList) {
                            $.insmGroup('showPlaylistForced', { tempForcedPlayList: forcedPlayList, isGroup: false, newgroup: options.newgroup });
                            div_main.hide();
                            div_edit.show();
                        },
                        error: function () {
                        }
                    })
                } else {
                    if (options.playerId && options.playerId.indexOf(",") > 0) {
                        var editplayerIds = options.playerId.split(', ');
                        var editplayerPlists = {};
                        var playlistEditDeferredList = [];
                        $.each(editplayerIds, function (index, editplayerId) {
                            var tempPlayerEditDrferred = new $.Deferred();
                            playlistEditDeferredList.push(tempPlayerEditDrferred);
                            $.insmFramework('getForcedPlaylistByPlayer', {
                                playerId: editplayerId,
                                success: function (forcedPlayList) {
                                    editplayerPlists[editplayerId] = forcedPlayList;
                                    tempPlayerEditDrferred.resolve();
                                },
                                error: function () {
                                }
                            })
                        })
                        $.when.apply($, playlistEditDeferredList).done(function () {
                            var tempList = [];
                            var isSame = true;
                            $.each(editplayerPlists, function (index, item) {
                                var tempPlaylists = '';
                                $.each(item, function (listindex, listitem) {
                                    tempPlaylists += listitem.PlayListID + ",";
                                })
                                tempList.push(tempPlaylists);
                            });
                            $.unique(tempList);
                            if (tempList.length == 0 || tempList.length == 1) {
                                $.insmGroup('showPlaylistForced', { tempForcedPlayList: editplayerPlists[editplayerIds[0]], isGroup: false });
                                div_main.hide();
                                div_edit.show();
                            }
                        });
                    } else {
                        if (options.playerId) {
                            $.insmFramework('getForcedPlaylistByPlayer', {
                                playerId: options.playerId,
                                success: function (forcedPlayList) {
                                    $.insmGroup('showPlaylistForced', { tempForcedPlayList: forcedPlayList, isGroup: false });
                                    div_main.hide();
                                    div_edit.show();
                                },
                                error: function () {
                                }
                            })
                        } else {
                            $.insmFramework('getForcedPlaylistByGroup', {
                                groupID: options.GroupID,
                                success: function (forcedPlayList) {
                                    $.insmGroup('showPlaylistForced', { tempForcedPlayList: forcedPlayList, isGroup: false, newgroup: options.newgroup });
                                    div_main.hide();
                                    div_edit.show();
                                },
                                error: function () {
                                }
                            })
                        }
                    }
                    
                }   
            }
        },
        showPlaylistForced: function (options) {
            var div_forcedplaylists = $('#forcedplaylists');
            div_forcedplaylists.empty();
            if (options.tempForcedPlayList) {
                $.each(options.tempForcedPlayList, function (index, Playlist) {
                    if ((Playlist.BindGroupID == selectedGroupID && options.isGroup && !options.newgroup) || (!options.isGroup && Playlist.BindGroupID == 0)) {
                        var div_Playlist = $('<div/>').addClass('m-portlet m-portlet--mobile m-portlet--sortable m-portlet--warning m-portlet--head-sm').attr('playlistId', Playlist.PlayListID);
                        var div_Playlisthead = $('<div/>').addClass('m-portlet__head');
                    } else {
                        var div_Playlist = $('<div/>').addClass('m-portlet m-portlet--mobile m-portlet--warning m-portlet--head-solid-bg');
                        var div_Playlisthead = $('<div/>').addClass('m-portlet__head ui-sortable-handle');
                    }

                    var div_head_caption = $('<div/>').addClass('m-portlet__head-caption');
                    var div_title = $('<div/>').addClass('m-portlet__head-title');
                    var span_head_icon = $("<span />").addClass('m-portlet__head-icon');
                    var span_i = '<i class="fa fa-file-text"></i>';
                    if ((Playlist.BindGroupID == selectedGroupID && options.isGroup) || (!options.isGroup && Playlist.BindGroupID == 0)) {
                        var head_text = $('<h3 />').addClass('m-portlet__head-text').append(Playlist.PlayListName);
                    } else {
                        var head_text = $('<h3 />').addClass('m-portlet__head-text').append(Playlist.PlayListName + '<br>' +'(' + Playlist.GroupName + ')');
                    }

                    span_head_icon.append(span_i);
                    div_title.append(span_head_icon);
                    div_title.append(head_text);
                    div_head_caption.append(div_title);
                    div_Playlisthead.append(div_head_caption);
                    var div_head_tools = $('<div/>').addClass('m-portlet__head-tools');
                    var div_portlet_nav = $('<ul>').addClass("m-portlet__nav");
                    if ((Playlist.BindGroupID == selectedGroupID && options.isGroup) || (!options.isGroup && Playlist.BindGroupID == 0)) {
                        var div_li = $('<li />').addClass('m-portlet__nav-item');
                        var href = $('<a />').addClass("m-portlet__nav-link m-portlet__nav-link--icon").attr("href", "javascript:;");
                        var href_i = $('<i />').addClass("la la-close");
                        href.append(href_i);
                        div_li.append(href);
                        div_portlet_nav.append(div_li);
                        div_li.click(function () {
                            div_Playlist.remove();
                        });
                    }
                    var div_li_list = $('<li />').addClass("m-portlet__nav-item m-dropdown m-dropdown--inline m-dropdown--arrow m-dropdown--align-right m-dropdown--align-push");
                    div_li_list.attr('data-dropdown-toggle', 'hover').attr('aria-expanded', 'true');
                    var div_li_a_toggle = $('<a href="javascript:;"/>').addClass('m-portlet__nav-link m-portlet__nav-link--icon m-dropdown__toggle');
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
                $('#forcedplaylists').sortable({
                    connectWith: ".m-portlet__head",
                    items: "div.m-portlet:not(.m-portlet--warning.m-portlet--head-solid-bg)",
                    opacity: 0.8,
                    handle: '.m-portlet__head',
                    coneHelperSize: true,
                    placeholder: 'm-portlet--sortable-placeholder',
                    forcePlaceholderSize: true,
                    tolerance: "pointer",
                    helper: "clone",
                    tolerance: "pointer",
                    forcePlaceholderSize: !0,
                    helper: "clone",
                    cancel: ".m-portlet--sortable-empty", // cancel dragging if portlet is in fullscreen mode
                    revert: 250, // animation in milliseconds
                    update: function (b, c) {
                        if (c.item.prev().hasClass("m-portlet--sortable-empty")) {
                            c.item.prev().before(c.item);
                        }
                    }
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
        div_groupTreeForPlayerEdit.jstree(true).deselect_all(true);
        div_groupTreeForPlayerEdit.jstree(true).select_node(div_groupTree.jstree(true).get_selected());

        $("#group_monday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_tuesday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_wednesday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_thursday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_friday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_saturday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_sunday_value").data("ionRangeSlider").update({ from: 0, to: 24 });

        var div_PlaylistEditorContent = $('#group_player_playlist');
        var div_forcedplaylists = $('#forcedplaylists');
        div_PlaylistEditorContent.empty();
        div_forcedplaylists.empty();
        $("#div_edit .m-portlet__head-caption:first").find("h3:first").text(localize_jap["New Group"]);
        $.insmFramework('getPlaylistByGroup', {
            GroupID: selectedGroupID,
            success: function (data) {
                if (data) {
                    $.insmGroup('showPlaylist', { Playlists: data, isGroup: true, GroupID: selectedGroupID, newGroup: true});
                }
            },
            error: function () {

            },
        })
    })
    $("#deletegroup").click(function (e) {
        //$.insmFramework('deleteGroup', {
        //    deleteGroupId: selectedGroupID,
        //    success: function (resultdata) {
        //        div_main.show();
        //        div_edit.hide();
        //        $.insmGroup('initGroupTree');
        //    }
        //})
        toastr.warning("Group is used!");
        return;
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
        div_groupTreeForPlayerEdit.jstree(true).deselect_all(true);
        $.each(temp_GroupTreeData, function (index, item) {
            if (item.id == div_groupTree.jstree(true).get_selected()) {
                div_groupTreeForPlayerEdit.jstree(true).select_node(item.parent);
                groupTreeForPlayerEditID = item.parent;
            }
        });

        $("#group_monday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_tuesday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_wednesday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_thursday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_friday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_saturday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_sunday_value").data("ionRangeSlider").update({ from: 0, to: 24 });

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
                    var Settings;
                    if (userGroupData) {
                        Settings = JSON.parse(userGroupData.Settings);
                    }
                    if (Settings != null) {
                        if (Object.getOwnPropertyNames(Settings).length > 0) {
                            $("#group_monday_value").data("ionRangeSlider").update({
                                from: Settings.Monday.split(';')[0], to: Settings.Monday.split(';')[1]
                            });
                            $("#group_tuesday_value").data("ionRangeSlider").update({
                                from: Settings.Tuesday.split(';')[0], to: Settings.Tuesday.split(';')[1]
                            });
                            $("#group_wednesday_value").data("ionRangeSlider").update({
                                from: Settings.Wednesday.split(';')[0], to: Settings.Wednesday.split(';')[1]
                            });
                            $("#group_thursday_value").data("ionRangeSlider").update({
                                from: Settings.Thursday.split(';')[0], to: Settings.Thursday.split(';')[1]
                            });
                            $("#group_friday_value").data("ionRangeSlider").update({
                                from: Settings.Friday.split(';')[0], to: Settings.Friday.split(';')[1]
                            });
                            $("#group_saturday_value").data("ionRangeSlider").update({
                                from: Settings.Saturday.split(';')[0], to: Settings.Saturday.split(';')[1]
                            });
                            $("#group_sunday_value").data("ionRangeSlider").update({
                                from: Settings.Sunday.split(';')[0], to: Settings.Sunday.split(';')[1]
                            });
                            if (Settings.resolution) {
                                $('#select_resolution').val(Settings.resolution);
                            }
                        }
                    }
                    

                    editGroupID = selectedGroupID;
                    $("#div_edit .m-portlet__head-caption:first").find("h3:first").text(userGroupData.GroupName);
                };
                $.insmFramework('getPlaylistByGroup', {
                    GroupID: selectedGroupID,
                    success: function (data) {
                        if (data) {
                            $.insmGroup('showPlaylist', { Playlists: data, isGroup: true, GroupID: selectedGroupID });
                        }
                    },
                    error: function () {
                    },
                })
            },
            error: function () {

            },
        })
    });
    $("#button_save").click(function (e) {
        $.insmGroup('addNewGroup');
        console.log("Group Save Button!");
        editGroupID = undefined;
        //$("#forcedplaylists").empty();
    })
    $("#button_back").click(function () {
        div_main.show();
        div_edit.hide();
        $("#forcedplaylists").empty();
    });

    $("#add_player").click(function () {
        $.insmGroup('defaultDataSet');
        $("#group_monday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_tuesday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_wednesday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_thursday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_friday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_saturday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_sunday_value").data("ionRangeSlider").update({ from: 0, to: 24 });

        editPlayerFlg = false;
        $("#button_save_Player").css('display', 'block').removeClass('m-dropdown__toggle');
        $("#button_save").css('display', 'none');
        $("#div_edit .m-portlet__head-caption:first").find("h3:first").text(localize_jap["Add"]);
        div_groupTreeForPlayerEdit.jstree(true).deselect_all(true);
        div_groupTreeForPlayerEdit.jstree(true).select_node(div_groupTree.jstree(true).get_selected());
        groupTreeForPlayerEditID = div_groupTree.jstree(true).get_selected()[0];
        $.insmFramework('getPlaylistByGroup', {
            GroupID: selectedGroupID,
            success: function (data) {
                if (data) {
                    $.insmGroup('showPlaylist', { Playlists: data, isGroup: false, GroupID: selectedGroupID });
                }
            },
            error: function () {
            },
        })
    });
    $("#button_save_Player").click(function () {
        var Settings = {};
        Settings = {
            Monday: $("#group_monday_value").val(),
            Tuesday: $("#group_tuesday_value").val(),
            Wednesday: $("#group_wednesday_value").val(),
            Thursday: $("#group_thursday_value").val(),
            Friday: $("#group_friday_value").val(),
            Saturday: $("#group_saturday_value").val(),
            Sunday: $("#group_sunday_value").val(),
            resolution: $("#select_resolution").val(),
            ActiveFlag: $("input[name='radio_Active']:checked").val(),
            OnlineFlag: $("input[name='radio_Online']:checked").val()
        };

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
                settings: JSON.stringify(Settings),
                note: $("#text_note").val(),
                ActivechangeFlg: ActivechangeFlg,
                OnlinechangeFlg: OnlinechangeFlg,
                DisplayNamechangeFlg: DisplayNamechangeFlg,
                NotechangeFlg: DisplayNamechangeFlg,
                success: function (data) {
                    var div_forcedplaylists = $('#forcedplaylists');
                    var forcedplaylists = div_forcedplaylists.find(".m-portlet.m-portlet--warning.m-portlet--head-sm");
                    playListgroup =[];
                    if (forcedplaylists.length > 0) {
                        $.each(forcedplaylists, function (index, forcedplaylist) {
                            var playlistItem = {};
                            var forcedplaylistID = $(forcedplaylist).attr('playlistId');
                            playListgroup.push(forcedplaylistID);
                        });
                    }
                    $.insmFramework('deletePlayerPlayListLinkTableByPlayerID', {
                        playerId: data.PlayerID,
                        success: function () {
                            $.insmFramework('playerPlayListLinkTables', {
                                playerId: data.PlayerID,
                                PlayListID: playListgroup,
                                success: function (data) {
                                },
                                error: function () {
                                }
                            })
                        },
                        error: function () {
                        }
                    })

                    div_main.show();
                    div_edit.hide();
                    $.insmGroup('refreshTree');
                    editGroupID = undefined;
                }
            })

            
        } else {
            var div_forcedplaylists = $('#forcedplaylists');
            var forcedplaylists = div_forcedplaylists.find(".m-portlet.m-portlet--warning.m-portlet--head-sm");
            playListgroup = [];
            if (forcedplaylists.length > 0) {
                $.each(forcedplaylists, function (index, forcedplaylist) {
                    var playlistItem = {};
                    var forcedplaylistID = $(forcedplaylist).attr('playlistId');
                    playListgroup.push(forcedplaylistID);
                });
            }
            $.insmFramework('editGroupPlayers', {
                Playerdata: selectPlayerdata,
                ActivechangeFlg: ActivechangeFlg,
                OnlinechangeFlg: OnlinechangeFlg,
                DisplayNamechangeFlg: DisplayNamechangeFlg,
                ActiveFlag: $("input[name='radio_Active']:checked").val(),
                OnlineFlag: $("input[name='radio_Online']:checked").val(),
                NotechangeFlg: DisplayNamechangeFlg,
                newGroupID: groupTreeForPlayerEditID,
                settings: JSON.stringify(Settings),
                success: function (data) {
                    $.each(selectPlayerdata, function (index, editedplayer) {
                        var editedplayerID = $(selectPlayerdata[index]).data().obj.PlayerID
                        $.insmFramework('deletePlayerPlayListLinkTableByPlayerID', {
                            playerId: editedplayerID,
                            success: function () {
                                $.insmFramework('playerPlayListLinkTables', {
                                    playerId: editedplayerID,
                                    PlayListID: playListgroup,
                                    success: function (data) {
                                    },
                                    error: function () {
                                    }
                                })
                            },
                            error: function () {
                            }
                        })
                    });


                    div_main.show();
                    div_edit.hide();
                    $.insmGroup('refreshTree');
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
        $.insmGroup('defaultDataSet');
        $("#group_monday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_tuesday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_wednesday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_thursday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_friday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_saturday_value").data("ionRangeSlider").update({ from: 0, to: 24 });
        $("#group_sunday_value").data("ionRangeSlider").update({ from: 0, to: 24 });

        var allPlayerNames = "";
        $.each(selectPlayer, function (playerIndex, playerItem) {
            allPlayerNames += ", " + $(playerItem).data().obj.PlayerName;
        })
        allPlayerNames = allPlayerNames.substr(2);
        div_edit.find("H3:first").text(selectPlayer.length + " Display Units / (" + allPlayerNames + ")");
        groupTreeForPlayerEditID = div_groupTree.jstree(true).get_selected()[0];

        var allPlayerID = "";
        $.each(selectPlayer, function (playerIndex, playerItem) {
            allPlayerID += ", " + $(playerItem).data().obj.PlayerID;
        })
        allPlayerID = allPlayerID.substr(2);

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
                if (div_groupTreeForPlayerEdit.jstree(true).get_selected() != $(selectPlayer[index]).data().obj.GroupID) {
                    div_groupTreeForPlayerEdit.jstree(true).deselect_all(true);
                }
            } else {
                div_groupTreeForPlayerEdit.jstree(true).deselect_all(true);
                div_groupTreeForPlayerEdit.jstree(true).select_node(div_groupTree.jstree(true).get_selected());

                $("#groupname").val($(selectPlayer[index]).data().obj.PlayerName);
                $("#text_note").val($(selectPlayer[index]).data().obj.Comments); 

                var Settings;
                $.each(player_Alldata, function (playerindex, playerdata) {
                    if (playerdata.PlayerID == $(selectPlayer[index]).data().obj.PlayerID) {
                        Settings = JSON.parse(playerdata.Settings);
                    }
                    
                });
                if (Settings != null) {
                    if (Object.getOwnPropertyNames(Settings).length > 0) {
                        $("#group_monday_value").data("ionRangeSlider").update({
                            from: Settings.Monday.split(';')[0], to: Settings.Monday.split(';')[1]
                        });
                        $("#group_tuesday_value").data("ionRangeSlider").update({
                            from: Settings.Tuesday.split(';')[0], to: Settings.Tuesday.split(';')[1]
                        });
                        $("#group_wednesday_value").data("ionRangeSlider").update({
                            from: Settings.Wednesday.split(';')[0], to: Settings.Wednesday.split(';')[1]
                        });
                        $("#group_thursday_value").data("ionRangeSlider").update({
                            from: Settings.Thursday.split(';')[0], to: Settings.Thursday.split(';')[1]
                        });
                        $("#group_friday_value").data("ionRangeSlider").update({
                            from: Settings.Friday.split(';')[0], to: Settings.Friday.split(';')[1]
                        });
                        $("#group_saturday_value").data("ionRangeSlider").update({
                            from: Settings.Saturday.split(';')[0], to: Settings.Saturday.split(';')[1]
                        });
                        $("#group_sunday_value").data("ionRangeSlider").update({
                            from: Settings.Sunday.split(';')[0], to: Settings.Sunday.split(';')[1]
                        });
                        if (Settings.resolution) {
                            $('#select_resolution').val(Settings.resolution);
                        }
                        $("#label_Active_" + Settings.ActiveFlag).click();
                        $("#label_Online_" + Settings.OnlineFlag).click();
                    }
                }

            }
        });
        $.insmFramework('getPlaylistByGroup', {
            GroupID: selectedGroupID,
            success: function (data) {
                if (data) {
                    $.insmGroup('showPlaylist', { Playlists: data, isGroup: false, GroupID: selectedGroupID, playerId: allPlayerID });
                }
                div_main.hide();
                div_edit.show();
            },
            error: function () {
            },
        })
    }
    $("#edit_player").click(function () {
        var selected = datatable.setSelectedRecords().getSelectedRecords();
        selectPlayerdata = selected;
        if (selectPlayerdata.length == 0) { return; }
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
                                                <a class="m-menu__link" title="' + allPlayerNames + '">\
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



    $("#label_Active_2").click(function () {
        if (editPlayerFlg) { $.insmGroup('activechange'); }
    })
    $("#label_Active_1").click(function () {
        if (editPlayerFlg) { $.insmGroup('activechange'); }
    })
    $("#label_Active_0").click(function () {
        if (editPlayerFlg) { $.insmGroup('activechange'); }
    })

    $("#label_Online_2").click(function () {
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