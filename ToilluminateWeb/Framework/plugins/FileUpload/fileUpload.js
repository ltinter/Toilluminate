uploadFile = function () {
    $.ajaxFileUpload({
        url: baseURL + "/fileCatalog.do?method=save",            //需要链接到服务器地址  
        secureuri: true,
        fileElementId: 'file',                                  
        success: function (data, status) {
            var results = $(data).find('body').html();
            var obj = eval("(" + results + ")");
            $("#fileSize").val(obj.fileSize);
            $("#fileUrl").val(obj.fileUrl);
            $('#fileCatalogForm').submit();
        }, error: function (data, status, e) {
            showDialogWithMsg('ideaMsg', 'Msg', 'error！');
        }
    });
}

getFileName = function (obj) {
    flag = 1;
    var pos = -1;
    if (obj.value.indexOf("/") > -1) {
        pos = obj.value.lastIndexOf("/") * 1;
    } else if (obj.value.indexOf("\\") > -1) {
        pos = obj.value.lastIndexOf("\\") * 1;
    }
    var fileName = obj.value.substring(pos + 1);
    $("#fileName").val(fileName);
    $('.files').text(fileName);
}

ev_save = function () {
    if (submitMyForm('fileCatalogForm')) {
        if (flag == 0) {
            $('#fileCatalogForm').submit();
        } else {
            uploadFile();
        }
    }
}

function ev_back() {
    window.location.href = baseURL + '/fileCatalog.do?method=list';
}

upload = function () {
    var formData = new FormData($('form')[0]);
    $.ajax({
        url: 'upload.php',  //server script to process data
        type: 'POST',
        xhr: function () {  // custom xhr
            myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) { // check if upload property exists
                myXhr.upload.addEventListener('progress', progressHandlingFunction, false); // for handling the progress of the upload
            }
            return myXhr;
        },
        //Ajax事件
        beforeSend: beforeSendHandler,
        success: function (data, status) {
            var results = $(data).find('body').html();
            var obj = eval("(" + results + ")");
            $("#fileSize").val(obj.fileSize);
            $("#fileUrl").val(obj.fileUrl);
            $('#fileCatalogForm').submit();
        },
        error: function (data, status, e) {
            showDialogWithMsg('ideaMsg', 'Msg', 'error！');
        },
        data: formData,
        //Options to tell JQuery not to process data or worry about content-type
        cache: false,
        contentType: false,
        processData: false
    });
};

progressHandlingFunction = function (e) {
    if (e.lengthComputable) {
        $('progress').attr({ value: e.loaded, max: e.total });
    }
}

//add
btnAdd = function () {
    var formData = new FormData($("#frm")[0]);

    $.ajax({
        url: "/Admin/ContentManage/SaveEdit",
        type: "POST",
        data: formData,
        contentType: false, 
        processData: false,
        success: function (data) {
            if (data == "OK") {
                alert("ok");
                $.iDialog("close");
            }
            else {
                alert("err：" + data);
            }
        }
    });
}

cancel = function(fileID, supressEvent) { 
  
    var args = arguments; 
  
    this.each(function() { 
        // Create a reference to the jQuery DOM object 
        var $this    = $(this), 
          swfuploadify = $this.data('uploadify'), 
          settings   = swfuploadify.settings, 
          delay    = -1; 
  
        if (args[0]) { 
            // Clear the queue 
            if (args[0] == '*') { 
                var queueItemCount = swfuploadify.queueData.queueLength; 
                $('#' + settings.queueID).find('.uploadify-queue-item').each(function() { 
                    delay++; 
                    if (args[1] === true) { 
                        swfuploadify.cancelUpload($(this).attr('id'), false); 
                    } else { 
                        swfuploadify.cancelUpload($(this).attr('id')); 
                    } 
                    $(this).find('.data').removeClass('data').html(' - Cancelled'); 
                    $(this).find('.uploadify-progress-bar').remove(); 
                    $(this).delay(1000 + 100 * delay).fadeOut(500, function() { 
                        $(this).remove(); 
                    }); 
                }); 
                swfuploadify.queueData.queueSize  = 0; 
                swfuploadify.queueData.queueLength = 0; 
                // Trigger the onClearQueue event 
                if (settings.onClearQueue) settings.onClearQueue.call($this, queueItemCount); 
            } else { 
                for (var n = 0; n < args.length; n++) { 
                    swfuploadify.cancelUpload(args[n]); 
                    /* add begin*/
                    delete swfuploadify.queueData.files[args[n]]; 
                    swfuploadify.queueData.queueLength = swfuploadify.queueData.queueLength - 1; 
                    /* add end */
                    $('#' + args[n]).find('.data').removeClass('data').html(' - Cancelled'); 
                    $('#' + args[n]).find('.uploadify-progress-bar').remove(); 
                    $('#' + args[n]).delay(1000 + 100 * n).fadeOut(500, function() { 
                        $(this).remove(); 
                    }); 
                } 
            } 
        } else { 
            var item = $('#' + settings.queueID).find('.uploadify-queue-item').get(0); 
            $item = $(item); 
            swfuploadify.cancelUpload($item.attr('id')); 
            $item.find('.data').removeClass('data').html(' - Cancelled'); 
            $item.find('.uploadify-progress-bar').remove(); 
            $item.delay(1000).fadeOut(500, function() { 
                $(this).remove(); 
            }); 
        } 
    }); 
  
}