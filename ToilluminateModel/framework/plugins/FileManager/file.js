(function ($) {
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
            field: "FileID",
            title: "",
            sortable: false, // disable sort for this column
            width: 40,
            textAlign: 'center',
            filterable: false,
            selector: { class: 'm-checkbox--solid m-checkbox--brand' },
        }, {
            field: "FileThumbnailUrl",
            title: "サムネイル",
            filterable: false, // disable or enable filtering
            width: 244,
            sortable: true,
            // basic templating support for column rendering,
            template: '<a href="{{FileUrl}}" target="_blank"><img src="{{FileThumbnailUrl}}" class="file-img" style="max-width:244px;max-height:160px;"/></a>'
        }, {
            field: "FileName",
            title: "ファイル名",
            sortable: true,
            responsive: { visible: 'lg' }
        }, {
            field: "InsertDate",
            title: "登録日時",
            sortable: true,
            filterable: false,
            textAlign: 'center',
            datetype: "yyyy-MM-dd HH:mm",
            responsive: { visible: 'lg' }
        }]
    };
    var selectedFileData = {};
    var selectedFolderID = null;
    var actionEnum = { "cut": "cutFile", "copy": "copyFile" };
    var action = "";
    var methods = {
        init: function (options) {
            selectedFolderID = options.selectedFolderID;
            $('#datatable_file').prop("outerHTML", "<div class='m_datatable' id='datatable_file'></div>");
            $.insmFramework('getFilesByFolder', {
                FolderID: selectedFolderID,
                success: function (fileData) {
                    tableData.data.source.data = fileData;
                    $("#datatable_file").data("datatable", $('#datatable_file').mDatatable(tableData));
                },
                error: function () {
                    //options.error();
                },
            });
        },
        uploadFile: function () {
            if (!selectedFolderID) {
                return false;
            }
        },
        editFile: function (node) {

        },
        remove: function () {
            var datatable = $("#datatable_file").data("datatable");
            if (datatable && datatable.setSelectedRecords().getSelectedRecords().length > 0) {
                toastr.warning("使用中ですので、削除できない。");
                return false;
                //remove selected files
                $.each(datatable.setSelectedRecords().getSelectedRecords(), function (index, item) {
                    $.insmFramework("deleteFile", {
                        fileID: $(item).data().obj.FileID,
                        fileObj:$(item).data().obj,
                        success: function (fileData) {
                            $.file('removeDataFromTable', item, fileData);
                        },
                        error: function () {
                            //invalid = true;
                        }
                    });
                });
            } else {
                //remove selected folder
                $.folder("deleteFolder");
            }
        },
        cutFile: function () {
            if ($("#datatable_file").data("datatable")) {
                var datatable = $("#datatable_file").data("datatable");
                selectedFileData = datatable.setSelectedRecords().getSelectedRecords();
                action = actionEnum.cut;
            }
        },
        copyFile: function () {
            if ($("#datatable_file").data("datatable")) {
                var datatable = $("#datatable_file").data("datatable");
                selectedFileData = datatable.setSelectedRecords().getSelectedRecords();
                action = actionEnum.copy;
            }
            
        },
        pasteFile: function () {
            if (selectedFolderID && selectedFileData.length > 0 && $("#datatable_file").data("datatable")) {
                $.each(selectedFileData, function (index, item) {
                    if($(item).data().obj != undefined){
                        $.insmFramework(action, {
                            sourceFile: $(item).data().obj,
                            newFolderID: selectedFolderID,
                            success: function (fileData) {
                                $.file('insertDataToTable', fileData);
                            },
                            error: function () {
                                //invalid = true;
                            }
                        });
                    }
                });
            }
            selectedFileData = {};
        },
        insertDataToTable: function (data) {
            tableData.data.source.data[tableData.data.source.data.length] = data;
            $("#datatable_file").data("datatable").fullJsonData = tableData.data.source.data;
            $("#datatable_file").data("datatable").reload();
        },
        removeDataFromTable: function (obj, data) {
            $.each(tableData.data.source.data, function (key, item) {
                if (item.FileID === data.FileID) {
                    tableData.data.source.data.splice(key, 1);
                    $(obj).fadeOut(500, function () {
                        $(obj).remove();
                    });
                    return false;
                }
            });
        },
        destroyFileTableData: function () {
            selectedFolderID = null;
            if ($("#datatable_file").data("datatable")) {
                $("#datatable_file").data("datatable").destroy();
            }
        }
        
    };
    $("#btn_uploadfile").click(function () {
        return $.file('uploadFile');
    });

    $("#btn_delete").click(function () {
        $.file('remove');
    });

    $("#btn_paste").click(function () {
        $.file('pasteFile');
    });

    $("#btn_copy").click(function () {
        $.file('copyFile');
    });

    $("#btn_cut").click(function () {
        $.file('cutFile');
    });
    $(".file-img").click(function (obj) {
    });

    $.file = function (method) {
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