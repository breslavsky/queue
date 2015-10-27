//#region bind global ajax events
$("#loading").hide();
$(document).ajaxSend(function () {
    $("#loading").show();
}).ajaxStop(function () {
    $("#loading").hide();
});
//#endregion

//#region globals
function globals() {
    this.sessionId;
    this.client;
    this.service;
    this.requestDate;
};
globals.prototype = {
    set DefaultConfig(config) {
        this.defaultConfig = config;
        $("#queue-name").html(config.QueueName);
        document.title = config.QueueName;
    },
    get DefaultConfig() {
        return this.defaultConfig;
    },
    set PortalConfig(config) {
        this.portalConfig = config;
    },
    get PortalConfig() {
        return this.portalConfig;
    }
};

var globals = new globals;
//#endregion

//#region config
$.ajax({
    url: "/config/default",
    dataType: "json",
    success: function (config) {
        globals.DefaultConfig = config;
        $(document).trigger("defaultConfigLoaded");
    }
});

$(document).on("defaultConfigLoaded", function () {
    $.ajax({
        url: "/config/portal",
        dataType: "json",
        success: function (config) {
            globals.PortalConfig = config;
            $(document).trigger("portalConfigLoaded");
        }
    });
});
//#endregion

// load metrics
$(document).on("portalConfigLoaded", function () {
    var data1 = [
        {
            label: "Обслужено",
            data: [],
            yaxis: 1
        },
        {
            label: "В очереди",
            data: [],
            yaxis: 1
        }];

    var data2 = [
        {
            label: "Время обслуживания",
            data: [],
            yaxis: 1
        },
        {
            label: "Время ожидания",
            data: [],
            yaxis: 1
        }];

    var workStartTime = TimeSpan.parseExact(globals.DefaultConfig.WorkStartTime);
    var workFinishTime = TimeSpan.parseExact(globals.DefaultConfig.WorkFinishTime);

    var maxRenderingTime = TimeSpan.parseExact(globals.DefaultConfig.MaxRenderingTime);

    var plot1 = $.plot("#queue-metric1", data1, {
        series: {
            points: {
                show: true
            },
            lines: {
                show: true
            }
        },
        xaxes: [
            { min: workStartTime.hours, max: workFinishTime.hours }
        ],
        yaxes: [
            { position: 'left', min: 0, max: globals.DefaultConfig.MaxClientRequests }
        ],
        grid: {
            hoverable: true,
            clickable: true
        },
        legend: {
            noColumns: 2
        }
    });

    var plot2 = $.plot("#queue-metric2", data2, {
        series: {
            points: {
                show: true
            },
            lines: {
                show: true
            }
        },
        xaxes: [
            { min: workStartTime.hours, max: workFinishTime.hours }
        ],
        yaxes: [
            { position: 'right', min: 0, max: globals.DefaultConfig.MaxRenderingTime, alignTicksWithAxis: 1, tickFormatter: function (v, axis) { return v + " мин."; } }
        ],
        grid: {
            hoverable: true,
            clickable: true
        },
        legend: {
            noColumns: 2
        }
    });

    $(".queue-metric").bind("plothover", function (event, pos, item) {
        $("#plot-tooltip").remove();
        if (item) {
            var x = item.datapoint[0].toFixed(2);
            var y = item.datapoint[1].toFixed(2);
            var yaxis = item.series.yaxis;

            $("<div id='plot-tooltip'></div>").html(item.series.label + " " + yaxis.tickFormatter(y, yaxis))
                .css({ left: item.pageX + 5, top: item.pageY + 5 })
                .appendTo("body").show();
        }
    });

    var today = Date.now();

    function getQueueMetric(hour) {
        $.ajax({
            url: "/get-queue-plan-metric",
            dataType: "json",
            global: false,
            hour: hour,
            data: { year: today.getFullYear(), month: today.getMonth() + 1, day: today.getDate(), hour: hour },
            success: function (metric) {
                data1[0].data.push([this.hour, metric.Rendered]);
                data1[1].data.push([this.hour, metric.Waiting]);

                var renderTime = TimeSpan.parseExact(metric.RenderTime);
                data2[0].data.push([this.hour, renderTime.minutes]);
                var waitingTime = TimeSpan.parseExact(metric.WaitingTime);
                data2[1].data.push([this.hour, waitingTime.minutes]);

                plot1.setData(data1);
                plot1.draw();

                plot2.setData(data2);
                plot2.draw();

                new JustGage({ id: "productivity", value: metric.Productivity, min: 0, max: 100, label: "%", title: "Производительность" });
            }
        }).always(function () {
            if (this.hour <= workFinishTime.hours) {
                getQueueMetric(this.hour + 1);
            }
        });
    };

    getQueueMetric(workStartTime.hours);
});