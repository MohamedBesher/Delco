/// <reference path="../js/angular.js" />

myApp.angular.controller('addRequestController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;
    var initMapBack;
    var initRequestLocationLatitude;
    var initRequestLocationLongitiude;
    var requestPrice;
    $scope.form = {};
    $scope.addRequestForm = {};

    function LoadCities(callBack) {
        var cities = [];
        SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
        appServices.CallService('signupAgent', 'GET', "api/City/GetAll", "", function (cities) {
            SpinnerPlugin.activityStop();
            if (cities) {
                $scope.cites = cities;
                myApp.fw7.Cities = cities;
                callBack(cities);
            }
        });
    }

    $scope.CalculateRequestPrice = function () {        
        var priceParamters = {
            CityId: $scope.addRequestForm.requestplaceID,
            ToLatitude:initRequestLocationLatitude,
            ToLongitude:initRequestLocationLongitiude,
        }
        if ($scope.addRequestForm.requestplaceID && initRequestLocationLatitude && initRequestLocationLongitiude) {
            appServices.CallService('addRequest', 'POST', "api/Price/CalculateRequestPrice", priceParamters, function (priceResult) {
                if (priceResult) {
                    requestPrice = priceResult;
                    $scope.addRequestForm.requestPrice = priceResult + "ريال";
                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                }
            });
        }
    }

    $document.ready(function () {
        app.onPageInit('addRequest', function (page) {
            if ($rootScope.currentOpeningPage != 'addRequest') return;
            $rootScope.currentOpeningPage = 'addRequest';

        });
        app.onPageReinit('addRequest', function (page) {
            if ($rootScope.currentOpeningPage != 'addRequest') return;
            $rootScope.currentOpeningPage = 'addRequest';
            if ($rootScope.mapClick) {
                $rootScope.mapClick = false;
                helpers.geocodeLatLng($rootScope.latLng.lat(), $rootScope.latLng.lng(), function (DeliveryLocation) {
                    $scope.addRequestForm.DeliveryAddress = DeliveryLocation;
                    initRequestLocationLatitude = $rootScope.latLng.lat();
                    initRequestLocationLongitiude = $rootScope.latLng.lng();
                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                    $scope.CalculateRequestPrice();
                })

            }
        });

        app.onPageBeforeAnimation('addRequest', function (page) {
            if ($rootScope.currentOpeningPage != 'addRequest') return;
            $rootScope.currentOpeningPage = 'addRequest';
            if (!$rootScope.initTripMap) {
                $scope.resetForm();
            }
            $rootScope.initTripMap=false;
           

        });

        app.onPageAfterAnimation('addRequest', function (page) {
            if ($rootScope.currentOpeningPage != 'addRequest') return;
            $rootScope.currentOpeningPage = 'addRequest';

            LoadCities(function (requestPlaces) {
                $scope.requestPlaces = requestPlaces;
                setTimeout(function () {
                    $scope.$apply();
                }, fw7.DelayBeforeScopeApply);
            });
         
        });

        $scope.submitForm = function (isValid) {
            $scope.addRequestReset = true;
            if (isValid) {
                var requestDetails = $scope.addRequestForm.requestDetails;
                var DeliveryAddress = $scope.addRequestForm.DeliveryAddress;
                var requestHomeNumber = $scope.addRequestForm.requestHomeNumber;
                var requestplaceID = $scope.addRequestForm.requestplaceID;

                var requestParamters = {
                    "id": 1,
                    "address": requestHomeNumber,
                    "fromLongtitude": "",
                    "fromLatitude": "",
                    "fromLocation": "",
                   "toLongtitude": initRequestLocationLongitiude,
                    "toLatitude": initRequestLocationLatitude,
                    "toLocation": $scope.addRequestForm.DeliveryAddress,
                    "price": requestPrice,
                    "Description":requestDetails,
                    "cityId": requestplaceID,
                    "userId": null,
                    "agentId": null,
                    "passengerNumber": 1,
                    "status": 1,
                    "type": 2
                };

                appServices.CallService('places', 'POST', "api/Request/SaveRequest", requestParamters, function (requestSaveResult) {                
                    if (requestSaveResult) {
                        $scope.resetForm();
                        var successMsg = ' تم إضافة الطلب رقم ';
                        successMsg += ' ' + requestSaveResult.id + ' ';
                        successMsg += ' بنجاح ';

                        language.openFrameworkModal('نجاح', successMsg, 'alert', function () { helpers.GoToPage('home', null); });
                    }
                    else {
                        language.openFrameworkModal('خطأ', 'حدث خطا اثناء اضافة طلب جديد', 'alert', function () { });
                    }

                });
            }
        };

        $scope.selectRequestOrderPLace = function (placeId) {
            $scope.CalculateRequestPrice();
        }
        $scope.resetForm = function () {
            $scope.addRequestReset = false;
            $scope.addRequestForm.requestplaceID = "";
            $('#orderPlace .item-after').html('مكان الطلب');
            $scope.addRequestForm.requestDetails = null;
            $scope.addRequestForm.DeliveryAddress = null;
            $scope.addRequestForm.requestHomeNumber = null;
            $scope.addRequestForm.requestPrice = null;
            if (typeof $scope.AddRequestForm != 'undefined' && $scope.AddRequestForm != null) {
                $scope.AddRequestForm.$setPristine(true);
                $scope.AddRequestForm.$setUntouched();
            }
        }

        $scope.openRequestMap = function () {
            helpers.GoToPage('tripMap', null);
        }
      
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

