
//#region TimeSpan
function TimeSpan() {
    this.days = 0;
    this.hours = 0;
    this.minutes = 0;
    this.seconds = 0;
}
TimeSpan.parseExact = function (source) {
    var result = new TimeSpan();
    var match = /([\.\d]+)D/.exec(source);
    if (match != null) {
        result.day = new Number(match[1]);
    }
    match = /([\.\d]+)H/.exec(source);
    if (match != null) {
        result.hours = new Number(match[1]);
    }
    match = /([\.\d]+)M/.exec(source);
    if (match != null) {
        result.minutes = new Number(match[1]);
    }
    match = /([\.\d]+)S/.exec(source);
    if (match != null) {
        result.seconds = new Number(match[1]);
    }

    return result;
};
TimeSpan.prototype.toString = function () {
    return ("{0:n(2)}").format(this.hours) + ":" + ("{0:n(2)}").format(this.minutes);
};
//#endregion

//#region extensions
String.prototype.fixLength = function () {
    return this.length < MAX_TEXT_LENGTH ? this : this.substring(0, MAX_TEXT_LENGTH) + "...";
}

String.nullOrEmpty = function (target) {
    return target == null || target == "";
}

String.prototype.clear = function () {
    return this.slice(1, -1);
}

Date.parseExact = function (source) {
    var match = /Date\((\d+)+\+(\d+)+\)/.exec(source);
    if (match != null) {
        var result = new Date(new Number(match[1]));
        return result;
    }
};
//#endregion