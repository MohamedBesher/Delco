/// <reference path="../js/angular.js" />

myApp.angular.controller('userOrderController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;
    var currentTab = 'userOrdersOffered';
    var isUserPull = false;
    var loadingUserOffersOrders;
    var loadingUserUnderWayOrders;
    var loadingUserExcutedOrders;
    var loadingUserDeletedOrders;
    var orderStatus = 1;;
    var userOrdersOffered = [];
    var userOrdersUnderWay = [];
    var UserOrdersExcuted = [];
    var userOrdersDeleted = [];

    $document.ready(function () {
        app.onPageInit('userOrder', function (page) {
            if ($rootScope.currentOpeningPage != 'userOrder') return;
            $rootScope.currentOpeningPage = 'userOrder';
            $$('#divInfiniteUserOrders').on('ptr:refresh', function (e) {
                isUserPull = true;
                if (currentTab == 'userOrdersOffered') {
                    getUserOrders(function (allOrders) {
                        if (allOrders) {
                           $scope.userOfferedOrders = allOrders; 
                            app.pullToRefreshDone();
                            setTimeout(function () {
                                $scope.$apply();
                            }, fw7.DelayBeforeScopeApply);
                        }
                        else {
                            app.pullToRefreshDone();
                        }
                    }, true);
                }
                else if (currentTab == 'userOrdersUnderWay') {
                    getUserOrders(function (allOrders) {
                        if (allOrders) {
                            $scope.userUnderWayOrders = allOrders;
                            app.pullToRefreshDone();
                            setTimeout(function () {
                                $scope.$apply();
                            }, fw7.DelayBeforeScopeApply);
                        }
                        else {
                            app.pullToRefreshDone();
                        }
                    }, true);

                }
                else if (currentTab == 'userOrdersExcuted') {
                    getUserOrders(function (allOrders) {
                        if (allOrders) {
                            $scope.userExcutedOrders = allOrders;
                            app.pullToRefreshDone();
                            setTimeout(function () {
                                $scope.$apply();
                            }, fw7.DelayBeforeScopeApply);
                        }
                        else {
                            app.pullToRefreshDone();
                        }
                    }, true);


                }
                else if (currentTab == 'userOrdersDeleted') {
                    getUserOrders(function (allOrders) {
                        if (allOrders) {
                            $scope.userDeletedOrders = allOrders;
                            app.pullToRefreshDone();
                            setTimeout(function () {
                                $scope.$apply();
                            }, fw7.DelayBeforeScopeApply);
                        }
                        else {
                            app.pullToRefreshDone();
                        }
                    }, true);
                }
               
            });

            $$('#divInfiniteUserOrders').on('infinite', function () {
                if (currentTab == 'userOrdersOffered') {
                    if (loadingUserOffersOrders) return;
                    loadingUserOffersOrders = true;
                    SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                    var offeredParamter = {
                        "status": orderStatus,
                        "type": null,
                        "pageNumber": parseInt(CookieService.getCookie('userOrdersOffered-page-number')),
                        "pageSize": 20
                    };
                    appServices.CallService('places', 'POST', "api/Request/UserRequest", offeredParamter, function (offeredOrders) {
                        SpinnerPlugin.activityStop();
                        if (offeredOrders != null && offeredOrders.length > 0) {
                            loadingUserOffersOrders = false;
                            angular.forEach(offeredOrders, function (offer) {
                                offer.userOrderName = offer.type == 1 ? ' طلب مشوار ' + offer.id : ' طلب جديد ' + offer.id;
                                offer.agent = offer.agent != null ? offer.agent : 'غير معروف';
                                userOrdersOffered.push(offer);
                               
                                CookieService.setCookie('userOrdersOffered-page-number', parseInt(CookieService.getCookie('userOrdersOffered-page-number')) + 1);
                            });

                            if ( offeredOrders.length <= fw7.PageSize) {
                               app.detachInfiniteScroll('#divInfiniteUserOrders');

                                setTimeout(function () {
                                    $scope.$apply();
                                }, fw7.DelayBeforeScopeApply);
                                return;
                            }
                        }
                        else {
                            app.detachInfiniteScroll('#divInfiniteUserOrders');
                            loadingUserOffersOrders = false;
                            SpinnerPlugin.activityStop();
                        }
                        setTimeout(function () {
                            $scope.$apply();
                        }, fw7.DelayBeforeScopeApply);
                    });

                }

               else if (currentTab == 'userOrdersUnderWay') {
                    if (loadingUserOffersOrders) return;
                    loadingUserOffersOrders = true;
                    SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                    var offeredParamter = {
                        "status": orderStatus,
                        "type": null,
                        "pageNumber": CookieService.getCookie('userOrdersUnderWay-page-number'),
                        "pageSize": fw7.PageSize
                    };
                    appServices.CallService('places', 'POST', "api/Request/UserRequest", offeredParamter, function (offeredOrders) {
                        SpinnerPlugin.activityStop();
                        if (offeredOrders != null && offeredOrders.length > 0) {
                            loadingUserOffersOrders = false;
                            angular.forEach(offeredOrders, function (offer) {
                                offer.userOrderName = offer.type == 1 ? ' طلب مشوار ' + offer.id : ' طلب جديد ' + offer.id;
                                offer.agent = offer.agent != null ? offer.agent : 'غير معروف';
                                userOrdersUnderWay.push(offer);
                                SpinnerPlugin.activityStop();
                                CookieService.setCookie('userOrdersUnderWay-page-number', parseInt(CookieService.getCookie('userOrdersUnderWay-page-number')) + 1);
                            });

                            if (offeredOrders.length <= fw7.PageSize) {
                                app.detachInfiniteScroll('#divInfiniteUserOrders');
                                setTimeout(function () {
                                    $scope.$apply();
                                }, fw7.DelayBeforeScopeApply);
                                return;
                            }
                        }
                        else {
                            app.detachInfiniteScroll('#divInfiniteUserOrders');
                            loadingUserOffersOrders = false;
                            SpinnerPlugin.activityStop();
                        }
                        setTimeout(function () {
                            $scope.$apply();
                        }, fw7.DelayBeforeScopeApply);
                    });
               }

               else if (currentTab == 'userOrdersExcuted') {
                   if (loadingUserOffersOrders) return;
                   loadingUserOffersOrders = true;
                   SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                   var offeredParamter = {
                       "status": orderStatus,
                       "type": null,
                       "pageNumber": CookieService.getCookie('userOrdersExcuted-page-number'),
                       "pageSize": fw7.PageSize
                   };
                   appServices.CallService('places', 'POST', "api/Request/UserRequest", offeredParamter, function (offeredOrders) {
                       if (offeredOrders != null && offeredOrders.length > 0) {
                           loadingUserOffersOrders = false;
                           angular.forEach(offeredOrders, function (offer) {
                               offer.userOrderName = offer.type == 1 ? ' طلب مشوار ' + offer.id : ' طلب جديد ' + offer.id;
                               offer.agent = offer.agent != null ? offer.agent : 'غير معروف';
                               UserOrdersExcuted.push(offer);
                               SpinnerPlugin.activityStop();
                               CookieService.setCookie('userOrdersExcuted-page-number', parseInt(CookieService.getCookie('userOrdersExcuted-page-number')) + 1);
                           });

                           if (offeredOrders.length <= fw7.PageSize) {
                               app.detachInfiniteScroll('#divInfiniteUserOrders');
                               setTimeout(function () {
                                   $scope.$apply();
                               }, fw7.DelayBeforeScopeApply);
                               return;
                           }
                       }
                       else {
                           app.detachInfiniteScroll('#divInfiniteUserOrders');
                           loadingUserOffersOrders = false;
                           SpinnerPlugin.activityStop();
                       }
                       setTimeout(function () {
                           $scope.$apply();
                       }, fw7.DelayBeforeScopeApply);
                   });

               }
                
               else if (currentTab == 'userOrdersDeleted') {
                   if (loadingUserOffersOrders) return;
                   loadingUserOffersOrders = true;
                   var offeredParamter = {
                       "status": orderStatus,
                       "type": null,
                       "pageNumber": CookieService.getCookie('userOrdersDeleted-page-number'),
                       "pageSize": fw7.PageSize
                   };
                   appServices.CallService('places', 'POST', "api/Request/UserRequest", offeredParamter, function (offeredOrders) {
                       if (offeredOrders != null && offeredOrders.length > 0) {
                           loadingUserOffersOrders = false;
                           angular.forEach(offeredOrders, function (offer) {
                               offer.userOrderName = offer.type == 1 ? ' طلب مشوار ' + offer.id : ' طلب جديد ' + offer.id;
                               offer.agent = offer.agent != null ? offer.agent : 'غير معروف';
                               userOrdersDeleted.push(offer);
                               CookieService.setCookie('userOrdersDeleted-page-number', parseInt(CookieService.getCookie('userOrdersDeleted-page-number')) + 1);
                           });

                           if (offeredOrders.length <= fw7.PageSize) {
                               app.detachInfiniteScroll('#divInfiniteUserOrders');
                               setTimeout(function () {
                                   $scope.$apply();
                               }, fw7.DelayBeforeScopeApply);
                               return;
                           }
                       }
                       else {
                           app.detachInfiniteScroll('#divInfiniteUserOrders');
                           loadingUserOffersOrders = false;
                           SpinnerPlugin.activityStop();
                       }
                       setTimeout(function () {
                           $scope.$apply();
                       }, fw7.DelayBeforeScopeApply);
                   });

               }

            });

        });

        app.onPageBeforeAnimation('userOrder', function (page) {
            if ($rootScope.currentOpeningPage != 'userOrder') return;
            $rootScope.currentOpeningPage = 'userOrder';
            app.showTab('#UserOfferedOrders');
        });

        app.onPageAfterAnimation('userOrder', function (page) {
            if ($rootScope.currentOpeningPage != 'userOrder') return;
            $rootScope.currentOpeningPage = 'userOrder';       
            orderStatus = 1;
            CookieService.setCookie('userOrdersOffered-page-number', 1);
            CookieService.setCookie('userOrdersUnderWay-page-number', 1);
            CookieService.setCookie('userOrdersExcuted-page-number', 1);
            CookieService.setCookie('userOrdersDeleted-page-number', 1);
            getUserOrders(function (allOrders) {
                if (allOrders) {
                    $scope.userOfferedOrders = allOrders;
                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                }
            },false);

        });

        $('#UserOfferedOrders').on('show', function () {
          
            userOrdersOffered = [];
            currentTab = 'userOrdersOffered';
            orderStatus = 1;
            getUserOrders(function (allOrders) {
                if (allOrders) {                   
                    $scope.userOfferedOrders = allOrders;
                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                }
            });

        });

        $('#UserunderWayOrders').on('show', function () {
          
            userOrdersUnderWay = [];
            currentTab = 'userOrdersUnderWay';
            orderStatus = 4; 
            getUserOrders(function (allOrders) {
                if (allOrders) {
                    $scope.userUnderWayOrders = allOrders;
                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                }
            });

        });

        $('#userExcutedOrders').on('show', function () {      
            UserOrdersExcuted = [];
            currentTab = 'userOrdersExcuted';
            orderStatus = 5;
            getUserOrders(function (allOrders) {
                if (allOrders) {
                    $scope.userExcutedOrders = allOrders;
                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                }
            });
            
        });

        $('#userDeletedOrders').on('show', function () {
     
            userOrdersDeleted = [];
            currentTab = 'userOrdersDeleted';
            orderStatus = 6;
            getUserOrders(function (allOrders) {
                if (allOrders) {
                    $scope.userDeletedOrders = allOrders;                 
                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                }
            });
         
        });

        function getUserOrders(callBack,isUserPull) {
              if (!isUserPull){
                  SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
              }
              $scope.userOfferedOrders = null;
              $scope.userUnderWayOrders = null;
              $scope.userExcutedOrders = null;
              $scope.userDeletedOrders = null;
              userOrdersOffered = [];
              userOrdersUnderWay = [];
              UserOrdersExcuted = [];
              userOrdersDeleted = [];           
            var offeredParamter = {
                "status": orderStatus,
                "type": null,
                "pageNumber": 0,
                "pageSize":20
            };
            appServices.CallService('places', 'POST', "api/Request/UserRequest", offeredParamter, function (offeredOrders) {
                if (offeredOrders && offeredOrders.length > 0) {
                    app.attachInfiniteScroll('#divInfiniteUserOrders');
                    SpinnerPlugin.activityStop();               
                    $scope.userOrdersOfferedDivAlert = false;
                    $scope.userOrdersUnderWayDivAlert = false;
                    $scope.userOrdersExcutedDivAlert = false;
                    $scope.userOrdersDeletedDivAlert = false;
                    angular.forEach(offeredOrders, function (offer) {
                        offer.userOrderName = offer.type == 1 ? ' طلب مشوار ' + offer.id : ' طلب جديد ' + offer.id;
                        offer.agentFullName = offer.agentFullName != null ? offer.agentFullName : 'غير معروف';
                        if (orderStatus == 1) {                                                    
                            userOrdersOffered.push(offer);
                        }
                        else if (orderStatus == 4) {                         
                            userOrdersUnderWay.push(offer);
                        }
                        else if (orderStatus == 5) {                      
                            userOrdersExcuted.push(offer);
                        }
                        else if (orderStatus == 6) {                       
                            userOrdersDeleted.push(offer);
                        }
                    });

                    if (orderStatus == 1) {
                        callBack(userOrdersOffered);
                    }
                    else if (orderStatus == 4) {
                        callBack(userOrdersUnderWay);
                    }
                    else if (orderStatus == 5) {
                        callBack(userOrdersExcuted);
                    }
                    else if (orderStatus == 6) {
                        callBack(userOrdersDeleted);
                    }

                    if (offeredOrders <= fw7.PageSize) {                                 
                        setTimeout(function () {
                            $scope.$apply();
                        }, fw7.DelayBeforeScopeApply);
                    }
                }
                else {
                    app.attachInfiniteScroll('#divInfiniteUserOrders');
                    SpinnerPlugin.activityStop();
                    if (isUserPull) {
                        isUserPull = false;
                        callBack(null);
                    }
                   else if (orderStatus == 1) {
                        $scope.userOrdersOfferedAlert = "لايوجد طلبات معروضة";
                        $scope.userOrdersOfferedDivAlert = true;
                    }
                    else if (orderStatus == 4) {
                        $scope.userOrdersUnderWayAlert = "لايوجد طلبات قيد التنفيذ";
                        $scope.userOrdersUnderWayDivAlert = true;
                    }
                    else if (orderStatus == 5) {
                        $scope.userOrdersExcutedAlert = "لايوجد طلبات منفذة";
                        $scope.userOrdersExcutedDivAlert = true;
                    }
                    else if (orderStatus == 6) {
                        $scope.userOrdersDeletedAlert = "لايوجد طلبات محذوفة";
                        $scope.userOrdersDeletedDivAlert = true;
                    }
                    setTimeout(function () {
                        $scope.$apply();
                    }, fw7.DelayBeforeScopeApply);
                }
            });

        }

        $scope.goToOrderDetails = function (order) {
            if (order.type == 1) {
                helpers.GoToPage('requestDetails', { tripId: order.id });
            }
            else {
                helpers.GoToPage('requestDetails', { requestId: order.id });
            }
        };

        $scope.cancelOrder = function (odrerId) {
            language.openFrameworkModal('تنبيه', ' , هل تريد الغاء هذا الطلب ؟', 'confirm', function () {
                var orderCanceled = {
                    Id: odrerId
                }
                appServices.CallService('places', 'POST', "api/Request/CancelUserRequest", orderCanceled, function (orderCanceled) {
                    if (orderCanceled) {
                        $scope.userOfferedOrders = $scope.userOfferedOrders.filter(function(item) {
                            return item.id !== orderCanceled.id;
                        });
                        setTimeout(function () {
                            $scope.$apply();
                        }, fw7.DelayBeforeScopeApply);
                    }
                });
            });
          }  
             
        $scope.GoToNotifications = function () {        
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

