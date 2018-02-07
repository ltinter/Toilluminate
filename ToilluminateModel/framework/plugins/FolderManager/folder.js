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
                }
            }
        }
    };
    var selectedFolderID = null;
    var selectedFolderData = null;


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
                        container: $('<div />').addClass('row'),
                        containerbody: $('<div />').addClass('col-xl-3'),
                        headcontainer: $('<div />').addClass('m-portlet m-portlet--success m-portlet--head-solid-bg m-portlet--bordered'),
                        head: $('<div />').addClass('m-portlet__head'),
                        headcaption: {
                            headcaptioncontainer: $('<div />').addClass('m-portlet__head-caption'),
                            headcaptiontitle: $('<div />').addClass('m-portlet__head-title'),
                            headcaptiontitletext: $('<h3 />').addClass('m-portlet__head-text intros'),
                        },
                        headtools: {
                            headtoolscontainer: $('<form />').addClass('m-portlet__head-tools'),
                            headtoolsgroup: $('<div />').addClass('m-btn-group m-btn-group--pill btn-group mr-2').attr('role', 'group').attr('aria-label', '...'),
                            headtoolsCollapseAll: $('<button type="button"/>').addClass('m-btn btn btn-outline-dark btn-secondary').attr('id', 'file_collapseAll'),
                            headtoolsCollapseAll_i: $('<i />').addClass('fa fa-angle-double-up'),
                            headtoolsExpandAll: $('<button type="button"/>').addClass('m-btn btn btn-outline-dark btn-secondary').attr('id', 'file_expandAll'),
                            headtoolsExpandAll_i: $('<i />').addClass('fa fa-angle-double-down'),
                        },
                        groupTree: $('<div />').addClass('m-portlet__body'),
                        groupTreecontainer: $('<div />').addClass('tree-demo  groupTree').attr('id', 'groupTreeForFileManager'),

                        folderButtonMenu: {
                            container: $('<div />').addClass('col-xl-9'),
                            menucontainer: $('<div />').addClass('m-portlet m-portlet--brand m-portlet--head-solid-bg m-portlet--bordered'),
                            menuhead: $('<div />').addClass('m-portlet__head'),
                            menuheadcaption: {
                                headcaptioncontainer: $('<div />').addClass('m-portlet__head-caption'),
                                headcaptiontitle: $('<div />').addClass('m-portlet__head-title'),
                                headcaptiontitletext: $('<span />').addClass('m-portlet__head-icon'),
                                headcaptiontitletext_i: $('<i />').addClass('fa fa-hdd-o'),
                                menutext: $('<h3 />').addClass('m-portlet__head-text intros'),
                            },
                            menubody: {
                                menubodycontainer: $('<div />').addClass('m-form m-form--label-align-right m--margin-top-20 m--margin-bottom-30 m--margin-left-50 m--margin-right-50'),
                                menu: $('<div />').addClass('row'),
                                menusearch: $('<span />').addClass('col-xl-4 order-2 order-xl-1'),
                                menusearchgroup: $('<h3 />').addClass('form-group m-form__group row align-items-center'),
                                menusearchgroupcol: $('<h3 />').addClass('col-md-10'),
                                menubuttons: {
                                    container: $('<div />').addClass('col-xl-8 order-2 order-xl-1 m-form m-form--label-align-right'),
                                    menubuttonsgroup: $('<div />').addClass('m-btn-group m-btn-group--pill btn-group mr-2').attr('role', 'group').attr('aria-label', '...'),

                                    menubuttonCreate: $('<button type="button"/>').addClass('m-btn btn btn-outline-dark btn-secondary').attr('id', 'btn_create').attr('data-container', 'body').attr('data-toggle', 'm-tooltip').attr('data-placement', 'bottom').attr('title', '').attr('data-original-title', 'Tooltip title'),
                                    menubuttonCreate_i: $('<i />').addClass('fa fa-folder-open-o'),
                                    menubuttonCut: $('<button type="button"/>').addClass('m-btn btn btn-outline-dark btn-secondary').attr('id', 'btn_cut'),
                                    menubuttonCut_i: $('<i />').addClass('fa fa-cut'),

                                    menubuttonCopy: $('<button type="button"/>').addClass('m-btn btn btn-outline-dark btn-secondary').attr('id', 'btn_copy'),
                                    menubuttonCopy_i: $('<i />').addClass('fa fa-copy'),

                                    menubuttonPaste: $('<button type="button"/>').addClass('m-btn btn btn-outline-dark btn-secondary').attr('id', 'btn_paste'),
                                    menubuttonPaste_i: $('<i />').addClass('fa fa-paste'),

                                    menubuttonDelete: $('<button type="button"/>').addClass('m-btn btn btn-outline-dark btn-secondary').attr('id', 'btn_delete'),
                                    menubuttonDelete_i: $('<i />').addClass('fa fa-times-circle-o'),

                                    menubuttonUploadfile: $('<button type="button"/>').addClass('m-btn btn btn-outline-dark btn-secondary').attr('id', 'btn_uploadfile').attr('data-toggle', 'modal').attr('data-target', '#m_blockui_4_1_modal'),
                                    menubuttonUploadfil_i: $('<i />').addClass('fa fa-send'),
                                }

                            }
                        },
                        containerdatabody: {
                            container: $('<div />').addClass('m-portlet__body'),
                            body: $('<div />').addClass('row'),
                            folderTreecontainer: $('<div />').addClass('col-xl-4'),
                            folderTree: $('<div />').addClass('tree-demo  folderTree').attr('id', 'folderTreeForFileManager'),
                            datatable_file: $('<div />').addClass('col-xl-8'),
                            datafile: $('<div />').addClass('m_datatable').attr('id', 'datatable_file'),
                        },
                    },
                    data: {
                        selectedGroupID: null
                    }
                }

                $this.find('#FileManagementContent')
                .append(_plugin.htmlElements.container
                    .append(_plugin.htmlElements.containerbody
                        .append(_plugin.htmlElements.headcontainer
                            .append(_plugin.htmlElements.head
                                .append(_plugin.htmlElements.headcaption.headcaptioncontainer
                                    .append(_plugin.htmlElements.headcaption.headcaptiontitle
                                        .append(_plugin.htmlElements.headcaption.headcaptiontitletext.append($.localize('translate', 'Groups')))
                                    )
                                )
                                .append(_plugin.htmlElements.headtools.headtoolscontainer
                                    .append(_plugin.htmlElements.headtools.headtoolsgroup
                                        .append(_plugin.htmlElements.headtools.headtoolsCollapseAll
                                            .append(_plugin.htmlElements.headtools.headtoolsCollapseAll_i)
                                        )
                                        .append(_plugin.htmlElements.headtools.headtoolsExpandAll
                                            .append(_plugin.htmlElements.headtools.headtoolsExpandAll_i)
                                        )
                                    )
                                )
                            )
                            .append(_plugin.htmlElements.groupTree.append(_plugin.htmlElements.groupTreecontainer))
                        )
                    )
                    .append(_plugin.htmlElements.folderButtonMenu.container
                        .append(_plugin.htmlElements.folderButtonMenu.menucontainer
                            .append(_plugin.htmlElements.folderButtonMenu.menuhead
                                .append(_plugin.htmlElements.folderButtonMenu.menuheadcaption.headcaptioncontainer
                                    .append(_plugin.htmlElements.folderButtonMenu.menuheadcaption.headcaptiontitle
                                        .append(_plugin.htmlElements.folderButtonMenu.menuheadcaption.headcaptiontitletext
                                            .append(_plugin.htmlElements.folderButtonMenu.menuheadcaption.headcaptiontitletext_i)
                                        )
                                        .append(_plugin.htmlElements.folderButtonMenu.menuheadcaption.menutext
                                            .append($.localize('translate', 'Files'))
                                        )
                                    )
                                )
                                .append(_plugin.htmlElements.folderButtonMenu.menubody.menubodycontainer
                                    .append(_plugin.htmlElements.folderButtonMenu.menubody.menu
                                        .append(_plugin.htmlElements.folderButtonMenu.menubody.menusearch
                                            .append(_plugin.htmlElements.folderButtonMenu.menubody.menusearchgroup
                                                .append(_plugin.htmlElements.folderButtonMenu.menubody.menusearchgroupcol)
                                            )
                                        )
                                        .append(_plugin.htmlElements.folderButtonMenu.menubody.menubuttons.container
                                            .append(_plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonsgroup
                                                .append(_plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonCreate
                                                    .append(_plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonCreate_i)
                                                )

                                                .append(_plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonCut
                                                    .append(_plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonCut_i)
                                                )
                                                .append(_plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonCopy
                                                    .append(_plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonCopy_i)
                                                )
                                                .append(_plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonPaste
                                                    .append(_plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonPaste_i)
                                                )
                                                .append(_plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonDelete
                                                    .append(_plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonDelete_i)
                                                )
                                                .append(_plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonUploadfile
                                                    .append(_plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonUploadfil_i)
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                            .append(_plugin.htmlElements.containerdatabody.container
                                .append(_plugin.htmlElements.containerdatabody.body
                                    .append(_plugin.htmlElements.containerdatabody.folderTreecontainer
                                        .append(_plugin.htmlElements.containerdatabody.folderTree)
                                    )
                                    .append(_plugin.htmlElements.containerdatabody.datatable_file
                                        .append(_plugin.htmlElements.containerdatabody.datafile)
                                    )
                                )
                            )

                        )
                    )
                );
                //Create
                _plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonCreate.click(function () {
                    $.folder('createFolder');
                });
                //Cut
                _plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonCut.click(function () {
                    $.file('cutFile');
                });
                //Copy
                _plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonCopy.click(function () {
                    $.file('copyFile');
                });
                //paste
                _plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonPaste.click(function () {
                    $.file('pasteFile');
                });
                //Delete
                _plugin.htmlElements.folderButtonMenu.menubody.menubuttons.menubuttonDelete.click(function () {
                    $.file('remove');
                });
                
                _plugin.htmlElements.headtools.headtoolsExpandAll.click(function () {
                    
                    _plugin.htmlElements.groupTreecontainer.jstree('open_all');
                });
                _plugin.htmlElements.headtools.headtoolsCollapseAll.click(function () {
                   
                    _plugin.htmlElements.groupTreecontainer.jstree('close_all');
                });

                $this.data('folder', _plugin);
            }
            
            
            var loginuser = $.insmFramework('user');
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
            var folderRef = _plugin.htmlElements.containerdatabody.folderTree.jstree(true),
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
            var folderRef = _plugin.htmlElements.containerdatabody.folderTree.jstree(true),
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