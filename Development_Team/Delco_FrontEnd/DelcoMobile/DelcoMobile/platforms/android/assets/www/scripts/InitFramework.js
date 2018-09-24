/// <reference path="../js/angular.js" />
/// <reference path="../js/framework7.js" />
/// <reference path="../js/jquery-2.1.0.js" />

var serviceURL = 'http://delcoapi.saned-projects.com/';
var hostUrl = 'http://delcoapi.saned-projects.com/';
var lang = 'En';
var appToken = '';
var userId = 0;
userId = localStorage.getItem("UID");
var user;
var allCities = [];
var allCountries = [];
var allUsers = [];
var allAreas = [];
var BackIsClicked = false;
var scrollLoadsBefore = false;
var initLoginPage = true;
var initSignupPage = true;
var initactivationPage = true;
var initForgetPassword = true;
var initResetPassword = true;
var initChangePassword = true;
var initSearch = true;
var initSideMenu = true;
var clientId = 'consoleApp';
var clientSecret = '123@abc';
var initUserBlocked = false;
var messageInterval;
var myPhotoBrowserPopupDark;


myApp.angular.factory('InitService', ['$document', '$log', 'helpers', function ($document, $log, helpers) {
    'use strict';

    console.log('InitService');

    var pub = {},
      eventListeners = {
          'ready': []
      };

    pub.addEventListener = function (eventName, listener) {
        eventListeners[eventName].push(listener);
    };

    (function () {
        $document.ready(function () {
            console.log('InitService document ready');
            var fw7 = myApp.fw7;
            var i;

            fw7.views.push(fw7.app.addView('.view-main', fw7.options));

            for (i = 0; i < eventListeners.ready.length; i = i + 1) {
                eventListeners.ready[i]();
            }
        });

        window.document.addEventListener('backbutton', function (event) {
            var fw7 = myApp.fw7;

            var currentPage = fw7.views[0].activePage.name;
            if (currentPage == 'home' || currentPage == 'login') {
                language.openFrameworkModal('تأكيد', 'هل تريد الخروج من التطبيق ؟', 'confirm', function () {
                    ExitApplication();
                });
            }
            else if (currentPage == 'tripDetails') {
                return false;
            }
            else if (currentPage == 'requestDetails') {
                return false;
            }
            else {
                fw7.app.closePanel();
                if ($('.modal-in').length > 0) {
                    fw7.app.closeModal('.modal-in');
                    return false;
                }
                else {
                    if (currentPage == 'activation') {
                        return false;
                    }
                    BackIsClicked = true;
                    helpers.GoBack();
                }
            }
        });

        window.document.addEventListener('pause', function (event) {

        });

        window.document.addEventListener('resume', function (event) {
            var fw7 = myApp.fw7;

            var currentPage = fw7.views[0].activePage.name;
        });

        window.addEventListener('native.keyboardhide', keyboardHideHandler);
        window.addEventListener('native.keyboardshow', keyboardShowHandler);

        function ExitApplication() {
            if (navigator.app) {
                navigator.app.exitApp();
            }
            else if (navigator.device) {
                navigator.device.exitApp();
            }
        }

        document.addEventListener("offline", onOffline, false);
        window.addEventListener('native.keyboardhide', keyboardHideHandler);
        window.addEventListener('native.keyboardshow', keyboardShowHandler);

        function onOffline() {
            navigator.notification.alert('انت غير متصل بالانترنت , من فضلك أعد تشغيل البرنامج بعد التأكد من الإنترنت .', function () {
                ExitApplication();
            }, 'خطأ', 'موافق');
        }

        function keyboardHideHandler(e) {
            if ($('.ui-footer').hasClass('ui-fixed-hidden')) {
                $('.ui-footer').removeClass('ui-fixed-hidden');
            }
        }

        function keyboardShowHandler(e) {
            $('.ui-footer').addClass('ui-fixed-hidden');
        }


    }());

    return pub;

}]);
