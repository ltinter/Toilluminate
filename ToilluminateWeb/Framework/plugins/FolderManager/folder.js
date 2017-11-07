(function ($) {
    var folderJstreeData = {
        "core": {
            "themes": {
                "responsive": true
            },
            // so that create works
            "check_callback": true,
            'data': {}
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
        "plugins": ["contextmenu", "dnd", "state", "types", "search"],
        "contextmenu": {
            "items": {
                "Create": {
                    "label": "Create",
                    "action": function (obj) {
                        $.folder('createFolder');
                    }
                },
                "Rename": {
                    "label": "Rename",
                    "action": function (obj) {
                        var inst = jQuery.jstree.reference(obj.reference);
                        var clickedNode = inst.get_node(obj.reference);
                        inst.edit(obj.reference, clickedNode.val, function (obj, tmp, nv, cancel) {
                            $.folder('editFolder', obj);
                        });
                    }
                },
                "Remove": {
                    "label": "Delete",
                    "action": function (obj) {
                        $.folder('deleteFolder');
                    }
                }
            }
        }
    };
    var selectedGroupID = null;
    var selectedFolderID = null;
    var div_groupTreeForFileManager = $("#groupTreeForFileManager");
    var div_folderTreeForFileManager = $("#folderTreeForFileManager");
    //folder tree
    
    var methods = {
        init: function (options) {
            selectedGroupID = options.selectedGroupID;
            $.folder('fileDataTableDestroy');
            $.insmFramework('getFolderTreeData', {
                groupID: options.selectedGroupID,
                success: function (tempdataFolderTreeData) {
                    if (tempdataFolderTreeData) {
                        var tree = $('.tree-demo.folderTree');
                        folderJstreeData.core.data = tempdataFolderTreeData;
                        tree.jstree("destroy");
                        tree.jstree(folderJstreeData);
                        tree.jstree(true).refresh();

                        tree.on("changed.jstree", function (e, data) {
                            if (data.node) {
                                selectedFolderID = data.node.id;
                                $.file('init', {
                                    selectedFolderID : selectedFolderID
                                });
                            }
                        });
                        tree.on("move_node.jstree", function (e, data) {
                            var node = data.node;
                            if (node) {
                                $.folder('editFolder', node);
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
        createFolder: function () {
            var groupRef = div_groupTreeForFileManager.jstree(true),
            groupSel = groupRef.get_selected();
            if (!groupSel.length) { return false; }// no group selected
            var folderRef = div_folderTreeForFileManager.jstree(true),
                folderSef = folderRef.get_selected();

            $.insmFramework('createFolder', {
                groupID: selectedGroupID,
                folderName: "New Folder",
                folderParentID: folderSef[0],
                success: function (data) {
                    if (!folderSef.length) {
                        folderSef = folderRef.create_node(null, data, "last", false, true);//create root
                    } else {
                        folderSef = folderRef.create_node(folderSef, data, "last", false, true);
                    }
                    if (folderSef) {
                        folderRef.edit(folderSef, folderSef.text, function (obj, tmp, nv, cancel) {
                            $.folder('editFolder',obj);
                        });
                    }
                }
            });
        },
        editFolder: function (node) {
            $.insmFramework('editFolder', {
                groupID: selectedGroupID,
                folderID: node.id,
                folderName: node.text,
                folderParentID: node.parent,
                success: function (data) {
                }
            });
        },
        deleteFolder: function () {
            var folderRef = div_folderTreeForFileManager.jstree(true),
                folderSef = folderRef.get_selected();
            if (!folderSef.length) { return };
            $.insmFramework("deleteFolder", {
                folderID: selectedFolderID,
                success: function (data) {
                    folderRef.delete_node(folderSef);
                    $.folder('fileDataTableDestroy');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    toastr.warning(XMLHttpRequest.responseJSON.Message);
                }
            });
        },
        getSelectedFolderID: function () {
            return selectedFolderID;
        },
        fileDataTableDestroy: function () {
            if ($("#datatable_file").data("datatable")) {
                $("#datatable_file").data("datatable").destroy();
            }
        }
        
    };
    $("#btn_create").click(function () {
        $.folder('createFolder');
    });

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