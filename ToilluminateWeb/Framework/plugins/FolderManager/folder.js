(function ($) {
    var FolderTreedata = [];
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
    
    var methods = {
        init: function (options) {
            $.insmFramework('getFolderTreeData', {
                groupID: options.selectedGroupID,
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
    };
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
    $.folder = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on $.insmGroup');
        }
        return null;
    };
})(jQuery);