/// <reference path="../js/angular.js" />

myApp.angular.controller('resetPasswordController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;

    $document.ready(function () {
        app.onPageInit('resetPassword', function (page) {
            if ($rootScope.currentOpeningPage != 'resetPassword') return;
            $rootScope.currentOpeningPage = 'resetPassword';

        });

        app.onPageBeforeAnimation('resetPassword', function (page) {
            if ($rootScope.currentOpeningPage != 'resetPassword') return;
            $rootScope.currentOpeningPage = 'resetPassword';

            $scope.resetForm();
        });

        app.onPageAfterAnimation('resetPassword', function (page) {
            if ($rootScope.currentOpeningPage != 'resetPassword') return;
            $rootScope.currentOpeningPage = 'resetPassword';

            
        });

        $scope.resetForm = function () {
            $scope.resetPasswordReset = false;
            $scope.resetPassForm.code = null;
            $scope.resetPassForm.newPassword = null;
            $scope.resetPassForm.confrmNewPassword = null;
            if (typeof $scope.ResetPasswordForm != 'undefined' && $scope.ResetPasswordForm != null) {
                $scope.ResetPasswordForm.$setPristine(true);
                $scope.ResetPasswordForm.$setUntouched();
            }
        }
        $scope.form = {};
        $scope.resetPassForm = {};

        $scope.submitForm = function (isValid) {
            $scope.resetPasswordReset = true;
            if (isValid) {
                var params = {
                    'code': $scope.resetPassForm.code,
                    'email': CookieService.getCookie('confirmationMail'),
                    'password': $scope.resetPassForm.newPassword,
                    'confirmPassword': $scope.resetPassForm.confrmNewPassword
                }

                SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                appServices.CallService('resetPassword', "POST", "api/Account/ResetPassword", params, function (res) {
                    if (res != null) {
                        CookieService.removeCookie('confirmationMail');
                        language.openFrameworkModal('نجاح', 'تم تغيير كلمة المرور القديمة بنجاح .', 'alert', function () {
                            SpinnerPlugin.activityStop();
                            helpers.GoToPage('login', null);
                        });
                    }
                    else {
                        SpinnerPlugin.activityStop();
                    }
                });
                
            }
        };

        app.init();
    });

}]);

