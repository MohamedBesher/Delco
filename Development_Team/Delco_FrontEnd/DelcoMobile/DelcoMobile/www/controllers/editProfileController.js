/// <reference path="../js/angular.js" />

myApp.angular.controller('editProfileController', ['$document', '$scope', '$rootScope', '$http', 'InitService', '$log', 'appServices', 'CookieService', 'SidePanelService', 'helpers', function ($document, $scope, $rootScope, $http, InitService, $log, appServices, CookieService, SidePanelService, helpers) {
    'use strict';

    var fw7 = myApp.fw7;
    var app = myApp.fw7.app;

    $document.ready(function () {
        app.onPageInit('editProfile', function (page) {
            if ($rootScope.currentOpeningPage != 'editProfile') return;
            $rootScope.currentOpeningPage = 'editProfile';

        });

        app.onPageBeforeAnimation('editProfile', function (page) {
            if ($rootScope.currentOpeningPage != 'editProfile') return;
            $rootScope.currentOpeningPage = 'editProfile';
            $scope.resetForm();
            var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
            $scope.editUserForm.name = userLoggedIn.fullName;        
            $scope.editUserForm.email = userLoggedIn.email;
        });

        app.onPageAfterAnimation('editProfile', function (page) {
            if ($rootScope.currentOpeningPage != 'editProfile') return;
            $rootScope.currentOpeningPage = 'editProfile';

        });


        $scope.submitForm = function (isValid) {
            $scope.editUserReset = true;
            if (isValid) {
                var editParamter = {
                    'FullName': $scope.editUserForm.name,
                    'Email': $scope.editUserForm.email,
                    'UserName':'medo',
                    'Role': 'User'
                }

                SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
                appServices.CallService('editProfile', "POST", "api/Account/ChangeUserInfo", editParamter, function (editResult) {
                    if (editResult != null) {
                        SpinnerPlugin.activityStop();
                        $scope.resetForm();
                        var userLoggedIn = JSON.parse(CookieService.getCookie('userLoggedIn'));
                        userLoggedIn.fullName = $scope.editUserForm.name;
                        userLoggedIn.email = $scope.editUserForm.email;
                        CookieService.setCookie('userLoggedIn', JSON.stringify(userLoggedIn));                       
                        language.openFrameworkModal('نجاح', 'تم تعديل بياناتك بنجاح .', 'alert', function () {                         
                        });
                    }
                    else {
                        SpinnerPlugin.activityStop();
                    }
                });
            }
        };

        $scope.resetForm = function () {
            $scope.editUserReset = false;
            $scope.editUserForm.name = null;
            $scope.editUserForm.email = null;
            if (typeof $scope.EditUserProfile != 'undefined' && $scope.EditUserProfile != null) {
                $scope.EditUserProfile.$setPristine(true);
                $scope.EditUserProfile.$setUntouched();
            }
        }

        function getImage() {
            navigator.camera.getPicture(uploadPhoto, function (message) {

            }, {
                quality: 100,
                destinationType: navigator.camera.DestinationType.DATA_URL,
                sourceType: navigator.camera.PictureSourceType.PHOTOLIBRARY,
                allowEdit: true,
                encodingType: Camera.EncodingType.JPEG,
                targetWidth: 300,
                targetHeight: 300,
                popoverOptions: CameraPopoverOptions,
                saveToPhotoAlbum: false,
                correctOrientation: true
            });
        }

        $scope.AddImage = function () {
            getImage();
        };

        function uploadPhoto(imageURI) {
            $scope.editUserForm.userImage = imageURI;
            var params = {
                'Picture': imageURI
            };

            $('#imgUploading').show();
            appServices.CallService('userDetails', "POST", "api/Account/ChangeImage", params, function (res) {
                if (res != null) {
                    language.openFrameworkModal('نجاح', 'تم رفع الصورة بنجاح .', 'alert', function () {
                        $('#imgUploading').hide();

                        CookieService.setCookie('usrPhoto', res);
                        var user = JSON.parse(CookieService.getCookie('userLoggedIn'));
                        user.photoUrl = res;
                        CookieService.setCookie('userLoggedIn', JSON.stringify(user));
                        $scope.userDetailsForm.userImage = res;
                        setTimeout(function () {
                            $scope.$digest();
                        }, fw7.DelayBeforeScopeApply);
                    });
                }
                else {
                    language.openFrameworkModal('خطأ', 'خطأ في إضافة صورة للمستخدم.', 'alert', function () { });
                }
            });
        }

        $scope.RemoveImage = function () {

            var params = {
                'Picture': 'none'
            };

            SpinnerPlugin.activityStart("تحميل ...", { dimBackground: true });
            appServices.CallService('userDetails', "POST", "api/Account/ChangeImage", params, function (res) {
                SpinnerPlugin.activityStop();
                if (res != null) {
                    language.openFrameworkModal('نجاح', 'تم حذف الصورة بنجاح .', 'alert', function () {
                        $('#imgUploading').hide();

                        CookieService.setCookie('usrPhoto', '');
                        var user = JSON.parse(CookieService.getCookie('userLoggedIn'));
                        user.photoUrl = '';
                        CookieService.setCookie('userLoggedIn', JSON.stringify(user));
                        $scope.userDetailsForm.userImage = 'img/avatar.png';
                        setTimeout(function () {
                            $scope.$digest();
                        }, fw7.DelayBeforeScopeApply);
                    });
                }
                else {
                    language.openFrameworkModal('خطأ', 'خطأ في حذف صورة للمستخدم.', 'alert', function () { });
                }
            });
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
        $scope.form = {};
        $scope.editUserForm = {};
        $scope.emailRegex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        $scope.nameRegex = /^[A-Za-z0-9]*$/;

        app.init();
    });

}]);

