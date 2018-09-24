$.fn.flashMessage = function (options) {
    var target = this;
    if (options!=null && typeof options !='undefined' &&  null !== options.class && typeof options.class != 'undefined') {
        options = $.extend({}, options, { timeout: 9000, alert: options.class });
    } else {
            options = $.extend({}, options, { timeout: 9000, alert: 'info' });
    }

    if (!options.message) {
        setFlashMessageFromCookie(options);
    }

    if (options.message) {
        $(target).addClass(options.alert.toString().toLowerCase());

        if (typeof options.message === "string") {
            $('p', target).html("<span style='word-wrap: break-word;'>" + decodeURI(options.message).replace(/\+/g, " ") + "</span>");
        } else {

            
            target.empty().append(decodeURI(options.message).toString().replace(/\+/g, " "));

        }
    } else {
        return;
    }

    if (target.children().length === 0) return;

    target.fadeIn().one("click", function () {
        $(this).fadeOut();
    });

    if (options.timeout > 0) {
        setTimeout(function () { target.fadeOut(); }, options.timeout);
    }

    return this;

    // Get the first alert message read from the cookie
    function setFlashMessageFromCookie() {
        $.each(new Array('Success', 'Error', 'Warning', 'Info'), function (i, alert) {
            var cookie = $.cookie("Flash." + alert);

            if (cookie) {
                options.message = cookie;
                options.alert = alert;

                deleteFlashMessageCookie(alert);
                return;
            }
        });
    }

    // Delete the named flash cookie
    function deleteFlashMessageCookie(alert) {
        $.cookie("Flash." + alert, null, { path: '/' });
    }
};