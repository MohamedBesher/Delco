/// <reference path="../js/angular.js" />

myApp.angular.controller('signupUserController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;

    $scope.TermsAccepted = function (model) {
        $scope.user.isChecked = !model;
        $scope.$apply();
    }

    $document.ready(function () {
        app.onPageInit('signupUser', function (page) {
            if ($rootScope.currentOpeningPage != 'signupUser') return;
            $rootScope.currentOpeningPage = 'signupUser';
        });

        app.onPageBeforeAnimation('signupUser', function (page) {
            if ($rootScope.currentOpeningPage != 'signupUser') return;
            $rootScope.currentOpeningPage = 'signupUser';

            $scope.resetForm();
        });

        app.onPageAfterAnimation('signupUser', function (page) {
            if ($rootScope.currentOpeningPage != 'signupUser') return;
            $rootScope.currentOpeningPage = 'signupUser';

            
        });

        

        $scope.resetForm = function () {
            $scope.signUpUserReset = false;
            $scope.userForm.name = null;
            $scope.userForm.username = null;
            $scope.userForm.mobile = null;
            $scope.userForm.email = null;
            $scope.userForm.password = null;
            $scope.userForm.confirmpassword = null;
            $scope.user = {};
            $scope.user.isChecked = false;
            if (typeof $scope.SignupUserForm != 'undefined' && $scope.SignupUserForm != null) {
                $scope.SignupUserForm.$setPristine(true);
                $scope.SignupUserForm.$setUntouched();
            }
        }

        

        $scope.submitForm = function (isValid) {
            $scope.signUpUserReset = true;
            if (isValid) {
                var user = {
                    'FullName': $scope.userForm.name,
                    'UserName': $scope.userForm.username,
                    'PhoneNumber': $scope.userForm.mobile,
                    'Email': $scope.userForm.email,
                    'Password': $scope.userForm.password,
                    'ConfirmPassword': $scope.userForm.confirmpassword,
                    'Role': 'User'
                }

                SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                appServices.CallService('signupUser', "POST", "api/Account/RegisterUser", user, function (res2) {
                    if (res2 != null) {
                        CookieService.setCookie('USName', 'مستخدم');
                        CookieService.setCookie('UserID', res2);
                        CookieService.setCookie('UserEntersCode', false);
                        SpinnerPlugin.activityStop();
                        language.openFrameworkModal('نجاح', 'تم تسجيل بياناتك بنجاح .', 'alert', function () {
                            helpers.GoToPage('activation', null);
                        });
                    }
                    else {
                        SpinnerPlugin.activityStop();
                    }
                });
            }
        };

        $scope.form = {};
        $scope.userForm = {};

        $scope.GoBack = function () {
            helpers.GoBack();
        };

        $scope.GoToTerms = function () {
            helpers.GoToPage('terms', null);
        };

        $scope.GoToNotifications = function () {
            //isPageReInit = false;
            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
            if (userLoggedIn.roles == 'User') {
                helpers.GoToPage('userNotification', null);
            }
            else {
                helpers.GoToPage('agentNotification', null);
            }
        };

        $scope.emailRegex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        $scope.nameRegex = /^[A-Za-z0-9]*$/;

        app.init();

    });
}]);

