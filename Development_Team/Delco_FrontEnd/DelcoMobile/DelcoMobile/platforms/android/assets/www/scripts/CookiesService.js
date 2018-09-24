myApp.angular.factory('CookieService', [function () {
    'use strict';

    var getCookie = function getCookie(cookieName) {
        return localStorage.getItem('Delco_' + cookieName) ? localStorage.getItem('Delco_' + cookieName) : null;
    }

    var setCookie = function setCookie(cookieName, cookieValue) {
        localStorage.setItem('Delco_' + cookieName, cookieValue);
    }

    var removeCookie = function (cookieName) {
        localStorage.removeItem('Delco_' + cookieName);
    }

    return {
        getCookie: getCookie,
        setCookie: setCookie,
        removeCookie: removeCookie
    };
}]);