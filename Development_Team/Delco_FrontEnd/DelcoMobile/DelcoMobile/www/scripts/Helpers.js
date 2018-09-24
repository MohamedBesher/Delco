/// <reference path="../js/angular.js" />
/// <reference path="../js/framework7.js" />


myApp.angular.factory('helpers', ['$rootScope', '$exceptionHandler', 'CookieService', '$interval', function ($rootScope, $exceptionHandler, CookieService, $interval) {
    'use strict';

    var notificationOpenedCallback;
    var timeinterval;
    var deadline = new Date(Date.parse(new Date()) + 3 * 60 * 1000);

    function getTimeRemaining(endtime) {
        var t = Date.parse(endtime) - Date.parse(new Date());
        var seconds = Math.floor((t / 1000) % 60);
        var minutes = Math.floor((t / 1000 / 60) % 60);
        return {
            'total': t,
            'minutes': minutes,
            'seconds': seconds
        };
    }

    function initializeClock(id, endtime, callBack) {
        var clock = document.getElementById(id);
        var minutesSpan = clock.querySelector('.minutes');
        var secondsSpan = clock.querySelector('.seconds');

        function updateClock() {
            var t = getTimeRemaining(endtime);

            minutesSpan.innerHTML = ('0' + t.minutes).slice(-2);
            secondsSpan.innerHTML = ('0' + t.seconds).slice(-2);

            if (t.total <= 0) {
                ClearTimer(false);
                callBack(true);
            }
        }

        updateClock();
        timeinterval = setInterval(updateClock, 1000);
    }

    var arabictonum = function arabictonum(arabicnumstr) {
        try {
            var num = 0;
            var c;
            for (var i = 0; i < arabicnumstr.length; i++) {
                c = arabicnumstr.charCodeAt(i);
                num += c - 1632;
                num *= 10;
            }
            return num / 10;
        }
        catch (exception) {
            return null;
            $exceptionHandler("An Error has occurred in Method: [Helpers.arabictonum()] , Error: [" + exception.name + "] - Message: [" + exception.message + "].");
        }
    }

    var parseArabic = function parseArabic(str) {
        try {
            yas = str;
            yas = yas.replace(/[٠١٢٣٤٥٦٧٨٩]/g, function (d) {
                return d.charCodeAt(0) - 1632;
            }).replace(/[۰۱۲۳۴۵۶۷۸۹]/g, function (d) {
                return d.charCodeAt(0) - 1776;
            });

            str = yas;

            return str;
        }
        catch (exception) {
            return null;
            $exceptionHandler("An Error has occurred in Method: [Helpers.parseArabic()] , Error: [" + exception.name + "] - Message: [" + exception.message + "].");
        }
    }

    var groupBy = function groupBy(collection, property) {
        try {
            var i = 0, val, index,
                values = [], result = [];
            for (; i < collection.length; i++) {
                val = collection[i][property];
                index = values.indexOf(val);
                if (index > -1)
                    result[index].push(collection[i]);
                else {
                    values.push(val);
                    result.push([collection[i]]);
                }
            }
            return result;
        }
        catch (exception) {
            return null;
            $exceptionHandler("An Error has occurred in Method: [Helpers.groupBy()] , Error: [" + exception.name + "] - Message: [" + exception.message + "].");
        }
    }

    var ClearBodyAfterGoogleMap = function ClearBodyAfterGoogleMap() {
        try {
            $('span').each(function () {
                var span = $(this);
                if ($(this).text() === 'BESbewy') {
                    $(this).remove();
                }
            });

            $('div').each(function () {
                var div = $(this);
                if ($(this).hasClass('pac-container pac-logo hdpi') && $(this).children().length == 0) {
                    $(this).remove();
                }
            });
        }
        catch (exception) {
            return null;
            $exceptionHandler("An Error has occurred in Method: [Helpers.ClearBodyAfterGoogleMap()] , Error: [" + exception.name + "] - Message: [" + exception.message + "].");
        }
    }

    var GetCurrentDateTime = function GetCurrentDateTime(date) {
        try {
            var currentdate = new Date();
            if (date == '') {
                currentdate = new Date();
            }
            else {
                currentdate = date;
            }

            var month = parseInt((currentdate.getMonth() + 1));
            var day = parseInt(currentdate.getDate());
            var datetime;

            if (month < 10) {
                if (day < 10) {
                    datetime = currentdate.getFullYear() + "-0" + (currentdate.getMonth() + 1) + "-0" + currentdate.getDate();
                    //+ " T"+ currentdate.getHours() + ":" + currentdate.getMinutes() + ":" + currentdate.getSeconds();
                }
                else {
                    datetime = currentdate.getFullYear() + "-0" + (currentdate.getMonth() + 1) + "-" + currentdate.getDate();
                    //+ " T" + currentdate.getHours() + ":" + currentdate.getMinutes() + ":" + currentdate.getSeconds();
                }
            }
            else {
                if (day < 10) {
                    datetime = currentdate.getFullYear() + "-" + (currentdate.getMonth() + 1) + "-0" + currentdate.getDate();
                    //+ " T" + currentdate.getHours() + ":" + currentdate.getMinutes() + ":" + currentdate.getSeconds();
                }
                else {
                    datetime = currentdate.getFullYear() + "-" + (currentdate.getMonth() + 1) + "-" + currentdate.getDate();
                    //+ " T" + currentdate.getHours() + ":" + currentdate.getMinutes() + ":" + currentdate.getSeconds();
                }
            }

            return datetime;
        }
        catch (exception) {
            return null;
            $exceptionHandler("An Error has occurred in Method: [Helpers.GetCurrentDateTime()] , Error: [" + exception.name + "] - Message: [" + exception.message + "].");
        }
    }

    var ShowDatePicker = function ShowDatePicker() {
        try {
            var doctorBirthDate = document.getElementById('doctorBirthDate');
            doctorBirthDate.onclick = function () {
                var options = {
                    date: new Date(),
                    mode: 'date'
                };

                function onSuccess(date) {
                    doctorBirthDate.value = GetCurrentDateTime(date);
                }

                function onError(error) { // Android only

                }

                datePicker.show(options, onSuccess, onError);
            }
        }
        catch (exception) {
            return null;
            $exceptionHandler("An Error has occurred in Method: [Helpers.ShowDatePicker()] , Error: [" + exception.name + "] - Message: [" + exception.message + "].");
        }
    }

    var geocodeLatLng = function geocodeLatLng(Lat, Lang, callBack) {
        try {
            var latlng = { lat: parseFloat(Lat), lng: parseFloat(Lang) };
            var geocoder = new google.maps.Geocoder;

            geocoder.geocode({ 'location': latlng }, function (results, status) {
                if (status === google.maps.GeocoderStatus.OK) {
                    if (results[0]) {
                        var result = results[0].formatted_address;
                        callBack(result);
                    } else {
                        callBack('');
                    }
                } else {
                    callBack('');
                }
            });
        }
        catch (exception) {
            return null;
            $exceptionHandler("An Error has occurred in Method: [Helpers.geocodeLatLng()] , Error: [" + exception.name + "] - Message: [" + exception.message + "].");
        }
    }

    var InitMapSearchBox = function InitMapSearchBox(map, markers, selectedAddress) {
        try {

            var fw7 = myApp.fw7;
            var app = myApp.fw7.app;

            if (selectedAddress != '') {
                $('#pac-input').val(selectedAddress);
            }

            var geocoder = new google.maps.Geocoder();
            if (geocoder) {
                geocoder.geocode({
                    'address': selectedAddress
                }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (status != google.maps.GeocoderStatus.ZERO_RESULTS) {
                            map.setCenter(results[0].geometry.location);

                            markers = [];

                            // Create a marker for each place.
                            markers.push(new google.maps.Marker({
                                map: map,
                                title: results[0].formatted_address,
                                position: results[0].geometry.location
                            }));


                            var lat = results[0].geometry.location.lat();
                            var lang = results[0].geometry.location.lng();

                            CookieService.setCookie('FacilityAddressLatitude', lat);
                            CookieService.setCookie('FacilityAddressLongtitude', lang);
                            CookieService.setCookie('FacilityAddress', $('#pac-input').val());

                            var infoWindow = new google.maps.InfoWindow();

                            for (var i = 0; i < markers.length; i++) {
                                var data = markers[i];
                                var myLatlng = new google.maps.LatLng(data.position.lat(), data.position.lng());
                                var marker = new google.maps.Marker({
                                    position: myLatlng,
                                    map: map,
                                    title: data.title
                                });

                                (function (marker, data) {
                                    google.maps.event.addListener(marker, "click", function (e) {
                                        infoWindow.setContent("<div style = 'width:200px;min-height:40px'>" + data.title + "</div>");
                                        infoWindow.open(map, marker);
                                    });
                                })(marker, data);
                            }

                        }
                        else {
                            language.openFrameworkModal('خطأ', 'لا توجد نتائج', 'alert', function () { });
                        }
                    } else {
                        language.openFrameworkModal('خطأ', 'خطأ في عملية أسترجاع العنوان', 'alert', function () { });
                    }
                });
            }
        }
        catch (exception) {
            return null;
            $exceptionHandler("An Error has occurred in Method: [Helpers.InitMapSearchBox()] , Error: [" + exception.name + "] - Message: [" + exception.message + "].");
        }
    }

    var initMap = function initMap(mapId, markers, fromPage, selectedAddress, callback) {
        try {
            var mapDiv;
            var flightPlanCoordinates = [];
            var isDraggable = false;

            mapDiv = document.getElementById(mapId);

            var map = new google.maps.Map(mapDiv, {
                center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
                //zoom: 10,
                //mapTypeId: 'terrain',
                zoom: 15,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                'draggable': false
            });

            if (markers.length > 0) {
                map = new google.maps.Map(mapDiv, {
                    center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
                    //zoom: 10,
                    //mapTypeId: 'terrain',
                    zoom: 15,
                    mapTypeId: google.maps.MapTypeId.ROADMAP,
                    'draggable': false
                });

                var infoWindow = new google.maps.InfoWindow();

                for (var i = 0; i < markers.length; i++) {
                    var data = markers[i];
                    var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                    var marker = new google.maps.Marker({
                        position: myLatlng,
                        map: map,
                        title: data.title
                    });
                }
            }

            google.maps.event.addListener(map, 'click', function (event) {
                $rootScope.mapClick = true;
                $rootScope.latLng = null;
                $rootScope.latLng = event.latLng;
                isDraggable = !isDraggable;
                this.setOptions({ 'draggable': isDraggable });
                GoBack();
            });

            callback(map);
        }
        catch (exception) {
            callback(null);
            $exceptionHandler("An Error has occurred in Method: [Helpers.initMap()] , Error: [" + exception.name + "] - Message: [" + exception.message + "].");
        }
    }

    var SetUserInLocalStorage = function SetUserInLocalStorage(usrName, usrPass, usrId) {
        CookieService.setCookie('USName', usrName);
        CookieService.setCookie('UPass', usrPass);
        CookieService.setCookie('UserId', usrId);
    }

    var ShowLoader = function ShowLoader(pageName) {
        //var divPage = document.getElementById(pageName + 'Page');
        //var divLoader = document.createElement('div');
        //var hdrWait = document.createElement('h3');
        //var loadImage = document.createElement('img');

        //divLoader.className += 'loader divLoader';
        //loadImage.src = 'img/load.svg';

        //var lang = CookieService.getCookie('lang');

        //if (lang == 'AR' || lang == null || typeof lang == 'undefined') {
        //    hdrWait.innerHTML = 'برجاء الإنتظار';
        //}
        //else {
        //    hdrWait.innerHTML = 'Please wait';
        //}

        //divLoader.appendChild(loadImage);
        //divLoader.appendChild(hdrWait);
        //divPage.appendChild(divLoader);
        //$(".loader").fadeIn("slow");
        var options = { dimBackground: true };
        SpinnerPlugin.activityStart("جار التحميل", options);
    }

    var HideLoader = function HideLoader() {
        //$(".loader").fadeOut("slow");
        //$('div').each(function () {
        //    var div = $(this);
        //    if ($(this).hasClass('loader divLoader')) {
        //        $(this).remove();
        //    }
        //});
        SpinnerPlugin.activityStop();
    }

    var GoToPage = function GoToPage(pageName, queryVariables) {
        var fw7 = myApp.fw7;
        $rootScope.currentOpeningPage = pageName;

        if (typeof queryVariables != 'undefined' && queryVariables != null && queryVariables != '' && queryVariables != ' ') {
            fw7.views[0].router.loadPage({ pageName: pageName, query: queryVariables });
        }
        else {
            fw7.views[0].router.loadPage({ pageName: pageName });
        }
    }

    var GoBack = function GoBack() {
        var fw7 = myApp.fw7;

        if (fw7.views[0].history.length > 2) {
            var previousPageInHistory = fw7.views[0].history[fw7.views[0].history.length - 2];
            var previousPage = previousPageInHistory.substr(1, previousPageInHistory.length - 1);
            $rootScope.currentOpeningPage = previousPage;
        }

        fw7.views[0].router.back();
    }

    var RegisterDevice = function RegisterDevice(callBack) {
        SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
        if (!CookieService.getCookie('deviceId')) {
            notificationOpenedCallback = function (jsonData) {
                var currentPage = $rootScope.currentOpeningPage;
                var result = JSON.stringify(jsonData);
            };

            //window.plugins.OneSignal.setLogLevel(window.plugins.OneSignal.LOG_LEVEL.DEBUG, window.plugins.OneSignal.LOG_LEVEL.DEBUG);

            window.plugins.OneSignal
              .startInit("4557f7f1-fc86-4147-b736-6233506c23b9")
              .handleNotificationOpened(notificationOpenedCallback)
              .inFocusDisplaying(window.plugins.OneSignal.OSInFocusDisplayOption.Notification)
              .endInit();

            window.plugins.OneSignal.getIds(function (ids) {
                console.log('getIds: ' + JSON.stringify(ids));
                CookieService.setCookie('deviceId', ids.userId);
                callBack(ids.userId);
            });

            window.plugins.OneSignal.enableInAppAlertNotification(false);
            window.plugins.OneSignal.enableNotificationsWhenActive(false);
        }
        else {

            notificationOpenedCallback = function (jsonData) {
                var currentPage = $rootScope.currentOpeningPage;
                var result = JSON.stringify(jsonData);
                //language.openFrameworkModal('إخطار', 'يوجد لديك إشعار جديد , هل تريد تقفد الإشعار ؟', 'confirm', function () {
                //    helpers.GoToPage('notification');
                //});
            };

            window.plugins.OneSignal
              .startInit("4557f7f1-fc86-4147-b736-6233506c23b9")
              .handleNotificationOpened(notificationOpenedCallback)
              .inFocusDisplaying(window.plugins.OneSignal.OSInFocusDisplayOption.Notification)
              .endInit();

            window.plugins.OneSignal.getIds(function (ids) {
                console.log('getIds: ' + JSON.stringify(ids));
                CookieService.setCookie('deviceId', ids.userId);
                callBack(ids.userId);
            });

            window.plugins.OneSignal.enableInAppAlertNotification(false);
            window.plugins.OneSignal.enableNotificationsWhenActive(false);

            //var deviceId = CookieService.getCookie('deviceId');
            //callBack(deviceId);
        }
    };

    var InitiateTimer = function InitiateTimer(id, callBack) {
        initializeClock(id, deadline, function (IsTimerEnds) {
            callBack(IsTimerEnds);
        });
    }

    var ClearTimer = function ClearTimer(Reset) {
        if (Reset) {
            deadline = new Date(Date.parse(new Date()) + 3 * 60 * 1000);
        }
        clearInterval(timeinterval);
    }

    var resetTimer = function resetTimer() {
        $interval.cancel(messageInterval);
        messageInterval = undefined;
    }

    return {
        arabictonum: arabictonum,
        parseArabic: parseArabic,
        groupBy: groupBy,
        ClearBodyAfterGoogleMap: ClearBodyAfterGoogleMap,
        GetCurrentDateTime: GetCurrentDateTime,
        ShowDatePicker: ShowDatePicker,
        geocodeLatLng: geocodeLatLng,
        InitMapSearchBox: InitMapSearchBox,
        initMap: initMap,
        SetUserInLocalStorage: SetUserInLocalStorage,
        ShowLoader: ShowLoader,
        HideLoader: HideLoader,
        GoToPage: GoToPage,
        GoBack: GoBack,
        RegisterDevice: RegisterDevice,
        InitiateTimer: InitiateTimer,
        ClearTimer: ClearTimer,
        resetTimer: resetTimer
    };
}]);