/// <reference path="../js/angular.js" />

myApp.angular.controller('addTripController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;
    var initCurrentLocation = false;
    var initDesiredLocation = false;
    var initCurrentLocationStorage;
    var initDesiredLocationStorage;
    var initCurrentLocationLatitude;
    var initCurrentLocationlongtiude;
    var initDesiredLocationLatitude;
    var initDesiredLocationlongtiude;
    var tripPrice;
    $scope.form = {};
    $scope.addTripForm = {};

    function LoadPassengers(callBack) {
        var passengers = [];
        appServices.CallService('addTrip', 'GET', "api/PassengerNumber/PassengerNumbers", "", function (passengers) {
            SpinnerPlugin.activityStop();
            if (passengers) {
                $scope.passengers = passengers;
                callBack(true);
            }
        });

        SpinnerPlugin.activityStop();
        callBack(true);
    }

   function CalculateTripPrice () {
        var priceParamters = {
            FromLatitude: initCurrentLocationLatitude,
            FromLongitude: initCurrentLocationlongtiude,
            ToLatitude: initDesiredLocationLatitude,
            ToLongitude: initDesiredLocationlongtiude,
        }
        if (initCurrentLocationLatitude && initCurrentLocationlongtiude && initDesiredLocationLatitude && initDesiredLocationlongtiude) {
            appServices.CallService('addRequest', 'POST', "api/Price/CalculateTripPrice", priceParamters, function (priceResult) {
                if (priceResult) {
                    tripPrice = priceResult;
                    $scope.addTripForm.tripPrice = priceResult+"ريال ";
                         setTimeout(function () {
                    $scope.$apply();
                }, fw7.DelayBeforeScopeApply);
                }
            });
        }
    }

    $document.ready(function () {

        app.onPageInit('addTrip', function (page) {
            if ($rootScope.currentOpeningPage != 'addTrip') return;
            $rootScope.currentOpeningPage = 'addTrip';
      
        });

        app.onPageReinit('addTrip', function (page) {
            if ($rootScope.currentOpeningPage != 'addTrip') return;
            $rootScope.currentOpeningPage = 'addTrip';
            if ($rootScope.mapClick) {
                $rootScope.mapClick = false;
                helpers.geocodeLatLng($rootScope.latLng.lat(), $rootScope.latLng.lng(), function (DeliveryLocation) {
                    if (initCurrentLocation) {
                        initCurrentLocation = false;
                        $scope.addTripForm.tripCurrentLocation = DeliveryLocation;
                        $scope.addTripForm.tripDesiredLocation = initDesiredLocationStorage;
                        initCurrentLocationStorage = DeliveryLocation;
                        initCurrentLocationLatitude = $rootScope.latLng.lat();
                        initCurrentLocationlongtiude = $rootScope.latLng.lng();
                        CalculateTripPrice()
                    }
                    else {
                        initDesiredLocation = false;
                        $scope.addTripForm.tripDesiredLocation = DeliveryLocation;
                        $scope.addTripForm.tripCurrentLocation = initCurrentLocationStorage;                       
                        initDesiredLocationStorage = DeliveryLocation;                                
                        initDesiredLocationLatitude = $rootScope.latLng.lat();
                        initDesiredLocationlongtiude = $rootScope.latLng.lng();
                        CalculateTripPrice();
                    }
                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                });
            }
        });
        app.onPageBeforeAnimation('addTrip', function (page) {
            if ($rootScope.currentOpeningPage != 'addTrip') return;
            $rootScope.currentOpeningPage = 'addTrip';
            if (!$rootScope.initTripMap) {
                $scope.addTripForm.tripCurrentLocation = null;
                $scope.addTripForm.tripDesiredLocation = null;
                $scope.resetForm();
            }
            $rootScope.initTripMap = false;   

         });

        app.onPageAfterAnimation('addTrip', function (page) {
            if ($rootScope.currentOpeningPage != 'addTrip') return;
            $rootScope.currentOpeningPage = 'addTrip';

            LoadPassengers(function () {
                setTimeout(function () {
                    $scope.$apply();
                }, fw7.DelayBeforeScopeApply);
            });
        });
        $scope.openTripMapForCurrentLocation = function () {
            initCurrentLocation = true;
            helpers.GoToPage('tripMap', null);
        }
        $scope.openTripMapForDesiredLocation = function () {
            initDesiredLocation = true;
            helpers.GoToPage('tripMap', null);
        }

        $scope.submitForm = function (isValid) {
            $scope.addTripReset = true;           
            if (isValid) {
                var tripCurrentLocation = $scope.addTripForm.tripCurrentLocation;
                var tripHomeNumber = $scope.addTripForm.tripHomeNumber;
                var tripDesiredLocation = $scope.addTripForm.tripDesiredLocation;
                var passengersCount = $scope.addTripForm.passengerCount.value;
                    var tripParamters={
                        "id": 1,
                        "address": tripHomeNumber,
                        "fromLongtitude": initDesiredLocationlongtiude,
                        "fromLatitude": initCurrentLocationLatitude,
                        "fromLocation":initCurrentLocationStorage,
                        "toLongtitude": initDesiredLocationlongtiude,
                        "toLatitude": initDesiredLocationLatitude,
                        "toLocation": initDesiredLocationStorage,
                        "price": tripPrice,
                        "cityId": "",
                        "userId": null,
                        "agentId": null,
                        "passengerNumber": $rootScope.pasangerId,
                        "status": 1,
                        "type": 1
                    };

                    appServices.CallService('places', 'POST', "api/Request/SaveRequest", tripParamters, function (tripSaveResult) {
                        if (tripSaveResult) {
                            $scope.resetForm();
                            var successMsg = ' تم إضافة المشوار رقم ';
                            successMsg += ' ' + tripSaveResult.id + ' ';
                            successMsg += ' بنجاح ';

                            language.openFrameworkModal('نجاح', successMsg, 'alert', function () { helpers.GoToPage('home', null); });
                        }
                        else {
                            language.openFrameworkModal('خطأ', 'حدث خطا اثناء اضافة مشوار', 'alert', function () { });
                        }
                    });
            };
        }

        $scope.resetForm = function () {
            $scope.addTripReset = false;
            $scope.addTripForm.passengerCount = "";
            $('#tripPassanger .item-after').html(' عدد ركاب السيارة');
            $scope.addTripForm.tripPrice = null;
            initDesiredLocationlongtiude = null;
            initCurrentLocationLatitude = null;
            initCurrentLocationStorage = null;
            initDesiredLocationlongtiude = null;
            initDesiredLocationLatitude = null;
            initDesiredLocationStorage = null;
            $scope.addTripForm.tripCurrentLocation = null; 
            $scope.addTripForm.tripDesiredLocation = null;
            $scope.addTripForm.tripPassangerCount = null;
            $scope.addTripForm.tripHomeNumber = null;
            //$scope.addTripForm.tripCurrentLocation = null;
            //$scope.addTripForm.tripDesiredLocation = null;
            if (typeof $scope.AddTripForm != 'undefined' && $scope.AddTripForm != null) {
                $scope.AddTripForm.$setPristine(true);
                $scope.AddTripForm.$setUntouched();
            }
        }

        $scope.getPassangerCount = function (selectedPassangerCount) {
            if (selectedPassangerCount) {
                $rootScope.pasangerId = selectedPassangerCount.id;
            }

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

