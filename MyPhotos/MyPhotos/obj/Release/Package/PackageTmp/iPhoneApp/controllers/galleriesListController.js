var app = angular.module('iPhoneApp');
app.controller("galleriesListController", [
    'contentService',
    '$scope',
    '$location', function (contentService, $scope, $location) {

        var w = Math.max(document.documentElement.clientWidth, window.innerWidth || 0)
        $scope.stackColumns = (w <= 730);

        contentService.getHomepageGalleries().then(
               function (success) {
                   if ($scope.stackColumns) {
                       computeColumnStack($scope, success.ViewGalleries);
                       $scope.galleryCollection = success;
                   }
                   else {
                       $scope.galleryCollection = success;
                   }
               },
               function (error) {
                   $scope.galleryCollection = [];
                   $scope.errorMessage = "failed to retrieve galleries";
               }
           );

        $scope.startViewer = function (startGalleryName, startImageName) {
            $location.path('/ViewGallery/' + startGalleryName + '/' + startImageName);
        }

        var computeColumnStack = function(scope, galleryCollection)
        {
            for (var i = 0; i<galleryCollection.length; i++)
            {
                galleryCollection[i].ColumnSets = {
                    'Column1': [],
                    'Column2': []
                };

                for (var x = 0; x<galleryCollection[i].Gallery.Images.length; x++)
                {
                    if ((x%2)!=0)
                    {
                        galleryCollection[i].ColumnSets.Column1.push(
                            galleryCollection[i].Gallery.Images[x]);
                    }else
                    {
                        galleryCollection[i].ColumnSets.Column2.push(
                            galleryCollection[i].Gallery.Images[x]);

                    }
                }
            }
        }
    }]);