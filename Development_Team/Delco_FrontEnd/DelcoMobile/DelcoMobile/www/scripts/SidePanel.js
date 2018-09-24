myApp.angular.factory('SidePanelService', ['$rootScope', 'CookieService', function ($rootScope,CookieService) {
    'use strict';

    function DrawMenu() {
        var userLoggedIn = CookieService.getCookie('userLoggedIn') ? CookieService.getCookie('userLoggedIn') : null;

        $('#linkMenuToHome').show();
        $('#linkMenuToSearch').show();
        $('#linkMenuToContact').show();
        $('#linkMenuToUserRequests').show();
        $('#linkMenuToUserNotifications').show();
        $('#linkMenuToAbuse').show();
        $('#linkMenuToTerms').show();
        $('#linkMenuToLogin').show();
        $('#linkMenuToChangePassword').show();
        if (typeof userLoggedIn != 'undefined' && userLoggedIn != null) {
            var imgUserPic = document.getElementById('imgUserPic');
            userLoggedIn = JSON.parse(userLoggedIn);

            if (typeof userLoggedIn.photoUrl != 'undefined' && userLoggedIn.photoUrl != null && userLoggedIn.photoUrl != ' ' && userLoggedIn.photoUrl != '') {
                imgUserPic.src = hostUrl + userLoggedIn.photoUrl;
            }
            $('#linkMenuToUserProfile').show();
            $('#hdrUserName').html(userLoggedIn.fullName);
            $('#lblMenuLoginText').html('تسجيل خروج');
        }
        else {
            $('#linkMenuToUserProfile').hide();
            $('#hdrUserName').html('زائر');
            $('#lblMenuLoginText').html('تسجيل دخول');
        }

        if (navigator.appInfo) {
            var versionNumber = navigator.appInfo.version;
            $('.pVersion').html('V ' + versionNumber);
        }
        else {
            
        }
    }

    return {
        DrawMenu: DrawMenu
    }

}]);