/// <reference path="../js/angular.js" />

myApp.angular.controller('termsController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;

    function LoadTerms(callBack) {
        var terms = [];
        SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });

        appServices.CallService('terms', 'GET', "api/Setting/TermsOfConditions", '', function (terms) {
            if (terms) {
                callBack(terms);
            }
        });
    }

    $document.ready(function () {
        app.onPageInit('terms', function (page) {
            if ($rootScope.currentOpeningPage != 'terms') return;
            $rootScope.currentOpeningPage = 'terms';

        });

        app.onPageBeforeAnimation('terms', function (page) {
            if ($rootScope.currentOpeningPage != 'terms') return;
            $rootScope.currentOpeningPage = 'terms';


        });

        app.onPageAfterAnimation('terms', function (page) {
            if ($rootScope.currentOpeningPage != 'terms') return;
            $rootScope.currentOpeningPage = 'terms';

            $scope.terms = $rootScope.terms;
            
            
            setTimeout(function () {
                $scope.$apply();
            }, fw7.DelayBeforeScopeApply);
        });

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

