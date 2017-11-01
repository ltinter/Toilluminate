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
        sortable: true,

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
            width: 200,
            // basic templating support for column rendering,
            template: '<img src="{{FileThumbnailUrl}}" width = "200px"/>'
        }, {
            field: "FileName",
            title: "File Name",
            responsive: { visible: 'lg' }
        }, {
            field: "InsertDate",
            title: "Great Date",
            filterable: false,
            textAlign: 'center',
            datetype: "yyyy-MM-dd HH:mm",
            responsive: { visible: 'lg' }
        }]
    };
    var methods = {
        init: function (options) {
            $('#datatable_file').prop("outerHTML", "<div class='m_datatable' id='datatable_file'></div>");
            $.insmFramework('getFilesByFolder', {
                FolderID: options.selectedFolderID,
                success: function (fileData) {
                    tableData.data.source.data = fileData;
                    $("#datatable_file").data("datatable", $('#datatable_file').mDatatable(tableData));
                },
                invalid: function () {
                    invalid = true;
                },
                error: function () {
                    options.error();
                },
            });
        },
        uploadFile: function () {
        },
        editFile: function (node) {

        },
        removeFile: function (node) {

        },
        insertDataToTable: function (data) {
            tableData.data.source.data[tableData.data.source.data.length] = data[0];
            $("#datatable_file").data("datatable").fullJsonData = tableData.data.source.data;
            $("#datatable_file").data("datatable").reload();
        }
        
    };
    $("#btn_uploadfile").click(function () {

    });

    $("#btn_delete").click(function () {

    });

    $("#btn_paste").click(function () {

    });

    $("#btn_copy").click(function () {

    });

    $("#btn_cut").click(function () {

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