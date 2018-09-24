/// <reference path="../js/angular.js" />

myApp.angular.controller('usersController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;

    $document.ready(function () {
        app.onPageInit('users', function (page) {
            if ($rootScope.currentOpeningPage != 'users') return;
            $rootScope.currentOpeningPage = 'users';

        });

        app.onPageBeforeAnimation('users', function (page) {
            if ($rootScope.currentOpeningPage != 'users') return;
            $rootScope.currentOpeningPage = 'users';


        });

        app.onPageAfterAnimation('users', function (page) {
            if ($rootScope.currentOpeningPage != 'users') return;
            $rootScope.currentOpeningPage = 'users';


        });

        $scope.GoToUserSignup = function () {
            helpers.GoToPage('signupUser', null);
        };

        $scope.GoToAgentSignup = function () {
            helpers.GoToPage('signupAgent', null);
        };

        app.init();
    });

}]);

