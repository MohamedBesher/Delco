/// <reference path="../js/jquery-2.1.0.js" />
/// <reference path="../js/framework7.min.js" />
/// <reference path="../js/angular.js" />

var calendarAddTransportArriveDate;

myApp.angular.controller('mainController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var mainView = fw7.views['.view-main'];
    var app = myApp.fw7.app;
    $rootScope.currentOpeningPage = 'landing';

    function LoadTerms(callBack) {
        var terms = [];
        //SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });

        appServices.CallService('terms', 'GET', "api/Setting/TermsOfConditions", '', function (terms) {
            //SpinnerPlugin.activityStop();
            if (terms) {
                callBack(terms);
            }
        });
    }

    $scope.GoToHome = function () {
        helpers.GoToPage('addRequest', null);
    };

    $scope.GoToMyProfile = function () {
        var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
        if (userLoggedIn.roles == 'User') {
            helpers.GoToPage('userProfile', null);
        }
        else {
            helpers.GoToPage('agentProfile', null);
        }
    };

    $scope.GoToMyRequests = function () {
        var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
        if (userLoggedIn.roles == 'User') {
            helpers.GoToPage('userOrder', null);
        }
        else {
            helpers.GoToPage('agentOrder', null);
        }
    };

    $scope.GoToNotifications = function () {
        var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
        if (userLoggedIn.roles == 'User') {
            helpers.GoToPage('userNotification', null);
        }
        else {
            helpers.GoToPage('agentNotification', null);
        }
    };

    $scope.GoToTerms = function () {
        helpers.GoToPage('terms', null);
    };

    $scope.GoToContact = function () {
        helpers.GoToPage('contact', null);
    };

    $scope.GoToAbuse = function () {
        helpers.GoToPage('report', null);
    };

    $scope.GoToChangePassword = function () {
        helpers.GoToPage('changePass', null);
    };

    $scope.GoToLogin = function () {
        SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
        appServices.CallService('signupAgent', 'POST', "api/Account/ChangeStatus/0", "", function (result) {
            SpinnerPlugin.activityStop();
            if (result) {
                CookieService.removeCookie('appToken');
                CookieService.removeCookie('USName');
                CookieService.removeCookie('refreshToken');
                CookieService.removeCookie('userLoggedIn');
                CookieService.removeCookie('loginUsingSocial');
                CookieService.setCookie('Visitor', false);
                helpers.GoToPage('login', null);
            }
        });
    };

    SidePanelService.DrawMenu();

    $document.ready(function () {

        LoadTerms(function (terms) {
            SpinnerPlugin.activityStop();
            $rootScope.terms = terms;
            $scope.terms = terms;
        });

    });

}]);


