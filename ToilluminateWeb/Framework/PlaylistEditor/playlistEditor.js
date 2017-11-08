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
    var tableData = {
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
                    "field": "FileID"
                },
                "data": {}
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
        //sortable: true,

        // column based filtering
        filterable: true,

        pagination: true,

        // columns definition
        columns: [{
            field: "FileID",
            title: "",
            sortable: false, // disable sort for this column
            width: 40,
            textAlign: 'center',
            filterable: false,
            selector: { class: 'm-checkbox--solid m-checkbox--brand' },
        }, {
            field: "FileThumbnailUrl",
            title: "IMG",
            filterable: false, // disable or enable filtering
            width: 120,
            height:80,
            sortable: true,
            // basic templating support for column rendering,
            template: '<a href="{{FileUrl}}" target="_blank"><img src="{{FileThumbnailUrl}}" class="file-img" width = "120px" height= "80px"/></a>'
        }, {
            field: "FileName",
            title: "File Name",
            sortable: true,
            responsive: { visible: 'lg' }
        }, {
            field: "InsertDate",
            title: "Great Date",
            sortable: true,
            filterable: false,
            textAlign: 'center',
            datetype: "yyyy-MM-dd HH:mm",
            responsive: { visible: 'lg' }
        }]
    };
    var playlist_groupID;
    var div_PlaylistEditorContent = $('#div_PlaylistEditorContent');
    var div_PlaylistEditor = $('<div/>').addClass('m-portlet m-portlet--warning m-portlet--head-sm');
    var div_head = $('<div/>').addClass('m-portlet__head');
    var div_head_caption = $('<div/>').addClass('m-portlet__head-caption');
    var div_head_title = $('<div/>').addClass("m-portlet__head-title");
    var spantitle = $("<span />").addClass('m-portlet__head-icon');
    var span_i = '<i class="fa fa-file-text"></i>';
    var head_text = $('<h3 />').addClass('m-portlet__head-text').text('Playlist1');

    var div_head_tools = $('<div/>').addClass('m-portlet__head-tools');
    var div_portlet_nav = $('<ul>').addClass("m-portlet__nav");
    var div_li = $('<li />').addClass('m-portlet__nav-item');
    var href = $('<a />').addClass("m-portlet__nav-link m-portlet__nav-link--icon");
    var href_i = $('<i />').addClass("fa fa-calendar").text('2017-11-01 13:35');

    var div_li_list = $('<li />').addClass("m-portlet__nav-item m-dropdown m-dropdown--inline m-dropdown--arrow m-dropdown--align-right m-dropdown--align-push");
    div_li_list.attr('data-dropdown-toggle', 'hover').attr('aria-expanded','true');
    var div_li_a_toggle = $('<a href="#"/>').addClass('m-portlet__nav-link m-portlet__nav-link--icon m-dropdown__toggle');
    var div_li_i = $('<i />').addClass('la la-ellipsis-v');
    var div_m_dropdown_wrapper = $('<div/>').addClass('m-dropdown__wrapper');

    var wrappe_spantitle = $("<span style='left: auto; right: 18.5px;'/>").addClass('m-dropdown__arrow m-dropdown__arrow--right m-dropdown__arrow--adjust');
    var div_m_dropdown_inner = $('<div/>').addClass("m-dropdown__inner");
    var div_m_dropdown_bodyr = $('<div/>').addClass("m-dropdown__body");
    var div_m_dropdown_content= $('<div/>').addClass("m-dropdown__content");

    var ul = $('<ul>').addClass("m-nav");
    var ul_li = $('<li />').addClass('m-nav__item');
    var ul_li_href = $('<a />').addClass("m-nav__link");
    var ul_li_a = $('<i />').addClass("m-nav__link-icon flaticon-share");
    var ul_li_href_span = $("<span />").addClass('m-nav__link-text');


    var methods = {
        init: function (options) {
            // Global vars
            var $this = $('html').eq(0);
            var _plugin = $this.data('playlistEditor');

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
                $this.data('playlistEditor', _plugin);


                spantitle.append(span_i);
                div_head_title.append(spantitle);
                div_head_title.append(head_text);
                div_head_caption.append(div_head_title);
                div_head.append(div_head_caption)


                ul_li_href_span.text('playlistItem1');
                ul_li_href.append(ul_li_a, ul_li_href_span);
                ul_li.append(ul_li_href);
                ul.append(ul_li);
                div_m_dropdown_content.append(ul);
                div_m_dropdown_bodyr.append(div_m_dropdown_content);
                div_m_dropdown_inner.append(div_m_dropdown_bodyr);

                div_m_dropdown_wrapper.append(wrappe_spantitle);
                div_m_dropdown_wrapper.append(div_m_dropdown_inner);
                div_li_a_toggle.append(div_li_i);
                div_li_list.append(div_li_a_toggle, div_m_dropdown_wrapper)

                href.append(href_i);
                div_li.append(href);
                div_portlet_nav.append(div_li);
                div_portlet_nav.append(div_li_list);
                div_head_tools.append(div_portlet_nav);

                div_head.append(div_head_tools);
                div_PlaylistEditorContent.append(div_PlaylistEditor.append(div_head));

            }
            playlist_groupID = options.selectedGroupID;
            return $this;
        },
        short: function (options) {
        },
        fileDataTableDestroy: function () {
            if ($("#datatable_file1").data("datatable")) {
                $("#datatable_file1").data("datatable").destroy();
            }
        },
        setfolder: function (options) {
            selectedGroupID = options.selectedGroupID;
            $.playlistEditor('fileDataTableDestroy');
            $.insmFramework('getFolderTreeData', {
                groupID: options.selectedGroupID,
                success: function (tempdataFolderTreeData) {
                    if (tempdataFolderTreeData) {
                        var tree = $('.tree-demo.folderTreePlaylist');
                        folderJstreeData.core.data = tempdataFolderTreeData;
                        tree.jstree("destroy");
                        tree.jstree(folderJstreeData);
                        tree.jstree(true).refresh();

                        tree.on("changed.jstree", function (e, data) {
                            if (data.node) {
                                selectedFolderID = data.node.id;
                                $.playlistEditor('setfile', {
                                    selectedFolderID: selectedFolderID
                                });
                            }
                        });
                        //tree.on("move_node.jstree", function (e, data) {
                        //    var node = data.node;
                        //    if (node) {
                        //        $.folder('editFolder', node);
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
        },
        setfile: function (options) {
            selectedFolderID = options.selectedFolderID;
            $('#datatable_file1').prop("outerHTML", "<div class='m_datatable' id='datatable_file1'></div>");
            $.insmFramework('getFilesByFolder', {
                FolderID: selectedFolderID,
                success: function (fileData) {
                    tableData.data.source.data = fileData;
                    $("#datatable_file1").data("datatable", $('#datatable_file1').mDatatable(tableData));
                },
                error: function () {
                    //options.error();
                },
            });
        },
        selectfile: function () {
            var datatable = $("#datatable_file1").data("datatable");
            if (datatable && datatable.setSelectedRecords().getSelectedRecords().length > 0) {
                //remove selected files
                $.each(datatable.setSelectedRecords().getSelectedRecords(), function (index, item) {
                    var screenshot = new Image();
                    screenshot.src =$(item).data().obj.FileThumbnailUrl;

                    $('#select_file').append(screenshot);

                });
            } else {
                //remove selected folder
                $.folder("deleteFolder");
            }
        },
    }
    $.playlistEditor = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on $.reports');
        }
        return null;
    };
    $("#save_change").click(function () {
        $.playlistEditor('selectfile');
    });
    $("#add_playlist").click(function () {
        $.playlistEditor('setfolder', { selectedGroupID: playlist_groupID });
    });
    $("#playlist_expandAll").click(function () {
        $('#groupTreeForPlaylistEditor').jstree('open_all');
    });
    $("#playlist_collapseAll").click(function () {
        $('#groupTreeForPlaylistEditor').jstree('close_all');
    });

    $("#playlist_save").click(function () {
        $.insmFramework('creatPlaylist', {
            GroupID: playlist_groupID,
            PlayListName: '11',
            InheritForced: '',
            Settings: '11112222',
            Comments: '333333',
            success: function (playlistData) {
                if (playlistData) {
                    alert('1')
                }
            }
        })
    });

    $("#playlist_delete").click(function () {
        
    });

    $("#playlist_back").click(function () {
        
    });
})(jQuery);