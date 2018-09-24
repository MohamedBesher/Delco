myApp.angular.controller('tripMapController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;
    var map;


    $document.ready(function () {
        app.onPageInit('tripMap', function (page) {
            if ($rootScope.currentOpeningPage != 'tripMap') return;
            $rootScope.currentOpeningPage = 'tripMap';

        });

        app.onPageReinit('tripMap', function (page) {
            if ($rootScope.currentOpeningPage != 'tripMap') return;
            $rootScope.currentOpeningPage = 'tripMap';

        });

        app.onPageBeforeAnimation('tripMap', function (page) {
            if ($rootScope.currentOpeningPage != 'tripMap') return;
            $rootScope.currentOpeningPage = 'tripMap';
           $rootScope.initTripMap = true;
           var projectMarkers = [{ "title": 'myMap', "lat": '24.713552', "lng": '46.675296', "description": "yeeees" }];
            helpers.initMap('tripMap', projectMarkers, '', '', function (map) {
            });
        });

        app.onPageAfterAnimation('tripMap', function (page) {
            if ($rootScope.currentOpeningPage != 'mapTrack') return;
            $rootScope.currentOpeningPage = 'mapTrack';
      
          
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