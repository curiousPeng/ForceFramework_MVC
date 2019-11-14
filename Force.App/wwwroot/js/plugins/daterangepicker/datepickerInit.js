var datePick = (function (model) {
    model.SetStart = function (name) {
        model.Start = $(name);
    };

    model.SetEnd = function (name) {
        model.End = $(name);
    };

    model.SetMin = function (minDate) {
        model.MinDate = minDate;
    };

    model.SetId = function (id) {
        model.Id = id;
    };

    model.Show = function (StartDate, EndDate) {
        var options = {};

        options.timePicker = true;
        options.timePicker24Hour = true;
        options.timePickerIncrement = 1;

        options.timePickerSeconds = true;
        options.autoApply = true;

        options.dateLimit = { days: 7 };

        var format = "YYYY/MM/DD HH:mm:ss";
        options.locale = {
            direction: 'ltr',
            format: format,
            separator: ' - ',
            applyLabel: '确认',
            cancelLabel: '关闭',
            fromLabel: 'From',
            toLabel: 'To',
            customRangeLabel: '自定义',
            daysOfWeek: ["日", "一", "二", "三", "四", "五", "六"],
            monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
            firstDay: 1
        };

        if (StartDate != undefined && StartDate != "") {
            options.startDate = StartDate;
        }

        if (EndDate != undefined && EndDate != "") {

            options.endDate = EndDate;
        }

        if (model.MinDate != undefined && model.MinDate != "") {
            options.minDate = model.MinDate;
        }

        options.maxDate = "9999-01-01";
        options.opens = "right";

        options.drops = "down";
        options.buttonClasses = 'btn btn-sm';
        options.applyClass = "btn-success";

        options.cancelClass = 'btn-default';
        //$('#config-text').val("$('#demo').daterangepicker(" + JSON.stringify(options, null, '    ') + ", function(start, end, label) {\n  console.log(\"New date range selected: ' + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD') + ' (predefined range: ' + label + ')\");\n});");

        $(model.Id).daterangepicker(
            options,
            function (start, end, label) {
                model.Start.val(start.format(format));
                model.End.val(end.format(format));
            })
    }
    //"startTime", "endTime", "2018/10/17 00:00:00", "2018/10/17 23:59:59"
    model.timeSection2 = function updateConfig(startTimeId, endTimeId, startDate, endDate, dayNumber, minDate) {
        var options = {};
        var formatTo = 'YYYY/MM/DD HH:mm:ss';

        //如果觉得卡 可以将当前两行注释  -是因为select的插件数据初始化过慢
        options.singleDatePicker = true;
        options.showDropdowns = true;
        //end

        options.timePicker = true;
        options.singleDatePicker = true;
        options.timePicker24Hour = true;
        options.timePickerIncrement = 1;

        options.timePickerSeconds = true;
        options.autoApply = true;
        options.dateLimit = { days: 30 };
        options.locale = {
            direction: 'ltr',
            format: formatTo,
            separator: ' - ',
            applyLabel: '确认',
            cancelLabel: '关闭',
            fromLabel: 'From',
            toLabel: 'To',
            customRangeLabel: '自定义',
            daysOfWeek: ["日", "一", "二", "三", "四", "五", "六"],
            monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
            firstDay: 1
        };

        if (minDate != undefined && minDate != "") {
            options.minDate = minDate;
        } else {
            options.minDate = "2000/01/01";
        }

        if (startDate != undefined && startDate != "") {
            options.startDate = startDate;
            $('#' + startTimeId).data("start", startDate);
        }

        if ((startDate == undefined || startDate == "") && dayNumber != undefined && dayNumber != "") {
            var monentDate = moment().subtract(parseInt(dayNumber), 'days');
            var nowMonentDate = moment();
            var dateStr = monentDate._d.getFullYear() + "/" + (monentDate._d.getMonth() + 1) + "/" + monentDate._d.getDate() + " 00:00:00";
            var nowDateStr = nowMonentDate._d.getFullYear() + "/" + (nowMonentDate._d.getMonth() + 1) + "/" + nowMonentDate._d.getDate() + " 23:59:59";
            options.startDate = dateStr;

            $('#' + startTimeId).data("start", dateStr);
        }

        options.maxDate = "3000-01-01";
        options.opens = "right";

        options.drops = "down";
        options.buttonClasses = 'btn btn-sm';
        options.applyClass = "btn-success";

        options.cancelClass = 'btn-default';

        options.locale = {
            direction: 'ltr',
            format: formatTo,
            separator: ' - ',
            applyLabel: '确认',
            cancelLabel: '关闭',
            fromLabel: 'From',
            toLabel: 'To',
            customRangeLabel: '自定义',
            daysOfWeek: ["日", "一", "二", "三", "四", "五", "六"],
            monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
            firstDay: 1
        };

        $('#' + startTimeId).daterangepicker(
            options,
            function (start, end, label) {
                if (start >= new Date($('#' + startTimeId).data("end"))) {
                    var nowDateStr = start._d.getFullYear() + "/" + (start._d.getMonth() + 1) + "/" + start._d.getDate() + " 23:59:59";
                    $('#' + endTimeId).data("start", nowDateStr);
                    $('#' + endTimeId).val(nowDateStr);
                }
                $('#' + startTimeId).data("start", start.format(formatTo));
            });

        if (endDate != undefined && endDate != "") {
            options.startDate = endDate;
            $('#' + startTimeId).data("end", endDate);
        }


        if ((startDate == undefined || startDate == "") && dayNumber != undefined && dayNumber != "") {
            var monentDate = moment().subtract(parseInt(dayNumber), 'days');
            var nowMonentDate = moment();
            var dateStr = monentDate._d.getFullYear() + "/" + (monentDate._d.getMonth() + 1) + "/" + monentDate._d.getDate() + " 00:00:00";
            var nowDateStr = nowMonentDate._d.getFullYear() + "/" + (nowMonentDate._d.getMonth() + 1) + "/" + nowMonentDate._d.getDate() + " 23:59:59";
            options.startDate = nowDateStr;
            $('#' + startTimeId).data("end", nowDateStr);
        }

        if (startDate != "" && (endDate == undefined || endDate == "")) {
            var nowMonentDate = moment();
            var nowDateStr = nowMonentDate._d.getFullYear() + "/" + (nowMonentDate._d.getMonth() + 1) + "/" + nowMonentDate._d.getDate() + " 23:59:59";

            options.startDate = nowDateStr;
            $('#' + startTimeId).data("end", nowDateStr);
        }

        $('#' + endTimeId).daterangepicker(
            options,
            function (start, end, label) {
                if (end <= new Date($('#' + startTimeId).data("start"))) {

                    var nowDateStr = end._d.getFullYear() + "/" + (end._d.getMonth() + 1) + "/" + end._d.getDate() + " 00:00:00";
                    $('#' + startTimeId).data("start", nowDateStr);
                    $('#' + startTimeId).val(nowDateStr);
                }

                $('#' + startTimeId).data("end", end.format(formatTo));
            });
    }

    model.OneTime = function updateConfig(startTimeId, startDate) {
        var options = {};
        var formatTo = 'YYYY/MM/DD HH:mm:ss';
       
        //如果觉得卡 可以将当前两行注释  -是因为select的插件数据初始化过慢
        options.singleDatePicker = true;
        options.showDropdowns = true;
        //end

        options.timePicker = true;
        options.singleDatePicker = true;
        options.timePicker24Hour = true;
        options.timePickerIncrement = 1;

        options.timePickerSeconds = true;
        options.autoApply = true;
        options.dateLimit = { days: 30 };
        options.locale = {
            direction: 'ltr',
            format: formatTo,
            separator: ' - ',
            applyLabel: '确认',
            cancelLabel: '关闭',
            fromLabel: 'From',
            toLabel: 'To',
            customRangeLabel: '自定义',
            daysOfWeek: ["日", "一", "二", "三", "四", "五", "六"],
            monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
            firstDay: 1
        };

        if (startDate != undefined && startDate != "") {
            options.minDate = startDate;

            options.startDate = startDate;
            $('#' + startTimeId).data("start", startDate);

        } else {
            options.minDate = "2000/01/01";
        }

        options.maxDate = "3000-01-01";
        options.opens = "right";

        options.drops = "down";
        options.buttonClasses = 'btn btn-sm';
        options.applyClass = "btn-success";

        options.cancelClass = 'btn-default';

        options.locale = {
            direction: 'ltr',
            format: formatTo,
            separator: ' - ',
            applyLabel: '确认',
            cancelLabel: '关闭',
            fromLabel: 'From',
            toLabel: 'To',
            customRangeLabel: '自定义',
            daysOfWeek: ["日", "一", "二", "三", "四", "五", "六"],
            monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
            firstDay: 1
        };

        $('#' + startTimeId).daterangepicker(
            options,
            function (start, end, label) { 
                $('#' + startTimeId).data("start", start.format(formatTo));
            });
    }

    return model;
})(window.datePick || {});


