/// <reference path="../js/angular.js" />

myApp.angular.controller('requestDetailsController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', 'SignalRFactory', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers, SignalRFactory) {
    'use strict';

    //New=1,
    //Inprogress=4,
    //Delivered= 5,
    //Canceled = 6,

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;
    var initBack = true;
    var watchID;

    $document.ready(function () {
        app.onPageInit('requestDetails', function (page) {
            if ($rootScope.currentOpeningPage != 'requestDetails') return;
            $rootScope.currentOpeningPage = 'requestDetails';

        });

        app.onPageBeforeAnimation('requestDetails', function (page) {
            if ($rootScope.currentOpeningPage != 'requestDetails') return;
            $rootScope.currentOpeningPage = 'requestDetails';
            var newRequestId = page.query.requestId;
               
            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));

            var params = {
                'UserId': userLoggedIn.id,
                'RequestId': newRequestId
            };

            SignalRFactory.configureSignalR('joinToGroup', JSON.stringify(params));
            
            var params = {
                "Id": newRequestId
            };

            SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
            appServices.CallService('requestDetails', 'POST', "api/Request/RequestDetails", params, function (reqRequest) {
                SpinnerPlugin.activityStop();
                if (reqRequest) {
                    $scope.showTimer = userLoggedIn.roles == 'Agent' && (reqRequest.status == 1 || reqRequest.status == 2) ? true : false;
                    $scope.hasAgent = userLoggedIn.roles == 'User' && (reqRequest.status == 4 || reqRequest.status == 5) ? true : false;
                    $scope.hasRatings = typeof reqRequest.degree != 'undefined' && reqRequest.degree != null ? true : false;
                    $scope.isCancelled = reqRequest.status == 6 ? true : false;
                    reqRequest.passengerNumber = reqRequest.passengerNumber == null ? 0 : reqRequest.passengerNumber;
                    $scope.request = reqRequest;

                    if (reqRequest.status == 1) {
                        helpers.ClearTimer(true);
                        helpers.InitiateTimer('clockDivRequestDetails', function (IsTimerEnds) {
                            $scope.ConfirmRequestForAgent(reqRequest);
                        });
                    }

                    $scope.canTrack = false;

                    $rootScope.TrackUserIfAgent(reqRequest);
                    $rootScope.proxy.on('agantMove', function (agentLocation) {
                        console.log(JSON.stringify(agentLocation));
                    });

                    if (reqRequest.status == 1) {
                        $scope.canCall = userLoggedIn.roles == 'Agent' ? true : false;
                        $scope.canConfirm = userLoggedIn.roles == 'Agent' ? true : false;
                        $scope.canCancel = userLoggedIn.roles == 'Agent' ? true : false;
                        $scope.statusText = 'طلب جديد';
                    }
                    else if (reqRequest.status == 2 || reqRequest.status == 3) {
                        $scope.canCall = userLoggedIn.roles == 'Agent' ? true : false;
                        $scope.canConfirm = false;
                        $scope.canCancel = false;
                        $scope.statusText = 'طلب تم تأكيده';
                    }
                    else if (reqRequest.status == 4) {
                        $scope.canCall = userLoggedIn.roles == 'Agent' ? true : false;
                        $scope.canConfirm = false;
                        $scope.canCancel = false;
                        $scope.statusText = 'طلب قيد التنفيذ';
                    }
                    else if (reqRequest.status == 5) {
                        $scope.canCall = false;
                        $scope.canConfirm = false;
                        $scope.canCancel = false;
                        $scope.statusText = 'طلب تم تنفيذه';
                    }
                    else {
                        $scope.canCall = userLoggedIn.roles == 'Agent' ? true : false;
                        $scope.canConfirm = false;
                        $scope.canCancel = false;
                        $scope.statusText = 'طلب ملغي';
                    }
                }
                else {
                    language.openFrameworkModal('خطأ', 'خطا اثناء استرجاع بيانات الطلب', 'alert', function () {
                        helpers.GoToPage('home', null);
                    });
                }

            });

            setTimeout(function () {
                $scope.$apply();
            }, fw7.DelayBeforeScopeApply);

        });

        app.onPageAfterAnimation('requestDetails', function (page) {
            if ($rootScope.currentOpeningPage != 'requestDetails') return;
            $rootScope.currentOpeningPage = 'requestDetails';
            
        });

        function cancelRequest(request, PageToGo,userId) {
            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
                    language.openFrameworkModal('تأكيد', ' سيتم اغلاق الحساب اذا وصل عدد مرات الاغلاق الي 3 مرات ', 'confirm', function () {
                        var params = {
                            "Id": request.id
                        };
                        appServices.CallService('requestDetails', 'POST', "api/Request/CancelAgentRequest", params, function (result) {
                            SpinnerPlugin.activityStop();
                            if (result!=null) {
                                appServices.CallService('requestDetails', 'GET', "api/RefuseRequest/RefuseAgentCount", '', function (cancelCount) {
                                    if (cancelCount==3) {
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
                               // language.openFrameworkModal('خطأ', 'خطأ اثناء الغاء الطلب', 'alert', function () { });
                            }

                            setTimeout(function () {
                                $scope.$apply();
                            }, fw7.DelayBeforeScopeApply);

                        });

                    });
  
        }

        $scope.clientCall = function (contactPhone) {
            window.plugins.CallNumber.callNumber(
         function onSuccess(successResult) {
             console.log("Success:" + successResult);
         }, function onError(errorResult) {
             console.log("Error:" + errorResult);
         }, contactPhone, true);

        }

        $scope.CancelRequestForAgent = function (request, callBack) {
            cancelRequest(request, 'Back');
        };

        $scope.GoBackAndCancelRequest = function (request) {
            initBack = true;
            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
            if (typeof userLoggedIn.roles === 'undefined' || userLoggedIn.roles == 'Agent' && $scope.request.status == 1) {
                cancelRequest(request, 'Back');
            }
            else {
                helpers.GoBack();
            }
           
        };

        $scope.ConfirmRequestForAgent = function (request) {
            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
            

            if (angular.isUndefined(request.id)) {
                request.id = request;
            }
            var params = {
                "Id": request.id
            };

            SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
            appServices.CallService('requestDetails', 'POST', "api/Request/ConfirmRequest", params, function (result) {
                SpinnerPlugin.activityStop();
                if (result) {
                    helpers.GoToPage('home', null);
                }
                else {
                    language.openFrameworkModal('خطأ', 'خطأ اثناء تأكيد الطلب', 'alert', function () {
                        helpers.GoToPage('home', null);
                    });
                }

                setTimeout(function () {
                    $scope.$apply();
                }, fw7.DelayBeforeScopeApply);

            });
        };

        $scope.RateRequest = function (rate, request) {
            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));

            
            var ratingParams = {
                  "id": 1,
                 "totalDegree": 5,
                 "agentId": request.agentId,
                 "degree": rate
            };

            SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
            appServices.CallService('requestDetails', 'POST', "api/Rating/SaveRating", ratingParams, function (ratingResult) {
                SpinnerPlugin.activityStop();
                if (ratingResult && ratingResult == 1) {
                    language.openFrameworkModal('نجاح', 'تم اضافة التقييم بنجاح', 'alert', function () { });
                }
                else {
                    language.openFrameworkModal('خطأ', 'خطا اثناء اضافة التقييم', 'alert', function () { });
                }
            });

        };

        $scope.GoToUserDetails = function (request,userId) {
            helpers.ClearTimer(true);
            initBack = true;

            for (var i = 0; i < fw7.views[0].history.length; i++) {
                if (fw7.views[0].history[i] === '#tripDetails' || fw7.views[0].history[i] === '#requestDetails') fw7.views[0].history.splice(i, 1);
            }

            cancelRequest(request, 'userProfile', userId);
        };

        $scope.GoToMapTrack = function (request) {
            helpers.GoToPage('mapTrack', { reqRequest: JSON.stringify(request) });
        };

        $scope.GoToNotifications = function (request) {
            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
            helpers.ClearTimer(true);
            initBack = true;

            for (var i = 0; i < fw7.views[0].history.length; i++) {
                if (fw7.views[0].history[i] === '#tripDetails' || fw7.views[0].history[i] === '#requestDetails') fw7.views[0].history.splice(i, 1);
            }

            cancelRequest(request, 'notification');
        };

        app.init();
    });

}]);

