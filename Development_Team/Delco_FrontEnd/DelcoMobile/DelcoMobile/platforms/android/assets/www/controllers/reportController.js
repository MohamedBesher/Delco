/// <reference path="../js/angular.js" />

myApp.angular.controller('reportController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;

    $document.ready(function () {
        app.onPageInit('report', function (page) {
            if ($rootScope.currentOpeningPage != 'report') return;
            $rootScope.currentOpeningPage = 'report';

        });

        app.onPageBeforeAnimation('report', function (page) {
            if ($rootScope.currentOpeningPage != 'report') return;
            $rootScope.currentOpeningPage = 'report';

            $scope.resetForm();
        });

        app.onPageAfterAnimation('report', function (page) {
            if ($rootScope.currentOpeningPage != 'report') return;
            $rootScope.currentOpeningPage = 'report';

            
        });

        $scope.resetForm = function () {
            $scope.reportReset = false;
            $scope.reportForm.title = null;
            $scope.reportForm.message = null;
            if (typeof $scope.ReportForm != 'undefined' && $scope.ReportForm != null) {
                $scope.ReportForm.$setPristine(true);
                $scope.ReportForm.$setUntouched();
            }
        }

        $scope.submitForm = function (isValid) {
            $scope.reportReset = true;
            if (isValid) {
                var params = {
                    'Title': $scope.reportForm.title,
                    'Message': $scope.reportForm.message
                }

                SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                appServices.CallService('report', "POST", "api/Abuse/SaveAbuse", params, function (res2) {
                    SpinnerPlugin.activityStop();
                    if (res2 != null) {
                        language.openFrameworkModal('نجاح', 'تم تسجيل البلاغ بنجاح .', 'alert', function () {
                            helpers.GoToPage('home', null);
                        });
                    }
                });
            }
        };

        $scope.form = {};
        $scope.reportForm = {};

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

