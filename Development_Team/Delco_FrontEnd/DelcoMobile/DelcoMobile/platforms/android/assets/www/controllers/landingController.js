/// <reference path="../js/angular.js" />

myApp.angular.controller('landingController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'helpers', '$interval', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, helpers, $interval) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;

    function EnterApplication() {
        if (CookieService.getCookie('userLoggedIn') || CookieService.getCookie('UserID')) {
            if (CookieService.getCookie('UserEntersCode') == "false") {
                if (CookieService.getCookie('loginUsingSocial') == 'true') {
                    helpers.GoToPage('home', null);
                } else {
                    helpers.GoToPage('activation', null);
                }
            }
            else {
                helpers.GoToPage('home', null);
            }
        }
        else {
            if (CookieService.getCookie('UID')) {
                helpers.GoToPage('signup', null);
            }
            else {
                helpers.GoToPage('login', null);
            }
        }
    }

    setTimeout(function () {
        EnterApplication();
    }, 2000);

    $document.ready(function () {

        app.onPageInit('landing', function (page) {
            if ($rootScope.currentOpeningPage != 'landing') return;
        });

        app.onPageAfterAnimation('landing', function (page) {
            if ($rootScope.currentOpeningPage != 'landing') return;
        });

        app.init();
    });

}]);