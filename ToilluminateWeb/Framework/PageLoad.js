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
                "icon": "fa fa-folder m--font-success"
            },
            "folder": {
                "icon": "fa fa-folder  m--font-success"
            }
        },
        "state": {
            "key": "demo2"
        },
        "plugins": ["dnd", "state", "types"]

    };
    var selectedGroupID = null;
    var selectedFolderID = null;
    var div_groupTreeForFileManager = $("#groupTreeForFileManager");
    var div_folderTreeForFileManager = $("#folderTreeForFileManager");
    //folder tree
    var initFolderTree = function (selectedGroupID) {
        $.insmFramework('getFolderTreeData', {
            groupID: selectedGroupID,
            success: function (tempdataFolderTreeData) {
                if (tempdataFolderTreeData) {
                    var tree = $('.tree-demo.folderTree');
                    tree.jstree(folderJstreeData);
                    tree.jstree(true).settings.core.data = tempdataFolderTreeData;
                    tree.jstree(true).refresh();

                    tree.on("changed.jstree", function (e, data) {
                        if (data.node) {
                            selectedFolderID = data.node.id;
                            //showFolderFiles({ FolderID: selectedFolderID });
                        }
                    });

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

    $("#btn_create").click(function () {
        var groupRef = div_groupTreeForFileManager.jstree(true),
            groupSel = groupRef.get_selected();
        if (!groupSel.length) { return false; }// no group selected
        var folderRef = div_folderTreeForFileManager.jstree(true),
            folderSef = folderRef.get_selected();
        if (!folderSef.length) {
            folderSef = folderRef.create_node(null, { "type": "folder" });//create root
        } else {
            folderSef = folderRef.create_node(folderSef, { "type": "folder" });
        }
        if (folderSef) {
            folderRef.edit(folderSef);
        }
        //$.insmFramework('creatFolder', {
        //    groupID: selectedGroupID,
        //    folderName: "New Folder",
        //    folderParentID: selectedFolderID,
        //    success: function (data) {
        //        var a = 1;
        //        //sel = sel[0];
        //        //sel = ref.create_node(sel, { "type": "file" });
        //        //if (sel) {
        //        //    ref.edit(sel);
        //        //}
        //    }
        //});
    });
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


$(document).ready(function ()
{
    playerStatusShare();
    $.insmGroup({});
});