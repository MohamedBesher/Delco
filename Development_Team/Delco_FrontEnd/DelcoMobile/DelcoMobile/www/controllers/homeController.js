/// <reference path="../js/angular.js" />

myApp.angular.controller('homeController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', 'SignalRFactory', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers, SignalRFactory) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;
    var loadingTrips = false;
    var loadingRequests = false;
    var loadingFavorites = false;
    var allAgentTripOrders = [];
    var allAgentRequestOrders = [];
    var allAgentFavoriteOrders = [];
    var isPageReInit = false;
    var currentTab = 'Trips';
    var currentTabInFavorites = 'Trips';
    var watchID;
    var NewOrdersSwiper;
    var PendingOrdersSwiper;
    var agentTabsSwiper;
    var allOrders = [];
    var reasonList = [];
    var requestId;
    var saveRefuseReason;

    function getRefuseReasons() {
        appServices.CallService('home', 'Get', "api/RefuseReason/RefuseReasons", '', function (refuseReasons) {
            if (refuseReasons) {
                $scope.reasons = refuseReasons;
                setTimeout(function () {
                    $scope.$apply();
                }, fw7.DelayBeforeScopeApply);
            }
        });
    }

    function InitializeSwipers() {
        NewOrdersSwiper = app.swiper('#NewOrdersSwiper', {
            pagination: '.swiper-pagination',
            loop: true,
            autoplay: 3000,
            autoplayDisableOnInteraction:false,
            spaceBetween: 5,
            slidesPerView: 1,
            centeredSlides: true
        });

        PendingOrdersSwiper = app.swiper('#PendingOrdersSwiper', {
            pagination: '.swiper-pagination',
            autoplay: 3000,
            autoplayDisableOnInteraction: false,
            spaceBetween: 5,
            slidesPerView: 1,
            centeredSlides: true
        });

        agentTabsSwiper = app.swiper('#agentTabsSwiper', {
            pagination: '.swiper-pagination',
            autoplayDisableOnInteraction: false,
            slidesPerView: 1,
            centeredSlides: true
        });
        setTimeout(function () {
            $scope.$apply();
            NewOrdersSwiper.update();
            PendingOrdersSwiper.update();
            agentTabsSwiper.update();
        }, fw7.DelayBeforeScopeApply);
    }

    function GetAllOrdersForUser(callBack) {
        allOrders = [];
        var params = {
            "type": null,
            "status": null,
            "pageNumber": 0,
            "pageSize": 20
        };

        appServices.CallService('places', 'POST', "api/Request/UserRequest", params, function (userOrders) {
            SpinnerPlugin.activityStop();
            if (userOrders && userOrders.length > 0) {
                angular.forEach(userOrders, function (order) {
                    allOrders.push(order);
                });
            }

            callBack(allOrders);
        });
    }

    function GetAllOrdersForAgent(callBack) {
        allOrders = [];
        var params = {
            "type": null,
            "status":1,
            "PageNumber": 0,
            "PageSize": 20
        };

        SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
        appServices.CallService('places', 'POST', "api/Request/AgentAllRequests", params, function (agentOrders) {
            SpinnerPlugin.activityStop();
            if (agentOrders && agentOrders.length > 0) {
                angular.forEach(agentOrders, function (order) {
                    allOrders.push(order);
                });
            }

            callBack(allOrders);
        });
    }

    function GetUserOrders() {
        var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
        $scope.userNewOrders = [];
        $scope.userPendingOrders = [];

        GetAllOrdersForUser(function (userOrders) {
            if (userOrders != null && userOrders.length > 0) {
                var userNewOrders = [];
                var userPendingOrders = [];

                angular.forEach(userOrders, function (order) {
                    var orderFullDate = order.createdDate;
                    order.day = orderFullDate.split('T')[0];
                    order.time = orderFullDate.split('T')[1].indexOf('.') > -1 ? orderFullDate.split('T')[1].split('.')[0] : orderFullDate.split('T')[1];
                    
                    order.timePeriod = parseInt(order.time.split(':')[0]) > 12 ? 'مساءا' : 'صباحا';
                    order.text = order.type == 1 ? ' مشوار جديد ' + order.id : ' طلب جديد ' + order.id;
                    order.agentFullName = order.agentFullName != null && order.agentFullName != '' && order.agentFullName != ' ' ? order.agentFullName : 'غير معرف';
                    order.photoUrl = order.type == 1 ? 'img/1.png' : 'img/4.png';
                    order.fallBackImage = order.type == 1 ? 'img/1.png' : 'img/4.png';
                });

                userNewOrders = userOrders.filter(function (order) {
                    return order.status == 1;
                });

                userPendingOrders = userOrders.filter(function (order) {
                    return order.status == 4;
                });

                $scope.userNewOrderAlert = userNewOrders != null && userNewOrders.length > 0 ? false : true;
                $scope.userPendingOrderAlert = userPendingOrders != null && userPendingOrders.length > 0 ? false : true;
                $scope.userNewOrders = userNewOrders;
                $scope.userPendingOrders = userPendingOrders;

            }
            else {
                $scope.userNewOrderAlert = true;
                $scope.userPendingOrderAlert = true;
            }
            setTimeout(function () {
                $scope.$apply();
            }, fw7.DelayBeforeScopeApply);
        });       
    }

    function GetAgentOrders(IsPull) {
        var agentFavoriteOrders = [];
        var agentTripOrders = [];
        var agentRequestOrders = [];

        var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
        var favoriteType = typeof userLoggedIn.type != 'undefined' || parseInt(userLoggedIn.type) > 0 ? userLoggedIn.type : 0;
        favoriteType = parseInt(favoriteType) > 2 ? 0 : favoriteType;
        $scope.resetHomeScope();

        GetAllOrdersForAgent(function (allOrders) {
            if (allOrders != null && allOrders.length > 0) {
                angular.forEach(allOrders, function (order) {
                    var orderFullDate = order.createdDate;
                    order.day = orderFullDate.split('T')[0];
                    order.time = orderFullDate.split('T')[1].indexOf('.') > -1 ? orderFullDate.split('T')[1].split('.')[0] : orderFullDate.split('T')[1];

                    order.timePeriod = parseInt(order.time.split(':')[0]) > 12 ? 'مساءا' : 'صباحا';
                    order.text = order.type == 1 ? ' مشوار جديد ' + order.id : ' طلب جديد ' + order.id;
                    order.userFullName = order.userFullName != null && order.userFullName != '' && order.userFullName != ' ' ? order.userFullName : 'غير معرف';
                    order.photoUrl = order.type == 1 ? 'img/1.png' : 'img/4.png';
                    order.fallBackImage = order.type == 1 ? 'img/1.png' : 'img/4.png';
                });

                if (parseInt(favoriteType) > 0) {
                    agentFavoriteOrders = allOrders.filter(function (order) {
                        return order.type == userLoggedIn.type;
                    });
                    $scope.agentFavoriteOrderAlert = agentFavoriteOrders != null && agentFavoriteOrders.length > 0 ? false : true;
                }
                else {
                    agentTripOrders = allOrders.filter(function (order) {
                        return order.type == 1;
                    });

                    agentRequestOrders = allOrders.filter(function (order) {
                        return order.type == 2;
                    });
                    $scope.agentTripTabAlert = agentTripOrders != null && agentTripOrders.length > 0 ? false : true;
                    $scope.agentRequestTabAlert = agentRequestOrders != null && agentRequestOrders.length > 0 ? false : true;

                    if (agentTripOrders != null && agentTripOrders.length > 0) {
                        $('#divAgentPage').removeClass('fullHeight');
                    }
                    else {
                        $('#divAgentPage').addClass('fullHeight');
                    }
                }

                allAgentFavoriteOrders = agentFavoriteOrders;
                allAgentTripOrders = agentTripOrders;
                allAgentRequestOrders = agentRequestOrders;
                if (allAgentFavoriteOrders.length < fw7.PageSize) {
                    $scope.agentFavoritesInfiniteLoader = false;
                }
                if (allAgentTripOrders.length < fw7.PageSize) {
                    $scope.agentTripTabInfiniteLoader = false;
                }
                if (allAgentRequestOrders.length < fw7.PageSize) {
                    $scope.agentRequestTabInfiniteLoader = false;
                }
                $scope.agentFavoriteOrders = agentFavoriteOrders;
                $scope.agentTripOrders = agentTripOrders;
                $scope.agentRequestOrders = agentRequestOrders;
            }
            else {
                $scope.agentFavoriteOrderAlert = true;
                $scope.agentTripTabAlert = true;
                $scope.agentRequestTabAlert = true;
            }
            if (IsPull) {
                app.pullToRefreshDone();
            }

            setTimeout(function () {
                $scope.$apply();
            }, fw7.DelayBeforeScopeApply);
        });

    }

    function LoadUserOrAgentData() {
        var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
        $scope.userIsAgent = typeof userLoggedIn.roles === 'undefined' || userLoggedIn.roles == 'Agent' ? true : false;
        $scope.hasFavoriteType = typeof userLoggedIn.type != 'undefined' && parseInt(userLoggedIn.type) > 0 && parseInt(userLoggedIn.type) < 3 ? true : false;
        setTimeout(function () {
            $scope.$apply();
        }, fw7.DelayBeforeScopeApply);

        if (typeof userLoggedIn.roles === 'undefined' || userLoggedIn.roles == 'Agent') {
            GetAgentOrders(false);      
        }
        else {
            GetUserOrders();
        }
    }

    function InitializeHome() {        
        SidePanelService.DrawMenu();
        currentTab = 'Trips';
        currentTabInFavorites = 'Trips';
        var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
        var favoriteType = typeof userLoggedIn.type != 'undefined' || parseInt(userLoggedIn.type) > 0 ? userLoggedIn.type : 0;
        favoriteType = parseInt(favoriteType) > 2 ? 0 : favoriteType;

        if (typeof userLoggedIn.roles === 'undefined' || userLoggedIn.roles == 'Agent') {
            app.attachInfiniteScroll('#divInfiniteHome');
        }
        else {
            app.detachInfiniteScroll('#divInfiniteHome');
        }

        if (parseInt(favoriteType) > 0) {
            currentTab = 'Favorites';
            currentTabInFavorites = 'Trips';
        }

     
    }


    $rootScope.TrackUserIfAgent = function TrackUserIfAgent(request) {
        if (typeof request != 'undefined' && request != null && typeof watchID == 'undefined') {
            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));

            if (typeof userLoggedIn != 'undefined' && userLoggedIn != null && userLoggedIn.roles == 'Agent') {
                navigator.geolocation.getCurrentPosition(function (position) {
                    var pos = position;
                }, function (PositionError) { var error = PositionError; }, { enableHighAccuracy: true });

                //setInterval(function () {
                //    navigator.geolocation.getCurrentPosition(function (position) {
                //       // var pos = position;
                //        var params = {
                //            'Latitude': position.coords.latitude,
                //            'Longitude': position.coords.longitude,
                //            'UserId': request.userId,
                //            'RequestId': request.id
                //        };

                //        $rootScope.proxy.invoke('trackingAgant', params);
                //    }, function (PositionError) { var error = PositionError; }, { enableHighAccuracy: true });
                //}, 3000);

                watchID = navigator.geolocation.watchPosition(function (position) {
                    var params = {
                        'Latitude': position.coords.latitude,
                        'Longitude': position.coords.longitude,
                        'UserId': request.userId,
                        'RequestId': request.id
                    };

                    $rootScope.proxy.invoke('trackingAgant', params);

                }, function error(err) {
                    console.log('ERROR(' + err.code + '): ' + err.message);
                }, {
                    enableHighAccuracy: true,
                    frequency: 5000
                });

            }
        }
    };


    $document.ready(function () {

        app.onPageInit('home', function (page) {
            if ($rootScope.currentOpeningPage != 'home') return;
            $rootScope.currentOpeningPage = 'home';       
            if (NewOrdersSwiper && PendingOrdersSwiper && agentTabsSwiper) {
                NewOrdersSwiper.destroy(true, true);
                PendingOrdersSwiper.destroy(true, true);
                agentTabsSwiper.destroy(true, true);
            }

            InitializeSwipers();

            $$('#divInfiniteHome').on('ptr:refresh', function (e) {
                GetAgentOrders(true);
            });

            $$('#divInfiniteHome').on('infinite', function () {
                if (currentTab == 'Trips') {
                    if (loadingTrips) return;
                    loadingTrips = true;

                    SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                    var agentParams = {
                        "type": 1,
                        "status": 1,
                        "PageNumber": parseInt(CookieService.getCookie('agentTripsTab-page-number')),
                        "PageSize": 20
                    };
                    appServices.CallService('places', 'POST', "api/Request/AgentAllRequests", agentParams, function (orders) {
                        SpinnerPlugin.activityStop();
                        if (orders && orders.length > 0) {
                            loadingTrips = false;
                            CookieService.setCookie('agentTripsTab-page-number', parseInt(CookieService.getCookie('agentTripsTab-page-number')) + 1);

                            angular.forEach(orders, function (order) {
                                allAgentTripOrders.push(order);
                            });

                            if (orders && orders.length <= fw7.PageSize) {
                                app.detachInfiniteScroll('#divInfiniteHome');

                                setTimeout(function () {
                                    $scope.$apply();
                                }, fw7.DelayBeforeScopeApply);
                                return;
                            }
                        }
                        else {
                            loadingTrips = false;
                        }

                        setTimeout(function () {
                            $scope.$apply();
                        }, fw7.DelayBeforeScopeApply);
                    });
                }
                else if (currentTab == 'Requests') {
                    if (loadingRequests) return;
                    loadingRequests = true;

                    $scope.agentRequestTabInfiniteLoader = true;

                    SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                    var agentParams = {
                        "type": 2,
                        "status": 1,
                        "PageNumber": CookieService.getCookie('agentRequestsTab-page-number'),
                        "PageSize": 20
                    };
                    appServices.CallService('places', 'POST', "api/Request/AgentAllRequests", agentParams, function (orders) {
                        SpinnerPlugin.activityStop();
                        if (orders && orders.length > 0) {
                            loadingRequests = false;
                            CookieService.setCookie('agentRequestsTab-page-number', parseInt(CookieService.getCookie('agentRequestsTab-page-number')) + 1);
                            angular.forEach(orders, function (order) {
                                allAgentRequestOrders.push(order);
                            });

                            if (orders && orders.length <= fw7.PageSize) {
                                $scope.agentRequestTabInfiniteLoader = false;
                                app.detachInfiniteScroll('#divInfiniteHome');

                                setTimeout(function () {
                                    $scope.$apply();
                                }, fw7.DelayBeforeScopeApply);
                                return;
                            }
                        }
                        else {
                            loadingRequests = false;
                        }

                        setTimeout(function () {
                            $scope.$apply();
                        }, fw7.DelayBeforeScopeApply);
                    });
                }
                else if (currentTab == 'Favorites') {
                    if (loadingFavorites) return;
                    loadingFavorites = true;

                    var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
                    var favoriteType = typeof userLoggedIn.type != 'undefined' || parseInt(userLoggedIn.type) > 0 ? userLoggedIn.type : 0;
                    favoriteType = parseInt(favoriteType) > 2 ? 0 : favoriteType;

                    $scope.agentFavoritesInfiniteLoader = true;

                    var agentParams = {
                        "type": favoriteType,
                        "status": 1,
                        "PageNumber": CookieService.getCookie('agentFavorites-page-number'),
                        "PageSize": 20
                    };

                    SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                    appServices.CallService('places', 'POST', "api/Request/AgentAllRequests", agentParams, function (orders) {
                        SpinnerPlugin.activityStop();
                        if (orders && orders.length > 0) {
                            loadingFavorites = false;
                            CookieService.setCookie('agentFavorites-page-number', parseInt(CookieService.getCookie('agentFavorites-page-number')) + 1);

                            angular.forEach(orders, function (order) {
                                allAgentFavoriteOrders.push(order);
                            });

                            if (orders && orders.length <= fw7.PageSize) {
                                $scope.agentFavoritesInfiniteLoader = false;
                                app.detachInfiniteScroll('#divInfiniteHome');

                                setTimeout(function () {
                                    $scope.$apply();
                                }, fw7.DelayBeforeScopeApply);
                                return;
                            }
                        }
                        else {
                            $scope.agentFavoritesInfiniteLoader = false;
                        }

                        setTimeout(function () {
                            $scope.$apply();
                        }, fw7.DelayBeforeScopeApply);
                    });
                }
            });
        });

        app.onPageReinit('home', function (page) {
            app.showTab('#agentTripsTab');
            NewOrdersSwiper.destroy(true, true);
            PendingOrdersSwiper.destroy(true, true);
            agentTabsSwiper.destroy(true, true);
            InitializeSwipers();
        });

        app.onPageBeforeAnimation('home', function (page) {
            if ($rootScope.currentOpeningPage != 'home') return;
            $rootScope.currentOpeningPage = 'home';
            InitializeHome();
            LoadUserOrAgentData();
            getRefuseReasons();
        });

        app.onPageAfterAnimation('home', function (page) {
            if ($rootScope.currentOpeningPage != 'home') return;
            $rootScope.currentOpeningPage = 'home';
            InitializeSwipers();
        });

        $('#agentTripsTab').on('show', function () {
            currentTab = 'Trips';
            currentTabInFavorites = 'Trips';
            $('#agentTripsTab').css('height', '100%');
            $('#agentRequestsTab').css('height', '0px');

            if ($scope.agentTripOrders != null && $scope.agentTripOrders.length > 0) {
                $('#divAgentPage').removeClass('fullHeight');
            }
            else {
                $('#divAgentPage').addClass('fullHeight');
            }
        });

        $('#agentRequestsTab').on('show', function () {
            currentTab = 'Requests';
            currentTabInFavorites = 'Requests';
            $('#agentTripsTab').css('height', '0px');
            $('#agentRequestsTab').css('height', '100%');

            if ($scope.agentRequestOrders != null && $scope.agentRequestOrders.length > 0) {
                $('#divAgentPage').removeClass('fullHeight');
            }
            else {
                $('#divAgentPage').addClass('fullHeight');
            }
        });

        

        $scope.resetHomeScope = function () {
            CookieService.setCookie('agentTripsTab-page-number', 1);
            CookieService.setCookie('agentRequestsTab-page-number', 1);
            CookieService.setCookie('agentFavorites-page-number', 1);
            $scope.agentFavoriteOrderAlert = false;
            $scope.agentTripTabAlert = false;
            $scope.agentRequestTabAlert = false;
            $scope.agentTripTabInfiniteLoader = false;
            $scope.agentRequestTabInfiniteLoader = false;
            $scope.agentFavoritesInfiniteLoader = false;
            $scope.agentFavoriteOrders = [];
            $scope.agentTripOrders = [];
            $scope.agentRequestOrders = [];
            loadingTrips = false;
            loadingRequests = false;
            loadingFavorites = false;
            allAgentTripOrders = [];
            allAgentRequestOrders = [];
            allAgentFavoriteOrders = [];
        }

        $scope.GoToOrderDetails = function (order) {
            isPageReInit = false;
            if (order.type == 1) {
                helpers.GoToPage('tripDetails', { tripId: order.id });
            }
            else {
                helpers.GoToPage('requestDetails', { requestId: order.id });
            }
        };

        $scope.GoToAddTrip = function () {
            isPageReInit = false;
            helpers.GoToPage('addTrip', null);
        };

        $scope.GoToAddRequest = function () {
            isPageReInit = false;
            helpers.GoToPage('addRequest', null);
        };

        $scope.GoToPreviousPage = function () {
            isPageReInit = false;
            helpers.GoBack();
        };

       
        $scope.GoToNotifications = function () {
            //isPageReInit = false;
            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
            if (typeof userLoggedIn.roles === 'undefined' || userLoggedIn.roles == 'Agent') {
                helpers.GoToPage('agentNotification', null);
            }
            else {
                helpers.GoToPage('userNotification', null);
            }
        };

        function SaveRefuseUserRequest(reasonId) {
            var refuseRequestParamters = {
                RefuseReasonId: reasonId,
                RequestId: requestId
            }
            appServices.CallService('home', 'POST', "api/RefuseRequest/SaveRefuseUserRequest", refuseRequestParamters, function (refuseReasons) {
                if (refuseReasons) {
                    saveRefuseReason=true              
                }
                else {
                    saveRefuseReason = false
                }
            });
        }

        $scope.getRefuseRequestId = function (reasonId) {
            SaveRefuseUserRequest(reasonId);
        }
         
        $scope.CancelOrder = function (order) {
            //app.popover('.popover', '#cancelButton', false, true, true);
            requestId = order.id;                  
                $scope.cancelOrder = function () {                
                    if (saveRefuseReason) {
                        app.closeModal('#popupSuspende');
                        language.openFrameworkModal('تأكيد', 'هل أنت متأكد من إلغاء الرحلة ؟', 'confirm', function () {
                            var params = {
                                "Id": order.id
                            };

                            appServices.CallService('requestDetails', 'POST', "api/Request/CancelUserRequest", params, function (result) {
                                SpinnerPlugin.activityStop();
                                if (result) {
                                    if (order.type == 1) {
                                        language.openFrameworkModal('خطأ', 'تم الغاءالمشوار بنجاح', 'alert', function () {
                                        });
                                    }
                                    else {
                                        language.openFrameworkModal('خطأ', 'تم الغاء الطلب بنجاح', 'alert', function () {
                                        });
                                    }
                                    $scope.userNewOrders = $scope.userNewOrders.filter(function (order) {
                                        return order.id != result.id;
                                    })
                                    helpers.GoToPage('home', null);

                                }
                                else {
                                    language.openFrameworkModal('خطأ', 'خطأ اثناء الغاء  الرحلة', 'alert', function () {
                                        helpers.GoToPage('home', null);
                                    });
                                }

                                setTimeout(function () {
                                    $scope.$apply();
                                }, fw7.DelayBeforeScopeApply);

                            });

                        });
                    }
                    else {
                        language.openFrameworkModal('خطأ', 'من فضلك قم باختيار سبب الغاء الرحلة', 'alert', function () { });
                    }
                }
        };

        app.init();
    });

}]);

