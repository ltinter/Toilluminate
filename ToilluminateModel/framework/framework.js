(function ($) {
    var _guid = 0;
    var methods = {
        init: function (options) {
            // Global vars
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');

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
                $this.data('insmFramework', _plugin);
            }

            if (!_plugin.settings.apiUrl || !_plugin.settings.applicationName || !_plugin.settings.version) {
                //throw new Error('INSM Framework not initialized correctly');
                $.insmNotification({
                    type: "warning",
                    message: 'INSM Framework not initialized correctly',
                    text: 'INSM Framework not initialized correctly'
                });
            }

            if (!_plugin.settings.apiUrl.indexOf('http://') == 0 && !_plugin.settings.apiUrl.indexOf('https://') == 0) {
                //throw new Error('Invalid configuration. API URL has to start with "http://" or "https://".');
                $.insmNotification({
                    type: "warning",
                    message: 'Invalid configuration. API URL has to start with "http://" or "https://".',
                    text: 'Invalid configuration. API URL has to start with "http://" or "https://".'
                });
            }

            _plugin.settings.apiUrl = _plugin.settings.apiUrl.replace(/\/+$/, "");
            if (_plugin.settings.session == "null") {
                _plugin.settings.session = null;
            }
            if (!!window.localStorage && typeof (Storage) !== "undefined" && !_plugin.settings.session) {
                _plugin.settings.session = localStorage.insmFrameworkSession;
                if (!_plugin.settings.session) {
                    _plugin.settings.session = '';
                }
            }


            $.insmFramework('downloadCurrentUser', {
                success: function (user) {
                    _plugin.settings.user = user;

                    $.insmFramework('regionTree', {
                        success: function (regionTree) {
                            _plugin.settings.user.regionTree = regionTree;
                            _plugin.data.initialized.resolve();
                        }
                    });
                }
            });
            return $this;
        },
        loggedIn: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');

            if ($.isEmptyObject(_plugin.settings.user)) {
                return false;
            }
            else {
                return true;
            }
        },
        getUserGroup: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var data = {
                success: function (result) {
                    options.success(result);
                },
                format: 'json',
                method: 'get',
                role: "ams",
                key: "userGroup",
                denied: function () {
                    // Just do it again and we should land in the success callback next time
                    $.insmFramework('getUsers', options);
                }
            };
            $.insmFramework('ajax', {
                url: _plugin.settings.apiUrl + '/AppSettings.aspx',
                data: data
            });
            return $this;
        }, 
        ajax: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var restartOptions = $.extend(true, {}, options);

            return $.ajax({
                type: options.type,
                url: options.url,
                contentType: options.contentType,
                data: options.data,
                success: options.success,
                error: options.error
            });
        },
        editGroup: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/GroupMasters' + "/" + options.groupID,
                format: 'json',
                contentType: "application/json; charset=utf-8",
                type: "GET",
                denied: function () {
                },
                error: function () {
                    options.error();
                },
            };
            return $.insmFramework('ajax', ajaxOptions);
        },
        getGroupTreeData: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/GroupMasters/GetJSTreeData',
                format: 'json',
                contentType: "application/json; charset=utf-8",
                type: "GET",
                denied: function () { },
                error: function () {
                    options.error();
                },
            }
            return $.insmFramework('ajax', ajaxOptions);
        },
        getUser: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');

            return _plugin.settings.user;
        },
        creatGroup: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var GroupMaster = {
                create: function () {
                    GroupName: "";
                    GroupParentID: '';
                    ActiveFlag: '';
                    OnlineFlag: '';
                    Settings: '';
                    Comments: '';
                    return GroupMaster;
                }
            }
            var newGroup = GroupMaster.create();
            if (options.groupID != undefined) {
                newGroup.GroupID = options.groupID;
            }
            newGroup.GroupName = options.newGroupName;
            newGroup.GroupParentID = options.newGroupNameParentID;
            newGroup.ActiveFlag = options.active;
            newGroup.OnlineFlag = options.onlineUnits;
            newGroup.Comments = options.note;
            newGroup.Settings = options.settings;
            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url:options.groupID == undefined ? 'api/GroupMasters' : 'api/GroupMasters' + "/" + options.groupID,
                format: 'json',
                data: JSON.stringify(newGroup),
                contentType: "application/json; charset=utf-8",
                type: options.groupID == undefined ? 'POST' : 'PUT',
                denied: function () {
                    // Just do it again and we should land in the success callback next time
                    //$.insmFramework('getUsers', options);
                },
                error: function () {
                    options.error();
                },
            };
            return $.insmFramework('ajax', ajaxOptions);
        },

        deleteGroup: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');

            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/GroupMasters' + "/" + options.deleteGroupId,
                format: 'json',
                data: '',
                contentType: "application/json; charset=utf-8",
                type: "DELETE",
                denied: function () {
                }
            };
            return $.insmFramework('ajax', ajaxOptions);
        },
        login: function (options) {

            var data = {
                username: options.username,
                password: options.password
            };
            return _plugin.data.loginDeferred;
        },
        logout: function (options) {
        // Global vars
        var $this = $('html').eq(0);
        var _plugin = $this.data('insmFramework');
        _plugin.data.previousUsername = _plugin.settings.user.name;
        var logoutDeferred = $.insmFramework('ajax', {
            url: _plugin.settings.apiUrl + '/Logout.aspx',
            data: {
                success: function () {
                    // We never get here but if we do in the future we want to do the same as in the denied callback.
                    _plugin.data.user = {};
                    delete _plugin.settings.session;
                    if (!!window.localStorage && typeof (Storage) !== "undefined") {
                        delete localStorage.insmFrameworkSession;
                    }
                },
                denied: function () {
                    _plugin.data.user = {};
                    delete _plugin.settings.session;
                    if (!!window.localStorage && typeof (Storage) !== "undefined") {
                        delete localStorage.insmFrameworkSession;
                    }
                }
            }
        });

        return logoutDeferred;
        },
        getGroupPlayers: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/PlayerMasters/GetPlayerWithChildByGroupID/' + options.GroupID,
                format: 'json',
                contentType: "application/json; charset=utf-8",
                type: "GET",
                denied: function () { },
                error: function () {
                    options.error();
                },
            }
            return $.insmFramework('ajax', ajaxOptions);
        },
        creatPlayer: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var PlayerMaster = {
                create: function () {
                    GroupID: '';
                    PlayerName: '';
                    PlayerAddress: '';
                    Comments: '';
                    ActiveFlag: '';
                    OnlineFlag: '';
                    Settings: '';
                    return PlayerMaster;
                }
            }
            var newPlayer = PlayerMaster.create();
            newPlayer.GroupID = options.GroupID;
            newPlayer.PlayerName = options.PlayerName;
            newPlayer.Comments = options.note;
            newPlayer.ActiveFlag = options.active;
            newPlayer.OnlineFlag = options.onlineUnits;

            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/PlayerMasters',
                format: 'json',
                data: JSON.stringify(newPlayer),
                contentType: "application/json; charset=utf-8",
                type: options.groupID == undefined ? 'POST' : 'PUT',
                denied: function () {
                    // Just do it again and we should land in the success callback next time
                    //$.insmFramework('getUsers', options);
                },
                error: function () {
                    options.error();
                },
            };
            return $.insmFramework('ajax', ajaxOptions);
        },
        editGroupPlayers: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var editPlayers = [];
            var PlayerMaster = {
                create: function () {
                    GroupID: '';
                    PlayerName: '';
                    PlayerAddress: '';
                    Comments: '';
                    ActiveFlag: '';
                    OnlineFlag: '';
                    Settings: '';
                    return PlayerMaster;
                }
            }
            var playerEditDeferredList = [];
            $.each(options.Playerdata, function (index, item) {
                var tempPlayerEditDrferred = new $.Deferred();
                playerEditDeferredList.push(tempPlayerEditDrferred);
                var Player = $(options.Playerdata[index]).data().obj;
                var newPlayer = PlayerMaster.create();

                newPlayer.PlayerID = Player.PlayerID;
                newPlayer.PlayerName = Player.PlayerName;
                newPlayer.Comments = Player.Comments;
                newPlayer.ActiveFlag = Player.ActiveFlag;
                newPlayer.OnlineFlag = Player.OnlineFlag;
                newPlayer.GroupID = options.newGroupID;

                var ajaxOptions = {
                    success: function (result) {
                        tempPlayerEditDrferred.resolve();
                    },
                    url: 'api/PlayerMasters',
                    format: 'json',
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(newPlayer),
                    type: "PUT",
                    denied: function () { },
                    error: function () {
                        options.error();
                    },
                }
                if (index == options.Playerdata.length-1) {
                    return $.insmFramework('ajax', ajaxOptions);
                } else {
                    $.insmFramework('ajax', ajaxOptions);
                }
            });     
            $.when(playerEditDeferredList).done(function () {
                options.success();
            });
            
        },
        getPlayerStaus: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');


            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/PlayerMasters',
                format: 'json',
                data: JSON.stringify(newPlayer),
                contentType: "application/json; charset=utf-8",
                type: options.groupID == undefined ? 'POST' : 'PUT',
                denied: function () {
                    // Just do it again and we should land in the success callback next time
                    //$.insmFramework('getUsers', options);
                },
                error: function () {
                    options.error();
                },
            };
            return $.insmFramework('ajax', ajaxOptions);
        },
        //Folder Start
        createFolder: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var FolderMaster = {
                create: function () {
                    FolderID: "";
                    GroupID: "";
                    FolderName: '';
                    FolderParentID: '';
                    Settings: '';
                    Comments: '';
                    return FolderMaster;
                }
            }
            var newFolder = FolderMaster.create();
            newFolder.GroupID = options.groupID;
            newFolder.FolderName = options.folderName;
            newFolder.FolderParentID = options.folderParentID;
            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/FolderMasters/GetJSTreeNodeDataByCreate',
                format: 'json',
                data: JSON.stringify(newFolder),
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                denied: function () {
                    // Just do it again and we should land in the success callback next time
                    //$.insmFramework('getUsers', options);
                },
                error: function () {
                    options.error();
                },
            };
            return $.insmFramework('ajax', ajaxOptions);
        },
        editFolder: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var FolderMaster = {
                create: function () {
                    FolderID: "";
                    GroupID: "";
                    FolderName: '';
                    FolderParentID: '';
                    Settings: '';
                    Comments: '';
                    return FolderMaster;
                }
            }
            var newFolder = FolderMaster.create();
            newFolder.FolderID = options.folderID;
            newFolder.GroupID = options.groupID;
            newFolder.FolderName = options.folderName;
            newFolder.FolderParentID = options.folderParentID;
            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/FolderMasters/EditTreeNodeFolder',
                format: 'json',
                data: JSON.stringify(newFolder),
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                denied: function () {
                    // Just do it again and we should land in the success callback next time
                    //$.insmFramework('getUsers', options);
                },
                error: function () {
                    options.error();
                },
            };
            return $.insmFramework('ajax', ajaxOptions);
        },
        getFolderTreeDataForPlaylist: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/FolderMasters/GetFolderJSTreeNodeWithInheritForcedByGroupID/' + options.groupID,
                format: 'json',
                contentType: "application/json; charset=utf-8",
                type: "GET",
                denied: function () { },
                error: function () {
                    options.error();
                },
            }
            return $.insmFramework('ajax', ajaxOptions);
        },
        getFolderTreeData: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/FolderMasters/GetJSTreeData/' + options.groupID,
                format: 'json',
                contentType: "application/json; charset=utf-8",
                type: "GET",
                denied: function () { },
                error: function () {
                    options.error();
                },
            }
            return $.insmFramework('ajax', ajaxOptions);
        },
        deleteFolder: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');

            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/FolderMasters' + "/" + options.folderID,
                format: 'json',
                contentType: "application/json; charset=utf-8",
                type: "DELETE",
                denied: function () {
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    options.error(XMLHttpRequest, textStatus, errorThrown);
                },
            };
            return $.insmFramework('ajax', ajaxOptions);
        },
        //Folder End

        //File Start
        getFilesByFolder: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/FileMasters/GetFilesByFolderID/' + options.FolderID,
                format: 'json',
                contentType: "application/json; charset=utf-8",
                type: "GET",
                denied: function () { },
                error: function () {
                    options.error();
                },
            }
            return $.insmFramework('ajax', ajaxOptions);
        },
        copyFile: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/FileMasters/CopyFile/' + options.newFolderID,
                format: 'json',
                data: JSON.stringify(options.sourceFile),
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                denied: function () {
                    // Just do it again and we should land in the success callback next time
                    //$.insmFramework('getUsers', options);
                },
                error: function () {
                    options.error();
                },
            };
            return $.insmFramework('ajax', ajaxOptions);
        },
        cutFile: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/FileMasters/CutFile/' + options.newFolderID,
                format: 'json',
                data: JSON.stringify(options.sourceFile),
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                denied: function () {
                    // Just do it again and we should land in the success callback next time
                    //$.insmFramework('getUsers', options);
                },
                error: function () {
                    options.error();
                },
            };
            return $.insmFramework('ajax', ajaxOptions);
        },
        deleteFile: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');

            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/FileMasters' + "/" + options.fileID,
                format: 'json',
                contentType: "application/json; charset=utf-8",
                type: "DELETE",
                denied: function () {
                }
            };
            return $.insmFramework('ajax', ajaxOptions);
        },
        //File End 
        creatPlaylist: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var PlaylistMaster = {
                create: function () {
                    GroupID: "";
                    PlayListName: '';
                    InheritForced: '';
                    Settings: '';
                    Comments: '';
                    return PlaylistMaster;
                }
            }
            var newPlaylist = PlaylistMaster.create();
            newPlaylist.GroupID = options.GroupID;
            newPlaylist.PlayListName = options.PlayListName;
            newPlaylist.InheritForced = options.InheritForced;
            newPlaylist.Settings = options.Settings;
            newPlaylist.Comments = options.Comments;

            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/PlayListMasters',
                format: 'json',
                data: JSON.stringify(PlaylistMaster),
                contentType: "application/json; charset=utf-8",
                type:'POST',
                denied: function () {
                    // Just do it again and we should land in the success callback next time
                    //$.insmFramework('getUsers', options);
                },
                error: function () {
                    options.error();
                },
            };
            return $.insmFramework('ajax', ajaxOptions);
        },
        editPlaylist: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var PlaylistMaster = {
                create: function () {
                    GroupID: "";
                    PlayListName: '';
                    InheritForced: '';
                    Settings: '';
                    Comments: '';
                    PlayListID: '';
                    return PlaylistMaster;
                }
            }
            var newPlaylist = PlaylistMaster.create();
            newPlaylist.GroupID = options.GroupID;
            newPlaylist.PlayListName = options.PlayListName;
            newPlaylist.PlayListID = options.playlistId;
            newPlaylist.InheritForced = options.InheritForced;
            newPlaylist.Settings = options.Settings;
            newPlaylist.Comments = options.Comments;

            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/PlayListMasters/' + options.playlistId,
                format: 'json',
                data: JSON.stringify(PlaylistMaster),
                contentType: "application/json; charset=utf-8",
                type: 'PUT',
                denied: function () {
                    // Just do it again and we should land in the success callback next time
                    //$.insmFramework('getUsers', options);
                },
                error: function () {
                    options.error();
                },
            };
            return $.insmFramework('ajax', ajaxOptions);
        },
        getPlaylistByGroup: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/PlayListMasters/GetOwnPlayListWithInheritByGroupID/' + options.GroupID,
                format: 'json',
                contentType: "application/json; charset=utf-8",
                type: "GET",
                denied: function () { },
                error: function () {
                    options.error();
                },
            }
            return $.insmFramework('ajax', ajaxOptions);
        },
        getForcedPlaylistByPlayer: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/PlayListMasters/GetTotalPlayListByPlayerID/' + options.playerId,
                format: 'json',
                contentType: "application/json; charset=utf-8",
                type: "POST",
                denied: function () { },
                error: function () {
                    options.error();
                },
            }
            return $.insmFramework('ajax', ajaxOptions);
        },
        getForcedPlaylistByGroup: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/PlayListMasters/GetForcedPlayListByGroupID/' + options.groupID,
                contentType: "application/json; charset=utf-8",
                type: "POST",
                denied: function () { },
                error: function () {
                    options.error();
                },
            }
            return $.insmFramework('ajax', ajaxOptions);
        },
        getPlaylistByPlayerID: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/PlayListMasters/' + options.playerID,
                format: 'json',
                contentType: "application/json; charset=utf-8",
                type: "GET",
                denied: function () { },
                error: function () {
                    options.error();
                },
            }
            return $.insmFramework('ajax', ajaxOptions);
        },
        getPlaylistByPlaylistID: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/PlayListMasters/' + options.playlistID,
                format: 'json',
                contentType: "application/json; charset=utf-8",
                type: "GET",
                denied: function () { },
                error: function () {
                    options.error();
                },
            }
            return $.insmFramework('ajax', ajaxOptions);
        },
        getPlaylistByGroupID: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');
            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/PlayListMasters/GetPlayListByGroupID/' + options.GroupID,
                format: 'json',
                contentType: "application/json; charset=utf-8",
                type: "POST",
                denied: function () { },
                error: function () {
                    options.error();
                },
            }
            return $.insmFramework('ajax', ajaxOptions);
        },
        playerPlayListLinkTables: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');

            var PlayerPlayListLink = {
                create: function () {
                    Index: "";
                    PlayerID: "";
                    PlayListID: '';
                    return PlayerPlayListLink;
                }
            }
            var playerPlayListLinkList = [];
            $.each(options.PlayListID, function (index, objId) {
                var tempPlayerPlayListLinkDrferred = new $.Deferred();
                playerPlayListLinkList.push(tempPlayerPlayListLinkDrferred)

                var newPlayerPlayList = PlayerPlayListLink.create();
                newPlayerPlayList.Index = index + 1;
                newPlayerPlayList.PlayerID = options.playerId;
                newPlayerPlayList.PlayListID = objId;

                var ajaxOptions = {
                    success: function (result) {
                        options.success(result);
                    },
                    url: 'api/PlayerPlayListLinkTables',
                    format: 'json',
                    data: JSON.stringify(newPlayerPlayList),
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    denied: function () { },
                    error: function () {
                        options.error();
                    },
                }
                if (index == options.PlayListID.length - 1) {
                    return $.insmFramework('ajax', ajaxOptions);
                } else {
                    $.insmFramework('ajax', ajaxOptions);
                }
            });
            $.when(playerPlayListLinkList).done(function () {
                options.success();
            });
        },
        GroupPlayListLinkTables: function (options) {

            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');

            var GroupPlayListLink = {
                create: function () {
                    Index: "";
                    GroupID: "";
                    PlayListID: "";
                    return GroupPlayListLink;
                }
            };

            var groupPlayListLinkDeferredList = [];        

            $.each(options.PlayListID, function (index, objId) {
                var tempGroupPlayListLinkDrferred = new $.Deferred();
                groupPlayListLinkDeferredList.push(tempGroupPlayListLinkDrferred);
                var newGroupPlayList = GroupPlayListLink.create();
                newGroupPlayList.Index = index + 1;
                newGroupPlayList.GroupID = options.groupID;
                newGroupPlayList.PlayListID = objId;

                var ajaxOptions = {
                    success: function (result) {
                        options.success(result);
                    },
                    url: 'api/GroupPlayListLinkTables',
                    format: 'json',
                    data: JSON.stringify(newGroupPlayList),
                    contentType: "application/json; charset=utf-8",
                    type: "POST",
                    denied: function () { },
                    error: function () {
                        options.error();
                    },
                }
                if (index == options.PlayListID.length - 1) {
                    return $.insmFramework('ajax', ajaxOptions);
                } else {
                    $.insmFramework('ajax', ajaxOptions);
                }
            });
            $.when(groupPlayListLinkDeferredList).done(function () {
                options.success();
            });
        },
        
        deletePlaylist: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');

            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/PlayListMasters' + "/" + options.deletePlaylistId,
                format: 'json',
                data: '',
                contentType: "application/json; charset=utf-8",
                type: "DELETE",
                denied: function () {
                }
            };
            return $.insmFramework('ajax', ajaxOptions);
        },
        deleteGroupPlayListLinkTableByGroupID: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');

            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/GroupPlayListLinkTables/DeleteGroupPlayListLinkTableByGroupID/' + options.groupID,
                format: 'json',
                data: '',
                contentType: "application/json; charset=utf-8",
                type: "POST",
                denied: function () {
                }
            };
            return $.insmFramework('ajax', ajaxOptions);
        },
        deletePlayerPlayListLinkTableByPlayerID: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');

            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/PlayerPlayListLinkTables/DeletePlayerPlayListLinkTableByPlayerID/' + options.playerId,
                format: 'json',
                data: '',
                contentType: "application/json; charset=utf-8",
                type: "POST",
                denied: function () {
                }
            };
            return $.insmFramework('ajax', ajaxOptions);
        },
        playerStatusShare: function (options) {
            var $this = $('html').eq(0);
            var _plugin = $this.data('insmFramework');

            var ajaxOptions = {
                success: function (result) {
                    options.success(result);
                },
                url: 'api/PlayerMasters/GetPlayerStatusReportData',
                format: 'json',
                contentType: "application/json; charset=utf-8",
                type: "GET",
                denied: function () { },
                error: function () {
                    options.error();
                },
            }
            return $.insmFramework('ajax', ajaxOptions);
        },
    }
    $.insmFramework = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on $.insmFramework');
        }
        return null;
    };
    })(jQuery);