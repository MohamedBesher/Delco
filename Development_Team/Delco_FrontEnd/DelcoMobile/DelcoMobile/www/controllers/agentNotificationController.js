/// <reference path="../js/angular.js" />

myApp.angular.controller('agentNotificationController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;
    var loadingNotifications = false;
    var allAgentNotifications = [];
    var isPageReInit = false;

    function GetNotifications(callBack) {
        $scope.notifications = null;
        allAgentNotifications = [];
        var agentNotificationParamter = {
            PageNumber: '0',
            PageSize: '10'
        }
        appServices.CallService('agentNotification', "POST", "api/Notification/GetUserNotifications", agentNotificationParamter, function (agentNotification) {
            if (agentNotification) {
                angular.forEach(agentNotification, function (notification) {
                    allAgentNotifications.push(notification);
                });
                callBack(allAgentNotifications);
            }
        });
    }

    $document.ready(function () {
        app.onPageInit('agentNotification', function (page) {
            if ($rootScope.currentOpeningPage != 'agentNotification') return;
            $rootScope.currentOpeningPage = 'agentNotification';
            $$('#divInfiniteAgentNotifications').on('ptr:refresh', function (e) {               
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

            $$('#divInfiniteAgentNotifications').on('infinite', function () {
                if (loadingNotifications) return;
                loadingNotifications = true;
                SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                var agentNotificationParamter = {
                    PageNumber: parseInt(CookieService.getCookie('agentNotifications-page-number')),
                    PageSize: fw7.PageSize
                }
                appServices.CallService('agentNotification', 'POST', "api/Notification/GetUserNotifications", agentNotificationParamter, function (notifications) {
                    SpinnerPlugin.activityStop();
                    if (notifications && notifications.length > 0) {
                        loadingNotifications = false;
                        CookieService.setCookie('agentNotifications-page-number', parseInt(CookieService.getCookie('agentNotifications-page-number')) + 1);

                        angular.forEach(notifications, function (notification) {
                            allAgentNotifications.push(notification);
                        });

                        if (notifications && notifications.length <= fw7.PageSize) {
                            app.detachInfiniteScroll('#divInfiniteAgentNotifications');

                            setTimeout(function () {
                                $scope.$apply();
                            }, fw7.DelayBeforeScopeApply);
                            return;
                        }
                    }
                    else {
                        app.detachInfiniteScroll('#divInfiniteAgentNotifications');
                        loadingNotifications = false;
                    }

                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                });
            });

        });

        app.onPageReinit('agentNotification', function (page) {

            if (!isPageReInit) {
                isPageReInit = true;

                var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
                $scope.user = userLoggedIn;
               
            }
        });

        app.onPageBeforeAnimation('agentNotification', function (page) {
            if ($rootScope.currentOpeningPage != 'agentNotification') return;
            $rootScope.currentOpeningPage = 'agentNotification';
            CookieService.setCookie('agentNotifications-page-number', 1);

        });

        app.onPageAfterAnimation('agentNotification', function (page) {
            if ($rootScope.currentOpeningPage != 'agentNotification') return;
            $rootScope.currentOpeningPage = 'agentNotification';

            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
            $scope.user = userLoggedIn;

            GetNotifications(function (notifications) {
                if (notifications != null && notifications.length > 0) {
                    $scope.notifications = notifications;
                    $scope.agentNotificationsAlert = false;
                    allAgentNotifications = notifications;
                    if (notifications.length < fw7.PageSize) {
                        $scope.agentNotificationsInfiniteLoader = false;
                    }
                }
                else {
                    $scope.agentNotificationsAlert = true;
                    $scope.agentNotificationsInfiniteLoader = false;
                }

                setTimeout(function () {
                    $scope.$apply();
                }, fw7.DelayBeforeScopeApply);
            });

        });

        $scope.resetNotifications = function () {
            CookieService.setCookie('agentNotifications-page-number', 1);
            $scope.agentNotificationsAlert = false;
            $scope.agentNotificationsInfiniteLoader = false;
            $scope.notifications = [];
            allAgentNotifications = [];
        };

        $scope.ChangeNewNotificationSettings = function (user) {
            //var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
            //userLoggedIn.isNewNotifications = user.isNewNotifications;
            //$scope.user = userLoggedIn;
            //CookieService.setCookie('userLoggedIn', JSON.stringify(userLoggedIn));
            var notifyUserParamter = {
                IsNotify: $scope.user.isNewNotifications
            }
            appServices.CallService('agentNotification', "POST", "api/Account/NotifyUsers", notifyUserParamter, function (agentNotification) {
                if (agentNotification) {
                    language.openFrameworkModal('نجاح', 'تم فتح الاشعارات  .', 'alert', function () { })
                }
                else {
                    language.openFrameworkModal('نجاح', 'تم غلق الاشعارات  .', 'alert', function () { })
                }
            });
        }

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

