var FolderTreedata = [];



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




    var folderJstreeData = {
        "core": {
            "themes": {
                "responsive": true
            },
            // so that create works
            "check_callback": true,
            'data': FolderTreedata
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

    //folder tree
    var initFolderTree = function (selectedGroupID) {
        $.insmFramework('getFolderTreeData', {
            groupID: selectedGroupID,
            success: function (tempdataFolderTreeData) {
                if (tempdataFolderTreeData) {
                    var tree = $('.tree-demo.folderTree');
                    tree.jstree(folderJstreeData);
                    $.each(tree, function (key, item) {
                        $(item).jstree(true).settings.core.data = tempdataFolderTreeData;
                        $(item).jstree(true).refresh();
                    });

                    //div_groupTree.on("changed.jstree", function (e, data) {
                    //    //存储当前选中的区域的名称
                    //    if (data.node) {
                    //        selectedGroupID = data.node.id;
                    //        showPlayerDetail({ GroupID: selectedGroupID });
                    //    }
                    //});

                    //$(div_groupTreeForPlayerEdit).on("changed.jstree", function (e, data) {
                    //    //存储当前选中的区域的名称
                    //    if (data.node) {
                    //        groupTreeForPlayerEditID = data.node.id;
                    //    }
                    //});
                    //tree.on("move_node.jstree", function (e, data) {
                    //    var node = data.node;
                    //    if (node) {
                    //        editgroup({
                    //            groupID: node.id,
                    //            newGroupNameParentID: node.parent,
                    //            newGroupName: node.text,
                    //            ActiveFlag: node.li_attr.ActiveFlag,
                    //            OnlineFlag: node.li_attr.OnlineFlag,
                    //            Comments: node.li_attr.Comments
                    //        })
                    //    }
                    //});
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

 
    function demo_rename() {
        var ref = div_groupTreeForFileManager.jstree(true),
            sel = ref.get_selected();
        if (!sel.length) { return false; }
        sel = sel[0];
        ref.edit(sel);
    };
    function demo_delete() {
        var ref = div_groupTreeForFileManager.jstree(true),
            sel = ref.get_selected();
        if (!sel.length) { return false; }
        ref.delete_node(sel);
    };

    //var createFolderForGroup = $.insmFramework('creatFolder', {
    //    groupID: selectedGroupID,
    //    newGroupName: $("#groupname").val(),
    //    active: $("input[name='radio_Active']:checked").val(),
    //    onlineUnits: $("input[name='radio_Online']:checked").val(),
    //    //resolution:$("#select_resolution").find("option:selected").text(),
    //    note: $("#text_note").val(),
    //    newGroupNameParentID: groupTreeForPlayerEditID,
    //    success: function (data) {
    //        div_main.show();
    //        div_edit.hide();
    //        initGroupTree();
    //        editGroupID = undefined;
    //    }
    //})

    var changeTab = function (tabDivId) {
        $(".mainPageTabDiv").hide();
        $("#" + tabDivId).show();
        $("#m_ver_menu").find("li.m-menu__item").removeClass("m-menu__item--active");
        $(event.currentTarget).parent("li").addClass("m-menu__item--active");
    }
$(document).ready(function ()
{
    playerStatusShare();
    $.insmGroup({});
    $("#span_success").text(100);
});