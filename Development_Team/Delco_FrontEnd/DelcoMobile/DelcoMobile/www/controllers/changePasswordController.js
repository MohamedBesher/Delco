/// <reference path="../js/angular.js" />

myApp.angular.controller('changePasswordController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;

    $document.ready(function () {
        app.onPageInit('changePass', function (page) {
            if ($rootScope.currentOpeningPage != 'changePass') return;
            $rootScope.currentOpeningPage = 'changePass';

        });

        app.onPageBeforeAnimation('changePass', function (page) {
            if ($rootScope.currentOpeningPage != 'changePass') return;
            $rootScope.currentOpeningPage = 'changePass';

            $scope.resetForm();
        });

        app.onPageAfterAnimation('changePass', function (page) {
            if ($rootScope.currentOpeningPage != 'changePass') return;
            $rootScope.currentOpeningPage = 'changePass';

            
        });

        $scope.resetForm = function () {
            $scope.changePasswordReset = false;
            $scope.changePassForm.oldPassword = null;
            $scope.changePassForm.newPassword = null;
            $scope.changePassForm.confrmNewPassword = null;
            if (typeof $scope.$parent.ChangePasswordForm != 'undefined' && $scope.$parent.ChangePasswordForm != null) {
                $scope.$parent.ChangePasswordForm.$setPristine(true);
                $scope.$parent.ChangePasswordForm.$setUntouched();
            }
        }

        $scope.form = {};
        $scope.changePassForm = {};

        $scope.submitForm = function (isValid) {
            $scope.changePasswordReset = true;
            if (isValid) {
                var params = {
                    'oldPassword': $scope.changePassForm.oldPassword,
                    'newPassword': $scope.changePassForm.newPassword,
                    'ConfirmPassword': $scope.changePassForm.confrmNewPassword
                }

                SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                appServices.CallService('changePass', "POST", "Api/Account/ChangePassword", params, function (res) {
                    if (res != null) {
                        language.openFrameworkModal('نجاح', 'تم تعديل كلمة السر بنجاح .', 'alert', function () {
                            SpinnerPlugin.activityStop();
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

        app.init();
    });

}]);

