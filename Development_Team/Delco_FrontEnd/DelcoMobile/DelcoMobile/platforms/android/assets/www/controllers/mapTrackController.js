/// <reference path="../js/angular.js" />

myApp.angular.controller('mapTrackController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;
    var map;
    var marker;
    var watchID;
    var oldPosition;
    var timeinterval;
    var reqMarker = null;

    function autoUpdate() {
        document.addEventListener("deviceready", function () {
            plugin.google.maps.Map.isAvailable(function (isAvailable, message) {
                if (isAvailable) {
                    var mapDiv = document.getElementById("mapTrack");
                    map = plugin.google.maps.Map.getMap(mapDiv);

                    map.on(plugin.google.maps.event.MAP_READY, function () {
                        map.clear();
                        map.setMapTypeId(plugin.google.maps.MapTypeId.ROADMAP);
                        map.setZoom(18);
                        map.setVisible(true);
                        map.showDialog();

                        map.addEventListener(plugin.google.maps.event.MAP_CLOSE, function () {
                            map.trigger("MARKER_REMOVE");
                            helpers.GoBack();
                        });
                    });

                } else {
                    console.error("Google Map Is Not Available: " + error);
                }
            });
        }, false);
    }

    function AddMarker(markerPosition) {
        
        map.addMarker({
            position: markerPosition,
            title: "",
            icon: 'blue',
        }, function (marker) {
            reqMarker = marker;
            marker.setPosition(markerPosition);
            map.setCenter(markerPosition);

            map.addEventListenerOnce("MARKER_REMOVE", function () {
                marker.remove();
            });
        });
    }

    $document.ready(function () {
        app.onPageInit('mapTrack', function (page) {
            if ($rootScope.currentOpeningPage != 'mapTrack') return;
            $rootScope.currentOpeningPage = 'mapTrack';

        });

        app.onPageReinit('mapTrack', function (page) {
            if ($rootScope.currentOpeningPage != 'mapTrack') return;
            $rootScope.currentOpeningPage = 'mapTrack';

        });

        app.onPageBeforeAnimation('mapTrack', function (page) {
            if ($rootScope.currentOpeningPage != 'mapTrack') return;
            $rootScope.currentOpeningPage = 'mapTrack';

            
        });

        app.onPageAfterAnimation('mapTrack', function (page) {
            if ($rootScope.currentOpeningPage != 'mapTrack') return;
            $rootScope.currentOpeningPage = 'mapTrack';

            $rootScope.reqRequest = page.query.reqRequest;

            $rootScope.TrackUserIfAgent(JSON.parse($rootScope.reqRequest));

            $rootScope.proxy.on('agantMove', function (agentLocation) {
                oldPosition = new plugin.google.maps.LatLng(agentLocation.Latitude, agentLocation.Longitude);
                map.clear();
                AddMarker(oldPosition);
            });

            autoUpdate();
           
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

