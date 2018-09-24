/// <reference path="../js/angular.js" />

myApp.angular.controller('agentProfileController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;

    function LoadAgentData(agent) {
        $scope.username = typeof agent != '' && agent != null ? agent.userName : false;
        $scope.name = agent.fullName;
        $scope.email = agent.email;
        $scope.mobile = agent.phoneNumber;
        $scope.companyName = agent.companyName;
        $scope.model = agent.model;
        $scope.carType = agent.carType;
        $scope.color = agent.color;
        $scope.plateNumber = agent.plateNumber;
        $scope.agentOrderExcuted = agent.agentStatisticsModel.requestCount;
        $scope.agntPrice = agent.agentStatisticsModel.delivaryPrice;
        $scope.ManagementRatio = agent.agentStatisticsModel.managPercentage;
        $scope.NetProfit = agent.agentStatisticsModel.agentProfit;
        $scope.rate = agent.agentStatisticsModel.agentRate;

        $scope.IsUserNameExists = typeof agent != 'undefined' && agent.userName != null && agent.userName != '' && agent.userName != ' ' ? true : false;
        $scope.IsEmailExists = typeof agent != 'undefined' && agent.email != null && agent.email != '' && agent.email != ' ' ? true : false;
        $scope.IsNameExists = typeof agent != 'undefined' && agent.fullName != null && agent.fullName != '' && agent.fullName != ' ' ? true : false;
        $scope.IsMobileExists = typeof agent != 'undefined' && agent.phoneNumber != null && agent.phoneNumber != '' && agent.phoneNumber != ' ' ? true : false;
        $scope.IsCompanyNameExists = typeof agent != 'undefined' && agent.companyName != null && agent.companyName != '' && agent.companyName != ' ' ? true : false;
        $scope.IsModelExists = typeof agent != 'undefined' && agent.model != null && agent.model != '' && agent.model != ' ' ? true : false;
        $scope.IsCarTypeExists = typeof agent != 'undefined' && agent.carType != null && agent.carType != '' && agent.carType != ' ' ? true : false;
        $scope.IsColorExists = typeof agent != 'undefined' && agent.color != null && agent.color != '' && agent.color != ' ' ? true : false;
        $scope.IsPlateNumberExists = typeof agent != 'undefined' && agent.plateNumber != null && agent.plateNumber != '' && agent.plateNumber != ' ' ? true : false;
        $scope.IsAgentExecutedOrdersExists = typeof agent != 'undefined' && agent.agentStatisticsModel.requestCount != null && agent.agentStatisticsModel.requestCount != '' && agent.agentStatisticsModel.requestCount != ' ' ? true : false;
        $scope.IsAgentPriceExists = typeof agent != 'undefined' && agent.agentStatisticsModel.delivaryPrice != null && agent.agentStatisticsModel.delivaryPrice != '' && agent.agentStatisticsModel.delivaryPrice != ' ' ? true : false;
        $scope.IsManagementRatioExists = typeof agent != 'undefined' && agent.agentStatisticsModel.managPercentage != null && agent.agentStatisticsModel.managPercentage != '' && agent.agentStatisticsModel.managPercentage != ' ' ? true : false;
        $scope.IsNetProfitExists = typeof agent != 'undefined' && agent.agentStatisticsModel.agentProfit != null && agent.agentStatisticsModel.agentProfit != '' && agent.agentStatisticsModel.agentProfit != ' ' ? true : false;

        if (typeof agent.photoUrl != 'undefined' && agent.photoUrl != null && agent.photoUrl != '' && agent.photoUrl != ' ') {
            $scope.photoUrl = hostUrl + '/Uploads/' + agent.photoUrl;
        }
        else {
            $scope.photoUrl = 'img/avatar.png';
        }
    }

    $document.ready(function () {
        app.onPageInit('agentProfile', function (page) {
            if ($rootScope.currentOpeningPage != 'agentProfile') return;
            $rootScope.currentOpeningPage = 'agentProfile';

        });

        app.onPageBeforeAnimation('agentProfile', function (page) {
            if ($rootScope.currentOpeningPage != 'agentProfile') return;
            $rootScope.currentOpeningPage = 'agentProfile';
            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
            var userId = page.query.userId;
            var isOwner = true;

            if(!angular.isUndefined(userId)){
                isOwner = typeof userLoggedIn != 'undefined' && userLoggedIn != null && userLoggedIn.id == userId ? true : false;
            }

            $scope.IsOwner = isOwner;

            if (isOwner) {
                LoadAgentData(userLoggedIn);
                setTimeout(function () {
                    $scope.$apply();
                }, fw7.DelayBeforeScopeApply);
            }
            else {
                var params = {
                    "UserId": userId
                };

                SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                appServices.CallService('userProfile', 'POST', "api/Account/UserInfo", params, function (userData) {
                    SpinnerPlugin.activityStop();
                    if (userData) {
                        LoadAgentData(userData);
                    }

                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                });

            }

        });

        app.onPageAfterAnimation('agentProfile', function (page) {
            if ($rootScope.currentOpeningPage != 'agentProfile') return;
            $rootScope.currentOpeningPage = 'agentProfile';

        });

        $scope.GoToNotifications = function () {
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

