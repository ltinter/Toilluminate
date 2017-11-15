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
            width: 150,
            height:100,
            sortable: true,
            // basic templating support for column rendering,
            template: '<a href="{{FileUrl}}" target="_blank"><img src="{{FileThumbnailUrl}}" class="file-img" style="max-width:150px;max-height:100px" /></a>'
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
    var divselectFile;
    var editflg = false;
    var div_PlaylistEditorContent = $('#div_PlaylistEditorContent');
    var div_playlist = $('#div_playlist');
    var div_Mainplaylist = $('#div_Mainplaylist');
    var edit_playlistId = null;
    var tempselectedGroupID = null;
    var selectedFolderID = null;
    var currentNeededFileType = "image";
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
            }

            tempselectedGroupID = options.selectedGroupID;
            $.playlistEditor('getPlaylistByGroupID', { selectedGroupID: options.selectedGroupID });

            return $this;
        },
        getPlaylistByGroupID: function (options) {
            div_PlaylistEditorContent.empty();
            $.insmFramework('getPlaylistByGroupID', {
                GroupID: options.selectedGroupID,
                success: function (playlistData) {
                    if (playlistData) {
                        $.each(playlistData, function (index, item) {

                            var div_PlaylistEditor = $('<div/>').addClass('m-portlet m-portlet--warning m-portlet--head-sm');
                            var div_head = $('<div/>').addClass('m-portlet__head');
                            var div_head_caption = $('<div/>').addClass('m-portlet__head-caption');
                            var div_head_title = $('<div/>').addClass("m-portlet__head-title");
                            var spantitle = $("<span />").addClass('m-portlet__head-icon');
                            var span_i = '<i class="fa fa-file-text"></i>';
                            var head_text = $('<h3 />').addClass('m-portlet__head-text').text(item.PlayListName);

                            var div_head_tools = $('<div/>').addClass('m-portlet__head-tools');
                            var div_portlet_nav = $('<ul>').addClass("m-portlet__nav");
                            var div_li = $('<li />').addClass('m-portlet__nav-item');
                            var href = $('<a />').addClass("m-portlet__nav-link m-portlet__nav-link--icon");
                            var href_i = $('<i />').addClass("fa fa-calendar");

                            var div_li_list = $('<li />').addClass("m-portlet__nav-item m-dropdown m-dropdown--inline m-dropdown--arrow m-dropdown--align-right m-dropdown--align-push")
                            //    .click(function () {
                            //    $.playlistEditor('editPlaylist', { playlistID: item.PlayListID });
                            //});
                            div_li_list.attr('data-dropdown-toggle', 'hover').attr('aria-expanded', 'true');

                            var div_li_a_toggle = $('<a href="#"/>').addClass('m-portlet__nav-link m-portlet__nav-link--icon m-dropdown__toggle');
                            var div_li_i = $('<i />').addClass('la la-ellipsis-v');
                            var div_m_dropdown_wrapper = $('<div/>').addClass('m-dropdown__wrapper');

                            var wrappe_spantitle = $("<span style='left: auto; right: 18.5px;'/>").addClass('m-dropdown__arrow m-dropdown__arrow--right m-dropdown__arrow--adjust');
                            var div_m_dropdown_inner = $('<div/>').addClass("m-dropdown__inner");
                            var div_m_dropdown_bodyr = $('<div/>').addClass("m-dropdown__body");
                            var div_m_dropdown_content = $('<div/>').addClass("m-dropdown__content");

                            spantitle.append(span_i);
                            div_head_title.append(spantitle);
                            div_head_title.append(head_text);
                            div_head_caption.append(div_head_title);
                            div_head.append(div_head_caption)

                            var ul = $('<ul>').addClass("m-nav");
                            //Item
                            if (item.Settings) {
                                var playlistSetting = JSON.parse(item.Settings);
                                $.each(playlistSetting.PlaylistItems, function (index, PlaylistItem) {
                                    var ul_li = $('<li />').addClass('m-nav__item');
                                    var ul_li_href = $('<a />').addClass("m-nav__link");
                                    var ul_li_a = $('<i />').addClass("m-nav__link-icon flaticon-share");
                                    var ul_li_href_span = $("<span />").addClass('m-nav__link-text');

                                    ul_li_href_span.text(PlaylistItem.PlaylistItemName);
                                    ul_li_href.append(ul_li_a, ul_li_href_span);
                                    ul_li.append(ul_li_href);
                                    ul.append(ul_li);
                                })
                            }

                            div_m_dropdown_content.append(ul);
                            div_m_dropdown_bodyr.append(div_m_dropdown_content);
                            div_m_dropdown_inner.append(div_m_dropdown_bodyr);

                            div_m_dropdown_wrapper.append(wrappe_spantitle);
                            div_m_dropdown_wrapper.append(div_m_dropdown_inner);
                            div_li_a_toggle.append(div_li_i);
                            div_li_list.append(div_li_a_toggle, div_m_dropdown_wrapper)

                            var datetime = new Date(item.UpdateDate).toLocaleDateString()
                            href.append(href_i.text(datetime));
                            div_li.append(href);

                            var div_li_edit = $('<li />').addClass('m-portlet__nav-item');
                            var edit_href = $('<a />').addClass("btn btn-outline-success m-btn m-btn--pill m-btn--wide btn-sm").text('Edit');

                            edit_href.click(function () {
                                $.playlistEditor('setfolder', { selectedGroupID: tempselectedGroupID });
                                $.playlistEditor('editPlaylist', { playlistID: item.PlayListID });
                            });

                            div_li_edit.append(edit_href);
                            div_portlet_nav.append(div_li);
                            div_portlet_nav.append(div_li_edit);

                            div_portlet_nav.append(div_li_list);
                            div_head_tools.append(div_portlet_nav);

                            div_head.append(div_head_tools);
                            div_PlaylistEditor.append(div_head);
                            div_PlaylistEditorContent.append(div_PlaylistEditor);
                        })
                    }
                }
            })
        },
        short: function (options) {
        },
        playlistDefaultvalue: function () {
            $('#playlist_name').val('New Playlist Name');
            $('#playlist_note').text('');
            $("#playlist_monday_value").data("ionRangeSlider").update({ from: '0', to: '24' });
            $("#playlist_tuesday_value").data("ionRangeSlider").update({ from: '0', to: '24' });
            $("#playlist_wednesday_value").data("ionRangeSlider").update({ from: '0', to: '24' });
            $("#playlist_thursday_value").data("ionRangeSlider").update({ from: '0', to: '24' });
            $("#playlist_friday_value").data("ionRangeSlider").update({ from: '0', to: '24' });
            $("#playlist_saturday_value").data("ionRangeSlider").update({ from: '0', to: '24' });
            $("#playlist_sunday_value").data("ionRangeSlider").update({ from: '0', to: '24' });
            $("#label_loop_0").click();
            $("#label_playtime_0").click();
            $("#m_touchspin_1").val('0');
            $("#m_touchspin_2").val('0');
            $("#m_touchspin_3").val('0');
            var div_AddnewItem = $("#playlistItem");
            div_AddnewItem.empty();
            $("#div_playlist").find("h3:first").text('New Playlist');
        },
        editPlaylist: function (options) {
            editflg = true;
            if (options.playlistID) {
                $.playlistEditor('playlistDefaultvalue');

                div_playlist.show();
                div_Mainplaylist.hide();
                edit_playlistId = options.playlistID;
                $.insmFramework('getPlaylistByPlaylistID', {
                    playlistID: options.playlistID,
                    success: function (playlistData) {
                        if (playlistData) {
                            var Settings = JSON.parse(playlistData.Settings);
                            $('#playlist_name').val(playlistData.PlayListName);
                            $('#playlist_note').text(playlistData.Comments);
                            $("#div_playlist").find("h3:first").text(playlistData.PlayListName);
                        }
                        if (Object.getOwnPropertyNames(Settings).length > 0) {
                            $("#playlist_monday_value").data("ionRangeSlider").update({
                                from: Settings.Monday.split(';')[0], to: Settings.Monday.split(';')[1]
                            });
                            $("#playlist_tuesday_value").data("ionRangeSlider").update({
                                from: Settings.Tuesday.split(';')[0], to: Settings.Tuesday.split(';')[1]
                            });
                            $("#playlist_wednesday_value").data("ionRangeSlider").update({
                                from: Settings.Wednesday.split(';')[0], to: Settings.Wednesday.split(';')[1]
                            });
                            $("#playlist_thursday_value").data("ionRangeSlider").update({
                                from: Settings.Thursday.split(';')[0], to: Settings.Thursday.split(';')[1]
                            });
                            $("#playlist_friday_value").data("ionRangeSlider").update({
                                from: Settings.Friday.split(';')[0], to: Settings.Friday.split(';')[1]
                            });
                            $("#playlist_saturday_value").data("ionRangeSlider").update({
                                from: Settings.Saturday.split(';')[0], to: Settings.Saturday.split(';')[1]
                            });
                            $("#playlist_sunday_value").data("ionRangeSlider").update({
                                from: Settings.Sunday.split(';')[0], to: Settings.Sunday.split(';')[1]
                            });
                            $("#label_loop_" + Settings.Loop).click();
                            $("#label_playtime_" + Settings.Playtime).click();
                            $("#m_touchspin_1").val(Settings.PlayHours);
                            $("#m_touchspin_2").val(Settings.PlayMinites);
                            $("#m_touchspin_3").val(Settings.PlaySeconds);


                            if (Settings.PlaylistItems) {
                                if (Settings.PlaylistItems.length > 0) {
                                    $.each(Settings.PlaylistItems, function (index, palylistItem) {
                                        if (palylistItem.type) {
                                            switch (palylistItem.type.toLowerCase()) {
                                                case "1":
                                                    $.playlistEditor('greateNewItemPicture', {
                                                        palylistItem: palylistItem
                                                    });
                                                    break;
                                                case "2":
                                                    $.playlistEditor('greateNewItemPicture', {
                                                        palylistItem: palylistItem
                                                    });
                                                    break;
                                                case "3":
                                                    $.playlistEditor('greateNewItemPicture', {
                                                        palylistItem: palylistItem
                                                    });
                                                    break;
                                            }
                                        }
                                    })
                                }
                            }
                        }
                    }
                })
            }
        },
        fileDataTableDestroy: function () {
            if ($("#datatable_file1").data("datatable")) {
                $("#datatable_file1").data("datatable").destroy();
            }
        },
        setfolder: function (options) {
            selectedGroupID = tempselectedGroupID;
            $.playlistEditor('fileDataTableDestroy');
            $.insmFramework('getFolderTreeDataForPlaylist', {
                groupID: tempselectedGroupID,
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
                                $.playlistEditor('setfile');
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
        setfile: function () {
            $('#datatable_file1').prop("outerHTML", "<div class='m_datatable' id='datatable_file1'></div>");
            $.insmFramework('getFilesByFolder', {
                FolderID: selectedFolderID,
                success: function (fileData) {
                    var supportedFileTypes = {
                        "image": [".jpg", ".gif", ".png", ".jpeg", ".bmp"],
                        "video": [".mp4", ".wmv", ".mpeg", ".mpg", ".avi", ".mov"]
                    };
                    var tempFileData = [];
                    $.each(fileData, function (fileIndex, fileItem) {
                        var tempFileType = supportedFileTypes[currentNeededFileType.toLowerCase()];
                        if (tempFileType && $.inArray(fileItem.FileType.toLowerCase().trim(), tempFileType) > -1) {
                            tempFileData.push(fileData[fileIndex]);
                        }
                    });
                    tableData.data.source.data = tempFileData;
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
                    screenshot.src = $(item).data().obj.FileThumbnailUrl;
                    screenshot.id = $(item).data().obj.FileID;
                    $(screenshot).css({ "max-height": "150px", "max-width": "200px","padding":"5px" });
                    divselectFile.append(screenshot);
                    var deleteImg = $("<i class='fa fa-remove'></i>").css({ "position": "relative", "left": "-17px", "background-color": "none", "cursor": "pointer", "top": "-64px" }).click(function () {
                        $(screenshot).remove();
                        $(this).remove();
                    });
                    divselectFile.append(deleteImg);
                });
            }
        },
    greateNewItemPicture: function (options) {
        var div_head = $('<div/>').addClass('m-portlet m-portlet--mobile m-portlet--sortable m-portlet--warning m-portlet--head-solid-bg');
        var div_head_handle = $('<div/>').addClass('m-portlet__head ui-sortable-handle');
        var div_head_caption = $('<div/>').addClass('m-portlet__head-caption');
        var div_head_title = $('<div/>').addClass('m-portlet__head-title');
        var span_head_title = $("<span />").addClass('m-portlet__head-icon');
        var span_i = '<i class="fa fa-file-text"></i>';
        var head_text = $('<h3 />').addClass('m-portlet__head-text');
        var div_head_tools = $('<div/>').addClass('m-portlet__head-tools');
        var div_portlet_nav = $('<ul>').addClass("m-portlet__nav");
        var div_li = $('<li />').addClass('m-portlet__nav-item');
        var href = $('<a />').addClass("m-portlet__nav-link m-portlet__nav-link--icon");
        var href_i = $('<i />').addClass("la la-close");
        span_head_title.append(span_i);
        div_head_title.append(span_head_title, head_text.text('PlaylistItemPicture\r\n(Picture)'));
        div_head_caption.append(div_head_title);
        href.append(href_i)
        div_li.append(href)
        div_portlet_nav.append(div_li)
        div_head_tools.append(div_portlet_nav)
        div_head_handle.append(div_head_caption, div_head_tools)
        var div_body = $('<div/>').addClass('m-portlet__body row').css('height', 'auto').css('overflow-y', 'auto').attr('type', '1');
        var div_bodyMain = $('<div/>').addClass('col-xl-4');
        var div_body_group = $('<div/>').addClass('form-group m-form__group');
        var lable = $('<lable/>').text(' Playlist Item Name:');
        var input = $("<input type='text'/>").addClass('form-control m-input').val("New Playlist Item");
        
        div_body_group.append(lable, input);
        div_bodyMain.append(div_body_group);

        var div_col = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
        var lable1 = $('<lable/>').text('Display Inteval(Seconds):');
        div_col.append(lable1);
        div_bodyMain.append(div_col);

        var div_col1 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
        var div_touchspin_brand = $('<div/>').addClass('m-bootstrap-touchspin-brand');
        var input1 = $("<input type='text'/>").addClass('form-control bootstrap-touchspin-vertical-btn').attr("name", "demo1").val("5");
        
        div_touchspin_brand.append(input1);
        div_col1.append(div_touchspin_brand);
        div_bodyMain.append(div_col1);

        var div_col2 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
        var lable2 = $('<lable/>').text('Sildeshow effects:');
        div_col2.append(lable2);
        div_bodyMain.append(div_col2);

        var div_col3 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
        var select_option = $('<select/>').addClass('form-control m-select2').attr("id", "m_select2_3").attr("multiple", "").attr("tabindex", "-1").attr("aria-hidden", "true");
        select_option.append("<option value='0'>Random</option>");
        select_option.append("<option value='1'>Left to Right</option>");
        select_option.append("<option value='2'>Right to Left</option>");
        select_option.append("<option value='3'>Top to Bottom</option>");
        select_option.append("<option value='4'>Bottom To Top</option>");
        select_option.append("<option value='5'>Grow</option>");
        select_option.append("<option value='6'>Fadein</option>");
        select_option.append("<option value='7'>Rotate horizontally</option>");
        select_option.append("<option value='8'>Rotate vertically</option>");
        div_col3.append(select_option);
        div_bodyMain.append(div_col3);


        var div_col4 = $('<div/>').addClass('col-xl-8');
        var div_col4_main = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
        div_col4.append(div_col4_main);

        var div_col_image = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12').css('overflow-y', 'auto').css('max-height', '400px').css('margin-top', '5px').css("line-height", "150px");
        var button_Selectimages = $("<button type='button'/>").attr("data-toggle", "modal").attr("data-target", "#m_modal_1").addClass('btn m-btn--pill m-btn--air         btn-outline-info btn-block').text('Select images').click(function () {
            divselectFile = div_col_image;
            currentNeededFileType = "image";
            if ($("#datatable_file1").data("datatable")) {
                $.playlistEditor('setfile');
            }
        });

        
        div_col4_main.append(button_Selectimages);
        div_col4.append(div_col_image);
        div_body.append(div_bodyMain, div_col4);
        div_head.append(div_head_handle, div_body)

        div_li.click(function () {
            div_head.empty();
        });

        var div_AddnewItem = $("#playlistItem");
        div_AddnewItem.append(div_head);
        select_option.select2({
                placeholder: "Select dildeshow effects"
            });
        input1.TouchSpin({
            buttondown_class: 'btn btn-secondary',
            buttonup_class: 'btn btn-secondary',
            verticalbuttons: true,
            verticalupclass: 'la la-angle-up',
            verticaldownclass: 'la la-angle-down',
            min: 0,
            max: 999999
        });
        if (options) {
            if (options.palylistItem) {
                input.val(options.palylistItem.PlaylistItemName);
            }
            if (options.palylistItem) {
                input1.val(options.palylistItem.DisplayIntevalSeconds);
            }
            if (options.palylistItem) {
                if (options.palylistItem.itemData) {
                    $.each(options.palylistItem.itemData.src, function (index, item) {
                        var screenshot = new Image();
                        screenshot.src = item;
                        //screenshot.id = $(item).data().obj.FileID;
                        div_col_image.append(screenshot);
                    });
                }
            }
        } 
    },
    greateNewItemText: function (options) {
        var div_head = $('<div/>').addClass('m-portlet m-portlet--mobile m-portlet--sortable m-portlet--warning m-portlet--head-solid-bg');
        var div_head_handle = $('<div/>').addClass('m-portlet__head ui-sortable-handle');
        var div_head_caption = $('<div/>').addClass('m-portlet__head-caption');
        var div_head_title = $('<div/>').addClass('m-portlet__head-title');
        var span_head_title = $("<span />").addClass('m-portlet__head-icon');
        var span_i = '<i class="fa fa-file-text"></i>';
        var head_text = $('<h3 />').addClass('m-portlet__head-text');
        var div_head_tools = $('<div/>').addClass('m-portlet__head-tools');
        var div_portlet_nav = $('<ul>').addClass("m-portlet__nav");
        var div_li = $('<li />').addClass('m-portlet__nav-item');
        var href = $('<a />').addClass("m-portlet__nav-link m-portlet__nav-link--icon");
        var href_i = $('<i />').addClass("la la-close");
        span_head_title.append(span_i);
        div_head_title.append(span_head_title, head_text.text('PlaylistItemText\r\n(Text)'));
        div_head_caption.append(div_head_title);
        href.append(href_i)
        div_li.append(href)
        div_portlet_nav.append(div_li)
        div_head_tools.append(div_portlet_nav)
        div_head_handle.append(div_head_caption, div_head_tools)
        var div_body = $('<div/>').addClass('m-portlet__body row').css('height', 'auto').css('overflow-y', 'auto');
        var div_bodyMain = $('<div/>').addClass('col-xl-4');
        var div_body_group = $('<div/>').addClass('form-group m-form__group');
        var lable = $('<lable/>').text(' Playlist Item Name:');
        var input = $("<input type='text'/>").addClass('form-control m-input');
        div_body_group.append(lable, input);
        div_bodyMain.append(div_body_group);

        var div_col = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
        var lable1 = $('<lable/>').text('Display Inteval(Seconds):');
        div_col.append(lable1);
        div_bodyMain.append(div_col);

        var div_col1 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
        var div_touchspin_brand = $('<div/>').addClass('m-bootstrap-touchspin-brand');
        var input1 = $("<input type='text'/>").addClass('form-control bootstrap-touchspin-vertical-btn').attr("name", "demo1");
        div_touchspin_brand.append(input1);
        div_col1.append(div_touchspin_brand);
        div_bodyMain.append(div_col1);

        var div_col2 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
        var lable2 = $('<lable/>').text('Sliding Speed:');
        div_col2.append(lable1);
        div_bodyMain.append(div_col2);

        var div_col3 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
        var div_touchspin_brand1 = $('<div/>').addClass('m-bootstrap-touchspin-brand');
        var input2 = $("<input type='text'/>").addClass('form-control bootstrap-touchspin-vertical-btn').attr("name", "demo1");
        div_touchspin_brand1.append(input2);
        div_col3.append(div_touchspin_brand1);
        div_bodyMain.append(div_col3);

        var div_col2 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
        var lable2 = $('<lable/>').text('Text Postion:');
        div_col2.append(lable2);
        div_bodyMain.append(div_col2);
        var div_col3 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
        var select_option = $('<select/>');
        select_option.append("<option value='0'>Top</option>");
        select_option.append("<option value='1'>Middle</option>");
        select_option.append("<option value='2'>Buttom</option>");
        div_col3.append(select_option);
        div_bodyMain.append(div_col3);

        var div_col4 = $('<div/>').addClass('col-xl-8');
        var div_col4_main = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');

        div_col4.append(div_col4_main);

        div_body.append(div_bodyMain, div_col4);
        div_head.append(div_head_handle, div_body)

        var div_AddnewItem = $("#playlistItem");
        div_AddnewItem.append(div_head);

        input1.TouchSpin({
            buttondown_class: 'btn btn-secondary',
            buttonup_class: 'btn btn-secondary',
            verticalbuttons: true,
            verticalupclass: 'la la-angle-up',
            verticaldownclass: 'la la-angle-down',
            min: 0,
            max: 60
        });

        input2.TouchSpin({
            buttondown_class: 'btn btn-secondary',
            buttonup_class: 'btn btn-secondary',
            verticalbuttons: true,
            verticalupclass: 'la la-angle-up',
            verticaldownclass: 'la la-angle-down',
            min: 0,
            max: 60
        });

        div_col4_main.summernote({
            height: 150,
            toolbar: [
        // [groupName, [list of button]]
        ['style', ['bold', 'italic', 'underline', 'clear']],
        ['font', ['strikethrough', 'superscript', 'subscript']],
        ['fontsize', ['fontsize', 'fontname']],
        ['color', ['color']],
        ['Misc', ['undo', 'redo']]
            ]
        });
    },
    greateNewItemvideo: function (options) {
        var div_head = $('<div/>').addClass('m-portlet m-portlet--mobile m-portlet--sortable m-portlet--warning m-portlet--head-solid-bg');
        var div_head_handle = $('<div/>').addClass('m-portlet__head ui-sortable-handle');
        var div_head_caption = $('<div/>').addClass('m-portlet__head-caption');
        var div_head_title = $('<div/>').addClass('m-portlet__head-title');
        var span_head_title = $("<span />").addClass('m-portlet__head-icon');
        var span_i = '<i class="fa fa-file-text"></i>';
        var head_text = $('<h3 />').addClass('m-portlet__head-text');
        var div_head_tools = $('<div/>').addClass('m-portlet__head-tools');
        var div_portlet_nav = $('<ul>').addClass("m-portlet__nav");
        var div_li = $('<li />').addClass('m-portlet__nav-item');
        var href = $('<a />').addClass("m-portlet__nav-link m-portlet__nav-link--icon");
        var href_i = $('<i />').addClass("la la-close");
        span_head_title.append(span_i);
        div_head_title.append(span_head_title, head_text.text('PlaylistItemText<br />(Picture)'));
        div_head_caption.append(div_head_title);
        href.append(href_i)
        div_li.append(href)
        div_portlet_nav.append(div_li)
        div_head_tools.append(div_portlet_nav)
        div_head_handle.append(div_head_caption, div_head_tools)
        var div_body = $('<div/>').addClass('m-portlet__body row').css('height', 'auto').css('overflow-y', 'auto');
        var div_bodyMain = $('<div/>').addClass('col-xl-4');
        var div_body_group = $('<div/>').addClass('form-group m-form__group');
        var lable = $('<lable/>').text(' Playlist Item Name:');
        var input = $("<input type='text'/>").addClass('form-control m-input');
        div_body_group.append(lable, input);
        var div_col = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
        var lable1 = $('<lable/>').text('Display Inteval(Seconds):');
        div_col.append(lable1);
        div_body_group.append(div_col);

        var div_col1 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
        var div_touchspin_brand = $('<div/>').addClass('m-bootstrap-touchspin-brand');
        var input1 = $("<input type='text'/>").addClass('form-control bootstrap-touchspin-vertical-btn').attr("name", "demo1");
        div_touchspin_brand.append(input1);
        div_col1.append(div_touchspin_brand);
        div_body_group.append(div_col1);

        var div_col2 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
        var lable2 = $('<lable/>').text('Sildeshow effects:');
        div_col2.append(lable2);
        div_body_group.append(div_col2);

        var div_col3 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
        var select_option = $('<select/>').addClass('form-control m-select2').attr("id", "m_select2_3").attr("multiple", "").attr("tabindex", "-1").attr("aria-hidden", "true");
        div_col3.append(select_option);
        div_body_group.append(div_col3);
        div_bodyMain.append(div_body_group);

        var div_col4 = $('<div/>').addClass('col-xl-8');
        var div_col4_main = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
        div_col4.append(div_col4_main);

        var div_col_image = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12').css('overflow-y', 'auto').css('max-height', '400px').css('margin-top', '5px');
        var button_Selectimages = $("<button type='button'/>").attr("data-toggle", "modal").attr("data-target", "#m_modal_1").addClass('btn m-btn--pill m-btn--air         btn-outline-info btn-block').text('Select images').click(function () {
            divselectFile = div_col_image;
        });
        div_col4_main.append(button_Selectimages);
        div_col4.append(div_col_image);
        div_body.append(div_bodyMain, div_col4);
        div_head.append(div_head_handle, div_body)

        var div_AddnewItem = $("#playlistItem");
        div_AddnewItem.append(div_head);
    }
}

    $.playlistEditor = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
            } else if (typeof method === 'object' || !method) {
                return methods.init.apply(this, arguments);
                } else {
                $.error('Method ' +method + ' does not exist on $.reports');
        }
        return null;
    };
    $("#save_change").click(function () {
        $.playlistEditor('selectfile');
    });
    $("#add_playlist").click(function () {
        if (tempselectedGroupID == null) {
            toastr.warning("Please select playlist's group!");
                return;
        }
        $.playlistEditor('setfolder', { selectedGroupID: tempselectedGroupID});
        $.playlistEditor('playlistDefaultvalue');
        div_playlist.show();
        div_Mainplaylist.hide();
        editflg = false;
    });
    $("#playlist_expandAll").click(function () {
        $('#groupTreeForPlaylistEditor').jstree('open_all');
    });
    $("#playlist_collapseAll").click(function () {
        $('#groupTreeForPlaylistEditor').jstree('close_all');
    });

    $("#playlist_save").click(function () {
        var div_AddnewItem = $("#playlistItem");
        var palylistItemItems = div_AddnewItem.find(".m-portlet__body.row");
        var Settings = {};

        Settings = {
            Loop: $("input[name='playlist_loop']:checked").val(),
            Playtime: $("input[name='playlist_playtime']:checked").val(),
            PlayHours: $("#m_touchspin_1").val(),
            PlayMinites: $("#m_touchspin_2").val(),
            PlaySeconds: $("#m_touchspin_3").val(),
            Monday: $("#playlist_monday_value").val(),
            Tuesday: $("#playlist_tuesday_value").val(),
            Wednesday: $("#playlist_wednesday_value").val(),
            Thursday: $("#playlist_thursday_value").val(),
            Friday: $("#playlist_friday_value").val(),
            Saturday: $("#playlist_saturday_value").val(),
            Sunday: $("#playlist_sunday_value").val()
        };
        var palylistItemItemsdata = [];
        if (palylistItemItems.length > 0) {
            $.each(palylistItemItems, function (index, palylistItem) {
                var playlistItem = {};
                var inputPlaylistItemName = $(palylistItem).find('.form-control.m-input');
                playlistItem.PlaylistItemName = inputPlaylistItemName.val();
                playlistItem.DisplayIntevalSeconds = 20;
                playlistItem.SildeshowEffects = 'Left to Right'
                playlistItem.type = $(palylistItem).attr('type');//images
                
                var imageItem = {};
                var imageId = [];
                var imagesrc = [];
                if ($(palylistItem).find("img").length > 0) {
                    $.each($(palylistItem).find("img"), function (index, imgItem) {
                        imageId.push(imgItem.id);
                        imagesrc.push(imgItem.src);
                    });
                }
                imageItem.name = 'imageItem'
                imageItem.id = imageId;
                imageItem.src = imagesrc;
                playlistItem.itemData = imageItem;
                palylistItemItemsdata.push(playlistItem); 
            });
            Settings.PlaylistItems= palylistItemItemsdata;
        }
        if (editflg) {
            $.insmFramework('creatPlaylist', {
                GroupID: tempselectedGroupID,
                PlayListName: $("#playlist_name").val(),
                InheritForced: '',
                Settings: JSON.stringify(Settings),
                Comments: $("#playlist_note").val(),
                success: function (playlistData) {
                    if (playlistData) {
                        div_playlist.hide();
                        div_Mainplaylist.show();
                    }
                }
            })
        } else {
            $.insmFramework('creatPlaylist', {
                GroupID: tempselectedGroupID,
                PlayListName: $("#playlist_name").val(),
                InheritForced: '',
                Settings: JSON.stringify(Settings),
                Comments: $("#playlist_note").val(),
                success: function (playlistData) {
                    if (playlistData) {
                        div_playlist.hide();
                        div_Mainplaylist.show();
                    }
                }
            })
        } 
    });
    $("#addPicture").click(function () {
        $.playlistEditor('greateNewItemPicture');
        
    });
    $("#addText").click(function () {
        $.playlistEditor('greateNewItemText');
    });
    $("#playlist_delete").click(function () {
        if (edit_playlistId) {
            $.insmFramework('deletePlaylist', {
                deletePlaylistId: edit_playlistId,
                success: function (fileData) {
                    div_playlist.hide();
                    div_Mainplaylist.show();
                    $.playlistEditor('getPlaylistByGroupID', { selectedGroupID: tempselectedGroupID });
                    edit_playlistId = null;
                },
                error: function () {
                },
            });
        }
        
    });

    $("#playlist_back").click(function () {
        div_playlist.hide();
        div_Mainplaylist.show();
    });

    $("#m_form_search").keyup(function () {
        $.each($("#div_PlaylistEditorContent").find(".m-portlet.m-portlet--warning.m-portlet--head-sm"), function (index, item) {
            if ($(item).find("h3").text().trim().toLowerCase().indexOf($("#m_form_search").val().toLowerCase()) > -1) {
                $(item).show();
            }
            else {
                $(item).hide();
            }
        })
    });

    $("#sort-alpha-asc").click(function () {
        var playlistDivs = $.makeArray($("#div_PlaylistEditorContent").find(".m-portlet.m-portlet--warning.m-portlet--head-sm"));
        playlistDivs.sort(function (a, b) {
            var aPlaylistName = $(a).find("h3").text().trim().toLowerCase();
            var bPlaylistName = $(b).find("h3").text().trim().toLowerCase();
            if (aPlaylistName == bPlaylistName) return 0;
            return (aPlaylistName < bPlaylistName) ? 1 : -1;
        })
        $.each(playlistDivs, function (playlistIndex, playlistItem) {
            $("#div_PlaylistEditorContent").append($(playlistItem));
        })
    });

    $("#sort-alpha-desc").click(function () {
        var playlistDivs = $.makeArray($("#div_PlaylistEditorContent").find(".m-portlet.m-portlet--warning.m-portlet--head-sm"));
        playlistDivs.sort(function (a, b) {
            var aPlaylistName = $(a).find("h3").text().trim().toLowerCase();
            var bPlaylistName = $(b).find("h3").text().trim().toLowerCase();
            if (aPlaylistName == bPlaylistName) return 0;
            return (aPlaylistName > bPlaylistName) ? 1 : -1;
        })
        $.each(playlistDivs, function (playlistIndex, playlistItem) {
            $("#div_PlaylistEditorContent").append($(playlistItem));
        })
    });

    $("#sort-numeric-asc").click(function () {
        var playlistDivs = $.makeArray($("#div_PlaylistEditorContent").find(".m-portlet.m-portlet--warning.m-portlet--head-sm"));
        playlistDivs.sort(function (a, b) {
            var aPlaylistName = new Date($(a).find("i.fa.fa-calendar").text().trim().toLowerCase());
            var bPlaylistName = new Date($(b).find("i.fa.fa-calendar").text().trim().toLowerCase());
            if (aPlaylistName == bPlaylistName) return 0;
            return (aPlaylistName < bPlaylistName) ? 1 : -1;
        })
        $.each(playlistDivs, function (playlistIndex, playlistItem) {
            $("#div_PlaylistEditorContent").append($(playlistItem));
        })
    });

    $("#sort-numeric-desc").click(function () {
        var playlistDivs = $.makeArray($("#div_PlaylistEditorContent").find(".m-portlet.m-portlet--warning.m-portlet--head-sm"));
        playlistDivs.sort(function (a, b) {
            var aPlaylistName = new Date($(a).find("i.fa.fa-calendar").text().trim().toLowerCase());
            var bPlaylistName = new Date($(b).find("i.fa.fa-calendar").text().trim().toLowerCase());
            if (aPlaylistName == bPlaylistName) return 0;
            return (aPlaylistName > bPlaylistName) ? 1 : -1;
        })
        $.each(playlistDivs, function (playlistIndex, playlistItem) {
            $("#div_PlaylistEditorContent").append($(playlistItem));
        })
    });
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-center",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
})(jQuery);