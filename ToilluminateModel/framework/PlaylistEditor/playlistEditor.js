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
    //var tempselectedGroupID = null;
    var selectedFolderID = null;
    var currentNeededFileType = "image";
    //var editplaylistID = null;
    var select_palylistItem = null;
    var methods = {
        init: function (options) {
            // Global vars
            var $this = $('body').eq(0);
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
                    htmlElements: {
                        groupplayerDetail: {
                            container: $('<div />').addClass('row').attr('id', 'div_playlist').css('display', 'none'),
                            containercol: $('<div />').addClass('col-xl-12'),
                            detailBody: {
                                detailBodycontainer: $('<div />').addClass('m-portlet m-portlet--success m-portlet--head-solid-bg m-portlet--rounded'),
                                detailhead: $('<div />').addClass('m-portlet__head'),
                                detailcaption: $('<div />').addClass('m-portlet__head-caption'),
                                detailtitle: $('<div />').addClass('m-portlet__head-title'),
                                span: $('<span />').addClass('m-portlet__head-icon'),
                                spani: $('<i />').addClass('fa fa-gear'),
                                spanh: $('<h3 />').addClass('m-portlet__head-text').text(''),
                                //Button
                                headtools: $('<div />').addClass('m-portlet__head-tools'),
                                toolsul: $('<ul />').addClass('m-portlet__nav'),
                                toolsSave: $('<li />').addClass('m-portlet__nav-item'),
                                deletebutton: $('<a />').addClass('m-portlet__nav-link btn btn-light m-btn m-btn--pill m-btn--air intros').attr('id', 'button_save').text($.localize('translate', 'Delete')),
                                toolssavePlayer: $('<li />').addClass('m-portlet__nav-item'),
                                savePlaylistbutton: $('<a />').addClass('m-portlet__nav-link btn btn-light m-btn m-btn--pill m-btn--air intros').attr('id', 'button_save_Player').text($.localize('translate', 'Save')),
                                toolsBack: $('<li />').addClass('m-portlet__nav-item'),
                                backbutton: $('<a />').addClass('m-portlet__nav-link m-dropdown__toggle btn btn-light m-btn m-btn--pill m-btn--air intros').attr('id', 'button_save').text($.localize('translate', 'Back')),
                                //
                                detail: {
                                    container: $('<div />').addClass('m-portlet__body'),
                                    content: $('<div />').addClass('row'),
                                    contentcol: $('<div />').addClass('col-xl-4'),
                                    contentLoop: {
                                        Loop: $('<div />').addClass('m-form__group form-group row'),
                                        Looplabel: $('<label />').addClass('col-3 col-form-label labeltext').text($.localize('translate', 'Loop:')),
                                        LoopRadio: $('<div />').addClass('col-9'),
                                        Loopgroup: $('<div />').addClass('btn-group').attr('data-toggle', 'buttons'),
                                        //On
                                        LoopOn: $('<label />').addClass('btn m-btn--pill m-btn--air btn-warning labeltext').attr('id', 'label_loop_1'),
                                        LoopOnInput: $('<input type="radio" />').addClass('').attr('name', 'radio_Loop').attr('id', 'playlist_loop_On').attr('value', '1').attr('autocomplete', 'off'),
                                        //Off
                                        LoopOff: $('<label />').addClass('btn m-btn--pill m-btn--air btn-warning labeltext').attr('id', 'label_loop_0'),
                                        LoopOffInput: $('<input type="radio" />').addClass('').attr('name', 'radio_Loop').attr('id', 'playlist_loop_Off').attr('value', '0').attr('autocomplete', 'off'),
                                    },
                                    contentPlaytime: {
                                        Playtime: $('<div />').addClass('m-form__group form-group row'),
                                        Playtimelabel: $('<label />').addClass('col-3 col-form-label labeltext').text($.localize('translate', 'Fixed playtime:')),
                                        PlaytimeRadio: $('<div />').addClass('col-9'),
                                        Playtimegroup: $('<div />').addClass('btn-group').attr('data-toggle', 'buttons'),
                                        //On
                                        PlaytimeOn: $('<label />').addClass('btn m-btn--pill m-btn--air btn-warning labeltext').attr('id', 'label_playtime_1'),
                                        PlaytimeOnInput: $('<input type="radio" />').addClass('').attr('name', 'radio_Playtime').attr('id', 'playlist_playtime_On').attr('value', '1').attr('autocomplete', 'off'),
                                        //Off
                                        PlaytimeOff: $('<label />').addClass('btn m-btn--pill m-btn--air btn-warning labeltext').attr('id', 'label_playtime_0'),
                                        PlaytimeOffInput: $('<input type="radio" />').addClass('').attr('name', 'radio_Playtime').attr('id', 'playlist_playtime_Off').attr('value', '0').attr('autocomplete', 'off'),
                                    },
                                    displaylabel: {
                                        container: $('<div />').addClass('form-group m-form__group'),
                                        title: $('<label />').addClass('form-group m-form__group').attr('for', 'exampleInputEmail1').text($.localize('translate', 'Playlist Name:')),
                                        input: $('<input type="text" />').addClass('form-group m-form__group').attr('id', 'groupname').addClass('form-control m-input'),
                                    },
                                    displayunits: {
                                        container: $('<div />').addClass('m-form__group form-group row'),
                                        playtimetitle: $('<div />').addClass('col-lg-12 col-md-12 col-sm-12'),
                                        title: $('<label />').attr('for', 'exampleInputEmail1').text($.localize('translate', 'Play time(Hours/Minites/Seconds):')),
                                        playtimehours: $('<div />').addClass('col-lg-4 col-md-9 col-sm-12'),
                                        hours: $('<div />').addClass('m-bootstrap-touchspin-brand'),
                                        hoursinput: $('<input type="text"/>').addClass('form-control bootstrap-touchspin-vertical-btn').attr('id', 'm_touchspin_1').attr('value', '').attr('name', 'demo1').attr('placeholder', '0'),

                                        playtimeMinites: $('<div />').addClass('col-lg-4 col-md-9 col-sm-12'),
                                        minites: $('<div />').addClass('m-bootstrap-touchspin-brand'),
                                        minitesinput: $('<input type="text"/>').addClass('form-control bootstrap-touchspin-vertical-btn').attr('id', 'm_touchspin_2').attr('value', '').attr('name', 'demo1').attr('placeholder', '0'),

                                        playtimeSeconds: $('<div />').addClass('col-lg-4 col-md-9 col-sm-12'),
                                        seconds: $('<div />').addClass('m-bootstrap-touchspin-brand'),
                                        secondsinput: $('<input type="text"/>').addClass('form-control bootstrap-touchspin-vertical-btn').attr('id', 'm_touchspin_3').attr('value', '').attr('name', 'demo1').attr('placeholder', '0'),

                                        comment: $('<div />').addClass('m-form__group form-group'),
                                        commentlabel: $('<label />').text($.localize('translate', 'Note:')),
                                        commenttextarea: $('<textarea />').attr('id', 'text_note').addClass('form-control m-input m-input--air').attr('rows', '3'),
                                    },
                                    editGroup: {
                                        container: $('<div />').addClass('m-form__group form-group row'),
                                        treebody: $('<div />').addClass('m-portlet__body'),
                                        tree: $('<div />').addClass('tree-demo groupTree')
                                    },
                                    displaytimes: {
                                        title: $('<div />').addClass('m-form__group form-group row'),
                                        titlelabel: $('<label />').attr('for', 'exampleInputEmail1').text($.localize('translate', 'Weekly time options:')),
                                        timebody: {
                                            Mondaycontainer: $('<div />'),
                                            Tuesdaycontainer: $('<div />'),
                                            Wednesdaycontainer: $('<div />'),
                                            Thursdaycontainer: $('<div />'),
                                            Fridaycontainer: $('<div />'),
                                            Saturdaycontainer: $('<div />'),
                                            Sundaycontainer: $('<div />'),
                                        },
                                    },
                                    showPlaylist: {
                                        container: $('<div />').addClass('col-xl-4').attr('id', 'group_player_playlist').css('max-height', '1000px').css('overflow-y', 'auto'),
                                    },
                                    showPlaylistForced: {
                                        container: $('<div />').addClass('col-xl-4 sortable-playlists ui-sortable').attr('id', 'forcedplaylists').css('max-height', '1000px').css('overflow-y', 'auto'),
                                    }
                                }
                            },
                            playlistItem: {
                                container: $('<div />').addClass('col-xl-8 sortable-playlists'),
                                playlistcontainer: $('<div />').attr('id', 'playlistItem').css('max-height', '60%').css('width', '100%').css('overflow-y', 'auto%').css('overflow-x', 'hidden'),
                                addItem: {
                                    addItemcontainer: $('<div />').addClass('m-portlet m-portlet--mobile m-portlet--sortable m-portlet--warning m-portlet--head-solid-bg'),
                                    addItembody: $('<div />').addClass('m-portlet__head ui-sortable-handle'),
                                    addButton: {
                                        addButtoncontainer: $('<div />').addClass('m-portlet__head-caption'),
                                        title: $('<div />').addClass('m-portlet__head-title'),
                                        span: $('<span />').addClass('m-portlet__head-icon'),
                                        spani: $('<i />').addClass('fa fa-file-text'),
                                        titlelabel: $('<h3 />').addClass('m-portlet__head-text'),
                                    },
                                    itemSelect: {
                                        container: $('<div />').addClass('m-portlet__head-tools'),
                                        ul: $('<ul />').addClass('m-portlet__nav'),
                                        li: $('<li />').addClass('m-portlet__nav-item m-dropdown m-dropdown--inline m-dropdown--arrow m-dropdown--align-right m-dropdown--align-push').attr('data-dropdown-toggle', 'hover'),
                                        a: $('<a  href="#"/>').addClass('m-portlet__nav-link m-portlet__nav-link--icon m-dropdown__toggle'),
                                        i: $('<i />').addClass('fa-plus fa'),
                                        selectItembody: {
                                            container: $('<div />').addClass('m-dropdown__wrapper'),
                                            containerspan: $('<span />').addClass('m-dropdown__arrow m-dropdown__arrow--right m-dropdown__arrow--adjust'),
                                            items: $('<div />').addClass('m-dropdown__inner'),
                                            itemsbody: $('<div />').addClass('m-dropdown__body'),
                                            itemsbodycontent: $('<div />').addClass('"m-dropdown__content'),
                                            ul: $('<ul />').addClass('m-nav'),
                                            liaddPicture: $('<li />').addClass('m-nav__item').attr('id', 'addPicture'),
                                            aaddPicture: $('<a  href="#"/>').addClass('m-nav__link'),
                                            iaddPicture: $('<i />').addClass('m-nav__link-icon flaticon-share'),
                                            spanaddPicture: $('<span />').addClass('m-nav__link-text'),

                                            liaddVideo: $('<li />').addClass('m-nav__item').attr('id', 'addVideo'),
                                            aaddVideo: $('<a  href="#"/>').addClass('m-nav__link'),
                                            iaddVideo: $('<i />').addClass('m-nav__link-icon flaticon-share'),
                                            spanaddVideo: $('<span />').addClass('m-nav__link-text'),

                                            liaddText: $('<li />').addClass('m-nav__item').attr('id', 'addText'),
                                            aaddText: $('<a  href="#"/>').addClass('m-nav__link'),
                                            iaddText: $('<i />').addClass('m-nav__link-icon flaticon-share'),
                                            spanaddText: $('<span />').addClass('m-nav__link-text'),

                                            liaddQRCode: $('<li />').addClass('m-nav__item').attr('id', 'addQRCode'),
                                            aaddQRCode: $('<a  href="#"/>').addClass('m-nav__link'),
                                            iaddQRCode: $('<i />').addClass('m-nav__link-icon flaticon-share'),
                                            spanaddQRCode: $('<span />').addClass('m-nav__link-text'),
                                        }
                                    }
                                }
                            },
                            playlistItemscontainer: $('<div />').addClass('m-portlet__body').css('max-height', '70px').css('overflow-y', 'auto'),
                        }
                    },
                    cache: {
                        players: {},
                        editflg : false
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

                $this.find('#div_playlist').first().
                    //Detail
                    append(_plugin.htmlElements.groupplayerDetail.containercol.
                      append(_plugin.htmlElements.groupplayerDetail.detailBody.detailBodycontainer.
                        append(_plugin.htmlElements.groupplayerDetail.detailBody.detailhead.
                          append(_plugin.htmlElements.groupplayerDetail.detailBody.detailcaption.
                            append(_plugin.htmlElements.groupplayerDetail.detailBody.detailtitle.
                                append(_plugin.htmlElements.groupplayerDetail.detailBody.span.
                                    append(_plugin.htmlElements.groupplayerDetail.detailBody.spani)
                                        )
                                .append(_plugin.htmlElements.groupplayerDetail.detailBody.spanh)
                                 )
                              ).
                              //BUTTON
                              append(_plugin.htmlElements.groupplayerDetail.detailBody.headtools.
                                append(_plugin.htmlElements.groupplayerDetail.detailBody.toolsul.
                                  append(_plugin.htmlElements.groupplayerDetail.detailBody.toolsSave.
                                     append(_plugin.htmlElements.groupplayerDetail.detailBody.deletebutton)).
                                  append(_plugin.htmlElements.groupplayerDetail.detailBody.toolssavePlayer.
                                     append(_plugin.htmlElements.groupplayerDetail.detailBody.savePlaylistbutton)).
                                   append(_plugin.htmlElements.groupplayerDetail.detailBody.toolsBack.
                                     append(_plugin.htmlElements.groupplayerDetail.detailBody.backbutton))
                                 )
                              )
                          ).
                          append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.container.
                            append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.content.
                                append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentcol
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.container.
                                        append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.title)
                                        .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.input)
                                    )
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentLoop.Loop.
                                        append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentLoop.Looplabel)
                                        .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentLoop.LoopRadio.
                                            append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentLoop.Loopgroup
                                                .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentLoop.LoopOn.
                                                    append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentLoop.LoopOnInput)
                                                    .append($.localize('translate', 'On'))
                                                )
                                                .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentLoop.LoopOff.
                                                    append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentLoop.LoopOffInput)
                                                    .append($.localize('translate', 'Off'))
                                                )
                                            )
                                        )
                                    )
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentPlaytime.Playtime.
                                        append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentPlaytime.Playtimelabel)
                                        .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentPlaytime.PlaytimeRadio.
                                            append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentPlaytime.Playtimegroup
                                                .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentPlaytime.PlaytimeOn.
                                                    append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentPlaytime.PlaytimeOnInput)
                                                    .append($.localize('translate', 'On'))
                                                )
                                                .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentPlaytime.PlaytimeOff.
                                                    append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentPlaytime.PlaytimeOffInput)
                                                    .append($.localize('translate', 'Off'))
                                                )
                                            )
                                        )
                                    )

                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.container.
                                        append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.playtimetitle.
                                        append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.title))
                                        .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.playtimehours.
                                            append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.hours.
                                                append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.hoursinput)))
                                        .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.playtimeMinites.
                                            append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.minites.
                                                append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.minitesinput)))
                                        .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.playtimeSeconds.
                                            append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.seconds.
                                                append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.secondsinput)))

                                    )

                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.comment
                                        .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.commentlabel)
                                        .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.commenttextarea)

                                    )

                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.container
                                        .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.treebody.
                                            append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree)
                                        )
                                    )
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.title
                                        .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.titlelabel)
                                    )
                                    //Monday
                                   .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.timeOptions({ buttonText: $.localize('translate', 'Monday') }))
                                    //Tuesday
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.timeOptions({ buttonText: $.localize('translate', 'Tuesday') }))
                                    //Wednesday
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.timeOptions({ buttonText: $.localize('translate', 'Wednesday') }))
                                    //Thursday
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.timeOptions({ buttonText: $.localize('translate', 'Thursday') }))
                                    //Friday
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.timeOptions({ buttonText: $.localize('translate', 'Friday') }))
                                    //Saturday
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.timeOptions({ buttonText: $.localize('translate', 'Saturday') }))
                                    //Sunday
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.timeOptions({ buttonText: $.localize('translate', 'Sunday') }))

                                )
                                .append(_plugin.htmlElements.groupplayerDetail.playlistItem.container.
                                    append(_plugin.htmlElements.groupplayerDetail.playlistItem.playlistcontainer)
                                    .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.addItemcontainer
                                        .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.addItembody
                                        .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.addButton.addButtoncontainer
                                            .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.addButton.title
                                                .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.addButton.span
                                                    .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.addButton.spani
                                                    ))
                                                 .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.addButton.titlelabel.append($.localize('translate', 'Click on the + button to add new playlist item')))))
                                            .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.container
                                                .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.ul
                                                    .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.li
                                                        .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.a
                                                            .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.i))
                                                        .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.container
                                                        .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.containerspan)
                                                        .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.items
                    .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.itemsbody.append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.itemsbodycontent.append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.ul
                    .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.liaddPicture.append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.aaddPicture.append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.iaddPicture)
                    .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.spanaddPicture.append($.localize('translate', 'Picture Slideshow')))))
                    .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.liaddVideo.append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.aaddVideo.append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.iaddVideo)
                    .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.spanaddVideo.append($.localize('translate', 'Video Player')))))
                    .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.liaddText.append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.aaddText.append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.iaddText)
                    .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.spanaddText.append($.localize('translate', 'Text')))))
                    .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.liaddQRCode.append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.aaddQRCode.append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.iaddQRCode)
                    .append(_plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.spanaddQRCode.append($.localize('translate', 'QR Code')))))
                    ))))))))
                    )
                    .append(_plugin.htmlElements.groupplayerDetail.playlistItemscontainer)))
                            )
                          )
                        )
                      )

                $this.data('playlistEditor', _plugin);
                $.playlistEditor('initTimeOptionsInPlayerEdit');
                //delete
                _plugin.htmlElements.groupplayerDetail.detailBody.deletebutton.click(function () {
                    if (edit_playlistId) {
                        //toastr.warning("使用中ですので、削除できない。");
                        //return;
                        $.confirmBox({
                            title: "Warning",
                            message: '削除しても宜しいでしょうか？',
                            onOk: function () {
                                $.insmFramework('deletePlaylist', {
                                    deletePlaylistId: edit_playlistId,
                                    deletepalylistItem: $.playlist('getselect_palylistItem'),
                                    success: function (fileData) {
                                        //deletepalylistItem = null;
                                        div_playlist.hide();
                                        $('#div_Mainplaylist').show();
                                        $.playlist('init', {
                                            selectedGroupID: $.playlist('getselectGroupId')
                                        });
                                        $.playlist('getPlaylistByGroupID', { selectedGroupID: $.playlist('getselectGroupId') });
                                        edit_playlistId = null;
                                        toastr.success("操作が完了しました。");
                                    },
                                    error: function () {
                                    },
                                });
                            }
                        });
                    }
                });
                //save
                _plugin.htmlElements.groupplayerDetail.detailBody.savePlaylistbutton.click(function () {
                    $("#playlist_save").css('display', 'none');
                    //var div_AddnewItem = $("#playlistItem");
                    var div_AddnewItem = _plugin.htmlElements.groupplayerDetail.playlistItem.playlistcontainer;
                    var palylistItemItems = div_AddnewItem.find(".m-portlet__body.row");
                    var Settings = {};

                    Settings = {
                        MondayisCheck: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.find('input:checkbox').is(':checked'),
                        Monday: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.find("input:hidden").val(),
                        TuesdayisCheck: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.find('input:checkbox').is(':checked'),
                        Tuesday: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.find("input:hidden").val(),
                        WednesdayisCheck: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.find('input:checkbox').is(':checked'),
                        Wednesday: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.find("input:hidden").val(),
                        ThursdayisCheck: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.find('input:checkbox').is(':checked'),
                        Thursday: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.find("input:hidden").val(),
                        FridayisCheck: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.find('input:checkbox').is(':checked'),
                        Friday: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.find("input:hidden").val(),
                        SaturdayisCheck: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.find('input:checkbox').is(':checked'),
                        Saturday: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.find("input:hidden").val(),
                        SundayisCheck: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.find('input:checkbox').is(':checked'),
                        Sunday: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.find("input:hidden").val(),

                        PlayHours: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.hoursinput.val(),
                        PlayMinites: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.minitesinput.val(),
                        PlaySeconds: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.secondsinput.val(),

                        Loop: _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentLoop.Loop.find("input[name='radio_Loop']:checked").val(),
                        Playtime: _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentPlaytime.Playtime.find("input[name='radio_Playtime']:checked").val()
                    };

                    var palylistItemItemsdata = [];
                    if (palylistItemItems.length > 0) {
                        $.each(palylistItemItems, function (index, palylistItem) {
                            var playlistItem = {};
                            var inputPlaylistItemName = $(palylistItem).find('.form-control.m-input');
                            playlistItem.PlaylistItemName = inputPlaylistItemName.val();
                            playlistItem.type = $(palylistItem).attr('type');//images
                            if (playlistItem.type == '0') {
                                playlistItem.DisplayIntevalSeconds = $(palylistItem).find('.form-control.bootstrap-touchspin-vertical-btn').val();
                                playlistItem.SildeshowEffects = $(palylistItem).find('#m_select2_3').val();
                                playlistItem.PicturePostion = $(palylistItem).find('select').eq(1).val();
                                var imageItem = {};
                                var imageId = [];
                                var imagesrc = [];
                                var imagefileUrl = [];
                                if ($(palylistItem).find("img").length > 0) {
                                    $.each($(palylistItem).find("img"), function (index, imgItem) {
                                        imageId.push(imgItem.id);
                                        imagesrc.push(imgItem.src);
                                        imagefileUrl.push(imgItem.fileUrl);
                                    });
                                }
                                imageItem.name = 'imageItem'
                                imageItem.id = imageId;
                                imageItem.src = imagesrc;
                                imageItem.fileUrl = imagefileUrl;
                                playlistItem.itemData = imageItem;
                            }
                            if (playlistItem.type == '1') {
                                playlistItem.DisplayIntevalSeconds = $(palylistItem).find('.form-control.bootstrap-touchspin-vertical-btn').val();
                                playlistItem.SlidingSpeed = $(palylistItem).find('.form-control.bootstrap-touchspin-vertical-btn.slidingSpeed').val();
                                playlistItem.TextPostion = $(palylistItem).find('select').val();
                                playlistItem.itemTextData = $(palylistItem).find(".note-editable.panel-body").html();
                            }
                            if (playlistItem.type == '2') {
                                playlistItem.ZoomOption = $(palylistItem).find('select').val();
                                var imageItem = {};
                                var imageId = [];
                                var imagesrc = [];
                                var imagefileUrl = [];
                                if ($(palylistItem).find("img").length > 0) {
                                    $.each($(palylistItem).find("img"), function (index, imgItem) {
                                        imageId.push(imgItem.id);
                                        imagesrc.push(imgItem.src);
                                        imagefileUrl.push(imgItem.fileUrl);
                                    });
                                }
                                imageItem.name = 'imageItem'
                                imageItem.id = imageId;
                                imageItem.src = imagesrc;
                                imageItem.fileUrl = imagefileUrl;
                                playlistItem.itemData = imageItem;
                            }
                            if (playlistItem.type == '3') {
                                playlistItem.DisplayIntevalSeconds = $(palylistItem).find('.form-control.bootstrap-touchspin-vertical-btn').val();
                                playlistItem.DisplayPostion = $(palylistItem).find('select').val();
                                playlistItem.DisplaySize = $(palylistItem).find('select').eq(1).val();
                                var imageItem = {};
                                var imageId = [];
                                var imagesrc = [];
                                var imagefileUrl = [];
                                if ($(palylistItem).find("img").length > 0) {
                                    $.each($(palylistItem).find("img"), function (index, imgItem) {
                                        imageId.push(imgItem.id);
                                        imagesrc.push(imgItem.src);
                                        imagefileUrl.push(imgItem.fileUrl);
                                    });
                                }
                                imageItem.name = 'imageItem'
                                imageItem.id = imageId;
                                imageItem.src = imagesrc;
                                imageItem.fileUrl = imagefileUrl;
                                playlistItem.itemData = imageItem;
                            }
                            palylistItemItemsdata.push(playlistItem);
                        });
                        Settings.PlaylistItems = palylistItemItemsdata;
                    }
                    if (_plugin.cache.editflg) {
                        $.insmFramework('editPlaylist', {
                            GroupID: $.playlist('getselectGroupId'),
                            playlistId: $.playlist('getselect_palylistItem').PlayListID,
                            //PlayListName: $("#playlist_name").val(),
                            PlayListName: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.input.val(),
                            InheritForced: '',
                            Settings: JSON.stringify(Settings),
                            //Comments: $("#playlist_note").val(),
                            Comments: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.commenttextarea.val(),
                            success: function (playlistData) {

                                div_playlist.hide();
                                $('#div_Mainplaylist').show();
                                $.playlist('init', {
                                    selectedGroupID: $.playlist('getselectGroupId')
                                });
                                $.playlist('getPlaylistByGroupID', { selectedGroupID: $.playlist('getselectGroupId') });

                                toastr.success("操作が完了しました。");
                                $("#playlist_save").css('display', '');
                            },
                            error: function () {
                                $("#playlist_save").css('display', '');
                            }
                        })
                    } else {
                        $.insmFramework('creatPlaylist', {
                            GroupID: $.playlist('getselectGroupId'),
                            //PlayListName: $("#playlist_name").val(),
                            PlayListName: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.input.val(),
                            InheritForced: '',
                            Settings: JSON.stringify(Settings),
                            //Comments: $("#playlist_note").val(),
                            Comments: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.commenttextarea.val(),
                            success: function (playlistData) {

                                div_playlist.hide();
                                $('#div_Mainplaylist').show();
                                $.playlist('init', {
                                    selectedGroupID: $.playlist('getselectGroupId')
                                });
                                $.playlist('getPlaylistByGroupID', { selectedGroupID: $.playlist('getselectGroupId') });

                                toastr.success("操作が完了しました。");
                                $("#playlist_save").css('display', '');
                            },
                            error: function () {
                                $("#playlist_save").css('display', '');
                            }
                        })
                    }

                });
                //back
                _plugin.htmlElements.groupplayerDetail.detailBody.backbutton.click(function () {
                    div_playlist.hide();
                    $('#div_Mainplaylist').show();
                });

                _plugin.htmlElements.groupplayerDetail.playlistItem.playlistcontainer.sortable({
                    connectWith: ".m-portlet__head",
                    items: "div.m-portlet.m-portlet--mobile.m-portlet--sortable.m-portlet--warning.m-portlet--head-solid-bg",
                    opacity: 0.8,
                    handle: '.m-portlet__head',
                    coneHelperSize: true,
                    placeholder: 'm-portlet--sortable-placeholder',
                    forcePlaceholderSize: true,
                    tolerance: "pointer",
                    helper: "clone",
                    tolerance: "pointer",
                    forcePlaceholderSize: !0,
                    helper: "clone",
                    cancel: ".m-portlet--sortable-empty", // cancel dragging if portlet is in fullscreen mode
                    revert: 250, // animation in milliseconds
                    update: function (b, c) {
                        if (c.item.prev().hasClass("m-portlet--sortable-empty")) {
                            c.item.prev().before(c.item);
                        }
                    }
                });

                //NewItemPicture
                _plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.liaddPicture.click(function () {
                    $.playlistEditor('greateNewItemPicture');

                });
                //NewItemText
                _plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.liaddText.click(function () {
                    $.playlistEditor('greateNewItemText');

                });
                //NewItemvideo
                _plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.liaddVideo.click(function () {
                    $.playlistEditor('greateNewItemvideo');

                });
                //NewItemQRCode
                _plugin.htmlElements.groupplayerDetail.playlistItem.addItem.itemSelect.selectItembody.liaddQRCode.click(function () {
                    $.playlistEditor('greateNewItemQRCode');

                });
                
            }

            return $this;
        },
        
        short: function (options) {
        },
        initTimeOptionsInPlayerEdit: function () {
            var $this = $('body').eq(0);
            var _plugin = $this.data('playlistEditor');


            $(".player-editor-timeoptions-hidden-input").each(function () {
                $(this).ionRangeSlider({
                    type: "double",
                    min: 0,
                    max: 24,
                    from: 0,
                    to: 24,
                    postfix: $.localize('translate', " o'clock"),
                    decorate_both: true,
                    grid: true,
                });
            })
        },
        playlistDefaultvalue: function (options) {
            var $this = $('body').eq(0);
            var _plugin = $this.data('playlistEditor');
            if (options) {
                _plugin.cache.editflg = options.editflg;
                if (!options.editflg) { _plugin.htmlElements.groupplayerDetail.detailBody.toolsSave.hide()}
                
            }
            //$('#playlist_name').val('New Playlist Name');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.input.val('New Playlist Name');
            //$('#playlist_note').val('');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.commenttextarea.val('');
            //monday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.find('label').addClass('Loop');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: 0, to: 24 });
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.find('checkbox').attr("checked", true);
            //tuesday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.find('label').addClass('Loop');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: 0, to: 24 });
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.find('checkbox').attr("checked", true);
            //wednesday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.find('label').addClass('Loop');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: 0, to: 24 });
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.find('checkbox').attr("checked", true);
            //thursday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.find('label').addClass('Loop');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: 0, to: 24 });
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.find('checkbox').attr("checked", true);
            //friday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.find('label').addClass('Loop');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: 0, to: 24 });
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.find('checkbox').attr("checked", true);
            //saturday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.find('label').addClass('Loop');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: 0, to: 24 });
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.find('checkbox').attr("checked", true);
            //sunday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.find('label').addClass('Loop');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: 0, to: 24 });
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.find('checkbox').attr("checked", true);

            //$("#label_loop_0").click();
            //$("#label_playtime_0").click();
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentLoop.LoopOff.click();
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentPlaytime.PlaytimeOff.click();
            //$("#m_touchspin_1").val('0');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.hoursinput.val('0'),
            //$("#m_touchspin_2").val('0');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.minitesinput.val('0')
            //$("#m_touchspin_3").val('0');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.secondsinput.val('0')
            //var div_AddnewItem = $("#playlistItem");
            var div_AddnewItem = _plugin.htmlElements.groupplayerDetail.playlistItem.playlistcontainer;
            div_AddnewItem.empty();
            $("#div_playlist").find("h3:first").text($.localize('translate', 'New Playlist'));
            $.playlistEditor('EnableTouchSpin');
        },
        editPlaylist: function (options) {
            var $this = $('body').eq(0);
            var _plugin = $this.data('playlistEditor');

            _plugin.cache.editflg = true;
            if (options.playlistID) {
                $.playlistEditor('playlistDefaultvalue');

                div_playlist.show();
                $('#div_Mainplaylist').hide();
                edit_playlistId = options.playlistID;
                $.insmFramework('getPlaylistByPlaylistID', {
                    playlistID: options.playlistID,
                    success: function (playlistData) {
                        if (playlistData) {
                            var Settings = JSON.parse(playlistData.Settings);
                            //$('#playlist_name').val(playlistData.PlayListName);
                            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.input.val(playlistData.PlayListName);
                            //$('#playlist_note').val(playlistData.Comments);
                            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.commenttextarea.val(playlistData.Comments);
                            $("#div_playlist").find("h3:first").text(playlistData.PlayListName);
                        }
                        if (Settings != null) {
                            if (Object.getOwnPropertyNames(Settings).length > 0) {
                                $.playlistEditor('setTimeOptions', Settings);

                                if (Settings.resolution) {
                                    $('#select_resolution').val(Settings.resolution);
                                }
                                
                            }                           

                            $("#label_loop_" + Settings.Loop).click();
                            $("#label_playtime_" + Settings.Playtime).click();

                            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.hoursinput.val(Settings.PlayHours);
                            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.minitesinput.val(Settings.PlayMinites);
                            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.secondsinput.val(Settings.PlaySeconds);

                            if (Settings.PlaylistItems) {
                                if (Settings.PlaylistItems.length > 0) {
                                    $.each(Settings.PlaylistItems, function (index, palylistItem) {
                                        if (palylistItem.type) {
                                            switch (palylistItem.type.toLowerCase()) {
                                                case "0":
                                                    $.playlistEditor('greateNewItemPicture', {
                                                        palylistItem: palylistItem
                                                    });
                                                    break;
                                                case "1":
                                                    $.playlistEditor('greateNewItemText', {
                                                        palylistItem: palylistItem
                                                    });
                                                    break;
                                                case "2":
                                                    $.playlistEditor('greateNewItemvideo', {
                                                        palylistItem: palylistItem
                                                    });
                                                    break;
                                                case "3":
                                                    $.playlistEditor('greateNewItemQRCode', {
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
        EnableTouchSpin: function () {
            $('#m_touchspin_1,#m_touchspin_4').TouchSpin({
                buttondown_class: 'btn btn-secondary',
                buttonup_class: 'btn btn-secondary',
                verticalbuttons: true,
                verticalupclass: 'la la-angle-up',
                verticaldownclass: 'la la-angle-down',
                min: 0,
                max: 23
            });
            $('#m_touchspin_2, #m_touchspin_3').TouchSpin({
                buttondown_class: 'btn btn-secondary',
                buttonup_class: 'btn btn-secondary',
                verticalbuttons: true,
                verticalupclass: 'la la-angle-up',
                verticaldownclass: 'la la-angle-down',
                min: 0,
                max: 59
            });
        },
        setTimeOptions: function (Settings) {
            var $this = $('body').eq(0);
            var _plugin = $this.data('playlistEditor');
            //Monday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.find('checkbox').attr("checked", Settings.MondayisCheck);
            if (Settings.MondayisCheck) {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.find('label').addClass('Loop');
            } else {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.find('label').removeClass('Loop');
            }
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: Settings.Monday.split(';')[0], to: Settings.Monday.split(';')[1] });

            //Tuesday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.find('checkbox').attr("checked", Settings.TuesdayisCheck);
            if (Settings.TuesdayisCheck) {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.find('label').addClass('Loop');
            } else {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.find('label').removeClass('Loop');
            }
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: Settings.Tuesday.split(';')[0], to: Settings.Tuesday.split(';')[1] });


            //Wednesday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.find('checkbox').attr("checked", Settings.WednesdayisCheck);
            if (Settings.WednesdayisCheck) {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.find('label').addClass('Loop');
            } else {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.find('label').removeClass('Loop');
            }
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: Settings.Wednesday.split(';')[0], to: Settings.Wednesday.split(';')[1] });

            //Thursday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.find('checkbox').attr("checked", Settings.ThursdayisCheck);
            if (Settings.ThursdayisCheck) {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.find('label').addClass('Loop');
            } else {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.find('label').removeClass('Loop');
            }
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: Settings.Thursday.split(';')[0], to: Settings.Thursday.split(';')[1] });

            //Friday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.find('checkbox').attr("checked", Settings.FridayisCheck);
            if (Settings.FridayisCheck) {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.find('label').addClass('Loop');
            } else {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.find('label').removeClass('Loop');
            }
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: Settings.Friday.split(';')[0], to: Settings.Friday.split(';')[1] });
            //Saturday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.find('checkbox').attr("checked", Settings.SaturdayisCheck);
            if (Settings.SaturdayisCheck) {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.find('label').addClass('Loop');
            } else {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.find('label').removeClass('Loop');
            }
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: Settings.Saturday.split(';')[0], to: Settings.Saturday.split(';')[1] });

            //Sunday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.find('checkbox').attr("checked", Settings.SundayisCheck);
            if (Settings.SundayisCheck) {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.find('label').addClass('Loop');
            } else {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.find('label').removeClass('Loop');
            }
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: Settings.Sunday.split(';')[0], to: Settings.Sunday.split(';')[1] });
        },
        setfolder: function (options) {
            selectedGroupID = $.playlist('getselectGroupId');
            $.playlistEditor('fileDataTableDestroy');
            $.insmFramework('getFolderTreeDataForPlaylist', {
                groupID: $.playlist('getselectGroupId'),
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
                        "video": [".mp4", ".wmv", ".mpeg", ".mpg", ".avi", ".mov", ".flv", ".mkv"],
                        "qrcode": [".jpg", ".gif", ".png", ".jpeg", ".bmp"],
                    };
                    var tempFileData = [];
                    $.each(fileData, function (fileIndex, fileItem) {
                        var tempFileType = supportedFileTypes[currentNeededFileType.toLowerCase()];
                        if (tempFileType && $.inArray(fileItem.FileExtension.toLowerCase().trim(), tempFileType) > -1) {
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
                    if (currentNeededFileType === "video") {
                        divselectFile.empty();
                    }
                    if (currentNeededFileType === "qrcode") {
                        divselectFile.empty();
                    }
                    var screenshot = new Image();
                    screenshot.src = $(item).data().obj.FileThumbnailUrl;
                    screenshot.fileUrl = $(item).data().obj.FileUrl;
                    screenshot.id = $(item).data().obj.FileID;
                    $(screenshot).css({ "max-height": "150px", "max-width": "200px", "padding": "5px" });
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
            var $this = $('body').eq(0);
            var _plugin = $this.data('playlistEditor');

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
            var href = $('<a href="#"/>').addClass("m-portlet__nav-link m-portlet__nav-link--icon");
            var href_i = $('<i />').addClass("la la-close");
            span_head_title.append(span_i);
            div_head_title.append(span_head_title, head_text.append($.localize('translate', 'PlaylistItemPicture<br>(Picture)')));
            div_head_caption.append(div_head_title);
            href.append(href_i)
            div_li.append(href)
            div_portlet_nav.append(div_li)
            div_head_tools.append(div_portlet_nav)
            div_head_handle.append(div_head_caption, div_head_tools)
            var div_body = $('<div/>').addClass('m-portlet__body row').css('height', 'auto').css('overflow-y', 'auto').attr('type', '0');
            var div_bodyMain = $('<div/>').addClass('col-xl-4');
            var div_body_group = $('<div/>').addClass('form-group m-form__group');
            var lable = $('<lable/>').text($.localize('translate', 'Playlist Item Name:'));
            var input = $("<input type='text'/>").addClass('form-control m-input').val($.localize('translate', 'New Playlist Item')).val($.localize('translate', 'New Playlist Item'));

            div_body_group.append(lable, input);
            div_bodyMain.append(div_body_group);

            var div_col = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var lable1 = $('<lable/>').text($.localize('translate', 'Display Inteval(Seconds):'));
            div_col.append(lable1);
            div_bodyMain.append(div_col);

            var div_col1 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var div_touchspin_brand = $('<div/>').addClass('m-bootstrap-touchspin-brand');
            var input1 = $("<input type='text'/>").addClass('form-control bootstrap-touchspin-vertical-btn').attr("name", "demo1").val("5");

            div_touchspin_brand.append(input1);
            div_col1.append(div_touchspin_brand);
            div_bodyMain.append(div_col1);

            var div_col2 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var lable2 = $('<lable/>').text($.localize('translate', 'Sildeshow effects:'));
            div_col2.append(lable2);
            div_bodyMain.append(div_col2);

            var div_col3 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var select_option = $('<select/>').addClass('form-control m-select2').attr("id", "m_select2_3").attr("multiple", "").attr("tabindex", "-1").attr("aria-hidden", "true");
            select_option.append("<option value='0'>" + $.localize('translate', 'Random') + "</option>");
            select_option.append("<option value='1'>" + $.localize('translate', 'Left to Right') + "</option>");
            select_option.append("<option value='2'>" + $.localize('translate', 'Right to Left') + "</option>");
            select_option.append("<option value='3'>" + $.localize('translate', 'Top to Bottom') + "</option>");
            select_option.append("<option value='4'>" + $.localize('translate', 'Bottom To Top') + "</option>");
            select_option.append("<option value='5'>" + $.localize('translate', 'Grow') + "</option>");
            select_option.append("<option value='6'>" + $.localize('translate', 'Fadein') + "</option>");
            select_option.append("<option value='7'>" + $.localize('translate', 'Rotate horizontally') + "</option>");
            select_option.append("<option value='8'>" + $.localize('translate', 'Rotate vertically') + "</option>");
            div_col3.append(select_option);
            div_bodyMain.append(div_col3);


            var div_colpicturetype = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var labletype = $('<lable/>').text($.localize('translate', 'Picture Postion:'));
            div_colpicturetype.append(labletype);
            div_bodyMain.append(div_colpicturetype);

            var div_coltypedata = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var select_typeoption = $('<select/>');
            select_typeoption.append("<option value='0'>" + $.localize('translate', 'Normal') + "</option>");
            select_typeoption.append("<option value='1'>" + $.localize('translate', 'Stretch') + "</option>");
            select_typeoption.append("<option value='2'>" + $.localize('translate', 'Fullscreen') + "</option>");
            div_coltypedata.append(select_typeoption);
            div_bodyMain.append(div_coltypedata);



            var div_col4 = $('<div/>').addClass('col-xl-8');
            var div_col4_main = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            div_col4.append(div_col4_main);

            var div_col_image = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12').css('overflow-y', 'auto').css('max-height', '400px').css('margin-top', '5px').css("line-height", "150px");
            var button_Selectimages = $("<button type='button'/>").attr("data-toggle", "modal").attr("data-target", "#m_modal_1").addClass('btn m-btn--pill m-btn--air         btn-outline-info btn-block').text($.localize('translate', 'Select images')).click(function () {
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
                div_head.remove();
            });

            //var div_AddnewItem = $("#playlistItem");
            var div_AddnewItem = _plugin.htmlElements.groupplayerDetail.playlistItem.playlistcontainer.append(div_head);
            //div_AddnewItem.append(div_head);
            select_option.select2({
                placeholder: $.localize('translate', "Select sildeshow effects")
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
                        $.each(options.palylistItem.itemData.id, function (index, item) {
                            var screenshot = new Image();
                            screenshot.src = options.palylistItem.itemData.fileUrl[index];
                            screenshot.fileUrl = options.palylistItem.itemData.fileUrl[index];
                            screenshot.id = item;
                            $(screenshot).css({ "max-height": "150px", "max-width": "200px", "padding": "5px" });
                            div_col_image.append(screenshot);
                            var deleteImg = $("<i class='fa fa-remove'></i>").css({ "position": "relative", "left": "-17px", "background-color": "none", "cursor": "pointer", "top": "-64px" }).click(function () {
                                $(screenshot).remove();
                                $(this).remove();
                            });
                            div_col_image.append(deleteImg);
                        });
                    }
                }
                if (options.palylistItem) {
                    if (options.palylistItem.SildeshowEffects) {
                        select_option.val(options.palylistItem.SildeshowEffects).trigger('change');
                    }
                }
                if (options.palylistItem) {
                    select_typeoption.val(options.palylistItem.PicturePostion);
                }
            }
        },
        greateNewItemText: function (options) {
            var $this = $('body').eq(0);
            var _plugin = $this.data('playlistEditor');
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
            var href = $('<a href="#"/>').addClass("m-portlet__nav-link m-portlet__nav-link--icon");
            var href_i = $('<i />').addClass("la la-close");
            span_head_title.append(span_i);
            div_head_title.append(span_head_title, head_text.append($.localize('translate', 'PlaylistItemText<br>(Text)')));
            div_head_caption.append(div_head_title);
            href.append(href_i)
            div_li.append(href)
            div_portlet_nav.append(div_li)
            div_head_tools.append(div_portlet_nav)
            div_head_handle.append(div_head_caption, div_head_tools)
            var div_body = $('<div/>').addClass('m-portlet__body row').css('height', 'auto').css('overflow-y', 'auto').attr('type', '1');
            var div_bodyMain = $('<div/>').addClass('col-xl-4');
            var div_body_group = $('<div/>').addClass('form-group m-form__group');
            var lable = $('<lable/>').text($.localize('translate', 'Playlist Item Name:'));
            var input = $("<input type='text'/>").addClass('form-control m-input').val($.localize('translate', 'New Playlist Item'));
            div_body_group.append(lable, input);
            div_bodyMain.append(div_body_group);

            var div_col = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var lable1 = $('<lable/>').text($.localize('translate', 'Display Inteval(Seconds):'));
            div_col.append(lable1);
            div_bodyMain.append(div_col);

            var div_col1 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var div_touchspin_brand = $('<div/>').addClass('m-bootstrap-touchspin-brand');
            var input1 = $("<input type='text'/>").addClass('form-control bootstrap-touchspin-vertical-btn').attr("name", "demo1").val("5");
            div_touchspin_brand.append(input1);
            div_col1.append(div_touchspin_brand);
            div_bodyMain.append(div_col1);

            var div_col2 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var lable2 = $('<lable/>').text($.localize('translate', 'Sliding Speed:'));
            div_col2.append(lable2);
            div_bodyMain.append(div_col2);

            var div_col3 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var div_touchspin_brand1 = $('<div/>').addClass('m-bootstrap-touchspin-brand');
            var input2 = $("<input type='text'/>").addClass('form-control bootstrap-touchspin-vertical-btn slidingSpeed').attr("name", "demo1").val("5");
            div_touchspin_brand1.append(input2);
            div_col3.append(div_touchspin_brand1);
            div_bodyMain.append(div_col3);

            var div_col4 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var lable4 = $('<lable/>').text($.localize('translate', 'Text Postion:'));
            div_col4.append(lable4);
            div_bodyMain.append(div_col4);

            var div_col5 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var select_option = $('<select/>');
            select_option.append("<option value='0'>" + $.localize('translate', 'Top') + "</option>");
            select_option.append("<option value='1'>" + $.localize('translate', 'Middle') + "</option>");
            select_option.append("<option value='2'>" + $.localize('translate', 'Bottom') + "</option>");
            div_col5.append(select_option);
            div_bodyMain.append(div_col5);

            var div_col6 = $('<div/>').addClass('col-xl-8');
            var div_col6_main = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');

            div_col6.append(div_col6_main);

            div_body.append(div_bodyMain, div_col6);
            div_head.append(div_head_handle, div_body)

            //var div_AddnewItem = $("#playlistItem");
            var div_AddnewItem = _plugin.htmlElements.groupplayerDetail.playlistItem.playlistcontainer;
            div_AddnewItem.append(div_head);
            div_li.click(function () {
                div_head.remove();
            });
            input1.TouchSpin({
                buttondown_class: 'btn btn-secondary',
                buttonup_class: 'btn btn-secondary',
                verticalbuttons: true,
                verticalupclass: 'la la-angle-up',
                verticaldownclass: 'la la-angle-down',
                min: 0,
                max: 9999999
            });

            input2.TouchSpin({
                buttondown_class: 'btn btn-secondary',
                buttonup_class: 'btn btn-secondary',
                verticalbuttons: true,
                verticalupclass: 'la la-angle-up',
                verticaldownclass: 'la la-angle-down',
                min: 0,
                max: 9999999
            });

            div_col6_main.summernote({
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
            if (options) {
                if (options.palylistItem) {
                    input.val(options.palylistItem.PlaylistItemName);
                }
                if (options.palylistItem) {
                    input1.val(options.palylistItem.DisplayIntevalSeconds);
                }
                if (options.palylistItem) {
                    input2.val(options.palylistItem.SlidingSpeed);
                }
                if (options.palylistItem) {
                    select_option.val(options.palylistItem.TextPostion);
                    div_col6_main.summernote("code", options.palylistItem.itemTextData);
                }
            }
        },
        greateNewItemvideo: function (options) {
            var $this = $('body').eq(0);
            var _plugin = $this.data('playlistEditor');
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
            var href = $('<a href="#"/>').addClass("m-portlet__nav-link m-portlet__nav-link--icon");
            var href_i = $('<i />').addClass("la la-close");
            span_head_title.append(span_i);
            div_head_title.append(span_head_title, head_text.append($.localize('translate', 'PlaylistItemVideo<br>(video)')));
            div_head_caption.append(div_head_title);
            href.append(href_i)
            div_li.append(href)
            div_portlet_nav.append(div_li)
            div_head_tools.append(div_portlet_nav)
            div_head_handle.append(div_head_caption, div_head_tools)
            var div_body = $('<div/>').addClass('m-portlet__body row').css('height', 'auto').css('overflow-y', 'auto').attr('type', '2');
            var div_bodyMain = $('<div/>').addClass('col-xl-4');
            var div_body_group = $('<div/>').addClass('form-group m-form__group');
            var lable = $('<lable/>').text($.localize('translate', 'Playlist Item Name:'));
            var input = $("<input type='text'/>").addClass('form-control m-input').val($.localize('translate', 'New Playlist Item'));
            div_body_group.append(lable, input);
            div_bodyMain.append(div_body_group);

            var div_col2 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var lable2 = $('<lable/>').text($.localize('translate', 'Zoom option:'));
            div_col2.append(lable2);
            div_bodyMain.append(div_col2);
            var div_col3 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var select_option = $('<select/>');
            select_option.append("<option value='0'>" + $.localize('translate', 'None') + "</option>");
            select_option.append("<option value='1'>" + $.localize('translate', 'Fullscreen') + "</option>");
            select_option.append("<option value='2'>" + $.localize('translate', 'Fullscreen with original aspect') + "</option>");
            div_col3.append(select_option);
            div_bodyMain.append(div_col3);

            var div_col4 = $('<div/>').addClass('col-xl-8');
            var div_col4_main = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            div_col4.append(div_col4_main);

            var div_col_image = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12').css('overflow-y', 'auto').css('max-height', '400px').css('margin-top', '5px');
            var button_Selectimages = $("<button type='button'/>").attr("data-toggle", "modal").attr("data-target", "#m_modal_1").addClass('btn m-btn--pill m-btn--air         btn-outline-info btn-block').text($.localize('translate', 'Select video')).click(function () {
                divselectFile = div_col_image;
                currentNeededFileType = "video";
                if ($("#datatable_file1").data("datatable")) {
                    $.playlistEditor('setfile');
                }
            });
            div_col4_main.append(button_Selectimages);
            div_col4.append(div_col_image);
            div_body.append(div_bodyMain, div_col4);
            div_head.append(div_head_handle, div_body)

            //var div_AddnewItem = $("#playlistItem");
            var div_AddnewItem = _plugin.htmlElements.groupplayerDetail.playlistItem.playlistcontainer;
            div_AddnewItem.append(div_head);

            div_li.click(function () {
                div_head.remove();
            });

            if (options) {
                if (options.palylistItem) {
                    input.val(options.palylistItem.PlaylistItemName);
                }
                if (options.palylistItem) {
                    if (options.palylistItem.itemData) {
                        $.each(options.palylistItem.itemData.src, function (index, item) {
                            var screenshot = new Image();
                            screenshot.src = item;
                            screenshot.fileUrl = options.palylistItem.itemData.fileUrl[index];
                            //screenshot.id = $(item).data().obj.FileID;
                            div_col_image.append(screenshot);
                        });
                    }
                }
                if (options.palylistItem) {
                    select_option.val(options.palylistItem.ZoomOption);
                }
            }
        },
        greateNewItemQRCode: function (options) {
            var $this = $('body').eq(0);
            var _plugin = $this.data('playlistEditor');
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
            var href = $('<a href="#"/>').addClass("m-portlet__nav-link m-portlet__nav-link--icon");
            var href_i = $('<i />').addClass("la la-close");
            span_head_title.append(span_i);
            div_head_title.append(span_head_title, head_text.append($.localize('translate', 'PlaylistItemQRCode<br>(Picture)')));
            div_head_caption.append(div_head_title);
            href.append(href_i)
            div_li.append(href)
            div_portlet_nav.append(div_li)
            div_head_tools.append(div_portlet_nav)
            div_head_handle.append(div_head_caption, div_head_tools)
            var div_body = $('<div/>').addClass('m-portlet__body row').css('height', 'auto').css('overflow-y', 'auto').attr('type', '3');
            var div_bodyMain = $('<div/>').addClass('col-xl-4');
            var div_body_group = $('<div/>').addClass('form-group m-form__group');
            var lable = $('<lable/>').text($.localize('translate', 'Playlist Item Name:'));
            var input = $("<input type='text'/>").addClass('form-control m-input').val($.localize('translate', 'New Playlist Item')).val($.localize('translate', 'New Playlist Item'));

            div_body_group.append(lable, input);
            div_bodyMain.append(div_body_group);

            var div_col = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var lable1 = $('<lable/>').text($.localize('translate', 'Display Inteval(Seconds):'));
            div_col.append(lable1);
            div_bodyMain.append(div_col);

            var div_col1 = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var div_touchspin_brand = $('<div/>').addClass('m-bootstrap-touchspin-brand');
            var input1 = $("<input type='text'/>").addClass('form-control bootstrap-touchspin-vertical-btn').attr("name", "demo1").val("5");

            div_touchspin_brand.append(input1);
            div_col1.append(div_touchspin_brand);
            div_bodyMain.append(div_col1);

            var div_colpicturetype = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var labletype = $('<lable/>').text($.localize('translate', 'Picture Postion:'));
            div_colpicturetype.append(labletype);
            div_bodyMain.append(div_colpicturetype);

            var div_coltypedata = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var select_typeoption = $('<select/>');
            select_typeoption.append("<option value='0'>" + $.localize('translate', 'Upper left') + "</option>");
            select_typeoption.append("<option value='1'>" + $.localize('translate', 'Upper middle') + "</option>");
            select_typeoption.append("<option value='2'>" + $.localize('translate', 'Upper right') + "</option>");
            select_typeoption.append("<option value='3'>" + $.localize('translate', 'Left') + "</option>");
            select_typeoption.append("<option value='4'>" + $.localize('translate', 'Middle') + "</option>");
            select_typeoption.append("<option value='5'>" + $.localize('translate', 'Right') + "</option>");
            select_typeoption.append("<option value='6'>" + $.localize('translate', 'Lower left') + "</option>");
            select_typeoption.append("<option value='7'>" + $.localize('translate', 'Lower middle') + "</option>");
            select_typeoption.append("<option value='8'>" + $.localize('translate', 'Lower right') + "</option>");
            div_coltypedata.append(select_typeoption);
            div_bodyMain.append(div_coltypedata);


            var div_size = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var lablesize = $('<lable/>').text($.localize('translate', 'size:'));
            div_size.append(lablesize);
            div_bodyMain.append(div_size);

            var div_sizedata = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            var select_sizeoption = $('<select/>');
            select_sizeoption.append("<option value='0'>" + $.localize('translate', 'Big') + "</option>");
            select_sizeoption.append("<option value='1'>" + $.localize('translate', 'Median') + "</option>");
            select_sizeoption.append("<option value='2'>" + $.localize('translate', 'Small') + "</option>");

            div_sizedata.append(select_sizeoption);
            div_bodyMain.append(div_sizedata);

            var div_col4 = $('<div/>').addClass('col-xl-8');
            var div_col4_main = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12');
            div_col4.append(div_col4_main);

            var div_col_image = $('<div/>').addClass('col-lg-12 col-md-12 col-sm-12').css('overflow-y', 'auto').css('max-height', '400px').css('margin-top', '5px').css("line-height", "150px");
            var button_Selectimages = $("<button type='button'/>").attr("data-toggle", "modal").attr("data-target", "#m_modal_1").addClass('btn m-btn--pill m-btn--air         btn-outline-info btn-block').text($.localize('translate', 'Select QRCode')).click(function () {
                divselectFile = div_col_image;
                currentNeededFileType = "qrcode";
                if ($("#datatable_file1").data("datatable")) {
                    $.playlistEditor('setfile');
                }
            });

            div_col4_main.append(button_Selectimages);
            div_col4.append(div_col_image);
            div_body.append(div_bodyMain, div_col4);
            div_head.append(div_head_handle, div_body)

            div_li.click(function () {
                div_head.remove();
            });

            var div_AddnewItem = _plugin.htmlElements.groupplayerDetail.playlistItem.playlistcontainer;
            div_AddnewItem.append(div_head);

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
                        $.each(options.palylistItem.itemData.id, function (index, item) {
                            var screenshot = new Image();
                            screenshot.src = options.palylistItem.itemData.fileUrl[index];
                            screenshot.fileUrl = options.palylistItem.itemData.fileUrl[index];
                            screenshot.id = item;
                            $(screenshot).css({ "max-height": "150px", "max-width": "200px", "padding": "5px" });
                            div_col_image.append(screenshot);
                            var deleteImg = $("<i class='fa fa-remove'></i>").css({ "position": "relative", "left": "-17px", "background-color": "none", "cursor": "pointer", "top": "-64px" }).click(function () {
                                $(screenshot).remove();
                                $(this).remove();
                            });
                            div_col_image.append(deleteImg);
                        });
                    }
                }
                if (options.palylistItem) {
                    select_typeoption.val(options.palylistItem.DisplayPostion);
                    select_sizeoption.val(options.palylistItem.DisplaySize);
                }
            }
        },
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