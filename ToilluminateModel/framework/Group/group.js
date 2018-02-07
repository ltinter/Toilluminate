(function ($) {
    //var div_groupTreeForFileManager = $("#groupTreeForFileManager");
    
    var methods = {
        init: function (options) {
            // Global vars
            var $this = $('body').eq(0);
            var _plugin = $this.data('insmGroup');

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
                        container: $('<div />').addClass('row').attr('id', 'div_main'),
                        containercol: $('<div />').addClass('col-xl-4'),
                        containerbody: $('<div />').addClass('m-portlet m-portlet--bordered-semi m-portlet--full-height'),
                        grouphead: {
                            container: $('<div />').addClass('m-portlet__head'),
                            headcaption: {
                                container: $('<div />').addClass('m-portlet__head-caption'),
                                headtitle: $('<div />').addClass('m-portlet__head-title'),
                                title: $('<h3 />').addClass('m-portlet__head-text intros').text($.localize('translate', 'Groups'))
                            },
                            headtools: {
                                container: $('<form />').addClass('m-portlet__head-tools'),
                                headtoolsul: $('<ul />').addClass('m-portlet__nav'),
                                headtoolsli: $('<li />').addClass('m-portlet__nav-item m-dropdown m-dropdown--inline m-dropdown--arrow m-dropdown--align-right m-dropdown--align-push').attr('data-dropdown-toggle', 'hover').attr('aria-expanded', 'true'),
                                headtoolsa: $('<a />').addClass('m-portlet__nav-link m-dropdown__toggle dropdown-toggle btn btn--sm m-btn--pill btn-secondary m-btn m-btn--label-brand intros').attr('id', 'editgroup').text($.localize('translate','Edit')),
                            },
                            headwrapper: {
                                container: $('<div />').addClass('m-dropdown__wrapper'),
                                span: $('<span />').addClass('m-dropdown__arrow m-dropdown__arrow--right m-dropdown__arrow--adjust').css('left', 'auto').css('right', '36.5px'),
                                dropdowninner: {
                                    container: $('<div />').addClass('m-dropdown__inner'),
                                    dropdowninnerbody: $('<div />').addClass('m-dropdown__body'),
                                    content: {
                                        container: $('<div />').addClass('m-dropdown__content'),
                                        contentul: $('<ul />').addClass('m-nav'),
                                        //New Group
                                        contentnewgroupli: $('<li />').addClass('m-nav__item').attr('id', 'newgroup'),
                                        contentnewgroupa: $('<a href="javascript:;" />').addClass('m-nav__link'),
                                        contentnewgroupi: $('<i />').addClass('m-nav__link-icon fa fa-plus'),
                                        newgroupspan: $('<span />').addClass('m-nav__link-text intros').text($.localize('translate','New Group')),
                                        //Delete Group
                                        contentdeletegroupli: $('<li />').addClass('m-nav__item').attr('id', 'deletegroup'),
                                        contentdeletegroupa: $('<a href="javascript:;" />').addClass('m-nav__link'),
                                        contentdeletegroupi: $('<i />').addClass('m-nav__link-icon fa fa-minus'),
                                        deletegroupspan: $('<span />').addClass('m-nav__link-text intros').text($.localize('translate','Delete Group')),
                                        //Expand All
                                        contentexpandAllli: $('<li />').addClass('m-nav__item').attr('id', 'expandAll'),
                                        contentexpandAlla: $('<a href="javascript:;" />').addClass('m-nav__link'),
                                        contentexpandAlli: $('<i />').addClass('m-nav__link-icon fa fa-angle-double-down'),
                                        expandAllspan: $('<span />').addClass('m-nav__link-text intros').text($.localize('translate','Expand All')),
                                        //Collapse All
                                        contentcollapseAllli: $('<li />').addClass('m-nav__item').attr('id', 'collapseAll'),
                                        contentcollapseAlla: $('<a href="javascript:;" />').addClass('m-nav__link'),
                                        contentcollapseAlli: $('<i />').addClass('m-nav__link-icon fa fa-angle-double-up'),
                                        collapseAllspan: $('<span />').addClass('m-nav__link-text intros').text($.localize('translate','Collapse All')),
                                    }
                                }
                            },
                        },
                        grouptreecontainer: $('<div />').addClass('m-portlet__body'),
                        grouptree: $('<div />').addClass('tree-demo groupTree').attr('id','groupTree'),
                        player: {
                            container: $('<div />').addClass('col-xl-8'),
                            containermain: $('<div />').addClass('m-portlet m-portlet--bordered-semi m-portlet--widget-fit m-portlet--full-height m-portlet--skin-light'),
                            playerBody: {
                                container: $('<div />').addClass('m-portlet__body').attr('id', 'PlayerDetail'),
                                playerform: $('<div />').addClass('m-form m-form--label-align-right m--margin-top-20 m--margin-bottom-30'),
                                containercenter: $('<div />').addClass('row align-items-center'),
                                playercol: {
                                    col: $('<div />').addClass('col-xl-4 order-2 order-xl-1'),
                                    groupitems: $('<div />').addClass('form-group m-form__group row align-items-center'),
                                    colmd: $('<div />').addClass('col-md-10'),
                                    minput: $('<div />').addClass('m-input-icon m-input-icon--left'),
                                    input: $('<input type="text"/>').addClass('form-control m-input').attr('id', 'm_form_search_Player'),
                                    inputicon: $('<span />').addClass('m-input-icon__icon m-input-icon__icon--left'),
                                    span: $('<span />'),
                                    spani: $('<i />').addClass('la la-search')
                                },
                                playerbuttoncol: {
                                    col: $('<div />').addClass('col-xl-8 order-1 order-xl-2 m--align-right'),
                                    buttongroup: $('<div />').addClass('btn-group').attr('data-toggle', 'buttons'),
                                    labelAll: $('<label />').addClass('btn m-btn--pill m-btn--air btn-warning active').attr('id', 'radio_All'),
                                    labelAllinput: $('<input type="radio"/>').attr('name', 'radio_1').attr('autocomplete', 'off').attr('checked', '').text($.localize('translate', 'All')),
                                    labelCurrent: $('<label />').addClass('btn m-btn--pill m-btn--air btn-warning').attr('id', 'radio_Current'),
                                    labelCurrentinput: $('<input type="radio"/>').attr('name', 'radio_1').attr('autocomplete', 'off').text($.localize('translate', 'Current')),

                                    buttons: $('<div />').addClass('btn-group m-btn-group m-btn-group--pill').attr('role', 'group').attr('aria-label', '...'),
                                    buttonAdd: $('<button />').addClass('m-btn btn btn-primary intros').attr('id', 'add_player').text($.localize('translate','Add')),
                                    buttonEdit: $('<button />').addClass('m-btn btn btn-primary intros').attr('id', 'edit_player').text($.localize('translate','Edit')),
                                    separator: $('<div />').addClass('m-separator m-separator--dashed d-xl-none')
                                },
                                playertable: $('<div />').addClass('m_datatable').attr('id', 'base_responsive_columns')
                            }
                        },
                        groupplayerDetail: {
                            container: $('<div />').addClass('row').attr('id', 'div_edit').css('display', 'none'),
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
                                savebutton: $('<a />').addClass('m-portlet__nav-link btn btn-light m-btn m-btn--pill m-btn--air intros').attr('id', 'button_save').text($.localize('translate','Save')),
                                toolssavePlayer: $('<li />').addClass('m-portlet__nav-item'),
                                savePlayerbutton: $('<a />').addClass('m-portlet__nav-link btn btn-light m-btn m-btn--pill m-btn--air intros').attr('id', 'button_save_Player').text($.localize('translate','Save')),
                                toolsBack: $('<li />').addClass('m-portlet__nav-item'),
                                backbutton: $('<a />').addClass('m-portlet__nav-link m-dropdown__toggle btn btn-light m-btn m-btn--pill m-btn--air intros').attr('id', 'button_save').text($.localize('translate','Back')),
                                //
                                detail: {
                                    container: $('<div />').addClass('m-portlet__body'),
                                    content: $('<div />').addClass('row'),
                                    contentcol: $('<div />').addClass('col-xl-4'),
                                    contentActive: {
                                        Active: $('<div />').addClass('m-form__group form-group row'),
                                        Activelabel: $('<label />').addClass('col-3 col-form-label labeltext').text($.localize('translate','Active:')),
                                        ActiveRadio: $('<div />').addClass('col-9'),
                                        Activegroup: $('<div />').addClass('btn-group').attr('data-toggle', 'buttons'),
                                        //Inherited
                                        ActiveInherited: $('<label />').addClass('btn m-btn--pill m-btn--air btn-warning active labeltext').attr('id', 'label_Active_2'),
                                        ActiveInheritedInput: $('<input type="radio" />').addClass('').attr('name', 'radio_Active').attr('id', 'radio_Active_1').attr('value', '2').attr('autocomplete', 'off').attr('checked', ''),
                                        //On
                                        ActiveOn: $('<label />').addClass('btn m-btn--pill m-btn--air btn-warning labeltext').attr('id', 'label_Active_1'),
                                        ActiveOnInput: $('<input type="radio" />').addClass('').attr('name', 'radio_Active').attr('id', 'radio_Active_2').attr('value', '1').attr('autocomplete', 'off'),
                                        //Off
                                        ActiveOff: $('<label />').addClass('btn m-btn--pill m-btn--air btn-warning labeltext').attr('id', 'label_Active_0'),
                                        ActiveOffInput: $('<input type="radio" />').addClass('').attr('name', 'radio_Active').attr('id', 'radio_Active_3').attr('value', '0').attr('autocomplete', 'off'),
                                    },
                                    contentOnline: {
                                        Online: $('<div />').addClass('m-form__group form-group row'),
                                        Onlinelabel: $('<label />').addClass('col-3 col-form-label labeltext').text($.localize('translate','Online units:')),
                                        OnlineRadio: $('<div />').addClass('col-9'),
                                        Onlinegroup: $('<div />').addClass('btn-group').attr('data-toggle', 'buttons'),
                                        //Inherited
                                        OnlineInherited: $('<label />').addClass('btn m-btn--pill m-btn--air btn-warning active labeltext').attr('id', 'label_Online_2'),
                                        OnlineInheritedInput: $('<input type="radio" />').addClass('').attr('name', 'radio_Online').attr('id', 'radio_Online_1').attr('value', '2').attr('autocomplete', 'off').attr('checked', ''),
                                        //On
                                        OnlineOn: $('<label />').addClass('btn m-btn--pill m-btn--air btn-warning labeltext').attr('id', 'label_Online_1'),
                                        OnlineOnInput: $('<input type="radio" />').addClass('').attr('name', 'radio_Online').attr('id', 'radio_Online_2').attr('value', '1').attr('autocomplete', 'off'),
                                        //Off
                                        OnlineOff: $('<label />').addClass('btn m-btn--pill m-btn--air btn-warning labeltext').attr('id', 'label_Online_0'),
                                        OnlineOffInput: $('<input type="radio" />').addClass('').attr('name', 'radio_Online').attr('id', 'radio_Online_3').attr('value', '0').attr('autocomplete', 'off'),
                                    },
                                    displaylabel: {
                                        container: $('<div />').addClass('form-group m-form__group'),
                                        title: $('<label />').addClass('form-group m-form__group').attr('for', 'exampleInputEmail1').text($.localize('translate','Display units also known as:')),
                                        input: $('<input type="text" />').addClass('form-group m-form__group').attr('id', 'groupname').addClass('form-control m-input'),
                                    },
                                    displayunits: {
                                        container: $('<div />').addClass('m-form__group form-group row'),
                                        title: $('<label />').addClass('col-6 col-form-label').text($.localize('translate','Resolution of the display units')),
                                        displayselect: $('<div />').addClass('col-6'),
                                        displayunitsselect: $('<select />').addClass('custom-select').attr('id', 'select_resolution'),
                                        comment: $('<div />').addClass('m-form__group form-group'),
                                        commentlabel: $('<label />').text($.localize('translate','Note:')),
                                        commenttextarea: $('<textarea />').attr('id', 'text_note').addClass('form-control m-input m-input--air').attr('rows', '3'),
                                    },
                                    editGroup: {
                                        container: $('<div />').addClass('m-form__group form-group row'),
                                        treebody: $('<div />').addClass('m-portlet__body'),
                                        tree: $('<div />').addClass('tree-demo groupTree')
                                    },
                                    displaytimes: {
                                        title: $('<div />').addClass('m-form__group form-group row'),
                                        titlelabel: $('<label />').attr('for', 'exampleInputEmail1').text($.localize('translate','Weekly time options:')),
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
                        retryFlag: false,
                        selectedGroupID: null,
                        selectedGroupIDparents: null,
                        groupTreeForPlayerEditID: null,
                        player_Alldata: null,
                        ActivechangeFlg: false,
                        OnlinechangeFlg: false,
                        DisplayNamechangeFlg : false,
                        NotechangeFlg: false,
                        editPlayerFlg : false,
                        editGroupFlg : false,
                        datatable : null,
                        temp_GroupTreeData:null,
                        firstPageload: true,
                        selectPlayerdata:null,
                        playListgroup:[],
                        editGroupID:null
                    }
                };
                $this.find('.m-content.mainPageTabDiv').first().append(
                    _plugin.htmlElements.container.
                        append(_plugin.htmlElements.containercol.
                        append(
                            _plugin.htmlElements.containerbody.
                            append(_plugin.htmlElements.grouphead.container.
                            append(_plugin.htmlElements.grouphead.headcaption.container.append(
                                _plugin.htmlElements.grouphead.headcaption.headtitle.append(_plugin.htmlElements.grouphead.headcaption.title)
                            )).append(_plugin.htmlElements.grouphead.headtools.container.
                                append(_plugin.htmlElements.grouphead.headtools.headtoolsul.
                                    append(_plugin.htmlElements.grouphead.headtools.headtoolsli.
                                        append(_plugin.htmlElements.grouphead.headtools.headtoolsa).
                                            append(_plugin.htmlElements.grouphead.headwrapper.container.
                                                append(_plugin.htmlElements.grouphead.headwrapper.span).
                                                append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.container.
                                                  append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.dropdowninnerbody.
                                                    append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.container.
                                                      append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.contentul.
                                                        append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.contentnewgroupli.
                                                          append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.contentnewgroupa.
                                                          append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.contentnewgroupi)
                                                            .append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.newgroupspan))
                                                          )
                                                          .append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.contentdeletegroupli.
                                                          append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.contentdeletegroupa.
                                                            append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.contentdeletegroupi)
                                                            .append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.deletegroupspan))
                                                          )
                                                          .append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.contentexpandAllli.
                                                          append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.contentexpandAlla.
                                                            append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.contentexpandAlli)
                                                            .append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.expandAllspan))
                                                          )
                                                          .append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.contentcollapseAllli.
                                                          append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.contentcollapseAlla.
                                                            append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.contentcollapseAlli)
                                                            .append(_plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.collapseAllspan))
                                                          )
                                                        )
                                                      )
                                                    )
                                                  )
                                               )
                                    )
                                )
                            )
                          )
                          .append(_plugin.htmlElements.grouptreecontainer.append(_plugin.htmlElements.grouptree))
                        )
                    ).
                    append(_plugin.htmlElements.player.container.append(_plugin.htmlElements.player.containermain.
                      append(_plugin.htmlElements.player.playerBody.container.append(_plugin.htmlElements.player.playerBody.playerform.
                        append(_plugin.htmlElements.player.playerBody.containercenter.
                          append(_plugin.htmlElements.player.playerBody.playercol.col.
                            append(_plugin.htmlElements.player.playerBody.playercol.groupitems.
                              append(_plugin.htmlElements.player.playerBody.playercol.colmd.
                              append(_plugin.htmlElements.player.playerBody.playercol.minput.
                                append(_plugin.htmlElements.player.playerBody.playercol.input)
                                .append(_plugin.htmlElements.player.playerBody.playercol.inputicon.
                                   append(_plugin.htmlElements.player.playerBody.playercol.span.
                                    append(_plugin.htmlElements.player.playerBody.playercol.spani))))))
                                )
                                .append(_plugin.htmlElements.player.playerBody.playerbuttoncol.col.
                                   append(_plugin.htmlElements.player.playerBody.playerbuttoncol.buttongroup.
                                     append(_plugin.htmlElements.player.playerBody.playerbuttoncol.labelAll.
                                       append(_plugin.htmlElements.player.playerBody.playerbuttoncol.labelAllinput)
                                       .append($.localize('translate','All')))
                                     .append(_plugin.htmlElements.player.playerBody.playerbuttoncol.labelCurrent.
                                     append(_plugin.htmlElements.player.playerBody.playerbuttoncol.labelCurrentinput)
                                     .append($.localize('translate','Current')))
                                       )
                                    .append(_plugin.htmlElements.player.playerBody.playerbuttoncol.buttons.
                                     append(_plugin.htmlElements.player.playerBody.playerbuttoncol.buttonAdd)
                                     .append(_plugin.htmlElements.player.playerBody.playerbuttoncol.buttonEdit)
                                       )
                                    .append(_plugin.htmlElements.player.playerBody.playerbuttoncol.separator)
                                   )
                                 )
                               )
                               .append(_plugin.htmlElements.player.playerBody.playertable)
                             )
                         )
                     )
                ).
                    //Detail
                    append(_plugin.htmlElements.groupplayerDetail.container.append(_plugin.htmlElements.groupplayerDetail.containercol.
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
                                     append(_plugin.htmlElements.groupplayerDetail.detailBody.savebutton)).
                                  append(_plugin.htmlElements.groupplayerDetail.detailBody.toolssavePlayer.
                                     append(_plugin.htmlElements.groupplayerDetail.detailBody.savePlayerbutton)).
                                   append(_plugin.htmlElements.groupplayerDetail.detailBody.toolsBack.
                                     append(_plugin.htmlElements.groupplayerDetail.detailBody.backbutton))
                                 )
                              )
                          ).
                          append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.container.
                            append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.content.
                                append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentcol.
                                    append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.Active.
                                        append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.Activelabel)
                                        .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.ActiveRadio.
                                            append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.Activegroup.
                                                append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.ActiveInherited.
                                                    append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.ActiveInheritedInput)
                                                    .append($.localize('translate','Inherited'))
                                                )
                                                .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.ActiveOn.
                                                    append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.ActiveOnInput)
                                                    .append($.localize('translate','On'))
                                                )
                                                .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.ActiveOff.
                                                    append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.ActiveOffInput)
                                                    .append($.localize('translate','Off'))
                                                )
                                            )
                                        )
                                    )
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.Online.
                                        append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.Onlinelabel)
                                        .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.OnlineRadio.
                                            append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.Onlinegroup.
                                                append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.OnlineInherited.
                                                    append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.OnlineInheritedInput)
                                                    .append($.localize('translate','Inherited'))
                                                )
                                                .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.OnlineOn.
                                                    append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.OnlineOnInput)
                                                    .append($.localize('translate','On'))
                                                )
                                                .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.OnlineOff.
                                                    append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.OnlineOffInput)
                                                    .append($.localize('translate','Off'))
                                                )
                                            )
                                        )
                                    )
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.container.
                                        append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.title)
                                        .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.input)
                                    )
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.container.
                                        append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.title)
                                        .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.displayselect.
                                            append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.displayunitsselect)
                                        )
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
                                   .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.timeOptions({ buttonText: $.localize('translate','Monday') }))
                                    //Tuesday
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.timeOptions({ buttonText: $.localize('translate','Tuesday') }))
                                    //Wednesday
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.timeOptions({ buttonText: $.localize('translate','Wednesday') }))
                                    //Thursday
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.timeOptions({ buttonText: $.localize('translate','Thursday') }))
                                    //Friday
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.timeOptions({ buttonText: $.localize('translate','Friday') }))
                                    //Saturday
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.timeOptions({ buttonText: $.localize('translate','Saturday') }))
                                    //Sunday
                                    .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.timeOptions({ buttonText: $.localize('translate','Sunday') }))
                                   
                                )
                                .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.showPlaylist.container)
                                .append(_plugin.htmlElements.groupplayerDetail.detailBody.detail.showPlaylistForced.container)
                            )
                          )
                        )
                      )
                    )
                $this.data('insmGroup', _plugin);
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.displayunitsselect
                    .append("<option value='0' selected =''>" + $.localize('translate', 'Inherit resolution') + "</option>")
                    .append("<option value='1'>" + $.localize('translate', '720P') + "</option>")
                    .append("<option value='2'>" + $.localize('translate', '1080P') + "</option>")
                    .append("<option value='3'>" + $.localize('translate', '2K') + "</option>")
                    .append("<option value='4'>" + $.localize('translate', '4K') + "</option>");

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

                var tree = $('.tree-demo.groupTree');
                tree.jstree(groupJstreeData);

                _plugin.htmlElements.grouptree.bind("refresh.jstree", function (e, data) {
                    var loginUser = $.insmFramework('user');
                    _plugin.htmlElements.grouptree.jstree(true).select_node(loginUser.GroupID);
                });
                //div_groupTree.on("ready.jstree", function (event, data) {
                //    $("#1_anchor").css("visibility", "hidden");
                //    $("li#1").css("position", "relative")
                //    $(".jstree-last .jstree-icon").first().hide();
                //});

                tree.on('loaded.jstree', function (e, data) {
                    var inst = data.instance;
                    var obj = inst.get_node(e.target.firstChild.firstChild.lastChild);

                    inst.select_node(obj);
                    if (_plugin.data.firstPageload) {
                        mApp.unblockPage();
                        $("#DisplayUnitsContent").show();
                        _plugin.data.firstPageload = false;
                    }
                })

                _plugin.htmlElements.grouptree.on("changed.jstree", function (e, data) {
                    //存储当前选中的区域的名称
                    if (data.node) {
                        _plugin.data.selectedGroupID = data.node.id;
                        _plugin.data.selectedGroupIDparents = data.node.parents
                        $.insmGroup('showPlayerDetail', { GroupID: _plugin.data.selectedGroupID });
                    }
                    _plugin.htmlElements.player.playerBody.playerbuttoncol.labelAll.click();
                });

                $.insmGroup('initTimeOptionsInPlayerEdit');
                //Back
                _plugin.htmlElements.groupplayerDetail.detailBody.backbutton.click(function () {
                    _plugin.htmlElements.container.show();
                    _plugin.htmlElements.groupplayerDetail.container.hide();
                    //$("#forcedplaylists")
                    _plugin.htmlElements.groupplayerDetail.detailBody.detail.showPlaylistForced.container.empty();
                });

                //AddGroup
                _plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.contentnewgroupli.click(function (e) {  
                    //$("#button_save")
                    _plugin.htmlElements.groupplayerDetail.detailBody.savebutton.css('display', 'block').removeClass('m-dropdown__toggle');
                    _plugin.htmlElements.groupplayerDetail.detailBody.savePlayerbutton.css('display', 'none')
                    _plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.jstree(true).deselect_all(true)
                    _plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.jstree(true).select_node(_plugin.htmlElements.grouptree.jstree(true).get_selected())

                    var div_PlaylistEditorContent = _plugin.htmlElements.groupplayerDetail.detailBody.detail.showPlaylist.container;
                    var div_forcedplaylists = _plugin.htmlElements.groupplayerDetail.detailBody.detail.showPlaylistForced.container;
                        //$('#forcedplaylists');
                    div_PlaylistEditorContent.empty();
                    div_forcedplaylists.empty();
                    //$("#div_edit .m-portlet__head-caption:first").find("h3:first").append(localize_jap["New Group"]);
                    _plugin.htmlElements.groupplayerDetail.detailBody.spanh.append(localize_jap["New Group"]);
                    $.insmFramework('getPlaylistByGroup', {
                        GroupID: _plugin.data.selectedGroupID,
                        success: function (data) {
                            $.insmGroup('defaultDataSet');
                            _plugin.htmlElements.container.hide();
                            _plugin.htmlElements.groupplayerDetail.container.show();
                            if (data) {
                                $.insmGroup('showPlaylist', { Playlists: data, isGroup: true, GroupID: _plugin.data.selectedGroupID, newGroup: true });
                            }
                        },
                        error: function () {

                        },
                    })
                })

                //editgroup
                _plugin.htmlElements.grouphead.headtools.headtoolsa.click(function () {
                    if (!_plugin.data.selectedGroupID) {
                        toastr.warning("Select Group first!");
                        return;
                    }
                    $.insmGroup('defaultDataSet');
                    _plugin.data.editGroupFlg = true;
                    _plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.show();
                    _plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.jstree(true).show_all();
                    _plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.jstree(true).deselect_all(true);
                    $.each(_plugin.data.temp_GroupTreeData, function (index, item) {
                        if (item.id == _plugin.htmlElements.grouptree.jstree(true).get_selected()) {
                            _plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.jstree(true).select_node(item.parent);
                            if (item.parent == '#') {
                                _plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.hide();
                            } else {
                                _plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.jstree(true).hide_node(_plugin.htmlElements.grouptree.jstree(true).get_node(item.id));
                            }
                            
                            _plugin.data.groupTreeForPlayerEditID = item.parent;
                        }
                    });

                    //$("#button_save")
                    _plugin.htmlElements.groupplayerDetail.detailBody.savebutton.css('display', 'block').removeClass('m-dropdown__toggle');
                    _plugin.htmlElements.groupplayerDetail.detailBody.savePlayerbutton.css('display', 'none');
                    $.insmFramework('editGroup', {
                        groupID: _plugin.data.selectedGroupID,
                        success: function (userGroupData) {
                            if (userGroupData) {
                                _plugin.htmlElements.container.hide();
                                _plugin.htmlElements.groupplayerDetail.container.show();
                                
                                _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.Active.find("label[id='label_Active_" + userGroupData.ActiveFlag + "']").click();
                                _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.Online.find("label[id='label_Online_" + userGroupData.OnlineFlag + "']").click();
                                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.input.val(userGroupData.GroupName);
                                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.commenttextarea.val(userGroupData.Comments);
   
                                var Settings;
                                if (userGroupData) {
                                    Settings = JSON.parse(userGroupData.Settings);
                                }
                                if (Settings != null) {
                                    if (Object.getOwnPropertyNames(Settings).length > 0) {
                                        $.insmGroup('setTimeOptions' , Settings);
                                        //Resolution of the display units
                                        if (Settings.resolution) {
                                            $('#select_resolution').val(Settings.resolution);
                                        }
                                    }
                                }

                                _plugin.data.editGroupID = _plugin.data.selectedGroupID;
                                //$("#div_edit .m-portlet__head-caption:first").find("h3:first").text(userGroupData.GroupName);
                                _plugin.htmlElements.groupplayerDetail.detailBody.spanh.text(userGroupData.GroupName);
                            };

                            $.insmFramework('getPlaylistByGroup', {
                                GroupID: _plugin.data.selectedGroupID,
                                success: function (data) {
                                    if (data) {
                                        $.insmGroup('showPlaylist', { Playlists: data, isGroup: true, GroupID: _plugin.data.selectedGroupID });
                                    }
                                },
                                error: function () {
                                },
                            })
                        },
                        error: function () {

                        },
                    })
                });

                //deletegroup
                _plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.contentdeletegroupli.click(function (e) {
                    var deleteGroup = _plugin.htmlElements.grouptree.jstree(true).get_node(_plugin.htmlElements.grouptree.jstree(true).get_selected());
                    var loginUser = $.insmFramework('user');
                    if (deleteGroup.li_attr.GroupID == loginUser.GroupID) {
                        toastr.warning("所属グループが削除できません。");
                        return;
                    }
                    if (deleteGroup) {
                        $.confirmBox({
                            title: "Warning",
                            message: '削除しても宜しいでしょうか？',
                            onOk: function () {
                                $.insmFramework('updateGroupUseFlg', {
                                    deleteGroupItem: deleteGroup.li_attr,
                                    success: function (resultdata) {
                                        _plugin.htmlElements.container.show();
                                        _plugin.htmlElements.groupplayerDetail.container.hide();
                                        var userGroupId = $.insmFramework('user').GroupID;
                                        //$.insmGroup('initGroupTree', {
                                        //    userGroupId: userGroupId
                                        //});
                                        $.insmGroup('refreshTree');
                                        toastr.success("操作が完了しました。");
                                    }
                                })
                            }
                        });
                    }
                })

                //groupsave
                _plugin.htmlElements.groupplayerDetail.detailBody.savebutton.click(function (e) {
                    _plugin.htmlElements.groupplayerDetail.detailBody.savebutton.css('display', 'none');
                    $.insmGroup('addNewGroup');
                    _plugin.data.editGroupID = null;
                })

                //AddPlayer
                _plugin.htmlElements.player.playerBody.playerbuttoncol.buttonAdd.click(function () {
                    $.insmGroup('defaultDataSet');

                    _plugin.data.editPlayerFlg = false;
                    //$("#button_save_Player").css('display', 'block').removeClass('m-dropdown__toggle');
                    _plugin.htmlElements.groupplayerDetail.detailBody.savePlayerbutton.css('display', 'block').removeClass('m-dropdown__toggle');
                    //$("#button_save")
                    _plugin.htmlElements.groupplayerDetail.detailBody.savebutton.css('display', 'none');
                    _plugin.htmlElements.groupplayerDetail.detailBody.spanh.text(localize_jap["Add"]);

                    _plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.jstree(true).deselect_all(true);
                    _plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.jstree(true).select_node(_plugin.htmlElements.grouptree.jstree(true).get_selected());

                    _plugin.data.groupTreeForPlayerEditID = _plugin.htmlElements.grouptree.jstree(true).get_selected()[0];
                    $.insmFramework('getPlaylistByGroup', {
                        GroupID: _plugin.data.selectedGroupID,
                        success: function (data) {
                            if (data) {
                                $.insmGroup('showPlaylist', { Playlists: data, isGroup: false, GroupID: _plugin.data.selectedGroupID });
                            }
                        },
                        error: function () {
                        },
                    })
                });

                //Editplayer
                _plugin.htmlElements.player.playerBody.playerbuttoncol.buttonEdit.click(function () {
                    var selected = _plugin.data.datatable.setSelectedRecords().getSelectedRecords();
                    _plugin.data.selectPlayerdata = selected;
                    if (_plugin.data.selectPlayerdata.length == 0) { return; }
                    var allPlayerNames = "";
                    $.each(_plugin.data.selectPlayerdata, function (playerIndex, playerItem) {
                        allPlayerNames += ", " + $(playerItem).data().obj.PlayerName;
                    });

                    var allPlayerID = "";
                    $.each(_plugin.data.selectPlayerdata, function (playerIndex, playerItem) {
                        allPlayerID += ", " + $(playerItem).data().obj.PlayerID;
                    })

                    allPlayerNames = allPlayerNames.substr(2);
                    var playerSelectionLi = $('<li class="m-menu__item " data-redirect="true" aria-haspopup="true">\
                                                <a class="m-menu__link" title="' + allPlayerNames + '">\
                                                    <span class="m-menu__link-title" style="text-overflow:ellipsis;white-space: nowrap;overflow: hidden;">\
                                                        <span class="m-menu__link-wrap">\
                                                            <span class="m-menu__link-badge">\
                                                                <span class="m-badge m-badge--success">'
                                                                                + _plugin.data.selectPlayerdata.length +
                                                                            '</span>\
                                                            </span>\
                                                            <span class="m-menu__link-text">'
                                                                            + allPlayerNames +
                                                                        '</span>\
                                                        </span>\
                                                    </span>\
                                                </a>\
                                            </li>');
                    playerSelectionLi.find("a").data("playersData", $.extend(true, {}, _plugin.data.selectPlayerdata)).click(function () {
                        _plugin.data.selectPlayerdata = $(this).data("playersData");
                        //edit_player_click(selectPlayerdata);
                        $.insmGroup('edit_player_click', _plugin.data.selectPlayerdata);
                    });
                    $("#playerSelectionHistroyUl").prepend(playerSelectionLi);
                    //edit_player_click(selectPlayerdata, allPlayerID);
                    $.insmGroup('edit_player_click', _plugin.data.selectPlayerdata, allPlayerID);
                })

                //savePlayer
                _plugin.htmlElements.groupplayerDetail.detailBody.savePlayerbutton.click(function () {
                    var Settings = {};
                    _plugin.htmlElements.groupplayerDetail.detailBody.savePlayerbutton.css("pointer-events", "none");
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

                        ActiveFlag: _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.Active.find("input[name='radio_Active']:checked").val(),
                        OnlineFlag: _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.Online.find("input[name='radio_Online']:checked").val(),

                        resolution: $("#select_resolution").val()
                    };

                    if (!_plugin.data.editPlayerFlg) {
                        if ($.trim(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.input.val()) == '' || _plugin.data.groupTreeForPlayerEditID == null) {
                            toastr.warning("Player name is empty!");
                            _plugin.htmlElements.groupplayerDetail.detailBody.savePlayerbutton.css("pointer-events", "");
                            return;
                        }
                        $.insmFramework('creatPlayer', {
                            GroupID: _plugin.data.groupTreeForPlayerEditID,
                            PlayerName: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.input.val(),
                            active: _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.Active.find("input[name='radio_Active']:checked").val(),
                            onlineUnits: _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.Online.find("input[name='radio_Online']:checked").val(),
                            note: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.commenttextarea.val(),
                            settings: JSON.stringify(Settings),
                            ActivechangeFlg: _plugin.data.ActivechangeFlg,
                            OnlinechangeFlg: _plugin.data.OnlinechangeFlg,
                            ActiveFlag: _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.Active.find("input[name='radio_Active']:checked").val(),
                            OnlineFlag: _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.Online.find("input[name='radio_Online']:checked").val(),
                            DisplayNamechangeFlg: _plugin.data.DisplayNamechangeFlg,
                            NotechangeFlg: _plugin.data.NotechangeFlg,
                            success: function (data) {
                                //$("#button_save_Player")
                                _plugin.htmlElements.groupplayerDetail.detailBody.savePlayerbutton.css("pointer-events", "");
                                var div_forcedplaylists = _plugin.htmlElements.groupplayerDetail.detailBody.detail.showPlaylistForced.container;
                                //$('#forcedplaylists');
                                var forcedplaylists = div_forcedplaylists.find(".m-portlet.m-portlet--warning.m-portlet--head-sm");
                                _plugin.data.playListgroup = [];
                                if (forcedplaylists.length > 0) {
                                    $.each(forcedplaylists, function (index, forcedplaylist) {
                                        var playlistItem = {};
                                        var forcedplaylistID = $(forcedplaylist).attr('playlistId');
                                        _plugin.data.playListgroup.push(forcedplaylistID);
                                    });
                                }

                                $.insmFramework('playerPlayListLinkTables', {
                                    playerId: data.PlayerID,
                                    isedit: false,
                                    PlayListID: _plugin.data.playListgroup,
                                    success: function (data) {
                                        setTimeout(function () {
                                            toastr.success("操作が完了しました。");
                                        }, 2000);
                                    },
                                    error: function () {
                                    }
                                })
                                _plugin.htmlElements.container.show();
                                _plugin.htmlElements.groupplayerDetail.container.hide();
                                $.insmGroup('refreshTree');
                                _plugin.data.editGroupID = null;
                                _plugin.htmlElements.groupplayerDetail.detailBody.savePlayerbutton.css("pointer-events", "");
                            },
                            error: function () {
                                _plugin.htmlElements.groupplayerDetail.detailBody.savePlayerbutton.css("pointer-events", "");
                            }

                        })


                    } else {
                        var div_forcedplaylists = _plugin.htmlElements.groupplayerDetail.detailBody.detail.showPlaylistForced.container;
                        var forcedplaylists = div_forcedplaylists.find(".m-portlet.m-portlet--warning.m-portlet--head-sm");
                        _plugin.data.playListgroup = [];
                        if (forcedplaylists.length > 0) {
                            $.each(forcedplaylists, function (index, forcedplaylist) {
                                var playlistItem = {};
                                var forcedplaylistID = $(forcedplaylist).attr('playlistId');
                                _plugin.data.playListgroup.push(forcedplaylistID);
                            });
                        }
                        $.insmFramework('editGroupPlayers', {
                            Playerdata: _plugin.data.selectPlayerdata,
                            ActivechangeFlg: _plugin.data.ActivechangeFlg,
                            OnlinechangeFlg: _plugin.data.OnlinechangeFlg,
                            DisplayNamechangeFlg: _plugin.data.DisplayNamechangeFlg,
                            ActiveFlag: _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.Active.find("input[name='radio_Active']:checked").val(),
                            OnlineFlag: _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.Online.find("input[name='radio_Online']:checked").val(),
                            NotechangeFlg: _plugin.data.NotechangeFlg,
                            newGroupID: _plugin.data.groupTreeForPlayerEditID,
                            settings: JSON.stringify(Settings),
                            success: function (data) {
                                $.insmFramework('playerPlayListLinkTables', {
                                    playerId: _plugin.data.selectPlayerdata,
                                    isedit: true,
                                    PlayListID: _plugin.data.playListgroup,
                                    success: function (data) {
                                        setTimeout(function () {
                                            _plugin.htmlElements.groupplayerDetail.detailBody.savePlayerbutton.css("pointer-events", "");
                                            $.insmGroup('showPlayerDetail', { GroupID: _plugin.data.selectedGroupID });
                                            toastr.success("操作が完了しました。");
                                            $.insmGroup('refreshTree');
                                        }, 2000);
                                    },
                                    error: function () {
                                    }
                                })
                                _plugin.data.editGroupID = null;
                            },
                            error: function () {
                                _plugin.htmlElements.groupplayerDetail.detailBody.savePlayerbutton.css("pointer-events", "");
                            }
                        })
                    }
                    $("#PlayerDetail").css('display', 'none');
                });
                
                //Open_all
                _plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.contentexpandAllli.click(function () {
                    _plugin.htmlElements.grouptree.jstree('open_all');
                });
                //Close_all
                _plugin.htmlElements.grouphead.headwrapper.dropdowninner.content.contentcollapseAllli.click(function () {
                    _plugin.htmlElements.grouptree.jstree('close_all');
                });
                //PlayerAll
                _plugin.htmlElements.player.playerBody.playerbuttoncol.labelAll.click(function () {
                    $('#m_form_search_Player').val('');
                    $.insmGroup('DatatableResponsiveColumnsDemo', { PlayersData: _plugin.data.player_Alldata });
                })
                //PlayerCurrent
                _plugin.htmlElements.player.playerBody.playerbuttoncol.labelCurrent.click(function () {
                    $('#m_form_search_Player').val('');
                    var Current_data = [];
                    $.each(_plugin.data.player_Alldata, function (key, item) {
                        if (item.GroupID == _plugin.data.selectedGroupID) {
                            Current_data.push(item)
                        }
                    });
                    $.insmGroup('DatatableResponsiveColumnsDemo', { PlayersData: Current_data });
                })

                _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.ActiveInherited.click(function () {
                    if (_plugin.data.editPlayerFlg) { $.insmGroup('activechange'); }
                })

                _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.ActiveOn.click(function () {
                    if (_plugin.data.editPlayerFlg) { $.insmGroup('activechange'); }
                })

                _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.ActiveOff.click(function () {
                    if (_plugin.data.editPlayerFlg) { $.insmGroup('activechange'); }
                })
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.OnlineInherited.click(function () {
                    if (_plugin.data.editPlayerFlg) { $.insmGroup('onlinechange'); }
                })
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.OnlineOn.click(function () {
                    if (_plugin.data.editPlayerFlg) { $.insmGroup('onlinechange'); }
                })
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.OnlineOff.click(function () {
                    if (_plugin.data.editPlayerFlg) { $.insmGroup('onlinechange'); }
                })

                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.input.change(function () {
                    _plugin.data.DisplayNamechangeFlg = true;
                    $.each(_plugin.data.selectPlayerdata, function (index, item) {
                        $(_plugin.data.selectPlayerdata[index]).data().obj.PlayerName = _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.input.val();
                    });
                })
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.commenttextarea.change(function () {
                    _plugin.data.NotechangeFlg = true;
                    $.each(_plugin.data.selectPlayerdata, function (index, item) {
                        $(_plugin.data.selectPlayerdata[index]).data().obj.Comments = _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.commenttextarea.val();
                    });
                })

            }
            _plugin.htmlElements.groupplayerDetail.container.hide();
 
            _plugin.htmlElements.groupplayerDetail.detailBody.savebutton.text($.localize('translate', "Save"));
            return $this;
        },
        editgroup: function (options) {
            $.insmFramework('creatGroup', {
                groupID: options.groupID,
                newGroupName: options.newGroupName,
                active: options.ActiveFlag,
                onlineUnits: options.OnlineFlag,
                note: options.Comments,
                newGroupNameParentID: options.newGroupNameParentID,
                settings: options.Settings,
                success: function (data) {
                    $.insmGroup('refreshTree');
                    _plugin.data.editGroupID = null;
                    toastr.success("操作が完了しました。");
                }
            })
        },
        refreshTree: function () {
            var $this = $('body').eq(0);
            var _plugin = $this.data('insmGroup');
            _plugin.htmlElements.grouptree.jstree(true).refresh();
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.jstree(true).refresh();
        },
        setTimeOptions: function (Settings) {
            var $this = $('body').eq(0);
            var _plugin = $this.data('insmGroup');
            //Monday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.find('checkbox').attr("checked", Settings.MondayisCheck);
            if (Settings.MondayisCheck) {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.find('label').addClass('active');
            } else {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.find('label').removeClass('active');
            }
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: Settings.Monday.split(';')[0], to: Settings.Monday.split(';')[1] });

            //Tuesday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.find('checkbox').attr("checked", Settings.TuesdayisCheck);
            if (Settings.TuesdayisCheck) {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.find('label').addClass('active');
            } else {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.find('label').removeClass('active');
            }
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: Settings.Tuesday.split(';')[0], to: Settings.Tuesday.split(';')[1] });


            //Wednesday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.find('checkbox').attr("checked", Settings.WednesdayisCheck);
            if (Settings.WednesdayisCheck) {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.find('label').addClass('active');
            } else {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.find('label').removeClass('active');
            }
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: Settings.Wednesday.split(';')[0], to: Settings.Wednesday.split(';')[1] });

            //Thursday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.find('checkbox').attr("checked", Settings.ThursdayisCheck);
            if (Settings.ThursdayisCheck) {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.find('label').addClass('active');
            } else {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.find('label').removeClass('active');
            }
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: Settings.Thursday.split(';')[0], to: Settings.Thursday.split(';')[1] });

            //Friday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.find('checkbox').attr("checked", Settings.FridayisCheck);
            if (Settings.FridayisCheck) {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.find('label').addClass('active');
            } else {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.find('label').removeClass('active');
            }
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: Settings.Friday.split(';')[0], to: Settings.Friday.split(';')[1] });
            //Saturday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.find('checkbox').attr("checked", Settings.SaturdayisCheck);
            if (Settings.SaturdayisCheck) {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.find('label').addClass('active');
            } else {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.find('label').removeClass('active');
            }
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: Settings.Saturday.split(';')[0], to: Settings.Saturday.split(';')[1] });

            //Sunday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.find('checkbox').attr("checked", Settings.SundayisCheck);
            if (Settings.SundayisCheck) {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.find('label').addClass('active');
            } else {
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.find('label').removeClass('active');
            }
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: Settings.Sunday.split(';')[0], to: Settings.Sunday.split(';')[1] });
        },
        initGroupTree: function (options) {
            var $this = $('body').eq(0);
            var _plugin = $this.data('insmGroup');

            var groupJstreeData = {
                "core": {
                    "multiple": false,
                    "themes": {
                        "responsive": true
                    },
                    // so that create works
                    "check_callback": true,
                    'data': {
                        url: 'api/GroupMasters/GetGroupJSTreeDataWithChildByGroupID/' + options.userGroupId,
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

            var tree = $('.tree-demo.groupTree');
            tree.jstree(groupJstreeData);

            _plugin.htmlElements.grouptree.bind("refresh.jstree", function (e, data) {
                var loginUser = $.insmFramework('user');
                _plugin.htmlElements.grouptree.jstree(true).select_node(loginUser.GroupID);
            });
            //div_groupTree.on("ready.jstree", function (event, data) {
            //    $("#1_anchor").css("visibility", "hidden");
            //    $("li#1").css("position", "relative")
            //    $(".jstree-last .jstree-icon").first().hide();
            //});

            tree.on('loaded.jstree', function(e, data) {
                var inst = data.instance;
                var obj = inst.get_node(e.target.firstChild.firstChild.lastChild);

                inst.select_node(obj);
                if (_plugin.data.firstPageload) {
                    mApp.unblockPage();
                    $("#DisplayUnitsContent").show();
                    _plugin.data.firstPageload = false;
                }
            })
            _plugin.htmlElements.grouptree.on("changed.jstree", function (e, data) {
                //存储当前选中的区域的名称
                if (data.node) {
                    _plugin.data.selectedGroupID = data.node.id;
                    _plugin.data.selectedGroupIDparents = data.node.parents
                    $.insmGroup('showPlayerDetail', { GroupID: _plugin.data.selectedGroupID });
                }
                _plugin.htmlElements.player.playerBody.playerbuttoncol.labelAll.click();
            });


            _plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.on("changed.jstree", function (e, data) {
                //存储当前选中的区域的名称
                if (data.node) {
                    _plugin.data.groupTreeForPlayerEditID = data.node.id;
                }
            });

            //$(div_groupTreeForFileManager).on("changed.jstree", function (e, data) {
            //    //存储当前选中的区域的名称
            //    if (data.node) {
            //        $.folder('init', {
            //            selectedGroupID: data.node.id
            //        });
            //    }
            //});

            tree.on("move_node.jstree", function (e, data) {
                var node = data.node;
                if (node) {
                    $.insmGroup('editgroup', {
                        groupID: node.id,
                        newGroupNameParentID: node.parent,
                        newGroupName: node.text,
                        ActiveFlag: node.li_attr.ActiveFlag,
                        OnlineFlag: node.li_attr.OnlineFlag,
                        Comments: node.li_attr.Comments,
                        Settings: node.li_attr.Settings
                    });
                }
            });
            //$("#PlayerDetail").css('display', 'none');
        },
        defaultDataSet: function () {
            var $this = $('body').eq(0);
            var _plugin = $this.data('insmGroup');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.ActiveInherited.click(),
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.OnlineInherited.click(),
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.input.val(''),
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.commenttextarea.val('');
            $("#select_resolution").val('0');
            //monday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.find('label').addClass('active');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: 0, to: 24 });
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Mondaycontainer.find('checkbox').attr("checked", true);
            //tuesday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.find('label').addClass('active');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: 0, to: 24 });
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Tuesdaycontainer.find('checkbox').attr("checked", true);
            //wednesday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.find('label').addClass('active');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: 0, to: 24 });
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Wednesdaycontainer.find('checkbox').attr("checked", true);
            //thursday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.find('label').addClass('active');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: 0, to: 24 });
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Thursdaycontainer.find('checkbox').attr("checked", true);
            //friday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.find('label').addClass('active');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: 0, to: 24 });
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Fridaycontainer.find('checkbox').attr("checked", true);
            //saturday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.find('label').addClass('active');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: 0, to: 24 });
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Saturdaycontainer.find('checkbox').attr("checked", true);
            //sunday
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.find('label').addClass('active');
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.find("input:eq(1)").data("ionRangeSlider").update({ from: 0, to: 24 });
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Sundaycontainer.find('checkbox').attr("checked", true);

            _plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.show();
            _plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.jstree(true).show_all();
        },
        showPlayerDetail: function (options) {
            var $this = $('body').eq(0);
            var _plugin = $this.data('insmGroup');

            $("#PlayerDetail").css('display', 'block');
            $.insmFramework('getGroupPlayers', {
                GroupID: options.GroupID,
                success: function (data) {
                    _plugin.htmlElements.container.show();
                    _plugin.htmlElements.groupplayerDetail.container.hide();
                    _plugin.data.player_Alldata = data;
                    $.insmGroup('DatatableResponsiveColumnsDemo', { PlayersData: data });
                }
            })
        },
        DatatableResponsiveColumnsDemo: function (options) {
            var $this = $('body').eq(0);
            var _plugin = $this.data('insmGroup');

            $('#base_responsive_columns').prop("outerHTML", "<div class='m_datatable' id='base_responsive_columns'></div>");
            _plugin.data.datatable = $('#base_responsive_columns').mDatatable({
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
                    //pageSize: 10,
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

                pagination: false,

                // columns definition
                columns: [{
                    field: "RecordID",
                    title: "",
                    sortable: false, // disable sort for this column
                    width: 40,
                    textAlign: 'center',
                    filterable: false,
                    selector: { class: 'm-checkbox--solid m-checkbox--brand' },
                }, {
                    field: "PlayerID",
                    title: "ディスプレイＩＤ",
                    filterable: false, // disable or enable filtering
                    width: 115
                }, {
                    field: "PlayerName",
                    title: "ディスプレイ名",
                    responsive: { visible: 'lg' }
                }, {
                    field: "GroupName",
                    title: "グループ名",
                    width: 150,
                    filterable: false,
                    responsive: { visible: 'lg' }
                }, {
                    field: "GreatDate",
                    title: "登録日時",
                    field: "UpdateDate",
                    title: "更新日時",
                    filterable: false,
                    textAlign: 'center',
                    datetype: "yyyy-MM-dd HH:mm",
                    responsive: { visible: 'lg' }
                }, {
                    field: "OnlineFlag",
                    title: "",
                    filterable: false,
                    textAlign: 'center',
                    responsive: { visible: 'lg' },
                    template: function (row, a, b) {
                        var dropup = (row.getDatatable().getPageSize() - row.getIndex()) <= 4 ? 'dropup' : '';

                        return '\
						<span class="m-menu__link-badge">\
                             <span class="m-badge m-badge--success" id="span_success"><i class="m-menu__link-icon fa fa-check-circle"></i></span>\
                        </span>\
                        ';
                    },
                    width: 0,
                }, {
                    field: "GroupID",
                    title: "",
                    filterable: false,
                    responsive: { visible: 'lg' },
                    width: 0,
                }]
            });
            _plugin.data.datatable.on('m-datatable--on-check', function (e, args) {
                var selected = _plugin.data.datatable.setSelectedRecords().getSelectedRecords();
                _plugin.data.selectPlayerdata = selected;
            })


            $('#m_form_search_Player').on('keyup', function (e) {
                _plugin.data.datatable.search($(this).val().toLowerCase(), "PlayerName");
            });

            $('#m_form_status, #m_form_type').selectpicker();
        },
        addNewGroup: function () {
            var $this = $('body').eq(0);
            var _plugin = $this.data('insmGroup');
            if ($.trim(_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.input.val()) == '') {
                toastr.warning("Group name is empty!");
                _plugin.htmlElements.groupplayerDetail.detailBody.savebutton.css('display', '');
                return;
            };
            if (_plugin.data.groupTreeForPlayerEditID == null) {
                toastr.warning("Please select new group's Parent Group !");
                _plugin.htmlElements.groupplayerDetail.detailBody.savebutton.css('display', '');
                return;
            };
            if (_plugin.data.editGroupID == _plugin.data.groupTreeForPlayerEditID) {
                toastr.warning("Group ID have same ID!");
                _plugin.htmlElements.groupplayerDetail.detailBody.savebutton.css('display', '');
                return;
            };
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

                ActiveFlag: _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.Active.find("input[name='radio_Active']:checked").val(),
                OnlineFlag: _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.Online.find("input[name='radio_Online']:checked").val(),
                resolution: $("#select_resolution").val()
            };
            $.insmFramework('creatGroup', {
                groupID: _plugin.data.editGroupID,
                newGroupName: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.input.val(),
                active: _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.Active.find("input[name='radio_Active']:checked").val(),
                onlineUnits: _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.Online.find("input[name='radio_Online']:checked").val(),
                note: _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.commenttextarea.val(),
                settings: JSON.stringify(Settings),
                newGroupNameParentID: _plugin.data.groupTreeForPlayerEditID,
                success: function (data) {
                    var editGroupID = null;
                    if (_plugin.data.editGroupFlg) {
                        editGroupID = _plugin.data.selectedGroupID;
                    } else {
                        editGroupID = data.GroupID;
                    }
                    
                    _plugin.htmlElements.groupplayerDetail.detailBody.savebutton.css('display', 'none');
                    var div_forcedplaylists = _plugin.htmlElements.groupplayerDetail.detailBody.detail.showPlaylistForced.container;
                    var forcedplaylists = div_forcedplaylists.find(".m-portlet.m-portlet--warning.m-portlet--head-sm");
                    _plugin.data.playListgroup = [];
                    if (forcedplaylists.length > 0) {
                        $.each(forcedplaylists, function (index, forcedplaylist) {
                            var playlistItem = {};
                            var forcedplaylistID = $(forcedplaylist).attr('playlistId');
                            _plugin.data.playListgroup.push(forcedplaylistID);
                        });
                    }
                    $.insmFramework('deleteGroupPlayListLinkTableByGroupID', {
                        groupID: editGroupID,
                        success: function (data) {
                            $.insmFramework('GroupPlayListLinkTables', {
                                groupID: editGroupID,
                                PlayListID: _plugin.data.playListgroup,
                                success: function (data) {
                                    _plugin.data.editGroupFlg = false;
                                    toastr.success("操作が完了しました。");
                                },
                                error: function () {
                                }
                            })
                        },
                        error: function () {
                            //$("#button_save")
                            _plugin.htmlElements.groupplayerDetail.detailBody.savebutton.css('display', '');
                        }
                    })
                    
                    _plugin.htmlElements.container.show();
                    _plugin.htmlElements.groupplayerDetail.container.hide();

                    $.insmGroup('refreshTree');
                    _plugin.data.editGroupID = undefined;
                    //$("#button_save")
                    _plugin.htmlElements.groupplayerDetail.detailBody.savebutton.css('display', '');
                },
                error: function () {
                    //$("#button_save")
                    _plugin.htmlElements.groupplayerDetail.detailBody.savebutton.css('display', '');
                }
            })
        },
        activechange: function () {
            var $this = $('body').eq(0);
            var _plugin = $this.data('insmGroup');

            _plugin.data.ActivechangeFlg = true;
            if (_plugin.data.selectPlayerdata) {
                $.each(_plugin.data.selectPlayerdata, function (index, item) {
                    $(_plugin.data.selectPlayerdata[index]).data().obj.ActiveFlag = _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentActive.Active.find("input[name='radio_Active']:checked").val();
                });
            }
        },
        onlinechange: function () {
            var $this = $('body').eq(0);
            var _plugin = $this.data('insmGroup');
            _plugin.data.OnlinechangeFlg = true;
            if (_plugin.data.selectPlayerdata) {
                $.each(_plugin.data.selectPlayerdata, function (index, item) {
                    $(_plugin.data.selectPlayerdata[index]).data().obj.OnlineFlag = _plugin.htmlElements.groupplayerDetail.detailBody.detail.contentOnline.Online.find("input[name='radio_Online']:checked").val();
                });
            }
        },
        getPlaylistByGroup: function (options) {
            $.insmFramework('getPlaylistByGroup', {
                groupID: options.groupID,
                success: function (GroupPlayLists) {
                    $.insmGroup('showPlaylist', { Playlists: GroupPlayLists, isGroup: options.isGroup });
                },
                error: function () {
                },
            });
        },
        showPlaylist: function (options) {
            var $this = $('body').eq(0);
            var _plugin = $this.data('insmGroup');

            var div_PlaylistEditorContent = _plugin.htmlElements.groupplayerDetail.detailBody.detail.showPlaylist.container;
            var div_forcedplaylists = _plugin.htmlElements.groupplayerDetail.detailBody.detail.showPlaylistForced.container;
                //$('#forcedplaylists');
            div_PlaylistEditorContent.empty();
            div_forcedplaylists.empty();
            if (options.Playlists) {
                $.each(options.Playlists, function (index, Playlist) {
                    var div_Playlist = $('<div/>').addClass('m-portlet m-portlet--warning m-portlet--head-sm').attr('playlistId', Playlist.PlayListID);
                    var div_Playlisthead = $('<div/>').addClass('m-portlet__head');
                    var div_head_caption = $('<div/>').addClass('m-portlet__head-caption');
                    var div_title = $('<div/>').addClass('m-portlet__head-title');
                    var span_head_icon = $("<span />").addClass('m-portlet__head-icon');
                    var span_i = '<i class="fa fa-file-text"></i>';
                    var head_text = $('<h3 />').addClass('m-portlet__head-text').text(Playlist.PlayListName);
                    span_head_icon.append(span_i);
                    div_title.append(span_head_icon);
                    div_title.append(head_text);
                    div_head_caption.append(div_title);
                    div_Playlisthead.append(div_head_caption);

                    var div_head_tools = $('<div/>').addClass('m-portlet__head-tools');
                    var div_portlet_nav = $('<ul>').addClass("m-portlet__nav");
                    var div_li = $('<li />').addClass('m-portlet__nav-item');
                    var href = $('<a />').addClass("m-portlet__nav-link m-portlet__nav-link--icon").attr("href", "javascript:;");
                    var href_i = $('<i />').addClass("fa fa-toggle-right");
                    href.append(href_i);
                    div_li.append(href);
                    div_portlet_nav.append(div_li);
                    var div_li_list = $('<li />').addClass("m-portlet__nav-item m-dropdown m-dropdown--inline m-dropdown--arrow m-dropdown--align-right m-dropdown--align-push");
                    div_li_list.attr('data-dropdown-toggle', 'hover').attr('aria-expanded', 'true');
                    var div_li_a_toggle = $('<a href="javascript:;"/>').addClass('m-portlet__nav-link m-portlet__nav-link--icon m-dropdown__toggle');
                    var div_li_i = $('<i />').addClass('la la-ellipsis-v');
                    div_li_a_toggle.append(div_li_i);
                    div_li_list.append(div_li_a_toggle);

                    var div_m_dropdown_wrapper = $('<div/>').addClass('m-dropdown__wrapper');
                    var wrappe_spantitle = $("<span style='left: auto; right: 18.5px;'/>").addClass('m-dropdown__arrow m-dropdown__arrow--right m-dropdown__arrow--adjust');
                    var div_m_dropdown_inner = $('<div/>').addClass("m-dropdown__inner");
                    var div_m_dropdown_bodyr = $('<div/>').addClass("m-dropdown__body");
                    var div_m_dropdown_content = $('<div/>').addClass("m-dropdown__content");

                    var ul = $('<ul>').addClass("m-nav");
                    //Item
                    if (Playlist.Settings) {
                        var playlistSetting = JSON.parse(Playlist.Settings);
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
                    div_head_tools.append(div_portlet_nav);
                    div_m_dropdown_wrapper.append(wrappe_spantitle);
                    div_m_dropdown_wrapper.append(div_m_dropdown_inner);
                    div_li_list.append(div_m_dropdown_wrapper)
                    div_portlet_nav.append(div_li_list);
                    div_Playlisthead.append(div_head_tools);
                    div_Playlist.append(div_Playlisthead);
                    div_li.click(function () {
                        var playlistclone = div_Playlist.clone(true);
                        div_forcedplaylists.append(playlistclone);
                        playlistclone.find("li.m-portlet__nav-item:first").empty();
                        playlistclone.find("li.m-portlet__nav-item:first").append($("<a href='javascript:;' class='m-portlet__nav-link m-portlet__nav-link--icon'><i class='la la-close'></i></a>").click(function () { playlistclone.remove() }));
                        //playListgroup.push(Playlist.PlayListID);
                    });
                    div_PlaylistEditorContent.append(div_Playlist);
                });
            };
            var tempForcedPlayList = null;
            if (options.isGroup) {
                if (options.newGroup) {
                    $.insmFramework('getForcedPlaylistByGroup', {
                        groupID: options.GroupID,
                        success: function (forcedPlayList) {
                            $.insmGroup('showPlaylistForced', { tempForcedPlayList: forcedPlayList, isGroup: false, newgroup: options.newgroup });
                            //div_main.hide();
                            //div_edit.show();
                            _plugin.htmlElements.container.hide();
                            _plugin.htmlElements.groupplayerDetail.container.show();
                        },
                        error: function () {
                        }
                    })
                } else {
                    $.insmFramework('getForcedPlaylistByGroup', {
                        groupID: options.GroupID,
                        success: function (forcedPlayList) {
                            $.insmGroup('showPlaylistForced', {
                                tempForcedPlayList: forcedPlayList, isGroup: true
                            });
                            //div_main.hide();
                            //div_edit.show();
                            _plugin.htmlElements.container.hide();
                            _plugin.htmlElements.groupplayerDetail.container.show();
                        },
                        error: function () {
                        }
                    })
                }
            } else {
                if(options.newgroup){
                    $.insmFramework('getForcedPlaylistByGroup', {
                        groupID: options.GroupID,
                        success: function (forcedPlayList) {
                            $.insmGroup('showPlaylistForced', { tempForcedPlayList: forcedPlayList, isGroup: false, newgroup: options.newgroup });
                            //div_main.hide();
                            //div_edit.show();
                            _plugin.htmlElements.container.hide();
                            _plugin.htmlElements.groupplayerDetail.container.show();
                        },
                        error: function () {
                        }
                    })
                } else {
                    if (options.playerId && options.playerId.indexOf(",") > 0) {
                        var editplayerIds = options.playerId.split(', ');
                        var editplayerPlists = {};
                        var playlistEditDeferredList = [];
                        $.each(editplayerIds, function (index, editplayerId) {
                            var tempPlayerEditDrferred = new $.Deferred();
                            playlistEditDeferredList.push(tempPlayerEditDrferred);
                            $.insmFramework('getForcedPlaylistByPlayer', {
                                playerId: editplayerId,
                                success: function (forcedPlayList) {
                                    editplayerPlists[editplayerId] = forcedPlayList;
                                    tempPlayerEditDrferred.resolve();
                                },
                                error: function () {
                                }
                            })
                        })
                        $.when.apply($, playlistEditDeferredList).done(function () {
                            var tempList = [];
                            var isSame = true;
                            $.each(editplayerPlists, function (index, item) {
                                var tempPlaylists = '';
                                $.each(item, function (listindex, listitem) {
                                    tempPlaylists += listitem.PlayListID + ",";
                                })
                                tempList.push(tempPlaylists);
                            });
                            $.unique(tempList);
                            if (tempList.length == 0 || tempList.length == 1) {
                                $.insmGroup('showPlaylistForced', { tempForcedPlayList: editplayerPlists[editplayerIds[0]], isGroup: false });
                                //div_main.hide();
                                //div_edit.show();
                                _plugin.htmlElements.container.hide();
                                _plugin.htmlElements.groupplayerDetail.container.show();
                            }
                        });
                    } else {
                        if (options.playerId) {
                            $.insmFramework('getForcedPlaylistByPlayer', {
                                playerId: options.playerId,
                                success: function (forcedPlayList) {
                                    $.insmGroup('showPlaylistForced', { tempForcedPlayList: forcedPlayList, isGroup: false });
                                    //div_main.hide();
                                    //div_edit.show();
                                    _plugin.htmlElements.container.hide();
                                    _plugin.htmlElements.groupplayerDetail.container.show();
                                },
                                error: function () {
                                }
                            })
                        } else {
                            $.insmFramework('getForcedPlaylistByGroup', {
                                groupID: options.GroupID,
                                success: function (forcedPlayList) {
                                    $.insmGroup('showPlaylistForced', { tempForcedPlayList: forcedPlayList, isGroup: false, newgroup: options.newgroup });
                                    //div_main.hide();
                                    //div_edit.show();
                                    _plugin.htmlElements.container.hide();
                                    _plugin.htmlElements.groupplayerDetail.container.show();
                                },
                                error: function () {
                                }
                            })
                        }
                    }
                    
                }   
            }
        },
        showPlaylistForced: function (options) {
            var $this = $('body').eq(0);
            var _plugin = $this.data('insmGroup');

            var div_forcedplaylists = _plugin.htmlElements.groupplayerDetail.detailBody.detail.showPlaylistForced.container;
                //$('#forcedplaylists');
            div_forcedplaylists.empty();
            if (options.tempForcedPlayList) {
                $.each(options.tempForcedPlayList, function (index, Playlist) {
                    if ((Playlist.BindGroupID == _plugin.data.selectedGroupID && options.isGroup && !options.newgroup) || (!options.isGroup && Playlist.BindGroupID == 0)) {
                        var div_Playlist = $('<div/>').addClass('m-portlet m-portlet--mobile m-portlet--sortable m-portlet--warning m-portlet--head-sm').attr('playlistId', Playlist.PlayListID);
                        var div_Playlisthead = $('<div/>').addClass('m-portlet__head');
                    } else {
                        var div_Playlist = $('<div/>').addClass('m-portlet m-portlet--mobile m-portlet--warning m-portlet--head-solid-bg');
                        var div_Playlisthead = $('<div/>').addClass('m-portlet__head ui-sortable-handle');
                    }

                    var div_head_caption = $('<div/>').addClass('m-portlet__head-caption');
                    var div_title = $('<div/>').addClass('m-portlet__head-title');
                    var span_head_icon = $("<span />").addClass('m-portlet__head-icon');
                    var span_i = '<i class="fa fa-file-text"></i>';
                    if ((Playlist.BindGroupID == _plugin.data.selectedGroupID && options.isGroup) || (!options.isGroup && Playlist.BindGroupID == 0)) {
                        var head_text = $('<h3 />').addClass('m-portlet__head-text').append(Playlist.PlayListName);
                    } else {
                        var head_text = $('<h3 />').addClass('m-portlet__head-text').append(Playlist.PlayListName + '<br>' +'(' + Playlist.GroupName + ')');
                    }

                    span_head_icon.append(span_i);
                    div_title.append(span_head_icon);
                    div_title.append(head_text);
                    div_head_caption.append(div_title);
                    div_Playlisthead.append(div_head_caption);
                    var div_head_tools = $('<div/>').addClass('m-portlet__head-tools');
                    var div_portlet_nav = $('<ul>').addClass("m-portlet__nav");
                    if ((Playlist.BindGroupID == _plugin.data.selectedGroupID && options.isGroup) || (!options.isGroup && Playlist.BindGroupID == 0)) {
                        var div_li = $('<li />').addClass('m-portlet__nav-item');
                        var href = $('<a />').addClass("m-portlet__nav-link m-portlet__nav-link--icon").attr("href", "javascript:;");
                        var href_i = $('<i />').addClass("la la-close");
                        href.append(href_i);
                        div_li.append(href);
                        div_portlet_nav.append(div_li);
                        div_li.click(function () {
                            div_Playlist.remove();
                        });
                    }
                    var div_li_list = $('<li />').addClass("m-portlet__nav-item m-dropdown m-dropdown--inline m-dropdown--arrow m-dropdown--align-right m-dropdown--align-push");
                    div_li_list.attr('data-dropdown-toggle', 'hover').attr('aria-expanded', 'true');
                    var div_li_a_toggle = $('<a href="javascript:;"/>').addClass('m-portlet__nav-link m-portlet__nav-link--icon m-dropdown__toggle');
                    var div_li_i = $('<i />').addClass('la la-ellipsis-v');
                    div_li_a_toggle.append(div_li_i);
                    div_li_list.append(div_li_a_toggle);

                    var div_m_dropdown_wrapper = $('<div/>').addClass('m-dropdown__wrapper');
                    var wrappe_spantitle = $("<span style='left: auto; right: 18.5px;'/>").addClass('m-dropdown__arrow m-dropdown__arrow--right m-dropdown__arrow--adjust');
                    var div_m_dropdown_inner = $('<div/>').addClass("m-dropdown__inner");
                    var div_m_dropdown_bodyr = $('<div/>').addClass("m-dropdown__body");
                    var div_m_dropdown_content = $('<div/>').addClass("m-dropdown__content");

                    var ul = $('<ul>').addClass("m-nav");
                    //Item
                    if (Playlist.Settings) {
                        var playlistSetting = JSON.parse(Playlist.Settings);
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
                    div_head_tools.append(div_portlet_nav);
                    div_m_dropdown_wrapper.append(wrappe_spantitle);
                    div_m_dropdown_wrapper.append(div_m_dropdown_inner);
                    div_li_list.append(div_m_dropdown_wrapper)
                    div_portlet_nav.append(div_li_list);
                    div_Playlisthead.append(div_head_tools);
                    div_Playlist.append(div_Playlisthead);
                    
                    div_forcedplaylists.append(div_Playlist);
                });
                //$('#forcedplaylists')
                _plugin.htmlElements.groupplayerDetail.detailBody.detail.showPlaylistForced.container.sortable({
                    connectWith: ".m-portlet__head",
                    items: "div.m-portlet:not(.m-portlet--warning.m-portlet--head-solid-bg)",
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
            }
        },
        edit_player_click: function (selectPlayer, allPlayerID) {
            var $this = $('body').eq(0);
            var _plugin = $this.data('insmGroup');
            $.insmGroup('defaultDataSet');

            var allPlayerNames = "";
            $.each(selectPlayer, function (playerIndex, playerItem) {
                allPlayerNames += ", " + $(playerItem).data().obj.PlayerName;
            })
            allPlayerNames = allPlayerNames.substr(2);
            _plugin.htmlElements.groupplayerDetail.container.find("H3:first").text(selectPlayer.length + " Display Units / (" + allPlayerNames + ")");
            _plugin.data.groupTreeForPlayerEditID = _plugin.htmlElements.grouptree.jstree(true).get_selected()[0];

            var allPlayerID = "";
            $.each(selectPlayer, function (playerIndex, playerItem) {
                allPlayerID += ", " + $(playerItem).data().obj.PlayerID;
            })
            allPlayerID = allPlayerID.substr(2);

            _plugin.data.editPlayerFlg = true;
            $.insmGroup('defaultDataSet');
            _plugin.htmlElements.groupplayerDetail.detailBody.savePlayerbutton.css('display', 'block').removeClass('m-dropdown__toggle');
            //$("#button_save_Player").css('display', 'block').removeClass('m-dropdown__toggle');
            //$("#button_save")
            _plugin.htmlElements.groupplayerDetail.detailBody.savebutton.css('display', 'none');

            $.each(selectPlayer, function (index, item) {
                if (index != 0) {
                    if (_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.input.val() != $(selectPlayer[index]).data().obj.PlayerName) {
                        _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.input.val('');
                    }
                    if (_plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.commenttextarea.val() != $(selectPlayer[index]).data().obj.Comments) {
                        _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.commenttextarea.val('');
                    }

                    if (_plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.jstree(true).get_selected() != $(selectPlayer[index]).data().obj.GroupID) {
                        _plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.jstree(true).deselect_all(true);
                    }
                } else {
                    _plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.jstree(true).deselect_all(true);
                    _plugin.htmlElements.groupplayerDetail.detailBody.detail.editGroup.tree.jstree(true).select_node($(selectPlayer[index]).data().obj.GroupID);

                    _plugin.htmlElements.groupplayerDetail.detailBody.detail.displaylabel.input.val($(selectPlayer[index]).data().obj.PlayerName);
                    _plugin.htmlElements.groupplayerDetail.detailBody.detail.displayunits.commenttextarea.val($(selectPlayer[index]).data().obj.Comments);

                    var Settings;
                    $.each(_plugin.data.player_Alldata, function (playerindex, playerdata) {
                        if (playerdata.PlayerID == $(selectPlayer[index]).data().obj.PlayerID) {
                            Settings = JSON.parse(playerdata.Settings);
                        }
                    
                    });
                    if (Settings != null) {
                        if (Object.getOwnPropertyNames(Settings).length > 0) {
                            $.insmGroup('setTimeOptions', Settings);

                            if (Settings.resolution) {
                                $('#select_resolution').val(Settings.resolution);
                            }
                            $("#label_Active_" + Settings.ActiveFlag).click();
                            $("#label_Online_" + Settings.OnlineFlag).click();
                        }
                    }

                }
            });
            $.insmFramework('getPlaylistByGroup', {
                GroupID: _plugin.data.selectedGroupID,
                success: function (data) {
                    if (data) {
                        $.insmGroup('showPlaylist', { Playlists: data, isGroup: false, GroupID: _plugin.data.selectedGroupID, playerId: allPlayerID });
                    }
                    //div_main.hide();
                    //div_edit.show();
                    _plugin.htmlElements.container.hide();
                    _plugin.htmlElements.groupplayerDetail.container.show();
                },
                error: function () {
                },
            })
        },
        initTimeOptionsInPlayerEdit: function () {
            var $this = $('body').eq(0);
            var _plugin = $this.data('insmGroup');
            //_plugin.htmlElements.groupplayerDetail.detailBody.detail.displaytimes.timebody.Monday.Mondaysliderinput.ionRangeSlider({
            //    type: "double",
            //    min: 0,
            //    max: 24,
            //    from: 0,
            //    to: 24,
            //    postfix: $.localize('translate', " o'clock"),
            //    decorate_both: true,
            //    grid: true,
            //});


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
        }
    }
    $.insmGroup = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on $.insmGroup'); 
        }
        return null;
    };


    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-center",
        "preventDuplicates": false,
        "onclick": '',
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