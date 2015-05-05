var MAX_TEXT_LENGTH = 80;
var SERVICE_URI = "/client";

var clientRequestRegistrator = {
    terminal: 1,
    manager: 2,
    portal: 4
};

//#region apng
$("#loading img").each(function () { APNG.animateImage(this); });
//#endregion

//#region ulogin
$("#uLogin").attr("data-ulogin", $("#uLogin").attr("data-ulogin")
    .replace("{HTTP_LOCATION}", document.location));
//#endregion

//#region bind global ajax events
$("#loading").hide();
$(document).ajaxSend(function () {
    $("#loading").show();
}).ajaxStop(function () {
    $("#loading").hide();
});
$.ajaxSetup({
    cache: false,
    beforeSend: function (request) {
        if (this.url.charAt(0) != "/") {
            this.url = SERVICE_URI + "/" + this.url;
        }
        if (globals.SessionId != undefined) {
            request.setRequestHeader("SessionId", globals.SessionId);
        }
    }
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
    set SessionId(sessionId) {
        this.sessionId = sessionId;
        $.cookie("SessionId", sessionId, { expires: 7 });
    },
    get SessionId() {
        return this.sessionId;
    },
    set Client(client) {
        this.client = client;
        this.SessionId = client.SessionId;
        $("#client-menu .name").html(client.Surname + " " + client.Name);
    },
    get Client() {
        return this.client;
    },
    set Service(service) {
        this.service = service;
        $(".service").html(service.Name);
    },
    get Service() {
        return this.service;
    },
    set RequestDate(requestDate) {
        this.requestDate = requestDate;
        $(".request-date").html(requestDate.toString("dddd, MMMM dd, yyyy HH:mm"));
    },
    get RequestDate() {
        return this.requestDate;
    },
    set DefaultConfig(config) {
        this.defaultConfig = config;
        document.title = config.QueueName;
    },
    get DefaultConfig() {
        return this.defaultConfig;
    },
    set PortalConfig(config) {
        this.portalConfig = config;
        $("footer").append(config.Footer);
    },
    get PortalConfig() {
        return this.portalConfig;
    }
};

var globals = new globals;
//#endregion

//#region global events
$(document).bind("client-login", function () {
    $("#client-panel-carousel").carousel("next");
});
//#endregion

//#region client-menu
$("#client-menu .requests").click(function () {
    $.ajax({
        url: "get-requests",
        dataType: "json",
        error: function (request, status) {
            alert(request.responseText.clear());
        },
        success: function (requests) {
            $("#main").empty();
            var table = $("<table class='table table-bordered table-striped' />");
            table.append($("<tr />")
                .append($("<th>Номер</th>"))
                .append($("<th>Дата</th>"))
                .append($("<th>Время</th>"))
                .append($("<th>Услуга</th>"))
                .append($("<th>Состояние</th>"))
                .append($("<th />")));
            for (var i = 0; i < requests.length; i++) {
                var request = requests[i];
                var row = $("<tr />")
                    .css("background-color", request.Color)
                    .append($("<td style='text-align:right;' />").html(request.Number));
                var requestDate = Date.parseExact(request.RequestDate);
                row.append($("<td />").html(requestDate.toString("dd.MM.yyyy")));
                var requestTime = TimeSpan.parseExact(request.RequestTime);
                row.append($("<td />").html(requestTime.toString()))
                    .append($("<td />").html(request.Service.Code + " " + request.Service.Name))
                    .append($("<td />").html(TRANSLATION.ClientRequestState[request.State]));
                var button = $("<a class='btn' request-id='" + request.Id + "'>Отменить</a>");
                if (request.IsClosed) {
                    button.addClass("disabled");
                } else {
                    button.bind("click", function () {
                        var requestId = $(this).attr('request-id');
                        $.ajax({
                            url: "cancel-request",
                            data: { requestId: requestId },
                            dataType: "json",
                            error: function (request, status) {
                                alert(request.responseText.clear());
                            },
                            success: function (client) {
                                $("#client-menu .requests").trigger("click");
                            }
                        });
                    });
                }
                row.append($("<td />").append(button));
                table.append(row);
            }
            $("#main").append(table);
        }
    });
});

$("#client-menu .profile").click(function () {
    $("#edit-profile-modal").modal("show");
});

$("#client-menu .logout").click(function () {
    $.removeCookie("SessionId");
    document.location.reload();
});
//#endregion

//#region carousels
$("#client-panel-carousel").carousel({
    interval: 0,
}).addClass("slide");

$("#restore-password-carousel").carousel({
    interval: 0,
}).addClass("slide");

$("#register-carousel").carousel({
    interval: 0
}).addClass("slide");

$("#add-request-carousel").carousel({
    interval: 0
}).addClass("slide");
//#endregion

//#region modals events
$("#register-modal").bind("show", function () {
    $("#register-carousel").carousel(0);
});

$("#add-request-modal").bind("show", function () {
    $("#add-request-carousel").carousel(0);
});

$("#add-request-modal").bind("hide", function () {
    $('#client-menu .requests').trigger('click');
});

$("#edit-profile-modal").bind("show", function () {
    $("#edit-profile .surname").val(globals.Client.Surname);
    $("#edit-profile .name").val(globals.Client.Name);
    $("#edit-profile .patronymic").val(globals.Client.Patronymic);
    $("#edit-profile .mobile").val(globals.Client.Mobile);
});

//#endregion

//#region buttons

$("#top-login .submit").click(function () {
    var email = $("#top-login .email").val();
    var password = $("#top-login .password").val();
    if (email.length > 0) {
        $.ajax({
            url: "login",
            data: { email: email, password: password },
            dataType: "json",
            error: function (request, status) {
                alert(request.responseText.clear());
            },
            success: function (client) {
                globals.Client = client;
                $(document).trigger("client-login");
            }
        });
    }
});

$("#top-login .restore-password").click(function () {
    var email = $("#top-login .email").val();
    if (email.length > 0) {
        $.ajax({
            url: "restore-password",
            data: { email: email },
            dataType: "json",
            error: function (request, status) {
                alert(request.responseText.clear());
            },
            success: function (client) {
                alert("Пароль отправлен на указанный электронный адрес");
            }
        });
    }
});

$("#login .submit").click(function () {
    var email = $("#login .email").val();
    var password = $("#login .password").val();
    if (email.length > 0) {
        $.ajax({
            url: "login",
            data: { email: email, password: password },
            dataType: "json",
            error: function (request, status) {
                alert(request.responseText.clear());
            },
            success: function (client) {
                globals.Client = client;
                $(document).trigger("client-login");
                $("#register-modal").modal("hide");
                $("#add-request-modal").modal("show");
            }
        });
    }
});

$("#login .restore-password").click(function () {
    var email = $("#login .email").val();
    if (email.length > 0) {
        $.ajax({
            url: "restore-password",
            data: { email: email },
            dataType: "json",
            error: function (request, status) {
                alert(request.responseText.clear());
            },
            success: function (client) {
                alert("Пароль отправлен на указанный электронный адрес");
            }
        });
    }
});

$("#edit-profile .submit").click(function () {
    var surname = $("#edit-profile .surname").val();
    var name = $("#edit-profile .name").val();
    var patronymic = $("#edit-profile .patronymic").val();
    var mobile = $("#edit-profile .mobile").val();
    $.ajax({
        url: "edit-profile",
        data: { surname: surname, name: name, patronymic: patronymic, mobile: mobile },
        dataType: "json",
        error: function (request, status) {
            alert(request.responseText.clear());
        },
        success: function (client) {
            globals.Client = client;
            $("#edit-profile-modal").modal("hide");
        }
    });
});

$("#send-pin-to-email .submit").click(function () {
    var email = $("#send-pin-to-email .email").val();
    if (email.length > 0) {
        $.ajax({
            url: "send-pin-to-email",
            data: { email: email },
            dataType: "json",
            error: function (request, status) {
                alert(request.responseText.clear());
            },
            success: function () {
                $("#register-carousel").carousel("next");
            }
        });
    }
});

$("#check-pin .submit").click(function () {
    var PIN = $("#check-pin .pin").val();
    if (PIN.length > 0) {
        var email = $("#send-pin-to-email .email").val();
        $.ajax({
            url: "check-pin",
            data: { email: email, PIN: PIN },
            dataType: "json",
            error: function (request, status) {
                alert(request.responseText.clear());
            },
            success: function () {
                $("#register-carousel").carousel("next");
            }
        });
    } else {
        alert("Введите PIN-код");
    }
});

$("#register .submit").click(function () {
    var email = $("#send-pin-to-email .email").val();
    var PIN = $("#check-pin .pin").val();
    var surname = $("#register .surname").val();
    var name = $("#register .name").val();
    var patronymic = $("#register .patronymic").val();
    var mobile = $("#register .mobile").val();
    $.ajax({
        url: "register",
        data: { email: email, PIN: PIN, surname: surname, name: name, patronymic: patronymic, mobile: mobile },
        dataType: "json",
        error: function (request, status) {
            alert(request.responseText.clear());
        },
        success: function (client) {
            globals.Client = client;
            $(document).trigger("client-login");
            $("#register-modal").modal("hide");
            $('#add-request-modal').modal("show");
        }
    });
});

$("#add-request .submit").click(function () {
    var requestDate = globals.RequestDate.toString("dd.MM.yyyy");
    var requestTime = globals.RequestDate.toString("HH:mm:ss");
    $.ajax({
        url: "add-request",
        data: { serviceId: globals.Service.Id, requestDate: requestDate, requestTime: requestTime, subjects: 1 },
        dataType: "json",
        error: function (request, status) {
            alert(request.responseText.clear());
        },
        success: function (request) {
            $("#coupon .link").attr("href", "request/" + request.Id + "/coupon");
            $("#add-request-carousel").carousel("next");
        }
    });
});
//#endregion

//#region search
$('#search').typeahead({
    minLength: 5,
    source: function (query, callback) {
        var map = {};
        $.ajax({
            url: "/find-services",
            data: { query: query },
            dataType: "json",
            global: false,
            success: function (services) {
                var source = [];
                $.each(services, function (i, service) {
                    map[service.Name] = service;
                    source.push(service.Name);
                });
                callback(source);
            }
        });

        this.updater = function (item) {
            loadService(map[item]);
        };
    }
});
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

var sessionId = $.cookie("SessionId");

var matches = new RegExp("SessionId\=([0-9a-zA-Z\-]+)", "g").exec(document.location.toString());
if (matches != null) {
    sessionId = matches[1];
}

if (sessionId != undefined) {
    globals.SessionId = sessionId;
    $.ajax({
        url: "get-profile",
        dataType: "json",
        error: function (request, status) {
            alert(request.responseText.clear());
            $.removeCookie("SessionId");
        },
        success: function (client) {
            globals.Client = client;
            $(document).trigger("client-login");
        }
    });
}

//#region functions
function loadServiceGroup(serviceGroup) {
    $.ajax({
        url: serviceGroup != undefined ? "service-group/" + serviceGroup.Id + "/child-groups" : "root-service-groups",
        dataType: "json",
        serviceGroup: serviceGroup,
        success: function (serviceGroups) {
            $("#main").empty();

            var breadcrumb = $("<ul class='breadcrumb' />");
            $("<li />").append(
                $("<a href='javascript:;'/>").html("Каталог услуг")
                .bind("click", function () {
                    loadServiceGroup();
                })).appendTo(breadcrumb);
            if (this.serviceGroup != undefined) {
                var parentServiceGroup = this.serviceGroup.Parent;
                while (parentServiceGroup != undefined) {
                    breadcrumb.children().last().append($("<span class='divider'>&rarr;</span>"));
                    $("<li />").append(
                        $("<a href='javascript:;' />").data("service-group", parentServiceGroup).html(parentServiceGroup.Name)
                        .bind("click", function () {
                            loadServiceGroup($(this).data("service-group"));
                        })).appendTo(breadcrumb);
                    parentServiceGroup = parentServiceGroup.Parent;
                };

                breadcrumb.children().last().append($("<span class='divider'>&rarr;</span>"));
                $("<li />").html(this.serviceGroup.Name).appendTo(breadcrumb);
                breadcrumb.appendTo("#main");

                if (!String.nullOrEmpty(this.serviceGroup.Description)) {
                    $("#main").append(this.serviceGroup.Description);
                    $("<hr/>").appendTo("#main");
                }
            }

            var grid = $("<div class='row' />");
            for (var i = 0; i < serviceGroups.length; i++) {
                var serviceGroup = serviceGroups[i];

                if (!serviceGroup.IsActive) {
                    continue;
                }

                var button = $("<div class='span6 service-group' />")
                    .append($("<button class='btn btn-large btn-block' />")
                        .append($("<table />")
                            .append($("<tr />")
                                .append($("<td />").addClass("code").css("background-color", serviceGroup.Color).html(serviceGroup.Code))
                                .append($("<td />").addClass("name").html(serviceGroup.Name.fixLength()))))
                        .data("service-group", serviceGroup)
                        .attr("title", serviceGroup.Name)
                        .bind("click", function () { loadServiceGroup($(this).data("service-group")); }));
                if (serviceGroup.Comment) {
                    button.append($("<div class='comment' />").html(serviceGroup.Comment));
                }
                grid.append(button);
            }
            $("#main").append(grid);

            $.ajax({
                url: this.serviceGroup != undefined ? "service-group/" + this.serviceGroup.Id + "/services" : "root-services",
                dataType: "json",
                success: function (services) {
                    var grid = $("<div class='row' />");
                    for (var i = 0; i < services.length; i++) {
                        var service = services[i];

                        if (!service.IsActive) {
                            continue;
                        }

                        var button = $("<div class='span6 service-group' />")
                        .append($("<button class='btn btn-large btn-block' />")
                            .append($("<table />")
                                .append($("<tr />")
                                    .append($("<td />").addClass("code").css("background-color", service.ServiceGroup != undefined ? service.ServiceGroup.Color : "default").html(service.Code))
                                    .append($("<td />").addClass("name").html(service.Name.fixLength()))))
                            .attr("title", service.Name)
                            .prop("service", service)
                            .bind("click", function () {
                                loadService($(this).prop("service"));
                            }));
                        grid.append(button);
                    }
                    $("#main").append(grid);
                }
            });
        }
    });
}

var events = [];
function loadService(service) {
    $("#main").empty();

    var breadcrumb = $("<ul class='breadcrumb' />");
    $("<li />").append(
    $("<a href='javascript:;'/>").html("Каталог услуг")
    .bind("click", function () {
        loadServiceGroup();
    })).appendTo(breadcrumb);
    var serviceGroup = service.ServiceGroup;
    while (serviceGroup != undefined) {
        breadcrumb.children().last().append($("<span class='divider'>&rarr;</span>"));
        $("<li />").append(
            $("<a href='javascript:;'/>").data("service-group", serviceGroup).html(serviceGroup.Name)
            .bind("click", function () {
                loadServiceGroup($(this).data("service-group"));
            })).appendTo(breadcrumb);
        serviceGroup = serviceGroup.Parent;
    };

    $("<li class='service' style='display:block;' />")
        .append($('<p></p>').html(service.Name)).appendTo(breadcrumb);
    breadcrumb.appendTo("#main");

    //load service information

    var information = $('<div></div>');

    if (!String.nullOrEmpty(service.Comment)) {
        $("<p></p>").addClass("comment").html(service.Comment)
            .appendTo(information);
    }

    if (!String.nullOrEmpty(service.Description)) {
        information.append(service.Description);
    }

    if (!String.nullOrEmpty(service.Link)) {
        $("<a class='btn btn-large btn-primary' type='button' target='_blank'>Подробная информация <i class='icon-share-alt icon-white'></i></a>")
            .attr('href', service.Link).appendTo(information);
    }

    if (!String.nullOrEmpty(information.html())) {
        information.appendTo("#main");
        $("<hr/>").appendTo("#main");
    }

    //load metrics

    $("<p>Мы онлайн:</p>").appendTo("#main");

    $("<div class='row'></div>")
        .append($("<div id='queue-metric1' class='queue-metric span6'></div>"))
        .append($("<div id='queue-metric2' class='queue-metric span6'></div>"))
        .appendTo("#main");

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

    function getQueueServiceMetric(hour) {
        $.ajax({
            url: "/get-queue-plan-service-metric",
            dataType: "json",
            global: false,
            hour: hour,
            data: { serviceId: service.Id, year: today.getFullYear(), month: today.getMonth() + 1, day: today.getDate(), hour: hour },
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
            }
        }).always(function () {
            if (this.hour <= workFinishTime.hours) {
                getQueueServiceMetric(this.hour + 1);
            }
        });
    }

    getQueueServiceMetric(workStartTime.hours);

    globals.Service = service;

    //load free time

    var workStartTime = TimeSpan.parseExact(globals.DefaultConfig.WorkStartTime);
    var workFinishTime = TimeSpan.parseExact(globals.DefaultConfig.WorkFinishTime);

    var calendar = $("<div></div>");
    $("#main").append(calendar);
    calendar.fullCalendar({
        defaultView: "agendaWeek",
        header: {
            left: "title",
            center: "",
            right: "prev,next"
        },
        firstDay: 1,
        allDaySlot: true,
        allDayText: "-",
        slotMinutes: 10,
        minTime: workStartTime.hours + ":" + workStartTime.minutes,
        maxTime: workFinishTime.hours + ":" + workFinishTime.minutes,
        timeFormat: "H(:mm)",
        axisFormat: "H(:mm)",
        contentHeight: 600,
        eventBackgroundColor: "#009900",
        columnFormat: {
            month: "ddd",
            week: "dddd / d",
            day: "dddd M/d"
        },
        titleFormat: {
            month: "MMMM yyyy",
            week: "Расписание на MMMM d[ yyyy]{ '&#8212;'[ MMMM] d / yyyy}",
            day: "dddd, MMM d, yyyy"
        },
        monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
        dayNames: ["Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота"],
        eventMouseover: function (event) {
            if (!event.allDay) {
                $(this).css("cursor", "pointer").css("text-decoration", "underline");
            }
        },
        eventMouseout: function (event) {
            if (!event.allDay) {
                $(this).css("text-decoration", "none");
            }
        },
        eventClick: function (event) {
            globals.RequestDate = event.start;
            if (!event.allDay) {
                if (globals.Client != undefined) {
                    $("#add-request-modal").modal("show");
                } else {
                    $("#register-modal").modal("show");
                }
            }
        },
        events: function (start, end, callback) {
            events = [];

            if (parseInt(service.EarlyRegistrator) & clientRequestRegistrator.portal) {
                var index = 0;
                var dayLoaded = function () {
                    if (--index == 0) {
                        $("#loading").hide();
                        callback(events);
                    }
                }

                for (var date = start; date < end; date.setDate(date.getDate() + 1)) {
                    if (date >= Date.today()) {
                        $.ajax({
                            url: "queue-plan/" + date.toString("dd.MM.yyyy") + "/1/service/" + service.Id + "/free-time",
                            dataType: "json",
                            global: false,
                            date: date.toString(),
                            error: function (request, status) {
                                var start = new Date(this.date);
                                events.push({
                                    title: request.responseText.clear(),
                                    start: start,
                                    allDay: true,
                                    backgroundColor: "#8A0000"
                                });
                            },
                            success: function (freeTime) {
                                var intervals = freeTime.TimeIntervals;

                                if (intervals.length > 0) {
                                    var start = new Date(this.date);
                                    events.push({
                                        title: "Доступно " + freeTime.FreeTimeIntervals + " талонов",
                                        start: start,
                                        allDay: true
                                    });

                                    var map = [];

                                    for (var i = 0; i < intervals.length; i++) {
                                        var interval = intervals[i];
                                        if (map[interval] != undefined) {
                                            continue;
                                        }

                                        var start = new Date(this.date);
                                        var timeSpan = TimeSpan.parseExact(interval);
                                        start.setHours(timeSpan.hours);
                                        start.setMinutes(timeSpan.minutes);

                                        var end = new Date(start);
                                        var timeSpan = TimeSpan.parseExact(freeTime.Schedule.EarlyClientInterval);
                                        end.setHours(start.getHours() + timeSpan.hours);
                                        end.setMinutes(start.getMinutes() + timeSpan.minutes);

                                        events.push({
                                            title: "Доступно",
                                            start: start,
                                            end: end,
                                            allDay: false
                                        });

                                        map[interval] = true;
                                    }
                                } else {
                                    var start = new Date(this.date);
                                    events.push({
                                        title: "Нет свободного времени",
                                        start: start,
                                        allDay: true,
                                        backgroundColor: "#8A0000"
                                    });
                                }
                            }
                        }).always(dayLoaded);
                        index++;
                    }
                }

                if (index > 0) {
                    $("#loading").show();
                }
            } else {
                events.push({
                    title: "Предварительная запись для данной услуги на портале отключена",
                    start: start,
                    end: end,
                    allDay: true,
                    backgroundColor: "#8A0000"
                });
                callback(events);
            }
        }
    });
}
//#endregion

$(document).on("portalConfigLoaded", function () {
    loadServiceGroup();
});