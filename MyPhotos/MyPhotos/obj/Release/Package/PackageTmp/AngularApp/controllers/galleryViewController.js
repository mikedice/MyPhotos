var app = angular.module('myPhotosApp');
app.controller('galleryViewController', ['$scope', '$routeParams', 'contentService', function ($scope, $routeParams, contentService) {

    var w = Math.max(document.documentElement.clientWidth, window.innerWidth || 0)
    $scope.mobile = (w <= 730);

    contentService.getGallery($routeParams.galleryName).then(
       function (success) {
           var gallery = success.ViewGalleries[0].Gallery;

           if ($routeParams.startImage)
           {
               for (var i = 0; i < gallery.Images.length; i++)
               {
                   if (gallery.Images[i].Name == $routeParams.startImage)
                   {
                       gallery.Images[i].active = true;
                       break;
                   }
               }
           }

           $scope.gallery = gallery
       },
       function (error) {
           $scope.errorMessage = "failed to retrieve galleries";
       }
   );
}]);