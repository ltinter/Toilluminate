(function ($) {
    var folderJstreeData = {
        "core": {
            "multiple": false,
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
                        var inst = jQuery.jstree.reference(obj.reference);
                        var clickedNode = inst.get_node(obj.reference);
                        inst.edit(obj.reference, clickedNode.val, function (obj, tmp, nv, cancel) {
                            $.folder('deleteFolder', obj);
                        });
                    }
                    //"action": function (obj) {
                    //    $.folder('deleteFolder', obj);
                    //}
                }
            }
        }
    };
    //var selectedGroupID = null;
    var selectedFolderID = null;
    var selectedFolderData = null;
    //var div_groupTreeForFileManager = $("#groupTreeForFileManager");
    var div_folderTreeForFileManager = $("#folderTreeForFileManager");
    //folder tree

    var methods = {
        init: function (options) {
            var $this = $('body').eq(0);
            var _plugin = $this.data('folder');

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
                        groupTreecontainer: $('<div />').addClass('tree-demo  groupTree').attr('id', 'groupTreeForFileManager'),
                    },
                    data: {
                        selectedGroupID: null
                    }
                }
            }
            $this.find('#tempforfoder').append(_plugin.htmlElements.groupTreecontainer);
            $this.data('folder', _plugin);
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
            //var div_groupTreeForFileManager = $("#groupTreeForFileManager");

            _plugin.htmlElements.groupTreecontainer.jstree(groupJstreeData);

            _plugin.htmlElements.groupTreecontainer.on("changed.jstree", function (e, data) {
                //存储当前选中的区域的名称
                if (data.node) {
                    $.file('destroyFileTableData');
                    $.insmFramework('getFolderTreeData', {
                        groupID: data.node.id,
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
                                        selectedFolderData = data.node;
                                        $.file('init', {
                                            selectedFolderID: selectedFolderID
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

                    //$.folder('init', {
                    //    selectedGroupID: data.node.id
                    //});
                }
            });

            _plugin.data.selectedGroupID = options.selectedGroupID == undefined ? loginuser.GroupID : options.selectedGroupID;
            
        },
        createFolder: function () {
            var $this = $('body').eq(0);
            var _plugin = $this.data('folder');

            var groupRef = _plugin.htmlElements.groupTreecontainer.jstree(true),
            groupSel = groupRef.get_selected();
            if (!groupSel.length) { return false; }// no group selected
            var folderRef = div_folderTreeForFileManager.jstree(true),
                folderSef = folderRef.get_selected();

            $.insmFramework('createFolder', {
                groupID: _plugin.data.selectedGroupID,
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
                            $.folder('editFolder', obj);
                        });
                    }
                    toastr.success("操作が完了しました。");
                }
            });
        },
        editFolder: function (node) {
            var $this = $('body').eq(0);
            var _plugin = $this.data('folder');

            $.insmFramework('editFolder', {
                groupID: _plugin.data.selectedGroupID,
                folderID: node.id,
                folderName: node.text,
                folderParentID: node.parent,
                success: function (data) {
                    toastr.success("操作が完了しました。");
                }
            });
        },
        deleteFolder: function (node) {
            var $this = $('body').eq(0);
            var _plugin = $this.data('folder');

            //toastr.warning("使用中ですので、削除できない。");
            //return false;
            if (node == undefined) {
                node = selectedFolderData;
            }
            if (node == undefined || node == null) { return;}
            var folderRef = div_folderTreeForFileManager.jstree(true),
                folderSef = folderRef.get_selected();
            if (!folderSef.length) { return };
            $.confirmBox({
                title: "Warning",
                message: '削除しても宜しいでしょうか？',
                onOk: function () {
                    $.insmFramework('deleteFolder', {
                        groupID: _plugin.data.selectedGroupID,
                        folderID: node.id,
                        folderName: node.text,
                        folderParentID: node.parent,
                        success: function (data) {
                            folderRef.delete_node(folderSef);
                            $.file('destroyFileTableData');
                            toastr.success("操作が完了しました。");
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            toastr.warning(XMLHttpRequest.responseJSON.Message);
                        }
                    });
                }
            });
        },
        getSelectedFolderID: function () {
            return selectedFolderID;
        },
        getSelectedGroupID: function () {
            var $this = $('body').eq(0);
            var _plugin = $this.data('folder');

            return _plugin.data.selectedGroupID;
        },
        expandAllFolder: function () {
            var $this = $('body').eq(0);
            var _plugin = $this.data('folder');
            _plugin.htmlElements.groupTreecontainer.jstree('open_all');
        },
        collapseAllFolder: function () {
            var $this = $('body').eq(0);
            var _plugin = $this.data('folder');
            _plugin.htmlElements.groupTreecontainer.jstree('close_all');
        }
    };
    $("#btn_create").click(function () {
        $.folder('createFolder');
    });
    $("#file_expandAll").click(function () {
        $.folder('expandAllFolder');
    });
    $("#file_collapseAll").click(function () {
        $.folder('collapseAllFolder');
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