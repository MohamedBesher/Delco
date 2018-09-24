/// <reference path="../js/angular.js" />

myApp.angular.controller('signupAgentController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;

    function LoadCities(callBack) {
        var cities = [];
        SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
        appServices.CallService('signupAgent', 'GET', "api/City/GetAll", "", function (cities) {
            if (cities) {
                $scope.cites = cities;
                myApp.fw7.Cities = cities;
                callBack(true);
            }
        });
    }

    function LoadPassengers(callBack) {
        var passengers = [];
        appServices.CallService('signupAgent', 'GET', "api/PassengerNumber/PassengerNumbers", "", function (passengers) {
            SpinnerPlugin.activityStop();
            if (passengers) {
                $scope.passengers = passengers;
                callBack(true);
            }
        });

        SpinnerPlugin.activityStop();
        callBack(true);
    }

    $scope.TermsAccepted = function (model) {
        $scope.user.isChecked = !model;
        $scope.$apply();
    }

    $document.ready(function () {
        app.onPageInit('signupAgent', function (page) {
            if ($rootScope.currentOpeningPage != 'signupAgent') return;
            $rootScope.currentOpeningPage = 'signupAgent';
        });

        app.onPageBeforeAnimation('signupAgent', function (page) {
            if ($rootScope.currentOpeningPage != 'signupAgent') return;
            $rootScope.currentOpeningPage = 'signupAgent';

            $scope.resetForm();
        });

        app.onPageAfterAnimation('signupAgent', function (page) {
            if ($rootScope.currentOpeningPage != 'signupAgent') return;
            $rootScope.currentOpeningPage = 'signupAgent';

            

            LoadCities(function () {
                LoadPassengers(function () {
                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                });
            });
        });



        $scope.resetForm = function () {
            $scope.signUpAgentReset = false;
            $scope.agentForm.name = null;
            $scope.agentForm.username = null;
            $scope.agentForm.mobile = null;
            $scope.agentForm.email = null;
            $scope.agentForm.password = null;
            $scope.agentForm.confirmpassword = null;
            $scope.agentForm.city = null;
            $scope.agentForm.orderType = null;
            $scope.agentForm.carCompany = null;
            $scope.agentForm.carType = null;
            $scope.agentForm.carModel = null;
            $scope.agentForm.carColor = null;
            $scope.agentForm.carTapletteNumber = null;
            $scope.user = {};
            $scope.user.isChecked = false;
            if (typeof $scope.SignupAgentForm != 'undefined' && $scope.SignupAgentForm != null) {
                $scope.SignupAgentForm.$setPristine(true);
                $scope.SignupAgentForm.$setUntouched();
            }
        }



        $scope.submitForm = function (isValid) {
            $scope.signUpAgentReset = true;
            if (isValid) {
                var user = {
                    'FullName': $scope.agentForm.name,
                    'UserName': $scope.agentForm.username,
                    'PhoneNumber': $scope.agentForm.mobile,
                    'Email': $scope.agentForm.email,
                    'Password': $scope.agentForm.password,
                    'ConfirmPassword': $scope.agentForm.confirmpassword,
                    'CityId': $scope.agentForm.city.id,
                    'RequestType': $scope.agentForm.orderType,
                    'CompanyName': $scope.agentForm.carCompany,
                    'Type': $scope.agentForm.carType,
                    'Model': $scope.agentForm.carModel,
                    'Color': $scope.agentForm.carColor,
                    'PlateNumber': $scope.agentForm.carTapletteNumber,
                    'PassengerCount': $scope.agentForm.passengerCount.value,
                    'Role': 'User'
                }

                SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                appServices.CallService('signupAgent', "POST", "api/Account/RegisterAgent", user, function (res2) {
                    if (res2 != null) {
                        CookieService.setCookie('USName', 'مستخدم');
                        CookieService.setCookie('UserID', res2);
                        CookieService.setCookie('UserEntersCode', false);
                        SpinnerPlugin.activityStop();
                        language.openFrameworkModal('نجاح', 'تم تسجيل بياناتك بنجاح , ستصلك رسالة علي البريد الإليكتروني بالموافقة علي طلب إنضمامك للتطبيق .', 'alert', function () {
                            helpers.GoToPage('activation', null);
                        });
                    }
                    else {
                        SpinnerPlugin.activityStop();
                    }
                });
            }
        };

        $scope.form = {};
        $scope.agentForm = {};

        $scope.GoBack = function () {
            helpers.GoBack();
        };

        $scope.GoToTerms = function () {
            helpers.GoToPage('terms', null);
        };

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

        $scope.emailRegex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        $scope.nameRegex = /^[A-Za-z0-9]*$/;

        app.init();

    });
}]);

