/// <reference path="../js/angular.js" />

myApp.angular.controller('contactController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;

    $document.ready(function () {
        app.onPageInit('contact', function (page) {
            if ($rootScope.currentOpeningPage != 'contact') return;
            $rootScope.currentOpeningPage = 'contact';

        });

        app.onPageBeforeAnimation('contact', function (page) {
            if ($rootScope.currentOpeningPage != 'contact') return;
            $rootScope.currentOpeningPage = 'contact';

            $scope.resetForm();
        });


        app.onPageAfterAnimation('contact', function (page) {
            if ($rootScope.currentOpeningPage != 'contact') return;
            $rootScope.currentOpeningPage = 'contact';
            
            
        });

        $scope.resetForm = function () {
            $scope.contactUsReset = false;
            $scope.contactUsForm.title = null;
            $scope.contactUsForm.message = null;
            if (typeof $scope.ContactUsForm != 'undefined' && $scope.ContactUsForm != null) {
                $scope.ContactUsForm.$setPristine(true);
                $scope.ContactUsForm.$setUntouched();
            }
        }

        $scope.submitForm = function (isValid) {
            $scope.contactUsReset = true;
            if (isValid) {
                var params = {
                    'Title': $scope.contactUsForm.title,
                    'Message': $scope.contactUsForm.message
                }

                SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                appServices.CallService('contact', "POST", "api/ContactUs/SaveContactUs", params, function (res2) {
                    SpinnerPlugin.activityStop();
                    if (res2 != null) {
                        language.openFrameworkModal('نجاح', 'تم إرسال رسالتك لإدارة التطبيق بنجاح .', 'alert', function () {
                            helpers.GoToPage('home', null);         
                        });
                    }
                });
            }
        };

        $scope.form = {};
        $scope.contactUsForm = {};

        $scope.GoBack = function () {
            helpers.GoBack();
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

