/// <reference path="../js/angular.js" />

myApp.angular.controller('tripDetailsController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', 'SignalRFactory', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers, SignalRFactory) {
    'use strict';


        //New=1,
        //Inprogress=4,
        //Delivered= 5,
        //Canceled = 6,

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;
    var watchID;

    function LoadTripDetails(requiredTripId, callBack) {
        var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));

        var params = {
            'UserId': userLoggedIn.id,
            'RequestId': requiredTripId
        };

        SignalRFactory.configureSignalR('joinToGroup', JSON.stringify(params));
        
        var params = {
            "Id": requiredTripId
        };

        SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
        appServices.CallService('tripDetails', 'POST', "api/Request/RequestDetails", params, function (reqTrip) {
            SpinnerPlugin.activityStop();
            if (reqTrip) {
                $scope.showTimer = userLoggedIn.roles == 'Agent' && reqTrip.status == 1 ? true : false;
                $scope.hasAgent = (reqTrip.status == 4 || reqTrip.status == 5) && userLoggedIn.roles == 'User';
                $scope.hasRatings = typeof reqTrip.degree != 'undefined' && reqTrip.degree != null;
                $scope.isCancelled = reqTrip.status == 6;
                reqTrip.passengerNumber = reqTrip.passengerNumber == null ? 0 : reqTrip.passengerNumber;
                $scope.trip = reqTrip;
                
                $rootScope.TrackUserIfAgent(reqRequest);
                $rootScope.proxy.on('agantMove', function (agentLocation) {
                    console.log(JSON.stringify(agentLocation));
                });

                if (reqTrip.status == 1) {
                    helpers.ClearTimer(true);
                    helpers.InitiateTimer('clockDivRequestDetails', function (IsTimerEnds) {
                        $scope.ConfirmRequestForAgent($scope.trip);
                    });
                    $scope.canCall = userLoggedIn.roles == 'Agent' ? true : false;
                    $scope.canTrack = false;
                    $scope.canConfirm = userLoggedIn.roles == 'Agent' ? true : false;
                    $scope.canCancel = userLoggedIn.roles == 'Agent' ? true : false;
                    $scope.canAbuse = false;
                    $scope.statusText = 'مشوار جديد';
                }
                else if (reqTrip.status == 2 || reqTrip.status == 3) {
                    $scope.canCall = userLoggedIn.roles == 'Agent' ? true : false;
                    $scope.canTrack = false;
                    $scope.canConfirm = false;
                    $scope.canCancel = false;
                    $scope.canAbuse = true;
                    $scope.statusText = 'مشوار تم تأكيده';
                }
                else if (reqTrip.status == 4) {
                    $scope.canCall = userLoggedIn.roles == 'Agent' ? true : false;
                    $scope.canTrack = userLoggedIn.roles == 'User' ? true : false;
                    $scope.canConfirm = false;
                    $scope.canCancel = false;
                    $scope.canAbuse = true;
                    $scope.statusText = 'مشوار قيد التنفيذ';
                }
                else if (reqTrip.status == 5) {
                    $scope.canCall = false;
                    $scope.canTrack = false;
                    $scope.canConfirm = false;
                    $scope.canCancel = false;
                    $scope.canAbuse = true;
                    $scope.statusText = 'مشوار تم تنفيذه';
                }
                else {
                    $scope.canCall = userLoggedIn.roles == 'Agent' ? true : false;
                    $scope.canTrack = false;
                    $scope.canConfirm = false;
                    $scope.canCancel = false;
                    $scope.canAbuse = true;
                    $scope.statusText = 'مشوار ملغي';
                }
            }
            else {
                language.openFrameworkModal('خطأ', 'خطا اثناء استرجاع بيانات الرحلة', 'alert', function () {
                    helpers.GoToPage('home', null);
                });
            }

            setTimeout(function () {
                $scope.$apply();
                callBack(true);
            }, fw7.DelayBeforeScopeApply);

        });
    }

    $document.ready(function () {
        

        app.onPageInit('tripDetails', function (page) {
            if ($rootScope.currentOpeningPage != 'tripDetails') return;
            $rootScope.currentOpeningPage = 'tripDetails';

        });

        app.onPageReinit('tripDetails', function (page) {
            if ($rootScope.currentOpeningPage != 'tripDetails') return;
            $rootScope.currentOpeningPage = 'tripDetails';
            
        });

        app.onPageBeforeAnimation('tripDetails', function (page) {
            if ($rootScope.currentOpeningPage != 'tripDetails') return;
            $rootScope.currentOpeningPage = 'tripDetails';


        });

        app.onPageAfterAnimation('tripDetails', function (page) {
            if ($rootScope.currentOpeningPage != 'tripDetails') return;
            $rootScope.currentOpeningPage = 'tripDetails';
            var tripId = page.query.tripId;

            helpers.ClearTimer(true);
            LoadTripDetails(tripId, function (result) {
                helpers.InitiateTimer('clockDivTripDetails', function (IsTimerEnds) {
                    $scope.ConfirmTripForAgent($scope.trip);
                });
            });

            setTimeout(function () {
                $scope.$apply();
            }, fw7.DelayBeforeScopeApply);
        });

        function cancelTrip(trip, PageToGo, userId) {
            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
            language.openFrameworkModal('تأكيد', 'هل أنت متأكد من إلغاء الرحلة ؟', 'confirm', function () {
                var params = {
                    "Id": trip.id
                };

                SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                appServices.CallService('tripDetails', 'POST', "api/Request/CancelAgentRequest", params, function (result) {
                    SpinnerPlugin.activityStop();
                    if (result) {
                        appServices.CallService('requestDetails', 'GET', "api/RefuseRequest/RefuseAgentCount", '', function (cancelCount) {
                            if (cancelCount == 3) {
                                language.openFrameworkModal('خطأ', 'تم اغلاق حسابك', 'alert', function () { });
                                helpers.GoToPage('login', null);
                                return;
                            }
                        })
                        if (initBack) {
                            initBack = false;
                            helpers.ClearTimer(true);
                            if (PageToGo == 'Back') {
                                helpers.GoBack();
                            }
                            else if (PageToGo == 'userProfile') {
                                helpers.GoToPage('userProfile', { userId: userId });
                            }
                            else if (PageToGo == 'agentProfile') {
                                helpers.GoToPage('agentProfile', { userId: userId });
                            }
                            else if (PageToGo == 'notification') {
                                if (userLoggedIn.roles == 'User') {
                                    helpers.GoToPage('userNotification', null);
                                }
                                else {
                                    helpers.GoToPage('agentNotification', null);
                                }
                            }
                        }
                    }
                    else {
                        language.openFrameworkModal('خطأ', 'خطأ اثناء الغاء  الرحلة', 'alert', function () { });
                    }

                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);

                });

            });

        }

        $scope.ConfirmTripForAgent = function (trip, callBack) {
            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
            
            var params = {
                "Id": trip.id
            };

            SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
            appServices.CallService('tripDetails', 'POST', "api/Request/ConfirmRequest", params, function (result) {
                SpinnerPlugin.activityStop();
                if (result) {
                    helpers.ClearTimer(true);
                    TrackUserIfAgent(trip);
                    helpers.GoToPage('home', null);
                }
                else {
                    language.openFrameworkModal('خطأ', 'خطأ اثناء تأكيد الرحلة', 'alert', function () { });
                }

                setTimeout(function () {
                    $scope.$apply();
                    callBack(true);
                }, fw7.DelayBeforeScopeApply);

            });
        };

        $scope.clientCall = function (contactPhone) {
            window.plugins.CallNumber.callNumber(
         function onSuccess(successResult) {
             console.log("Success:" + successResult);
         }, function onError(errorResult) {
             console.log("Error:" + errorResult);
         }, contactPhone, true);

        }

        $scope.CancelTripForAgent = function (trip, callBack) {
            cancelTrip(trip, 'Back');
        };

        $scope.GoBackAndCancelTrip = function (trip) {    
            initBack = true;
            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
            if (typeof userLoggedIn.roles === 'undefined' || userLoggedIn.roles == 'Agent' && $scope.request.status == 1) {
                cancelTrip(trip, 'Back');
            }
            else {
                helpers.GoBack();
            }
        };

        $scope.ReportTripForAgent = function (trip, callBack) {
            helpers.ClearTimer(true);
            helpers.GoToPage('report', null);
        };

        

        $scope.RateTrip = function (rate, trip) {
            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));

            var ratingParams = {
                "id": 1,
                "totalDegree": 5,
                "agentId": trip.agentId,
                "degree": rate
            };

            SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
            appServices.CallService('tripDetails', 'POST', "api/Rating/SaveRating", ratingParams, function (ratingResult) {
                SpinnerPlugin.activityStop();
                if (ratingResult && ratingResult == 1) {
                    language.openFrameworkModal('نجاح', 'تم اضافة التقييم بنجاح', 'alert', function () { });
                }
                else {
                    language.openFrameworkModal('خطأ', 'خطا اثناء اضافة التقييم', 'alert', function () { });
                }
            });
        };

        $scope.GoToMapTrack = function (trip) {
            helpers.GoToPage('mapTrack', { reqRequest: JSON.stringify(trip) });
        };

        $scope.GoToUserDetails = function (trip, userId) {
            helpers.ClearTimer(true);
            initBack = true;

            for (var i = 0; i < fw7.views[0].history.length; i++) {
                if (fw7.views[0].history[i] === '#tripDetails' || fw7.views[0].history[i] === '#requestDetails') fw7.views[0].history.splice(i, 1);
            }

            cancelTrip(trip, 'userProfile', userId);
        };

        $scope.GoToAgentDetails = function (trip, agentId) {
            helpers.ClearTimer(true);
            initBack = true;

            for (var i = 0; i < fw7.views[0].history.length; i++) {
                if (fw7.views[0].history[i] === '#tripDetails' || fw7.views[0].history[i] === '#requestDetails') fw7.views[0].history.splice(i, 1);
            }

            cancelTrip(trip, 'agentProfile', agentId);
        };

        $scope.GoToNotifications = function (trip) {
            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
            helpers.ClearTimer(true);
            initBack = true;

            for (var i = 0; i < fw7.views[0].history.length; i++) {
                if (fw7.views[0].history[i] === '#tripDetails' || fw7.views[0].history[i] === '#requestDetails') fw7.views[0].history.splice(i, 1);
            }

            cancelTrip(trip, 'notification');
        };

        app.init();
    });

}]);

