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
    }
    
};
$.download = function (method) {
    if (methods[method]) {
        return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
    } else if (typeof method === 'object' || !method) {
        return methods.init.apply(this, arguments);
    } else {
        $.error('Method ' + method + ' does not exist on $.insmGroup');
    }
    return null;
};