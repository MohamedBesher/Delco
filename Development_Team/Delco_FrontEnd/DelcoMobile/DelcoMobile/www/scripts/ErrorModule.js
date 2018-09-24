/// <reference path="../js/angular.js" />
/// <reference path="../js/framework7.js" />
/// <reference path="../js/jquery-2.1.0.js" />

myApp.angular = angular.module('ErrorCatcher', [])
    .factory('errorHttpInterceptor', function ($exceptionHandler, $q) {
        return {
            responseError: function responseError(rejection) {
                $exceptionHandler("An HTTP request error has occurred.\nHTTP URL: [" + rejection.config.url + "] - Status: [" + rejection.status + "]  - StatusText: [" + rejection.statusText + "].");
                return $q.reject(rejection);
            }
        };
    })
    .config(function ($httpProvider) {
        $httpProvider.interceptors.push('errorHttpInterceptor');
    });