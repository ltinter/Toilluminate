(function ($) {
    var _guid = 0;
    var GroupTreedata = [];
    
    var GroupData;
    var selectPlayerdata;

    var editGroupID;
    var selectedGroupID = null;
    var groupTreeForPlayerEditID = null;
    var div_main = $("#div_main");
    var div_edit = $("#div_edit");
    var div_groupTree = $("#groupTree");
    var div_groupTreeForPlayerEdit = $("#groupTreeForPlayerEdit");
    var player_Alldata;

    var ActivechangeFlg = false;
    var OnlinechangeFlg = false;
    var DisplayNamechangeFlg = false;
    var NotechangeFlg = false;

    
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
                "icon": "fa fa-sitemap  m--font-success"
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
            return $this;
        },
        editgroup: function (options) {
            var newGroupdata = $.insmFramework('creatGroup', {
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
        },
        defaultDataSet : function () {
            $("#label_Active_null").click();
            $("#label_Online_null").click();
            $("#groupname").val('');
            $("#text_note").val('');
        },
        showPlayerDetail : function (options) {
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
        DatatableResponsiveColumnsDemo : function (options) {
            $('#base_responsive_columns').prop("outerHTML", "<div class='m_datatable' id='base_responsive_columns'></div>");
            var datatable = $('#base_responsive_columns').mDatatable({
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
                    pageSize: 10,
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

                pagination: true,

                // columns definition
                columns: [{
                    field: "RecordID",
                    title: "#",
                    sortable: false, // disable sort for this column
                    width: 40,
                    textAlign: 'center',
                    filterable: false,
                    selector: { class: 'm-checkbox--solid m-checkbox--brand' }
                }, {
                    field: "PlayerID",
                    title: "PlayerID",
                    filterable: false, // disable or enable filtering
                    width: 150
                }, {
                    field: "PlayerName",
                    title: "PlayerName",
                    responsive: { visible: 'lg' }
                }, {
                    field: "GroupName",
                    title: "GroupName",
                    width: 200,
                    filterable: false,
                    responsive: { visible: 'lg' }
                }, {
                    field: "UpdateDate",
                    title: "UpdateDate",
                    field: "GreatDate",
                    title: "GreatDate",
                    filterable: false,
                    responsive: { visible: 'lg' }
                }, {
                    field: "Online",
                    title: "Online",
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
        addNewGroup : function () {
            if ($.trim($("#groupname").val()) == '') {
                alert('Group name is empty!');
                return;
            }
            var newGroupdata = $.insmFramework('creatGroup', {
                groupID: editGroupID,
                newGroupName: $("#groupname").val(),
                active: $("input[name='radio_Active']:checked").val(),
                onlineUnits: $("input[name='radio_Online']:checked").val(),
                //resolution:$("#select_resolution").find("option:selected").text(),
                note: $("#text_note").val(),
                newGroupNameParentID: groupTreeForPlayerEditID,
                success: function (data) {
                    div_main.show();
                    div_edit.hide();
                    $.insmGroup('initGroupTree');
                    editGroupID = undefined;
                }
            })
        }
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
        div_main.hide();
        div_edit.show();
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
                }
            }
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
    });
    $("#button_save_Player").click(function () {
        div_main.show();
        div_edit.hide();
        //if ($.trim($("#groupname").val()) == '') {
        //    alert('Player name is empty!');
        //    return;
        //}
        if (1==1) {
            $.insmFramework('creatPlayer', {
                GroupID: groupTreeForPlayerEditID,
                newGroupName: $("#groupname").val(),
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
                }
            })
        } else {
            $.insmFramework('editGroupPlayers', {
                Playerdata: selectPlayerdata,
                ActivechangeFlg: ActivechangeFlg,
                OnlinechangeFlg: OnlinechangeFlg,
                DisplayNamechangeFlg: DisplayNamechangeFlg,
                NotechangeFlg: DisplayNamechangeFlg,
                success: function (data) {
                    div_main.show();
                    div_edit.hide();
                    $.insmGroup('initGroupTree');
                    editGroupID = undefined;
                }
            })
        }
        
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
    $("#edit_player").click(function () {
        if (!selectPlayerdata) { return; }
        div_main.hide();
        div_edit.show();
        $.insmGroup('defaultDataSet');
        $("#button_save_Player").css('display', 'block').removeClass('m-dropdown__toggle');
        $("#button_save").css('display', 'none');
        $.each(selectPlayerdata, function (index, item) {

            if (index != 0) {
                if ($("#groupname").val() != $(selectPlayerdata[index]).data().obj.PlayerName) {
                    $("#groupname").val('');
                }
                if ($("#groupname").val() != $(selectPlayerdata[index]).data().obj.PlayerName) {
                    $("#text_note").val('');
                }
            } else {
                $("#groupname").val($(selectPlayerdata[index]).data().obj.PlayerName);
                $("#text_note").val($(selectPlayerdata[index]).data().obj.Comments);

                $("#label_Active_" + $(selectPlayerdata[index]).data().obj.ActiveFlag).click();
                $("#label_Online_" + $(selectPlayerdata[index]).data().obj.OnlineFlag).click();
            }
        });

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
})(jQuery);