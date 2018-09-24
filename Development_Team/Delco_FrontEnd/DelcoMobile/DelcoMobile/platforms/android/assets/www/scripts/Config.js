/// <reference path="../js/angular.js" />
/// <reference path="../js/framework7.js" />
/// <reference path="../js/jquery-2.1.0.js" />

var isAndroid = Framework7.prototype.device.android === true;
var isIos = Framework7.prototype.device.ios === true;

Template7.global = {
    android: isAndroid,
    ios: isIos
};

var $$ = Dom7;

if (isAndroid) {
    $$('.view.navbar-through').removeClass('navbar-through').addClass('navbar-fixed');
    $$('.view .navbar').prependTo('.view .page');
}

if (isIos) {
    $$('.view.navbar-fixed').removeClass('navbar-fixed').addClass('navbar-through');
    $$('.view .navbar').prependTo('.view .page');
}

var myApp = {};

myApp.config = {
};

var compareTo = function () {
    return {
        require: "ngModel",
        scope: {
            otherModelValue: "=compareTo"
        },
        link: function (scope, element, attributes, ngModel) {

            ngModel.$validators.compareTo = function (modelValue) {
                return modelValue == scope.otherModelValue;
            };

            scope.$watch("otherModelValue", function () {
                ngModel.$validate();
            });
        }
    };
};

myApp.angular = angular.module('myApp', ['ErrorCatcher', 'jkAngularRatingStars']).config(function ($provide) {
    $provide.decorator("$exceptionHandler", function ($delegate, $injector, $log) {
        return function (exception, cause) {
            $log.error(exception)
            $delegate(exception, cause);
        };
    });
}).directive('homePage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'homeController',
        scope: {},
        templateUrl: 'pages/home.html'
    };
}).directive('landingPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'landingController',
        scope: {},
        templateUrl: 'pages/landing.html'
    };
}).directive('loginPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'loginController',
        scope: {},
        templateUrl: 'pages/login.html'
    };
}).directive('usersPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'usersController',
        scope: {},
        templateUrl: 'pages/users.html'
    };
}).directive('signupUserPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'signupUserController',
        scope: {},
        templateUrl: 'pages/signupUser.html'
    };
}).directive('signupAgentPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'signupAgentController',
        scope: {},
        templateUrl: 'pages/signupAgent.html'
    };
}).directive('forgetPassPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'forgetPasswordController',
        scope: {},
        templateUrl: 'pages/forgetPassword.html'
    };
}).directive('resetPasswordPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'resetPasswordController',
        scope: {},
        templateUrl: 'pages/resetPassword.html'
    };
}).directive('changePassPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'changePasswordController',
        scope: {},
        templateUrl: 'pages/changePassword.html'
    };
}).directive('activationPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'activationController',
        scope: {},
        templateUrl: 'pages/activation.html'
    };
}).directive('userProfilePage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'userProfileController',
        scope: {},
        templateUrl: 'pages/userProfile.html'
    };
}).directive('contactPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'contactController',
        scope: {},
        templateUrl: 'pages/contact.html'
    };
}).directive('editProfilePage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'editProfileController',
        scope: {},
        templateUrl: 'pages/editProfile.html'
    };
}).directive('agentProfilePage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'agentProfileController',
        scope: {},
        templateUrl: 'pages/agentProfile.html'
    };
}).directive('userOrderPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'userOrderController',
        scope: {},
        templateUrl: 'pages/userOrder.html'
    };
}).directive('agentOrderPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'agentOrderController',
        scope: {},
        templateUrl: 'pages/agentOrder.html'
    };
}).directive('tripDetailsPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'tripDetailsController',
        scope: {},
        templateUrl: 'pages/tripDetails.html'
    };
}).directive('requestDetailsPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'requestDetailsController',
        scope: {},
        templateUrl: 'pages/requestDetails.html'
    };
}).directive('addTripPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'addTripController',
        scope: {},
        templateUrl: 'pages/addTrip.html'
    };
}).directive('addRequestPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'addRequestController',
        scope: {},
        templateUrl: 'pages/addRequest.html'
    };
}).directive('reportPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'reportController',
        scope: {},
        templateUrl: 'pages/report.html'
    };
}).directive('termsPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'termsController',
        scope: {},
        templateUrl: 'pages/terms.html'
    };
}).directive('userNotificationPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'userNotificationController',
        scope: {},
        templateUrl: 'pages/userNotification.html'
    };
}).directive('agentNotificationPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'agentNotificationController',
        scope: {},
        templateUrl: 'pages/agentNotification.html'
    };
}).directive('mapTrackPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'mapTrackController',
        scope: {},
        templateUrl: 'pages/mapTrack.html'
    };
}).directive('tripMapPage', function () {
    return {
        restrict: 'E',
        bindToController: true,
        controller: 'tripMapController',
        scope: {},
        templateUrl: 'pages/tripMap.html'
    };
});

myApp.angular.directive('fallbackSrc', function () {
    var fallbackSrc = {
        link: function postLink(scope, iElement, iAttrs) {
            iElement.bind('error', function () {
                angular.element(this).attr("src", iAttrs.fallbackSrc);
            });
        }
    }
    return fallbackSrc;
});

myApp.fw7 = {
    app: new Framework7({
        swipeBackPage: false,
        swipePanel: false,
        panelsCloseByOutside: true,
        animateNavBackIcon: true,
        material: isAndroid ? true : false,
        materialRipple: false,
        modalButtonOk: 'تم',
        modalButtonCancel: 'إلغاء'
    }),
    options: {
        dynamicNavbar: true,
        domCache: true
    },
    views: [],
    Countries: [],
    Cities: [],
    Categories: [],
    Advertisements: [],
    DelayBeforeScopeApply: 100,
    PageSize: 5,
    proxy: {},
    connection: {},
    apiUrl: 'http://delcoapi.saned-projects.com/'
};