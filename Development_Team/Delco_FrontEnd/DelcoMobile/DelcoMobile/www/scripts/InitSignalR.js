/// <reference path="../js/angular.js" />
/// <reference path="../js/jquery-2.1.0.js" />
/// <reference path="../js/jquery.signalR-2.1.1.min.js" />

myApp.angular.factory('SignalRFactory', ['$rootScope', '$exceptionHandler', 'CookieService', '$interval', function ($rootScope, $exceptionHandler, CookieService, $interval) {
    'use strict';

    var fw7 = myApp.fw7;
    var apisignalrUrl = fw7.apiUrl + "signalr";
    var watchID;

    fw7.connection = $.hubConnection();
    fw7.proxy = fw7.connection.createHubProxy('notificationhub');
    fw7.connection.url = apisignalrUrl;
    $rootScope.connection = fw7.connection;
    $rootScope.proxy = fw7.proxy;

    fw7.proxy.on('agantMove', function (agentLocation) {
        console.log(JSON.stringify(agentLocation));
    });

    //if ($.connection.hub && $.connection.hub.state === $.signalR.connectionState.disconnected) {
    //    $.connection.hub.start()
    //}
    
    var configureSignalR = function (signalRMethod, parameters, callBack) {
        var params = {};
        if (parameters) {
            params = JSON.parse(parameters);
        }

        fw7.connection.start({ jsonp: true }).done(function () {
            console.log('SignalR Connection Successfull');
            fw7.proxy.invoke('joinToGroup', params);
            return true;
        }).fail(function (error) {
            console.log('failed to start SignalR', error);
            return false;
        });
    }

    return {
        configureSignalR: configureSignalR
    };
}]);