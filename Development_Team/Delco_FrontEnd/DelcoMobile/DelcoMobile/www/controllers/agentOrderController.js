/// <reference path="../js/angular.js" />

myApp.angular.controller('agentOrderController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;
    var currentTab = 'ordersUnderWay';
    var orderStatus = 4;
    var isAgentPull = false;
    var loadingUnderWayOrders;
    var loadingExcutedOrders;
    var loadingdeletedOrders;
    var  agentOrdersUnderWay = [];
    var  agentOrdersExcuted = [];
    var agentOrdersDeleted = [];

    $document.ready(function () {
        app.onPageInit('agentOrder', function (page) {
            if ($rootScope.currentOpeningPage != 'agentOrder') return;
            $rootScope.currentOpeningPage = 'agentOrder';

            $$('#divInfiniteAgentOrders').on('ptr:refresh', function (e) {
                isAgentPull = true;
                if (currentTab == 'ordersUnderWay') {
                    getAgentOrders(function (allOrders) {
                        if (allOrders) {
                            $scope.agentUnderWayOrders = allOrders;
                            app.pullToRefreshDone();
                            setTimeout(function () {
                                $scope.$apply();
                            }, fw7.DelayBeforeScopeApply);
                        }
                        else {
                            app.pullToRefreshDone();
                        }
                    },true);
                 
                }
                else if (currentTab == 'ordersExcuted') {
                    getAgentOrders(function (allOrders) {
                        if (allOrders) {
                            $scope.agentExcutedOrders = allOrders;
                            app.pullToRefreshDone();
                            setTimeout(function () {
                                $scope.$apply();
                            }, fw7.DelayBeforeScopeApply);
                        }
                        else {
                            app.pullToRefreshDone();
                        }
                    },true);
                }
                else if (currentTab == 'ordersDeleted') {
                    getAgentOrders(function (allOrders) {
                        if (allOrders) {
                            $scope.agentDeletedOrders = allOrders;
                            app.pullToRefreshDone();
                            setTimeout(function () {
                                $scope.$apply();
                            }, fw7.DelayBeforeScopeApply);
                        }
                        else {
                            app.pullToRefreshDone();
                        }
                    },true);
                }
               
            });

            $$('#divInfiniteAgentOrders').on('infinite', function () {
                if (currentTab == 'ordersUnderWay') {                
                    if (loadingUnderWayOrders) return;
                    loadingUnderWayOrders = true;
                    SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                    var underWayParamter = {
                        "status": orderStatus,
                        "type": null,
                        "pageNumber": CookieService.getCookie('agentOrdersUnderWay-page-number'),
                        "pageSize": fw7.PageSize
                    };
                    appServices.CallService('places', 'POST', "api/Request/AgentRequest", underWayParamter, function (underWayOrders) {
                        SpinnerPlugin.activityStop();
                        if (underWayOrders != null && underWayOrders.length > 0) {
                            loadingUnderWayOrders = false;
                            angular.forEach(underWayOrders, function (underWay) {
                                agentOrdersUnderWay.push(underWay);                             
                                CookieService.setCookie('userOrdersOffered-page-number', parseInt(CookieService.getCookie('userOrdersOffered-page-number')) + 1);
                            });

                            if (underWayOrders.length <= fw7.PageSize) {
                                app.detachInfiniteScroll('#divInfiniteAgentOrders');
                                SpinnerPlugin.activityStop();
                                setTimeout(function () {
                                    $scope.$apply();
                                }, fw7.DelayBeforeScopeApply);
                                return;
                            }
                        }
                        else {
                            loadingUnderWayOrders = false;
                            app.detachInfiniteScroll('#divInfiniteAgentOrders');
                            SpinnerPlugin.activityStop();
                            $scope.agentOrdersUnderWayDivAlert = false;
                        }
                        setTimeout(function () {
                            $scope.$apply();
                        }, fw7.DelayBeforeScopeApply);
                    });
                }

                else if (currentTab == 'ordersExcuted') {
                    if (loadingUserOffersOrders) return;
                    loadingUserOffersOrders = true;
                    SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                    var ExcutedParamter = {
                        "status": orderStatus,
                        "type": null,
                        "pageNumber": CookieService.getCookie('agentOrdersExcuted-page-number'),
                        "pageSize": fw7.PageSize
                    };
                    appServices.CallService('places', 'POST', "api/Request/AgentRequest", ExcutedParamter, function (excutedOrders) {
                        SpinnerPlugin.activityStop();
                        if (excutedOrders != null && excutedOrders.length > 0) {
                            loadingUserOffersOrders = false;
                            angular.forEach(excutedOrders, function (excuted) {
                                agentOrdersExcuted.push(excuted);
                                CookieService.setCookie('agentOrdersExcuted-page-number', parseInt(CookieService.getCookie('agentOrdersExcuted-page-number')) + 1);
                            });

                            if (offeredOrders.length <= fw7.PageSize) {
                                app.detachInfiniteScroll('#divInfiniteAgentOrders');
                                SpinnerPlugin.activityStop();
                                setTimeout(function () {
                                    $scope.$apply();
                                }, fw7.DelayBeforeScopeApply);
                                return;
                            }
                        }
                        else {
                            loadingUserOffersOrders = false;
                            app.detachInfiniteScroll('#divInfiniteAgentOrders');
                            SpinnerPlugin.activityStop();
                        }
                        setTimeout(function () {
                            $scope.$apply();
                        }, fw7.DelayBeforeScopeApply);
                    });

                }

                else if (currentTab == 'OrdersDeleted') {
                    if (loadingUserOffersOrders) return;
                    loadingUserOffersOrders = true;
                    SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                    var offeredParamter = {
                        "status": orderStatus,
                        "type": null,
                        "pageNumber": CookieService.getCookie('agentOrdersCompleted-page-number'),
                        "pageSize": fw7.PageSize
                    };
                    appServices.CallService('places', 'POST', "api/Request/AgentRequest", offeredParamter, function (deletedOrders) {
                        SpinnerPlugin.activityStop();
                        if (deletedOrders != null && deletedOrders.length > 0) {
                            loadingUserOffersOrders = false;
                            angular.forEach(deletedOrders, function (deleted) {
                                agentOrdersDeleted.push(deleted);
                                CookieService.setCookie('agentOrdersCompleted-page-number', parseInt(CookieService.getCookie('userOrdersDeleted-page-number')) + 1);
                            });

                            if (offeredOrders.length <= fw7.PageSize) {
                                SpinnerPlugin.activityStop();
                                app.detachInfiniteScroll('#divInfiniteAgentOrders');

                                setTimeout(function () {
                                    $scope.$apply();
                                }, fw7.DelayBeforeScopeApply);
                                return;
                            }
                        }
                        else {
                            loadingUserOffersOrders = false;
                            app.detachInfiniteScroll('#divInfiniteAgentOrders');
                            SpinnerPlugin.activityStop();
                        }
                        setTimeout(function () {
                            $scope.$apply();
                        }, fw7.DelayBeforeScopeApply);
                    });

                }


                                                   
            });
        });

        app.onPageBeforeAnimation('agentOrder', function (page) {
            if ($rootScope.currentOpeningPage != 'agentOrder') return;
            $rootScope.currentOpeningPage = 'agentOrder';
            app.showTab('#agentUnderWayOrders');
        });

        app.onPageAfterAnimation('agentOrder', function (page) {
            if ($rootScope.currentOpeningPage != 'agentOrder') return;
            $rootScope.currentOpeningPage = 'agentOrder';
            SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
            orderStatus = 4;
            CookieService.setCookie('agentOrdersUnderWay-page-number', 1);
            CookieService.setCookie('agentOrdersExcuted-page-number', 1);
            CookieService.setCookie('agentOrdersCompleted-page-number', 1);
            getAgentOrders(function (allOrders) {
                if (allOrders) {
                    $scope.agentUnderWayOrders = allOrders;
                    SpinnerPlugin.activityStop();
                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                }
            },false);

        });
     
        $('#agentUnderWayOrders').on('show', function () {
            agentOrdersUnderWay = [];
            currentTab = 'ordersUnderWay';
            orderStatus = 4;
            getAgentOrders(function (allOrders) {
                if (allOrders) {
                    $scope.agentUnderWayOrders = allOrders;
                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                }
            },false);

        });
         
        $('#agentExcutedOrders').on('show', function () {
            agentOrdersExcuted = [];
            currentTab = 'ordersExcuted';
            orderStatus = 5;
            getAgentOrders(function (allOrders) {
                if (allOrders) {
                    $scope.agentExcutedOrders = allOrders;
                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                }
            },false);

        });
         
        $('#agentDeletedOrders').on('show', function () {
            agentOrdersDeleted = [];
            currentTab = 'ordersDeleted';
            orderStatus = 6;
            getAgentOrders(function (allOrders) {
                if (allOrders) {
                    $scope.agentDeletedOrders = allOrders;
                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                }
            });
        });
     
    
        function getAgentOrders(callBack, isAgentPull) {
            if (!isAgentPull) {
                SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
            }
            agentOrdersUnderWay = [];
            agentOrdersExcuted = [];
            agentOrdersDeleted = [];
            var offeredParamter = {
                "status": orderStatus,
                "type": null,
                "pageNumber": 0,
                "pageSize": 1
            };
            appServices.CallService('places', 'POST', "api/Request/AgentRequest", offeredParamter, function (offeredOrders) {
                if (offeredOrders && offeredOrders.length > 0) {
                    app.attachInfiniteScroll('#divInfiniteAgentOrders');
                    SpinnerPlugin.activityStop();
                    $scope.agentOrdersUnderWayDivAlert = false;
                    $scope.agentOrdersExcutedDivAlert = false;
                    $scope.agentOrdersDeletedDivAlert = false;
                    angular.forEach(offeredOrders, function (offer) {
                        offer.agentOrderName = offer.type == 1 ? ' طلب مشوار ' + offer.id : ' طلب جديد ' + offer.id;
                        offer.agentUserName = offer.agentUserName ? offer.agentUserName : 'غير معروف';
                        offer.userFullName = offer.userFullName != null ? offer.userFullName : 'غير معروف';
                        if (orderStatus == 4) {
                            agentOrdersUnderWay.push(offer);
                        }
                        else if (orderStatus == 5) {
                            agentOrdersExcuted.push(offer);
                        }
                        else if (orderStatus == 6) {
                            agentOrdersDeleted.push(offer);
                        }
                        if (orderStatus == 4) {                          
                            callBack(agentOrdersUnderWay);
                        }
                        else if (orderStatus == 5) {                       
                            callBack(agentOrdersExcuted);
                        }
                        else if (orderStatus == 6) {
                            callBack(agentOrdersDeleted);
                        }
                    });
                    if (offeredOrders <= fw7.PageSize) {
                        setTimeout(function () {
                            $scope.$apply();
                        }, fw7.DelayBeforeScopeApply);
                    }

                }
                else {
                    app.attachInfiniteScroll('#divInfiniteAgentOrders');
                    SpinnerPlugin.activityStop();
                    if (isAgentPull) {
                        isAgentPull = false;
                        callBack(null);
                    }
                 
                    else if (orderStatus == 4) {
                         $scope.agentOrdersUnderWayAlert = "لايوجد طلبات قيد التنفيذ";
                        $scope.agentOrdersUnderWayDivAlert = true;
                    }
                    else if (orderStatus == 5) {
                        $scope.agentOrdersExcutedAlert = "لايوجد طلبات منفذة";
                        $scope.agentOrdersExcutedDivAlert = true;
                    }
                    else if (orderStatus == 6) {
                        $scope.agentOrdersDeletedAlert = "لايوجد طلبات محذوفة";
                        $scope.agentOrdersDeletedDivAlert = true;
                    }
                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                }
            });
        }
    
           $scope.clientCall = function (contactPhone) {
               window.plugins.CallNumber.callNumber(
            function onSuccess(successResult) {
                console.log("Success:" + successResult);
            },  function onError(errorResult) {
                console.log("Error:" + errorResult);
            }, contactPhone, true);
 
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

           $scope.goToOrderDetails = function (order) {
               if (order.type == 1) {
                   helpers.GoToPage('requestDetails', { tripId: order.id});
               }
               else {
                   helpers.GoToPage('requestDetails', { requestId: order.id });
               }
        };

        app.init();
    });

}]);

