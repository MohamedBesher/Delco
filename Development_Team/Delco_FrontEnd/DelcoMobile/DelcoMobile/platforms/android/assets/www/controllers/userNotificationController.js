/// <reference path="../js/angular.js" />

myApp.angular.controller('userNotificationController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;
    var loadingNotifications = false;
    var allUserNotification = [];
    var isPageReInit = false;

    function GetNotifications(callBack) {
        var allUserNotification = [];
        $scope.notifications = null;
        var userNotificationParamter = {
            PageNumber: '0',
            PageSize:'10'
        }
        appServices.CallService('userNotification', "POST", "api/Notification/GetUserNotifications", userNotificationParamter, function (userNotifications) {
            if (userNotifications) {
                angular.forEach(userNotifications, function (notification) {
                    allUserNotification.push(notification);
                });
                callBack(allUserNotification);
            }
        });
        
    }

    $document.ready(function () {
        app.onPageInit('userNotification', function (page) {
            if ($rootScope.currentOpeningPage != 'userNotification') return;
            $rootScope.currentOpeningPage = 'userNotification';

            $$('#divInfiniteUserNotifications').on('ptr:refresh', function (e) {
                GetNotifications(function (notifications) {
                    if (notifications != null && notifications.length > 0) {
                        $scope.notifications = notifications;
                        app.pullToRefreshDone();
                    }
                    else {
                        app.pullToRefreshDone();
                    }
                });
                setTimeout(function () {
                    $scope.$apply();
                }, fw7.DelayBeforeScopeApply);
            });

            $$('#divInfiniteUserNotifications').on('infinite', function () {
                if (loadingNotifications) return;
                loadingNotifications = true;
                SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                var userNotificationParamter = {
                    PageNumber: parseInt(CookieService.getCookie('userNotifications-page-number')),
                    PageSize: fw7.PageSize
                }
                appServices.CallService('userNotification', 'POST', "api/Notification/GetUserNotifications", userNotificationParamter, function (notifications) {
                    SpinnerPlugin.activityStop();
                    if (notifications && notifications.length > 0) {
                        loadingNotifications = false;
                        CookieService.setCookie('userNotifications-page-number', parseInt(CookieService.getCookie('userNotifications-page-number')) + 1);

                        angular.forEach(notifications, function (notification) {
                            allUserNotification.push(notification);
                        });

                        if (notifications && notifications.length <= fw7.PageSize) {
                            app.detachInfiniteScroll('#divInfiniteUserNotifications');

                            setTimeout(function () {
                                $scope.$apply();
                            }, fw7.DelayBeforeScopeApply);
                            return;
                        }
                    }
                    else {
                        app.detachInfiniteScroll('#divInfiniteUserNotifications');
                    }

                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                });
            });

        });

        app.onPageReinit('userNotification', function (page) {
            if (!isPageReInit) {
                isPageReInit = true;

        
            }
        });

        app.onPageBeforeAnimation('userNotification', function (page) {
            if ($rootScope.currentOpeningPage != 'userNotification') return;
            $rootScope.currentOpeningPage = 'userNotification';
            CookieService.setCookie('userNotifications-page-number',1);
        });

        app.onPageAfterAnimation('userNotification', function (page) {
            if ($rootScope.currentOpeningPage != 'userNotification') return;
            $rootScope.currentOpeningPage = 'userNotification';
            GetNotifications(function (notifications) {
                if (notifications != null && notifications.length > 0) {
                    $scope.userNotification = notifications;
                    $scope.userNotificationsAlert = false;
                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                    allUserNotification = notifications;
                    if (notifications.length < fw7.PageSize) {
                        $scope.userNotificationsInfiniteLoader = false;
                    }
                }
                else {
                    $scope.userNotificationsAlert = true;
                    $scope.userNotificationsInfiniteLoader = false;
                }

                setTimeout(function () {
                    $scope.$apply();
                }, fw7.DelayBeforeScopeApply);
            });
        });

        $scope.resetNotifications = function () {
            CookieService.setCookie('userNotifications-page-number', 1);
            $scope.userNotificationsAlert = false;
            $scope.userNotificationsInfiniteLoader = false;
            $scope.notifications = [];
            allUserNotification = [];
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

        app.init();
    });

}]);

