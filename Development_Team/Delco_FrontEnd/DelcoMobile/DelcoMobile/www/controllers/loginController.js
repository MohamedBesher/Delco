/// <reference path="../js/angular.js" />

myApp.angular.controller('loginController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', 'SignalRFactory', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers, SignalRFactory) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;

    $document.ready(function () {
        app.onPageInit('login', function (page) {
            if ($rootScope.currentOpeningPage != 'login') return;
            $rootScope.currentOpeningPage = 'login';
        });

        app.onPageBeforeAnimation('login', function (page) {
            if ($rootScope.currentOpeningPage != 'login') return;
            $rootScope.currentOpeningPage = 'login';
            SpinnerPlugin.activityStop();
            $scope.resetForm();
        });

        app.onPageAfterAnimation('login', function (page) {
            if ($rootScope.currentOpeningPage != 'login') return;
            $rootScope.currentOpeningPage = 'login';
            $scope.resetForm();

        });
        app.init();
    });

    $scope.resetForm = function () {
        $scope.loginReset = false;
        $scope.loginForm.username = null;
        $scope.loginForm.password = null;
        if (typeof $scope.LoginForm != 'undefined' && $scope.LoginForm != null) {
            $scope.LoginForm.$setPristine(true);
            $scope.LoginForm.$setUntouched();
        }
    }

    function RegisterDevice(callBack) {
        //helpers.RegisterDevice(function (result) {
        //    callBack(true);
        //});
        callBack(true);
    }

    function OpenSuspendPopup() {
        var button = document.getElementById('btnLogin');
        app.popover('#popupSuspend', button, false, true, true);
        $$('#popupSuspend').on('opened', function () {

            var params = {
                "PageNumber": 0,
                "PageSize": 20
            };

          SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
            appServices.CallService('places', 'POST', "api/Setting/GetSetting", params, function (settings) {
                SpinnerPlugin.activityStop();
                if (settings) {
                    $scope.suspendMessage = settings.suspendAgentMessage;
                }

                setTimeout(function () {
                    $scope.$apply();
                }, fw7.DelayBeforeScopeApply);
            });

            $scope.GoToContactUs = function () {
                helpers.GoToPage('contact', null);
            };
        });
    }

    $scope.GoToUsers = function () {
        helpers.GoToPage('users', null);
    };

    $scope.GoToForgetPassword = function () {
        helpers.GoToPage('forgetPass', null);
    };
    $scope.form = {};
    $scope.loginForm = {};   
    $scope.submitForm = function (isValid) {
        $scope.loginReset = true;
        if (isValid) {
            var loginEmail = $scope.loginForm.username, loginPass = $scope.loginForm.password;

            loginEmail = loginEmail.trim();

            if ((loginEmail != '' && loginEmail != ' ' && loginEmail != null) && loginPass != null) {
               RegisterDevice(function (result) {
                    appServices.GetToken('login', "POST", "token", loginEmail, loginPass, function (resToken) {
                        if (resToken != null) {
                            CookieService.setCookie('appToken', resToken.access_token);
                            CookieService.setCookie('USName', resToken.userName);
                            CookieService.setCookie('refreshToken', resToken.refresh_token);
                            CookieService.setCookie('Visitor', false);
                            
                            CookieService.setCookie('loginUsingSocial', false);
                            appServices.CallService('login', "POST", "api/Account/GetUserInfo", '', function (res) {
                                SpinnerPlugin.activityStop();
                                if (res != null) {
                                    CookieService.setCookie('IsSuspended', res.isSuspend);
                                    if (res.isSuspend == true) {
                                        OpenSuspendPopup();
                                    }
                                    else {
                                        initUserBlocked = false;
                                        res.roles = resToken.roles;
                                        CookieService.setCookie('userLoggedIn', JSON.stringify(res));
                                        SidePanelService.DrawMenu();
                                        if (typeof res.photoUrl != 'undefined' && res.photoUrl != null && res.photoUrl != '' && res.photoUrl != ' ') {
                                            CookieService.setCookie('usrPhoto', res.photoUrl);
                                        }
                                        else {
                                            CookieService.removeCookie('usrPhoto');
                                        }

                                        helpers.GoToPage('home', null);
                                    }
                                }
                            });
                        }
                        else {
                            SpinnerPlugin.activityStop();
                        }
                    });
                });
            }
            else {
                language.openFrameworkModal('تنبيه', 'من فضلك أدخل إسم الدخول وكلمة المرور', 'alert', function () { });
            }
        }
    };
}]);

