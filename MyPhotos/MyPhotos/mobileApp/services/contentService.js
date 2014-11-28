
angular.module('mobileApp').service('contentService', ['$http', '$q', function ($http, $q) {
    this.getHomepageGalleries = function () {

        var deferred = $q.defer();

        $http.get('/Content/HomepageGalleries')
            .success(function (data, status, headers, config) {
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.resolve(data);
            });
        return deferred.promise;
    }

    this.getGallery = function(galleryName)
    {
        var deferred = $q.defer();

        $http.get('/Content/HomepageGallery?galleryName='+galleryName)
            .success(function (data, status, headers, config) {
                deferred.resolve(data);
            })
            .error(function (data, status, headers, config) {
                deferred.resolve(data);
            });
        return deferred.promise;
    }
}]);