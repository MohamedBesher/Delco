myApp.angular.filter('generateURL', function ($http) {
    return function (image) {

        if (image != null && image != '' && image != ' ' && image.indexOf('img/') == -1) {
            if (image.length > 200) {
                return 'data:image/jpeg;base64, ' + image;
            }
            else {
                return hostUrl + 'Uploads/' + image;
            }
            
        }
        else {
            return image;
        }
    };
});