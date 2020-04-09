var videoAttachmentType = 4;
$("#btnAdd").click(function () {
    saveAttachment();
});
$("#attachmentType").change(function () {
    if (parseInt($(this).val()) == videoAttachmentType) {
        $('#PathGroup1,#PathGroup3,#PathGroup4').slideDown();
        $('#PathGroup2>label').html('فایل کیفیت متوسط');
    }
    else {
        $('#PathGroup1,#PathGroup3,#PathGroup4').slideUp();
        $('#PathGroup2>label').html('مسیر فایل');
    }
});
$('#btn-cancel').click(function () {
    $('#attachmentType').removeAttr('disabled');
    clearAttachment();
});
$("#btnSave").click(function () {
    $('#message').html('');
    var checked_ids = [];
    $($('#tree-categories').jstree("get_checked", null, true)).each
        (function () {
            checked_ids.push(this);
        });
    if ($("#form-content").validationEngine('validate')) {
        var content = {
            Title: $("#Title").val(),
            Summary: $("#Summary").val(),
            Body: CKEDITOR.instances.Body.getData(),
            Price: parseInt($("#Price").val()),
            Priority: parseInt($("#Priority").val()),
            Thumbnail: $("#Thumbnail").val(),
            Photo: $("#Photo").val(),
            CategoryId: checked_ids,
            FromGrade: parseInt($("#FromGrade").val()),
            ToGrade: parseInt($("#ToGrade").val()),
            Tags: null,
            Positions: null,
            StartShowDate: $("#StartShowDate").val(),
            EndShowDate: $("#EndShowDate").val(),
            ContentTypeId: $("#ContentTypeId").val(),
            CommentCount: 0,
            LikeCount: 0,
            ViewCount: 0,
            DisLikeCount: 0,
            Status: $("#Status").val()
        };
        var url = '/Contents/Create';
        if (page_action == 'edit') {
            content.Id = modelId;
            url = '/Contents/Edit';
        }
        var model = { 'content': content, 'ContentAttachments': getAttachments() };
        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify({ 'content': content, 'ContentAttachments': getAttachments() }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.status == 200) {
                    window.location.href = redirectUrl;
                } else {
                    $('#message').html(response.message);
                }
            },
            error: function (e) {
                console.log(e);
            }
        });
    }
});
function checkPartNo(model) {
    var attachments = getAttachments();
    for (var i = 0; i < attachments.length; i++) {
        if (attachments[i].PartNo == model.PartNo && attachments[i].QualityId == model.QualityId) {
            alert('این بخش با این کیفیت قبلا ثبت شده است.');
            return false;
        }
    }
    return true;
}
function saveAttachment() {
    if (!$("#form-attachment").validationEngine('validate')) {
        return;
    }
    var baseAttachment = {
        Id: $("#attachmentId").val() == null || $("#attachmentId").val() === '' ? new Date().getTime() : $("#attachmentId").val(),
        GroupTitle: $("#groupTitle").val(),
        Title: $("#title").val(),
        Description: $("#description").val(),
        Path: $("#path").val(),
        AttachmentTypeTitle: $("#attachmentType option:selected").text(),
        PartNo: parseInt($("#partNo").val()),
        TypeId: $("#attachmentType").val(),
        IsFree: document.getElementById("isFree").checked,
        QualityId: $("#Attachquality").val(),
        QualityTypeTitle: $("#Attachquality option:selected").text(),
        Status: $("#attachstatus").val(),
        StatusTitle: $("#attachstatus option:selected").text()
    };
    if (page_action == 'edit') {
        baseAttachment.ContentId = modelId;
    }
    if (baseAttachment.PartNo <= 0) {
        alert('شماره بخش درست نمی باشد.');
        return;
    }
    var attachments = [];
    if (parseInt(baseAttachment.TypeId) != videoAttachmentType) {
        baseAttachment.Path = $("#medium-path").val();
        if (baseAttachment.Path == '' || baseAttachment.Path == null) {
            alert('فایل اتنخاب نشده است.');
            return;
        }
        baseAttachment.Id = $("#medium-attachmentId").val() == null || $("#medium-attachmentId").val() === '' ? new Date().getTime() : $("#medium-attachmentId").val();
        baseAttachment.QualityId = 2;
        baseAttachment.QualityTypeTitle = $("#attachmentType option[value='" + baseAttachment.QualityId + "']").text();
        attachments.push(baseAttachment);
    }
    else {
        if ($('#low-path').val().trim() != '') {
            var lowAttachment = jQuery.extend(true, {}, baseAttachment);
            lowAttachment.Id = $("#low-attachmentId").val() == null || $("#low-attachmentId").val() === '' ? new Date().getTime() : $("#low-attachmentId").val();
            lowAttachment.QualityId = 1;
            lowAttachment.QualityTypeTitle = $("#attachmentType option[value='" + lowAttachment.QualityId + "']").text();
            lowAttachment.Path = $("#low-path").val();
            attachments.push(lowAttachment);
        }
        if ($('#medium-path').val().trim() != '') {
            var mediumAttachment = jQuery.extend(true, {}, baseAttachment);
            mediumAttachment.Id = $("#medium-attachmentId").val() == null || $("#medium-attachmentId").val() === '' ? new Date().getTime() : $("#medium-attachmentId").val();
            mediumAttachment.QualityId = 2;
            mediumAttachment.QualityTypeTitle = $("#attachmentType option[value='" + mediumAttachment.QualityId + "']").text();
            mediumAttachment.Path = $("#medium-path").val();
            attachments.push(mediumAttachment);
        }
        if ($('#high-path').val().trim() != '') {
            var highAttachment = jQuery.extend(true, {}, baseAttachment);
            highAttachment.Id = $("#high-attachmentId").val() == null || $("#high-attachmentId").val() === '' ? new Date().getTime() : $("#high-attachmentId").val();
            highAttachment.QualityId = 3;
            highAttachment.QualityTypeTitle = $("#attachmentType option[value='" + highAttachment.QualityId + "']").text();
            highAttachment.Path = $("#high-path").val();
            attachments.push(highAttachment);
        }
        if ($('#best-path').val().trim() != '') {
            var bestAttachment = jQuery.extend(true, {}, baseAttachment);
            bestAttachment.Id = $("#best-attachmentId").val() == null || $("#best-attachmentId").val() === '' ? new Date().getTime() : $("#best-attachmentId").val();
            bestAttachment.QualityId = 4;
            bestAttachment.QualityTypeTitle = $("#attachmentType option[value='" + bestAttachment.QualityId + "']").text();
            bestAttachment.Path = $("#best-path").val();
            attachments.push(bestAttachment);
        }
        if (attachments.length == 0) {
            alert('هیچ فایلی اتنخاب نشده است.');
            return;
        }
        if ($('#low-path').val().trim() == '' && $("#low-attachmentId").val() != null && $("#low-attachmentId").val() != '') {
            deleteAttachmentById($("#low-attachmentId").val());
        }
        if ($('#medium-path').val().trim() == '' && $("#medium-attachmentId").val() != null && $("#medium-attachmentId").val() != '') {
            deleteAttachmentById($("#medium-attachmentId").val());
        }
        if ($('#high-path').val().trim() == '' && $("#high-attachmentId").val() != null && $("#high-attachmentId").val() != '') {
            deleteAttachmentById($("#high-attachmentId").val());
        }
        if ($('#best-path').val().trim() == '' && $("#best-attachmentId").val() != null && $("#best-attachmentId").val() != '') {
            deleteAttachmentById($("#best-attachmentId").val());
        }
    }
    addUpdateAttachments(attachments);
}
function addUpdateAttachments(attachments) {
    $.each(attachments, function (i) {
        var isAdd = false;
        if (parseInt(this.TypeId) == videoAttachmentType) {
            switch (this.QualityId) {
                case 1:
                    isAdd = $("#low-attachmentId").val() == null || $("#low-attachmentId").val() === '';
                    break;
                case 2:
                    isAdd = $("#medium-attachmentId").val() == null || $("#medium-attachmentId").val() === '';
                    break;
                case 3:
                    isAdd = $("#high-attachmentId").val() == null || $("#high-attachmentId").val() === '';
                    break;
                case 4:
                    isAdd = $("#best-attachmentId").val() == null || $("#best-attachmentId").val() === '';
                    break;
            }
        }
        else {
            isAdd = $("#medium-attachmentId").val() == null || $("#medium-attachmentId").val() === '';
        }
        if (isAdd) {
            if (!checkPartNo(this)) return;
            this.UniqueId = this.Id;
            addAttachment(this);
        }
        else {
            updateAttachment(this);
        }
    });
    cleanTable();
    SortTableRows();
    clearAttachment();
}
function cleanTable() {
    $('#table-attachments tr:not(.head)').each(function () {
        var partNo = $(this).attr('data-partno');
        var index = getArrayIndexForKey(contentAttachments, 'PartNo', partNo);
        if (index == -1) {
            $(this).remove();
        }
    });
}
function editAttachment(id) {
    $('#btnAdd').removeAttr('disabled').show();
    var theAttachment = $.grep(contentAttachments, function (e) { return e.PartNo == id; });
    if (theAttachment == null || theAttachment.length == 0) {
        alert('پیوست انتخابی معتبر نمی باشد.');
        return;
    }
    $("#title").val(theAttachment[0].Title);
    $("#description").val(theAttachment[0].Description);
    $("#attachmentType").val(theAttachment[0].TypeId).attr('disabled', 'disabled');
    $("#attachmentType").change();
    $("#partNo").val(theAttachment[0].PartNo);
    document.getElementById("isFree").checked = theAttachment[0].IsFree;
    $("#attachstatus").val(theAttachment[0].Status);
    $("#title").focus();
    if (parseInt(theAttachment[0].TypeId) != videoAttachmentType) {
        $("#medium-path").val(theAttachment[0].Path);
        $("#medium-attachmentId").val(theAttachment[0].Id);
    }
    else {
        $.each(theAttachment, function (i) {
            switch (parseInt(this.QualityId)) {
                case 1:
                    $("#low-path").val(this.Path);
                    $("#low-attachmentId").val(this.Id);
                    break;
                case 2:
                    $("#medium-path").val(this.Path);
                    $("#medium-attachmentId").val(this.Id);
                    break;
                case 3:
                    $("#high-path").val(this.Path);
                    $("#high-attachmentId").val(this.Id);
                    break;
                case 4:
                    $("#best-path").val(this.Path);
                    $("#best-attachmentId").val(this.Id);
                    break;
            }
        });
    }
}
function addAttachment(attachment) {
    if ($('tr[data-partno=' + attachment.PartNo + ']').length > 0) {
        $($('tr[data-partno=' + attachment.PartNo + ']')[0].cells[0]).html(attachment.Title);
        $($('tr[data-partno=' + attachment.PartNo + ']')[0].cells[1]).html(attachment.AttachmentTypeTitle);
        $($('tr[data-partno=' + attachment.PartNo + ']')[0].cells[2]).html(attachment.PartNo);
        $($('tr[data-partno=' + attachment.PartNo + ']')[0].cells[3]).html("<input " + (attachment.IsFree ? "checked" : "") + " class='check-box' disabled type='checkbox'>");
        $($('tr[data-partno=' + attachment.PartNo + ']')[0].cells[4]).html(attachment.StatusTitle);
    }
    else {
        var $row = $("<tr/>").attr("id", attachment.Id).attr("class", "row-attachment").attr('data-PartNo', attachment.PartNo);
        $row.append($("<td/>").html(attachment.Title));
        $row.append($("<td/>").html(attachment.AttachmentTypeTitle));
        $row.append($("<td/>").html(attachment.PartNo));
        $row.append($("<td/>").html("<input " + (attachment.IsFree ? "checked" : "") + " class='check-box' disabled type='checkbox'>"));
        $row.append($("<td/>").html(attachment.StatusTitle));
        var node = $("<td/>");
        var edit = $("<a/>").attr("onclick", "viewAttachment(" + attachment.PartNo + ");").attr('style', 'cursor:pointer;').addClass("btnEdit").html("مشاهده");
        var remove = $("<a/>").attr("onclick", "deleteAttachmentByPartNo(" + attachment.PartNo + ");").attr('style', 'cursor:pointer;').addClass("btnRemove").html("حذف");
        node.append(edit).append($("<span/>").html(" | ")).append(remove);
        $row.append(node);
        $("#table-attachments").append($row);
    }
    contentAttachments.push(attachment);
}
function viewAttachment(id) {
    editAttachment(id);
    $('#btnAdd').attr('disabled', 'disabled').hide();
}
function SortTableRows() {
    var $table = $('#table-attachments');
    var rows = $table.find('tr:not(.head)').get();
    rows.sort(function (a, b) {
        var keyA = parseInt($(a).attr('data-partno'));
        var keyB = parseInt($(b).attr('data-partno'));
        if (keyA < keyB) return -1;
        if (keyA > keyB) return 1;
        return 0;
    });

    $.each(rows, function (index, row) {
        $table.children('tbody').append(row);
    });
}
function updateAttachment(attachment) {
    var index = getArrayIndexForKey(contentAttachments, 'Id', attachment.Id);
    contentAttachments[index].Id = attachment.Id;
    contentAttachments[index].Title = attachment.Title;
    contentAttachments[index].Description = attachment.Description;
    contentAttachments[index].Path = attachment.Path;
    contentAttachments[index].TypeId = attachment.TypeId;
    contentAttachments[index].AttachmentTypeTitle = attachment.AttachmentTypeTitle;
    contentAttachments[index].PartNo = attachment.PartNo;
    contentAttachments[index].IsFree = attachment.IsFree;
    contentAttachments[index].QualityId = attachment.QualityId;
    contentAttachments[index].QualityTypeTitle = attachment.QualityTypeTitle;
    contentAttachments[index].Status = attachment.Status;
    contentAttachments[index].StatusTitle = attachment.StatusTitle;
    var rowCount = $('tr[data-partno=' + attachment.PartNo + ']').length;

    if (rowCount == 0) {
        var $row = $("<tr/>").attr("id", attachment.Id).attr("class", "row-attachment").attr('data-PartNo', attachment.PartNo);
        $row.append($("<td/>").html(attachment.Title));
        $row.append($("<td/>").html(attachment.AttachmentTypeTitle));
        $row.append($("<td/>").html(attachment.PartNo));
        $row.append($("<td/>").html("<input " + (attachment.IsFree ? "checked" : "") + " class='check-box' disabled type='checkbox'>"));
        $row.append($("<td/>").html(attachment.StatusTitle));
        var node = $("<td/>");
        var edit = $("<a/>").attr("onclick", "viewAttachment(" + attachment.PartNo + ");").attr('style', 'cursor:pointer;').addClass("btnEdit").html("مشاهده");
        var remove = $("<a/>").attr("onclick", "deleteAttachmentByPartNo(" + attachment.PartNo + ");").attr('style', 'cursor:pointer;').addClass("btnRemove").html("حذف");
        node.append(edit).append($("<span/>").html(" | ")).append(remove);
        $row.append(node);
        $("#table-attachments").append($row);
    }
    else {
        if (rowCount > 1) {
            $('tr[data-partno=' + attachment.PartNo + ']').slice(1).remove();
        }
        $($('tr[data-partno=' + attachment.PartNo + ']')[0].cells[0]).html(attachment.Title);
        $($('tr[data-partno=' + attachment.PartNo + ']')[0].cells[1]).html(attachment.AttachmentTypeTitle);
        $($('tr[data-partno=' + attachment.PartNo + ']')[0].cells[2]).html(attachment.PartNo);
        $($('tr[data-partno=' + attachment.PartNo + ']')[0].cells[3]).html("<input " + (attachment.IsFree ? "checked" : "") + " class='check-box' disabled type='checkbox'>");
        $($('tr[data-partno=' + attachment.PartNo + ']')[0].cells[4]).html(attachment.StatusTitle);
        $('a[data-id="edit-' + attachment.UniqueId + '"]').attr('onclick', 'editAttachment(' + attachment.PartNo + ');');
        $('a[data-id="remove-' + attachment.UniqueId + '"]').attr('onclick', 'deleteAttachmentByPartNo(' + attachment.PartNo + ');');
    }

}
function getAttachments() {
    var result = contentAttachments;
    for (var i = 0; i < result.length; i++) {
        if (result[i].Id != null && result[i].Id.toString().length > 8) {
            result[i].Id = null;
        }
        if (result[i].Description == null || result[i].Description === '') {
            result[i].Description = ' ';
        }
    }
    return result;
}
function clearAttachment() {
    $("#best-attachmentId,#low-attachmentId,#medium-attachmentId,#high-attachmentId").val("");
    $("#title").val("");
    $('#description').val('');
    $("#low-path,#medium-path,#high-path,#best-path").val("");
    $("#attachmentType").val("").removeAttr('disabled');
    $("#attachmentType").change();
    $("#partNo").val(0);
    $("#refrenceId").val(0);
    $("#isFree").removeAttr("checked");
    $("#Attachquality").val("");
    $("#attachstatus").val("");
    $('#btnAdd').removeAttr('disabled').show();
}
function deleteAttachmentByPartNo(partNo) {
    contentAttachments = contentAttachments.filter(function (el) {
        return parseInt(el.PartNo) !== parseInt(partNo);
    });
    $($('tr[data-PartNo=' + partNo + ']')[0]).remove();
}
function deleteAttachmentById(id) {
    var uniqueId = contentAttachments.filter(function (el) {
        return parseInt(el.Id) == parseInt(id);
    })[0].UniqueId;

    contentAttachments = contentAttachments.filter(function (el) {
        return parseInt(el.Id) !== parseInt(id);
    });

    if (contentAttachments.filter(function (el) {
        return el.UniqueId == uniqueId;
    }).length == 0) {
        $($('#' + uniqueId)[0]).remove();
    }
}
function getArrayIndexForKey(arr, key, val) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i][key] == val)
            return i;
    }
    return -1;
}