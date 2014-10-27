var app = angular.module('myPhotosApp');
app.controller('galleryViewController', ['$scope', '$routeParams', 'contentService', function ($scope, $routeParams, contentService) {
    contentService.getGallery($routeParams.galleryName).then(
       function (success) {
           $scope.gallery = success.ViewGalleries[0];
       },
       function (error) {
           $scope.errorMessage = "failed to retrieve galleries";
       }
   );
}]);