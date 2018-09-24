/// <reference path="../js/angular.js" />

myApp.angular.controller('forgetPasswordController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;

    $document.ready(function () {
        app.onPageInit('forgetPass', function (page) {
            if ($rootScope.currentOpeningPage != 'forgetPass') return;
            $rootScope.currentOpeningPage = 'forgetPass';

        });

        app.onPageBeforeAnimation('forgetPass', function (page) {
            if ($rootScope.currentOpeningPage != 'forgetPass') return;
            $rootScope.currentOpeningPage = 'forgetPass';

            $scope.resetForm();
        });

        app.onPageAfterAnimation('forgetPass', function (page) {
            if ($rootScope.currentOpeningPage != 'forgetPass') return;
            $rootScope.currentOpeningPage = 'forgetPass';

            

        });

        $scope.resetForm = function () {
            $scope.forgetPassReset = false;
            $scope.forgetPassForm.mobile = null;
            if (typeof $scope.ForgetPasswordForm != 'undefined' && $scope.ForgetPasswordForm != null) {
                $scope.ForgetPasswordForm.$setPristine(true);
                $scope.ForgetPasswordForm.$setUntouched();
            }
        }

        $scope.form = {};
        $scope.forgetPassForm = {};

        $scope.submitForm = function (isValid) {
            $scope.forgetPassReset = true;
            if (isValid) {
                var params = {
                    'mobile': $scope.forgetPassForm.mobile
                }

                SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                appServices.CallService('forgetPass', "POST", "api/Account/ForgetPassword", params, function (res) {
                    if (res != null) {
                        CookieService.setCookie('confirmationMobile', $scope.forgetPassForm.mobile);
                        language.openFrameworkModal('نجاح', 'تم إرسال الكود لجوالك بنجاح .', 'alert', function () {
                            SpinnerPlugin.activityStop();
                            helpers.GoToPage('resetPassword', null);
                        });
                    }
                    else {
                        SpinnerPlugin.activityStop();
                    }
                });
            }
        };

        $scope.GoBack = function () {
            helpers.GoBack();
        }

        $scope.emailRegex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

        app.init();
    });

}]);

