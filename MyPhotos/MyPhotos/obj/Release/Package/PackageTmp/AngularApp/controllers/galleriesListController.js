var app = angular.module('myPhotosApp');
app.controller("galleriesListController", [
    'contentService',
    '$scope',
    '$location', function (contentService, $scope, $location) {

        contentService.getHomepageGalleries().then(
               function (success) {
                   $scope.galleryCollection = success;
               },
               function (error) {
                   $scope.galleryCollection = [];
                   $scope.errorMessage = "failed to retrieve galleries";
               }
           );

        $scope.startViewer = function (startGalleryName, startImageName) {
            $location.path('/ViewGallery/' + startGalleryName + '/' + startImageName);
        }
    }]);