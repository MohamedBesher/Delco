/// <reference path="../js/angular.js" />

myApp.angular.controller('activationController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;

    $document.ready(function () {
        app.onPageInit('activation', function (page) {
            if ($rootScope.currentOpeningPage != 'activation') return;
            $rootScope.currentOpeningPage = 'activation';

        });

        app.onPageBeforeAnimation('activation', function (page) {
            if ($rootScope.currentOpeningPage != 'activation') return;
            $rootScope.currentOpeningPage = 'activation';

        });

        app.onPageAfterAnimation('activation', function (page) {
            if ($rootScope.currentOpeningPage != 'activation') return;
            $rootScope.currentOpeningPage = 'activation';

            $scope.resetForm();

        });

        $scope.resetForm = function () {
            $scope.activationForm.code = null;
            if (typeof $scope.ActivationForm != 'undefined' && $scope.ActivationForm != null) {
                $scope.ActivationForm.$setPristine(true);
                $scope.ActivationForm.$setUntouched();
            }
        }

        $scope.form = {};
        $scope.activationForm = {};

        $scope.submitForm = function (isValid) {
            if (isValid) {
                var code = $scope.activationForm.code;
                var userId = CookieService.getCookie('UserID');

                SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                appServices.CallService('activation', "POST", "api/Account/ConfirmEmail", { "userId": userId, "code": code }, function (res) {
                    if (res != null) {
                        CookieService.setCookie('Visitor', false);
                        CookieService.setCookie('UserEntersCode', true);
                        CookieService.removeCookie('UserID');
                        CookieService.setCookie('ActivationCode', $scope.activationForm.code);
                        SpinnerPlugin.activityStop();
                        helpers.GoToPage('login', null);
                    }
                    else {
                        SpinnerPlugin.activityStop();
                    }
                });
                
            }
        };

        $scope.ResendConfirmationCode = function () {
            var userId = CookieService.getCookie('UserID');

            SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
            appServices.CallService('activation', "POST", "api/Account/ReSendConfirmationCode/" + userId, '', function (res) {
                if (res != null) {
                    language.openFrameworkModal('نجاح', 'تم إعادة إرسال الكود بنجاح .', 'alert', function () { SpinnerPlugin.activityStop(); });
                }
                else {
                    SpinnerPlugin.activityStop();
                }
            });
        };

        app.init();
    });

}]);

