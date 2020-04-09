//Resolve conflict in jQuery UI tooltip with Bootstrap tooltip
$.widget.bridge('uibutton', $.ui.button);
/*grid view begins*/
function GoToPage(p) {
    $('#pageIndex').val(p);
    $('#searchForm').submit();
}
function RefreshList() {
    $('#key').val('');
    SearchKey();
    document.getElementById('searchForm').submit();
}
function SearchKey() {
    if ($('#key').val().trim() != $('#CurrentKey').val()) {
        $('#pageIndex').val(1);
    }
}
$(document).ready(function () {
    if ($('.AdvancedGrid').length > 0) {
        $('.AdvancedGrid th label[for*="' + $('#PageOrderBy').val() + '"]').parent().addClass($('#PageOrderBy').val().toLowerCase());
        var texts = $('#AdvancedSearchBox').find('input[type="text"]').filter(function () { return $(this).val().trim() != ""; }).length;
        var numbers = $('#AdvancedSearchBox').find('input[type="number"]').filter(function () { return $(this).val().trim() != ""; }).length;
        var selects = $('#AdvancedSearchBox').find('select:enabled').length;
        var checks = $('#AdvancedSearchBox .inner').find('input[type="checkbox"]:enabled').length;
        var filledInputs = texts + numbers + selects + checks;
        if (filledInputs > 0) {
            $("#AdvancedSearchBox .box-body").show();
            $("#AdvancedSearchBox").removeClass('collapsed-box');
            $("#AdvancedSearchBox .box-header .btn i").removeClass('fa-plus').addClass('fa-minus');
            $("#AdvancedSearchBox .box-body").show();
        }
    }
    if ($("#AdvancedSearchBox").length > 0) {
        $("#AdvancedSearchBox").find("[data-type='datetime']").children('input[type="text"]').MdPersianDateTimePicker({
            Trigger: 'click',
            TargetSelector: $(this).attr('id'),
            ToDate: true,
            EnableTimePicker: false,
            Placement: 'right',
            Format: "yyyy/MM/dd",
            GroupId: '',
            FromDate: false,
            DisableBeforeToday: false,
            Disabled: false,
            IsGregorian: false,
            EnglishNumber: false,
        });

        if ($('#PreviousUrl').val()) {
            window.history.pushState({}, null, UpdateQueryString(myInput.attr('id'), myInput.val()));
        }
        else {
            $('#advance-search-form-container').find('input[type="hidden"]').each(function () {
                var myInput = $(this);
                window.history.pushState({}, null, UpdateQueryString(myInput.attr('id'), myInput.val()));
            });

            $('#AdvancedSearchForm').find('.searchRow').each(function () {
                if ($(this).children('input[type="checkbox"]').is(':checked')) {
                    var myInput = $(this).children('input[type="text"]', 'select');
                    if (myInput.val()) {
                        window.history.pushState({}, null, UpdateQueryString(myInput.attr('id'), myInput.val()));
                    }
                }
            });
        }
    }
});


$(document).on('click', '.jsPager ul.pagination li:not(.active) a', function () {
    var page = $(this).attr('href').toLowerCase().match(/pageindex=(.*)/)[1];
    $('#ThisPageIndex').val(page);
    CheckAdvancedSearch();
    return false;
});

function CheckAdvancedSearch() {

    $('#AdvancedSearchForm').find('.searchRow').each(function () {
        if (!$(this).children('input[type="checkbox"]').is(':checked')) {
            if ($(this).children('.inner').length > 0) {
                $(this).children('.inner').children('input[type="hidden"]').val(null);
            }
        }
    });

    $('#AdvancedSearchForm').submit();
}

$("#AdvancedSearchForm").submit(function (event) {
    $('#advance-search-form-container').find('input[type="hidden"]').each(function () {
        var myInput = $(this);
        window.history.pushState({}, null, UpdateQueryString(myInput.attr('id'), myInput.val()));
    });

    $('#AdvancedSearchForm').find('.searchRow').each(function () {
        if ($(this).children('input[type="checkbox"]').length === 0 || $(this).children('input[type="checkbox"]').is(':checked')) {
            var myInput = $(this).children('input[type="text"]');
            if (myInput.length === 0) {
                myInput = $(this).children('select');
            }

            if (myInput.val()) {
                window.history.pushState({}, null, UpdateQueryString(myInput.attr('id'), myInput.val()));
            } else if (myInput.val() === "") {
                window.history.pushState({}, null, UpdateQueryString(myInput.attr('id'), myInput.val()));
            }
        }
    });
});

function ResetAdvancedSearchForm() {
    $('#AdvancedSearchForm')[0].reset();

    $('#AdvancedSearchForm').find('.searchRow').each(function () {
            var myInput = $(this).children('input[type="text"]');
            if (myInput.length === 0) {
                myInput = $(this).children('select');
            }

            if (myInput.val()) {
                myInput.val("");
                window.history.pushState({}, null, UpdateQueryString(myInput.attr('id'), myInput.val()));
            }
    });

    CheckAdvancedSearch();
}

function UpdateQueryString(key, value) {
    var uri = window.location.href;
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";

    if (value) {
        if (uri.match(re)) {
            uri = uri.replace(re, '$1' + key + "=" + value + '$2');
        }
        else {
            uri = uri + separator + key + "=" + value;
        }
    }
    else {
        if (uri.match(re)) {
            uri = uri.replace(re, '$1$2');
        }
    }

    return uri;
}

function AddPreviousUrl(newUrl) {
    var uri = window.location.href;
    newUrl = uri + newUrl;
    window.history.pushState({}, null, newUrl);
}

$("#AdvancedSearchForm .searchRow input[type='checkbox']").change(function () {
    if (this.checked) {
        if ($(this).next().hasClass('inner')) {
            $(this).next().children("input[type='checkbox']").removeAttr('disabled');
            $(this).next().removeClass('disabled');
        }
        else {
            $(this).next().removeAttr('disabled');
        }
    }
    else {
        if ($(this).next().hasClass('inner')) {
            $(this).next().children("input[type='checkbox']").attr('disabled', 'disabled');
            $(this).next().addClass('disabled');
        }
        else {
            $(this).next().attr('disabled', 'disabled');
        }
    }
});
$('.AdvancedGrid th').click(function () {
    if ($(this).find('label').length == 0) {
        return;
    }
    var currentOrder = $('#PageOrder').val();
    var currentOrderBy = $('#PageOrderBy').val();
    var split = $(this).find('label').attr('for').split('_');
    var thisOrderBy = split[split.length - 1];
    if (thisOrderBy == currentOrderBy) {
        if (currentOrder == "asc") {
            $('#PageOrder').val("desc");
        }
        else {
            $('#PageOrder').val("asc");
        }
    }
    else {
        $('#PageOrderBy').val(thisOrderBy);
        $('#PageOrder').val("desc");
    }
    CheckAdvancedSearch();
});
/*grid view ends*/
function openCustomRoxy(id) {
    $('#modal-file iframe').attr('src', '/fileman/index.html?integration=custom&txtFieldId=' + id);
    $('#modal-file').modal('show');
}
function closeCustomRoxy() {
    $('#modal-file').modal('hide');
}
function formatRepoSelection(repo) {
    return repo.name || repo.text;
}
function formatRepo(repo) {
    if (repo.loading) return repo.text;
    var markup = "<div class='dropDownListItem' >";
    if (repo.image != null) {
        markup += "<img width='40' height='40' src='" + repo.image + "' />";
    }
    markup += "<p>" + repo.name + "</p></div>";
    return markup;
}
function InitializeObjectDrp(element, url, initText, initVal, minimumLength) {
    if (minimumLength == undefined || minimumLength == null) {
        minimumLength = 3;
    }
    $(element).select2({
        escapeMarkup: function (markup) {
            return markup;
        },
        minimumInputLength: minimumLength,
        templateResult: formatRepo,
        templateSelection: formatRepoSelection,
        dir: "rtl",
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    key: params.term,
                    p: params.page,
                    //type: $('#Type').val()
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data.Items,
                    pagination: {
                        more: (params.page * 30) < data.TotalCount
                    }
                };
            },
            cache: true
        }
    });
    var option1 = new Option(initText, initVal, true, true);
    $(element).append(option1);
    $(element).trigger('change');
}