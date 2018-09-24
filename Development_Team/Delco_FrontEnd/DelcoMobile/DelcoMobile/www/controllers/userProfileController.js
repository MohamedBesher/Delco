/// <reference path="../js/angular.js" />

myApp.angular.controller('userProfileController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;
    var isOwner = false;

    function LoadUserData(user) {
        $scope.username = typeof user != 'undefined' && user != null ? user.userName : false;
        $scope.name = user.fullName;
        $scope.email = user.email;
        $scope.mobile = user.phoneNumber;

        $scope.IsUserNameExists = typeof user != 'undefined' && user.userName != null && user.userName != '' && user.userName != ' ' ? true : false;
        $scope.IsEmailExists = typeof user != 'undefined' && user.email != null && user.email != '' && user.email != ' ' ? true : false;
        $scope.IsNameExists = typeof user != 'undefined' && user.fullName != null && user.fullName != '' && user.fullName != ' ' ? true : false;
        $scope.IsMobileExists = typeof user != 'undefined' && user.phoneNumber != null && user.phoneNumber != '' && user.phoneNumber != ' ' ? true : false;

        if (typeof user.photoUrl != 'undefined' && user.photoUrl != null && user.photoUrl != '' && user.photoUrl != ' ') {
            $scope.photoUrl = hostUrl + '/Uploads/' + user.photoUrl;
        }
        else {
            $scope.photoUrl = 'img/avatar.png';
        }
    }

    $document.ready(function () {
        app.onPageInit('userProfile', function (page) {
            if ($rootScope.currentOpeningPage != 'userProfile') return;
            $rootScope.currentOpeningPage = 'userProfile';

        });

        app.onPageBeforeAnimation('userProfile', function (page) {
            if ($rootScope.currentOpeningPage != 'userProfile') return;
            $rootScope.currentOpeningPage = 'userProfile';
            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
            var userId = page.query.userId;
            var isOwner = true;

            if (!angular.isUndefined(userId)) {
                isOwner = typeof userLoggedIn != 'undefined' && userLoggedIn != null && userLoggedIn.id == userId ? true : false;
            }

            $scope.IsOwner = isOwner;

            if (isOwner) {
                LoadUserData(userLoggedIn);
                setTimeout(function () {
                    $scope.$apply();
                }, fw7.DelayBeforeScopeApply);
            }
            else {
                var params = {
                    "UserId": userId
                };

                SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                appServices.CallService('userProfile', 'POST', "api/Account/UserInfo", params, function (userData) {
                    SpinnerPlugin.activityStop();
                    if (userData) {
                        LoadUserData(userData);
                    }

                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                });

            }
        });

        app.onPageAfterAnimation('userProfile', function (page) {
            if ($rootScope.currentOpeningPage != 'userProfile') return;
            $rootScope.currentOpeningPage = 'userProfile';           
        
        });

        $scope.GoToEditProfile = function () {
            helpers.GoToPage('userDetails', null);
        };


        $scope.GoToPreviousPage = function () {
            helpers.GoBack();
        };

        $scope.openEditProfile = function () {
            helpers.GoToPage('editProfile', null);
        }

        $scope.GoToNotifications = function () {
           // isPageReInit = false;
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

