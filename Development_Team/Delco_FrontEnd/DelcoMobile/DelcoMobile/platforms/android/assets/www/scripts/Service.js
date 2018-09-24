/// <reference path="../js/angular.js" />

(function () {

    myApp.angular.factory('appServices', ['$http', '$rootScope', '$log', 'helpers', 'CookieService', '$exceptionHandler', '$interval', function ($http, $rootScope, $log, helpers, CookieService, $exceptionHandler, $interval) {
        'use strict';

        function getSettings(pageName, CallType, MethodName, dataVariables, callBack) {
            var contentType = "application/json";
            var token = CookieService.getCookie('appToken');
            var deviceId = CookieService.getCookie('deviceId');

            if (typeof deviceId == 'undefined' || deviceId == null || deviceId == '') {
                deviceId = 'e373a27a-92c2-4624-8119-d34cfcc8468d'
            }

            SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
            $http({
                method: CallType,
                url: serviceURL + MethodName,
                //cache: true,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache",
                    "authorization": "bearer " + token
                },
                data: dataVariables,
                async: true,
                crossDomain: true,

            }).then(
        function successCallback(response) {
            if (response.data) {
                var message = response.data.suspendAgentMessage;
                language.openFrameworkModal('خطأ', message, 'alert', function () {
                    SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                    MakeUserOffline('places', 'POST', "api/Account/ChangeStatus/0", "", function (result) { });
                });
            }
            else {
                SpinnerPlugin.activityStop();
            }
        },
        function errorCallback(error) {
            callBack(null);
        });
        }

        function MakeUserOffline(pageName, CallType, MethodName, dataVariables, callBack) {
            var contentType = "application/json";
            var token = CookieService.getCookie('appToken');
            var deviceId = CookieService.getCookie('deviceId');

            if (typeof deviceId == 'undefined' || deviceId == null || deviceId == '') {
                deviceId = 'e373a27a-92c2-4624-8119-d34cfcc8468d'
            }

            SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
            $http({
                method: CallType,
                url: serviceURL + MethodName,
                //cache: true,
                headers: {
                    "content-type": "application/json",
                    "cache-control": "no-cache",
                    "authorization": "bearer " + token
                },
                data: dataVariables,
                async: true,
                crossDomain: true,

            }).then(
        function successCallback(response) {
            SpinnerPlugin.activityStop();
            CookieService.removeCookie('appToken');
            CookieService.removeCookie('USName');
            CookieService.removeCookie('refreshToken');
            CookieService.removeCookie('userLoggedIn');
            CookieService.removeCookie('loginUsingSocial');
            helpers.GoToPage('login', null);
        },
        function errorCallback(error) {
            callBack(null);
        });
        }

        var RefreshToken = function RefreshToken(pageName, CallType, MethodName, callBack) {
            var token = CookieService.getCookie('refreshToken');

            $.ajax({
                method: CallType,
                url: serviceURL + MethodName,
                headers: {
                    "content-type": "application/x-www-form-urlencoded",
                    "cache-control": "no-cache",
                    "postman-token": "a7924ea4-7d97-e2f6-5b56-33cbffb586a7"
                },
                data: { 'refresh_token': token, 'grant_type': 'refresh_token', 'client_id': clientId, 'client_secret': clientSecret },
                async: true,
                crossDomain: true,
                success: function (result) {

                },
                error: function (error, textStatus) {

                }
            }).done(function (result, status, headers) {
                callBack(result);
            }).fail(function (error, textStatus) {
                var responseText = JSON.parse(error.responseText);
                if (error.status === 401) {
                    language.openFrameworkModal('خطأ', 'لا يمكن إعادة تنشيط رمز التحقق الخاص بك لإنتهاء صلاحيته .', 'alert', function () {
                        SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                        MakeUserOffline('places', 'POST', "api/Account/ChangeStatus/0", "", function (result) { });
                    });
                }
                else if (error.status == 500) {
                    language.openFrameworkModal('خطأ', 'خطأ في الخدمة .', 'alert', function () {
                        SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                        MakeUserOffline('places', 'POST', "api/Account/ChangeStatus/0", "", function (result) { });
                    });
                }
                else {
                    getSettings('places', 'POST', "api/Setting/GetSetting", { "PageNumber": 0, "PageSize": 20 }, function (settings) { });
                }
                callBack(null);
            });
        }

        var GetToken = function GetToken(pageName, CallType, MethodName, userName, password, callBack) {
            var contentType = "application/json";

            var options = { dimBackground: true };

            if (!$rootScope.load) {
                $rootScope.load = false;
                //SpinnerPlugin.activityStart("تحميل...", options);
            }
            else {
                $rootScope.load = false;
            }

            $.ajax({
                method: CallType,
                url: serviceURL + MethodName,
                headers: {
                    "content-type": "application/x-www-form-urlencoded",
                    "cache-control": "no-cache",
                    "postman-token": "a7924ea4-7d97-e2f6-5b56-33cbffb586a7"
                },
                data: { 'userName': userName, 'Password': password, 'grant_type': 'password', 'client_id': clientId, 'client_secret': clientSecret, 'Role': 'User' },
                async: true,
                crossDomain: true,
                success: function (result) {

                },
                error: function (error, textStatus) {

                }
            }).done(function (result, status, headers) {
                callBack(result);
            }).fail(function (error, textStatus) {
                //SpinnerPlugin.activityStop();

                helpers.resetTimer();
                if (typeof error.responseText != 'undefined') {
                    var errorResponse = JSON.parse(error.responseText);
                    if (typeof errorResponse.error_description != 'undefined' && errorResponse.error_description == 'You Have No Right To Enter') {
                        language.openFrameworkModal('خطأ', 'حسابك لم يتم الموافقة عليه , لا يمكنك تسجيل الدخول , من فضلك تواصل مع الإدارة .', 'alert', function () { });
                    }
                }

                if (typeof error.responseText != 'undefined') {
                    var responseText = JSON.parse(error.responseText);
                    if (error.status === 401) {
                        language.openFrameworkModal('خطأ', 'مدة صلاحية رمز التحقق الخاص بك قد انتهت , جاري تنشيط رمز التحقق  .', 'alert', function () {
                            RefreshToken(pageName, CallType, 'Token', function (result) {
                                CookieService.setCookie('appToken', result.access_token);
                                CookieService.setCookie('refreshToken', result.refresh_token);
                                language.openFrameworkModal('نجاح', 'تم تنشيط رمز التحقق الخاص بك  .', 'alert', function () {
                                    helpers.GoToPage('index', null);
                                });
                            });
                        });
                    }
                    else if (error.status == 500) {
                        language.openFrameworkModal('خطأ', 'خطأ في الخدمة .', 'alert', function () { });
                    }
                    else {
                        if (typeof responseText.error_description != 'undefined') {
                            var error_description = responseText.error_description;
                            if (error_description === 'The user name or password is incorrect.') {
                                language.openFrameworkModal('خطأ', 'خطأ في اسم الدخول أو كلمة المرور .', 'alert', function () { });
                            }
                            else if (error_description === 'User are Arhieve') {
                                language.openFrameworkModal('خطأ', 'تمت أرشفة حسابك, من فضلك اتصل بإدارة التطبيق .', 'alert', function () { });
                            }
                            else if (error_description === 'SuspendedUser') {
                                getSettings('places', 'POST', "api/Setting/GetSetting", { "PageNumber": 0, "PageSize": 20 }, function (settings) { });
                            }
                            else if (error_description === 'Email Need To Confirm') {
                                language.openFrameworkModal('خطأ', 'حسابك غير مفعل...من فضلك فعل حسابك من خلال إدخال الكود الخاص ببريدك الإلكتروني .', 'alert', function () {
                                    CookieService.setCookie('UserEntersCode', 'false');
                                    helpers.GoToPage('activation', null);
                                });
                            }
                            else if (error_description === 'waiting for acceptance by admin') {
                                language.openFrameworkModal('خطأ', 'يجب تفعيل حسابك أولا من قبل الأدمن', 'alert', function () { });
                            }
                            else {
                                language.openFrameworkModal('خطأ', 'يوجد خطأ في عملية التسجيل .', 'alert', function () { });
                            }
                        }
                        else if (typeof responseText.modelState != 'undefined') {
                            if (typeof responseText.modelState.email != 'undefined') {
                                language.openFrameworkModal('خطأ', 'البريد الإلكتروني مسجل من قبل', 'alert', function () { });
                            }

                            else {
                                var messages = responseText.modelState[""];
                                var viewModelMessage = responseText.modelState["viewModel.DeviceId"];
                                var message = "";
                                if (viewModelMessage) {
                                    language.openFrameworkModal('خطأ', 'خطا اثناء تسجيل الجهاز', 'alert', function () { });
                                }
                                else if (messages.length > 0) {
                                    if (messages[0] === 'Incorrect password.') {
                                        language.openFrameworkModal('خطأ', 'كلمة السر القديمة غير صحيحة .', 'alert', function () { });
                                    }
                                    else if (messages[0] === "Email Must Be Unique") {
                                        language.openFrameworkModal('خطأ', 'البريد الإلكتروني مسجل من قبل', 'alert', function () { });
                                    }
                                    else if (messages[0] === "The Password must be at least 6 characters long.") {
                                        language.openFrameworkModal('خطأ', 'كلمة السر 6 حروف على الاقل', 'alert', function () { });
                                    }
                                    else if (messages[0] === "The password and confirmation password do not match.") {
                                        language.openFrameworkModal('خطأ', 'لا تتطابق كلمة السر مع تأكيد كلمة السر', 'alert', function () { });
                                    }
                                    else if (messages[0].startsWith('Name') && messages[0].endsWith('is already taken.')) {
                                        language.openFrameworkModal('خطأ', 'اسم المستخدم مسجل من قبل', 'alert', function () { });
                                    }
                                    else if (messages[0].startsWith('Phone Number') && messages[0].endsWith('is already taken.')) {
                                        language.openFrameworkModal('خطأ', 'رقم الجوال مسجل من قبل', 'alert', function () { });
                                    }
                                    else if (messages[0].startsWith('User name') && messages[0].endsWith('can only contain letters or digits.')) {
                                        language.openFrameworkModal('خطأ', 'اسم المستخدم يجب ان يحتوي  على حروف  وارقام فقط و يكون باللغه الانجليزية', 'alert', function () { });
                                    }
                                    else if (messages[0].indexOf('Invalid token') > -1) {
                                        language.openFrameworkModal('خطأ', 'كود التفعيل غير صحيح  , برجاء تفقد البريد الالكترونى', 'alert', function () { });
                                    }
                                    else if (messages[0].startsWith('Email') && messages[0].endsWith(' is already related to request.')) {
                                        language.openFrameworkModal('خطأ', 'الايميل مسجل من قبل', 'alert', function () { });
                                    }
                                    else if (messages[0].startsWith('PhoneNumber') && messages[0].endsWith('is already related to User.')) {
                                        language.openFrameworkModal('خطأ', 'رقم الجوال مسجل من قبل', 'alert', function () { });
                                    }

                                    else {
                                        callBack(null);
                                    }
                                }
                                else {
                                    callBack(null);
                                }
                            }
                        }

                        else {
                            callBack(null);
                        }
                    }
                    callBack(null);
                }
                else {
                    callBack(null);
                }
                callBack(null);
            });
        }

        var CallService = function (pageName, CallType, MethodName, dataVariables, callBack) {
            var counter = 0;
            var options = { dimBackground: true };

            if (!$rootScope.load) {
                $rootScope.load = false;
                if ((pageName == 'home' && MethodName.indexOf('api/Country/GetCountries') > -1) || pageName != 'home') {
                }
                else {
                    $rootScope.load = false;
                }
            }

            if (messageInterval) {
                helpers.resetTimer();
            }

            messageInterval = $interval(function () {
                counter++;
                if (counter == 30) {
                    helpers.resetTimer();
                    language.openFrameworkModal('تنبيه', 'الخدمة غير متوفرة الآن بسبب بطء الإتصال بالإنترنت,من فضلك أعد المحاولة مرة أخري .', 'alert', function () {
                        callBack(null);
                    });
                }
            }, 1000);

            var contentType = "application/json";
            var token = CookieService.getCookie('appToken');
            var deviceId = CookieService.getCookie('deviceId');

            
            if (typeof deviceId == 'undefined' || deviceId == null || deviceId == '') {
                deviceId = 'e373a27a-92c2-4624-8119-d34cfcc8468d'
            }

            if (MethodName.indexOf('api/Account/ReSendConfirmationCode') > -1 ||
                MethodName.indexOf('api/Comments/DeleteComment/') > -1) {
                contentType = "application/x-www-form-urlencoded";
            }

            if (dataVariables != '' && dataVariables != null) {
                if (typeof deviceId != 'undefined' && deviceId != null) {
                    dataVariables.deviceId = deviceId;
                }
                dataVariables = JSON.stringify(dataVariables);
            }
            else {
                if (typeof deviceId != 'undefined' && deviceId != null) {
                    dataVariables = { 'deviceId': deviceId };
                }
                dataVariables = JSON.stringify(dataVariables);
            }

            $http({
                method: CallType,
                url: serviceURL + MethodName,
                //cache: true,
                headers: {
                    "content-type": contentType,
                    "cache-control": "no-cache",
                    "authorization": "bearer " + token
                },
                data: dataVariables,
                async: true,
                crossDomain: true,

            }).then(
            function successCallback(response) {
                helpers.resetTimer();
                if (typeof response.status != 'undefined' && response.status == 203) {
                    getSettings('places', 'POST', "api/Setting/GetSetting", { "PageNumber": 0, "PageSize": 20 }, function (settings) { });
                }
                else {
                    callBack(response.data);
                }
            },
            function errorCallback(error) {
                helpers.resetTimer();
                SpinnerPlugin.activityStop();
                if (typeof error.data != 'undefined') {
                    var responseText = error.data;
                    if (error.status === 401) {
                        language.openFrameworkModal('خطأ', 'مدة صلاحية رمز التحقق الخاص بك قد انتهت , جاري تنشيط رمز التحقق  .', 'alert', function () {
                            RefreshToken(pageName, CallType, 'Token', function (result) {
                                CookieService.setCookie('appToken', result.access_token);
                                CookieService.setCookie('refreshToken', result.refresh_token);
                                language.openFrameworkModal('نجاح', 'تم تنشيط رمز التحقق الخاص بك  .', 'alert', function () {
                                    helpers.GoToPage('index', null);
                                });
                            });
                        });
                    }
                    else if (error.status == 400) {
                        if (responseText.message) {
                            language.openFrameworkModal('خطأ', responseText.message, 'alert', function () { });
                        }
                    }
                    else if (error.status == 500) {
                        language.openFrameworkModal('خطأ', 'خطأ في الخدمة .', 'alert', function () { });
                    }
                    else {
                        if (typeof responseText.error_description != 'undefined') {
                            var error_description = responseText.error_description;
                            if (error_description === 'The user name or password is incorrect.') {
                                language.openFrameworkModal('خطأ', 'خطأ في اسم الدخول أو كلمة المرور .', 'alert', function () { });
                            }
                            else if (error_description === '1-User are Arhieve') {
                                language.openFrameworkModal('خطأ', 'تمت أرشفة حسابك, من فضلك اتصل بإدارة التطبيق .', 'alert', function () { });
                            }
                            else if (error_description === '2-Email Need To Confirm') {
                                language.openFrameworkModal('خطأ', 'حسابك غير مفعل...من فضلك فعل حسابك من خلال إدخال الكود الخاص ببريدك الإلكتروني .', 'alert', function () {
                                    CookieService.setCookie('UserEntersCode', 'false');
                                    helpers.GoToPage('activation', null);
                                });
                            }
                            else if (error_description === '3-waiting for acceptance by admin') {
                                language.openFrameworkModal('خطأ', 'يجب تفعيل حسابك أولا من قبل الأدمن', 'alert', function () { });
                            }
                            else {
                                language.openFrameworkModal('خطأ', 'يوجد خطأ في عملية التسجيل .', 'alert', function () { });
                            }
                        }
                        else if (typeof responseText.modelState != 'undefined') {
                            if (typeof responseText.modelState.email != 'undefined') {
                                language.openFrameworkModal('خطأ', 'البريد الإلكتروني مسجل من قبل', 'alert', function () { });
                            }
                            else {
                                var messages = responseText.modelState[""];
                                var viewModelMessage = responseText.modelState["viewModel.DeviceId"];
                                var message = "";
                                if (viewModelMessage) {
                                    language.openFrameworkModal('خطأ', 'خطا اثناء تسجيل الجهاز', 'alert', function () { });
                                }
                                else if (messages.length > 0) {
                                    if (messages[0] === 'Incorrect password.') {
                                        language.openFrameworkModal('خطأ', 'كلمة السر القديمة غير صحيحة .', 'alert', function () { });
                                    }
                                    else if (messages[0] === "Email Must Be Unique") {
                                        language.openFrameworkModal('خطأ', 'البريد الإلكتروني مسجل من قبل', 'alert', function () { });
                                    }
                                    else if (messages[0] === "The Password must be at least 6 characters long.") {
                                        language.openFrameworkModal('خطأ', 'كلمة السر 6 حروف على الاقل', 'alert', function () { });
                                    }
                                    else if (messages[0] === "The password and confirmation password do not match.") {
                                        language.openFrameworkModal('خطأ', 'لا تتطابق كلمة السر مع تأكيد كلمة السر', 'alert', function () { });
                                    }
                                    else if (messages[0].startsWith('Name') && messages[0].endsWith('is already taken.')) {
                                        language.openFrameworkModal('خطأ', 'اسم المستخدم مسجل من قبل', 'alert', function () { });
                                    }
                                    else if (messages[0].startsWith('Phone Number') && messages[0].endsWith('is already taken.')) {
                                        language.openFrameworkModal('خطأ', 'رقم الجوال مسجل من قبل', 'alert', function () { });
                                    }
                                    else if (messages[0].startsWith('User name') && messages[0].endsWith('can only contain letters or digits.')) {
                                        language.openFrameworkModal('خطأ', 'اسم المستخدم يجب ان يحتوي  على حروف  وارقام فقط و يكون باللغه الانجليزية', 'alert', function () { });
                                    }
                                    else if (messages[0].indexOf('Invalid token') > -1) {
                                        language.openFrameworkModal('خطأ', 'كود التفعيل غير صحيح  , برجاء تفقد البريد الالكترونى', 'alert', function () { });
                                    }
                                    else if (messages[0].startsWith('Email') && messages[0].indexOf('is already') > -1) {
                                        language.openFrameworkModal('خطأ', 'الايميل مسجل من قبل', 'alert', function () { });
                                    }
                                    else if (messages[0].startsWith('PhoneNumber') && messages[0].endsWith('is already related to User.')) {
                                        language.openFrameworkModal('خطأ', 'رقم الجوال مسجل من قبل', 'alert', function () { });
                                    }

                                    else {
                                        callBack(null);
                                    }
                                }
                                else {
                                    callBack(null);
                                }
                            }
                        }

                        else {
                            callBack(null);
                        }
                    }
                    callBack(null);
                }
                else {
                    callBack(null);
                }
                callBack(null);
            });
        }

        return {
            CallService: CallService,
            GetToken: GetToken
        }
    }]);

}());

    
