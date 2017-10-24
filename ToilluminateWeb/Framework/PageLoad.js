﻿var GroupTreedata = [];
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



var playerStatusShare = function () {
        if ($('#m_chart_player_status_share').length == 0) {
            return;
        }

        var chart = new Chartist.Pie('#m_chart_player_status_share', {
            series: [{
                value: 46,
                className: 'custom',
                meta: {
                    color: mUtil.getColor('success')
                }
            },
                {
                    value: 4,
                    className: 'custom',
                    meta: {
                        color: mUtil.getColor('danger')
                    }
                },
                {
                    value: 2,
                    className: 'custom',
                    meta: {
                        color: mUtil.getColor('warning')
                    }
                },
                {
                    value: 15,
                    className: 'custom',
                    meta: {
                        color: mUtil.getColor('metal')
                    }
                }
            ],
            labels: [1, 2, 3]
        }, {
            donut: true,
            donutWidth: 17,
            showLabel: false
        });

        chart.on('draw', function (data) {
            if (data.type === 'slice') {
                // Get the total path length in order to use for dash array animation
                var pathLength = data.element._node.getTotalLength();

                // Set a dasharray that matches the path length as prerequisite to animate dashoffset
                data.element.attr({
                    'stroke-dasharray': pathLength + 'px ' + pathLength + 'px'
                });

                // Create animation definition while also assigning an ID to the animation for later sync usage
                var animationDefinition = {
                    'stroke-dashoffset': {
                        id: 'anim' + data.index,
                        dur: 1000,
                        from: -pathLength + 'px',
                        to: '0px',
                        easing: Chartist.Svg.Easing.easeOutQuint,
                        // We need to use `fill: 'freeze'` otherwise our animation will fall back to initial (not visible)
                        fill: 'freeze',
                        'stroke': data.meta.color
                    }
                };

                // If this was not the first slice, we need to time the animation so that it uses the end sync event of the previous animation
                if (data.index !== 0) {
                    animationDefinition['stroke-dashoffset'].begin = 'anim' + (data.index - 1) + '.end';
                }

                // We need to set an initial value before the animation starts as we are not in guided mode which would do that for us

                data.element.attr({
                    'stroke-dashoffset': -pathLength + 'px',
                    'stroke': data.meta.color
                });

                // We can't use guided mode as the animations need to rely on setting begin manually
                // See http://gionkunz.github.io/chartist-js/api-documentation.html#chartistsvg-function-animate
                data.element.animate(animationDefinition, false);
            }
        });

        // For the sake of the example we update the chart every time it's created with a delay of 8 seconds
        chart.on('created', function () {
            if (window.__anim21278907124) {
                clearTimeout(window.__anim21278907124);
                window.__anim21278907124 = null;
            }
            window.__anim21278907124 = setTimeout(chart.update.bind(chart), 15000);
        });
    }

    var jstreeData = {
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
    var initGroupTree = function () {
        $.insmFramework('getGroupTreeData', {
            success: function (tempdataGroupTreeData) {
                if (tempdataGroupTreeData) {
                    //GroupData = tempdataGroupTreeData;
                    //GroupTreedata = [];
                    //scanRoot(tempdataGroupTreeData, GroupTreedata);

                    var tree = $('.tree-demo');
                    //jstreeData.core.data = tempdataGroupTreeData;
                    tree.jstree(jstreeData);
                    $.each(tree, function (key, item) {
                        $(item).jstree(true).settings.core.data = tempdataGroupTreeData;
                        $(item).jstree(true).refresh();
                    });

                    div_groupTree.on("changed.jstree", function (e, data) {
                        //存储当前选中的区域的名称
                        if (data.node) {
                            selectedGroupID = data.node.id;
                            showPlayerDetail({ GroupID: selectedGroupID });
                        }
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
                            editgroup({
                                groupID: node.id,
                                newGroupNameParentID: node.parent,
                                newGroupName: node.text,
                                ActiveFlag: node.li_attr.ActiveFlag,
                                OnlineFlag: node.li_attr.OnlineFlag,
                                Comments: node.li_attr.Comments
                            })
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
    }
    var editgroup = function (options) {
        var newGroupdata = $.insmFramework('creatGroup', {
            groupID: options.groupID,
            newGroupName: options.newGroupName,
            active: options.ActiveFlag,
            onlineUnits: options.OnlineFlag,
            note: options.Comments,
            newGroupNameParentID: options.newGroupNameParentID,
            success: function (data) {
                initGroupTree();
                editGroupID = undefined;
            }
        })
    }
    var addNewGroup = function () {
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
                initGroupTree();
                editGroupID = undefined;
            }
        })
    }

    $("#newgroup").click(function (e) {
        div_main.hide();
        div_edit.show();
        defaultDataSet();
        editGroupID = undefined;
    })   
    $("#deletegroup").click(function (e) {
        var newGroupdata = $.insmFramework('deleteGroup', {
            deleteGroupId: selectedGroupID,
            success: function (resultdata) {
                div_main.show();
                div_edit.hide();
                initGroupTree();
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
        $.insmFramework('editGroup', {
            groupID: selectedGroupID,
            success: function (userGroupData) {
                if (userGroupData) {
                    $("input[name='radio_Active'][value='" + userGroupData.ActiveFlag + "]").click();
                    $("input[name='radio_Online'][value='" + userGroupData.OnlineFlag + "]").click();
                    $("#groupname").val(userGroupData.GroupName);
                    $("#text_note").val(userGroupData.Comments);
                    editGroupID = selectedGroupID;
                }
            }
        })
    });
    $("#button_save").click(function(e) {
                addNewGroup();
                editGroupID = undefined;
        })
    $("#button_back").click(function () {
        div_main.show();
        div_edit.hide();
    });

    $("#add_player").click(function () {
        div_main.hide();
        div_edit.show();
        $("#button_save_Player").css('display', 'block').removeClass('m-dropdown__toggle');
        $("#button_save").css('display', 'none');
    });
    $("#button_save_Player").click(function () {
        div_main.show();
        div_edit.hide();
        if ($.trim($("#groupname").val()) == '') {
            alert('Group name is empty!');
            return;
        }
        var newGroupdata = $.insmFramework('creatPlayer', {
            groupID: editGroupID,
            newGroupName: $("#groupname").val(),
            active: $("input[name='radio_Active']:checked").val(),
            onlineUnits: $("input[name='radio_Online']:checked").val(),
            //resolution:$("#select_resolution").find("option:selected").text(),
            note: $("#text_note").val(),
            newGroupNameParentID: groupTreeForPlayerEditID,
            ActivechangeFlg: ActivechangeFlg,
            OnlinechangeFlg: OnlinechangeFlg,
            DisplayNamechangeFlg: DisplayNamechangeFlg,
            NotechangeFlg: DisplayNamechangeFlg,
            success: function (data) {
                div_main.show();
                div_edit.hide();
                initGroupTree();
                editGroupID = undefined;
            }
        })
    });

    var DatatableResponsiveColumnsDemo = function (options) {
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
           

        $('#m_form_search').on('keyup', function (e) {
            //// shortcode to datatable.getDataSourceParam('query');
            //var query = datatable.getDataSourceQuery();
            //query.generalSearch = $(this).val().toLowerCase();
            //// shortcode to datatable.setDataSourceParam('query', query);
            //datatable.setDataSourceQuery(query);
            //datatable.load();
            datatable.search($(this).val().toLowerCase(), "PlayerName");
        });//.val(query.generalSearch);

        $('#m_form_status, #m_form_type').selectpicker();
    }
    var defaultDataSet = function () {
    $("input[name='radio_Active'][value='null]").click();
    $("input[name='radio_Online'][value='null]").click();
    $("#groupname").val('');
    $("#text_note").val('');
}
    var showPlayerDetail = function (options) {
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
            DatatableResponsiveColumnsDemo({ PlayersData: data });
        }
    })
}

    $("#radio_All").click(function () {
    DatatableResponsiveColumnsDemo({ PlayersData: player_Alldata });
})
    $("#radio_Current").click(function () {
    var Current_data = [];
    $.each(player_Alldata, function (key, item) {
        if (item.GroupID == selectedGroupID) {
            Current_data.push(item)
        }
    });
    DatatableResponsiveColumnsDemo({ PlayersData: Current_data });
})
    $("#edit_player").click(function () {
    div_main.hide();
    div_edit.show();
    $("#button_save_Player").css('display', 'block').removeClass('m-dropdown__toggle');
    $("#button_save").css('display', 'none');
    //var editPlayer = [];
    $.each(selectPlayerdata, function (index, item) {
        //editPlayer.push((selectPlayerdata[index]).data().obj);
        //$("input[name='radio_Active'][value='null]").click();
        //$("input[name='radio_Online'][value='null]").click();
        
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
$(document).ready(function ()
{
    div_edit.hide();
    playerStatusShare();
    initGroupTree();
    defaultDataSet();
    $("#PlayerDetail").css('display', 'none');
});